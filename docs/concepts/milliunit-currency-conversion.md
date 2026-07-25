# Milliunit currency conversion

## Premise

YNAB represents every currency amount over its API as an integer in
**milliunits** — one thousandth of the budget's major currency unit. A
transaction of $12.34 is transmitted and stored as the integer `12340`.
This is deliberate on YNAB's part: integers have no floating-point
rounding error, so money arithmetic on their end is exact. Every value
YnabSharp reads from or writes to the API has to cross this boundary.

## Problem

Two different, incompatible things both get called "the balance"/"the
amount" depending on which side of the API boundary you're looking at —
a raw milliunit integer on the wire, and a `decimal` in the currency's
major unit (pounds, in this codebase's naming) everywhere else. Getting
the direction of conversion wrong, or applying it twice, doesn't crash —
it silently produces a number that's off by exactly 1000x, which will
usually still look like a plausible amount until someone notices their
YNAB budget is showing $100,000 instead of $100.

## Solution

All conversion goes through `YnabSharp/Sanitisers/MilliunitConverter.cs`:

```csharp
public static class MilliunitConverter
{
    private const decimal ConversationRate = 1000m;

    // milliunits -> pounds (reading FROM the API)
    public static decimal Calculate(int amountInMilliunits)
        => amountInMilliunits / ConversationRate;

    public static decimal? MilliunitToPounds(int? amountInMilliunits)
        => amountInMilliunits.HasValue ? Calculate(amountInMilliunits.Value) : null;

    // pounds -> milliunits (writing TO the API)
    public static int PoundsToMilliunit(decimal amountInPounds)
        => (int)(amountInPounds * ConversationRate);
}
```

**The rule:** a value should be converted through this class **exactly
once**, at the point it crosses the API boundary — either right after
deserializing a response, or right before serializing a request.
Everywhere else in the codebase, a `decimal` field means pounds, full
stop; an `int` field on a `*Response`/`*Request` DTO means milliunits,
full stop.

Reading direction (`Calculate`/`MilliunitToPounds`) is used throughout
the domain layer — e.g. `Account.ClearedBalance`,
`SplitTransactions.Amount`, `Category.GoalOverallFunded` all wrap a raw
response field with exactly one call to `Calculate`.

Writing direction (`PoundsToMilliunit`) is used in the mapping layer —
e.g. `Mappers/NewAccountMapping.cs`'s `ToAccountRequest(this Account
account)` correctly calls `PoundsToMilliunit(account.ClearedBalance)`
exactly once, converting the domain type's already-in-pounds value into
the milliunit integer the request DTO needs.

## Constraints & tradeoffs

**Integer milliunits, not `decimal` milliunits, on the wire.** This is
YNAB's choice, not this library's — `int` overflow is a real (if
unlikely) constraint on very large amounts, and `PoundsToMilliunit`
narrowing a `decimal` to `int` is a checked conversion that throws
`OverflowException` on out-of-range input rather than failing gracefully.

**No compile-time distinction between "pounds" and "milliunits".** Both
are represented as plain `decimal`/`int` — nothing in the type system
stops a value from crossing the boundary twice, or zero times. This is
the root cause of a real, confirmed bug in this exact conversion (see
below) and is worth treating as a known weak point, not a solved
problem — a value type wrapping "amount in milliunits" distinctly from
"amount in pounds" would make the mistake below a compile error instead
of a runtime one.

## Questions & answers

**Is this converter correct today?**
Partially. The reading direction is exact (dividing by `1000m` in
decimal arithmetic has no rounding error). The writing direction
(`PoundsToMilliunit`) truncates via a bare `(int)` cast instead of
rounding — a fractional-milliunit input silently loses precision instead
of rounding to the nearest one. Neither direction has full test
coverage; only `Calculate` has a test in the whole repository.

**Is there a known bug in how this gets applied?**
Yes — `NewAccount.ClearedBalance` (`YnabSharp/NewAccount.cs`) already
calls `PoundsToMilliunit` once, and `NewAccountMapping.ToAccountRequest`
calls it *again* on that already-converted value — applying the
conversion twice, inflating the balance sent to YNAB's real API by
~1000x. This is exactly the failure mode this doc exists to prevent:
converting a value that had already crossed the boundary. See the
tracked issue for the fix; don't copy this pattern.

**How do I know if a field I'm looking at is pounds or milliunits?**
Check the type it's declared on. Anything on a `*Response`/`*Request`
DTO (`Responses/`, `Requests/`) is raw milliunits, matching the wire
format exactly. Anything on a domain type (`Account`, `Category`,
`SplitTransactions`, etc., in the `YnabSharp` root namespace) should
already be pounds — if you find one that isn't (see `Category.GoalTarget`,
which is left unconverted while its sibling goal fields are converted),
that's a bug, not a convention to follow.

**What about currencies that don't use decimal subdivisions, or budgets in a different currency?**
`MilliunitConverter`'s math is currency-agnostic — it's always "divide/
multiply by 1000," regardless of what currency the budget is actually
denominated in. The `Pounds` in the method names is a naming artifact,
not a GBP-only assumption baked into the logic.
