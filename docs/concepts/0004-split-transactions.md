# 0004. Split transactions

## Premise

A YNAB transaction can be split across multiple categories — one card
swipe at a store categorized as "Groceries: £30, Household: £20." This
library models that with two closely related, easily confused types:
`SplitTransactions` (a single line item) and `Transaction` (the whole
transaction — which is *itself* a `SplitTransactions` too).

## Problem

`Transaction` needs to represent both "the transaction as a whole" (its
own id, amount, payee, category, memo — the same shape as an unsplit
transaction) and, when split, its child line items, which have exactly
that same shape. Modelling that needs either two unrelated types with
duplicated fields, or a shared shape one extends. Separately: because
the write path was built around whole transactions, it's not obvious
from the domain types alone whether creating a transaction with splits
through this library actually sends the split data anywhere.

## Solution

The same inheritance shape repeats on both sides of the API boundary:

**Wire (`Responses/Transactions/`):**
`SplitTransactionResponse` is the DTO for one line item — `id`, `memo`,
`date`, `amount` (milliunits), `payee_id`/`payee_name`,
`category_id`/`category_name`, `transfer_transaction_id`, `account_id`.
`TransactionResponse : SplitTransactionResponse` adds `flag_color`,
`flag_name`, and `subtransactions` (an `IEnumerable<SplitTransactionResponse>`)
— a whole transaction's wire shape *is* a line item's shape, plus flags,
plus its own children.

**Domain (`SplitTransactions.cs`, `Transaction.cs`):**
`SplitTransactions` wraps a `SplitTransactionResponse` and exposes the
converted (milliunit → pounds, via
[`MilliunitConverter`](0003-milliunit-currency-conversion.md)) view of one
line item: `Id`, `Occured`, `Memo`, `Amount`, `PayeeId`/`PayeeName`,
`CategoryId`/`CategoryName`, `IsTransfer`, `AccountId`, plus
`IsFullyFormed` (has a payee, category, and memo) and
`InCategories(categoryIds)`.

```csharp
public class SplitTransactions(SplitTransactionResponse splitTransactionResponse)
{
    public decimal Amount => MilliunitConverter.Calculate(splitTransactionResponse.Amount);
    public bool IsFullyFormed => this is { PayeeId: not null, CategoryId: not null, Memo: not null };
    // ...
}
```

`Transaction : SplitTransactions` adds `FlagName`/`FlagColour` and its
own `SplitTransactions` property — the children, each rebuilt with
`Occured` forced to the parent's own date, because subtransactions never
carry a `date` on the wire:

```csharp
public class Transaction(TransactionResponse transactionResponse) : SplitTransactions(transactionResponse)
{
    public IEnumerable<SplitTransactions> SplitTransactions
        => transactionResponse.SplitTransactions
            .Select(splitTransactionResponse => new SplitTransactions(splitTransactionResponse with
            {
                Occured = transactionResponse.Occured // Splits do not have Occured set.
            }));
}
```

So: **a `Transaction` *is* a `SplitTransactions`** — same shape as a
single line item — **and separately *has* a collection of
`SplitTransactions`**, its own children, if split. Same type name, two
different roles depending on whether you're looking at a `Transaction`
itself or one of the entries inside its `.SplitTransactions` property.
This is flagged in-code on the class itself
(`// TODO: Hate that this is plural...`) — it's a known naming wart, not
an intentional pun.

`SplitTransactionExtensions.AllFullyFormed(this IEnumerable<SplitTransactions>)`
is the one helper built on top of this shape: it checks that every line
item in a sequence — parent or child — has a payee, category, and memo
set.

## Constraints & tradeoffs

**Splits are read-only today.** `TransactionClient.Create`/`Move` map a
`Transaction` to a `TransactionRequest` via
`Mappers/TransactionRequestMapping.cs` — and `TransactionRequest` has no
field for subtransactions at all. Any splits present on a `Transaction`
you construct are silently dropped when you call `Create`; there is no
supported path in this library today for pushing split data to YNAB.
Don't assume `Create` round-trips a split transaction — verify against
the response you get back, or check for a tracked issue before relying
on it.

**Reusing one shape for both "the transaction" and "a line item"** keeps
the field list from being duplicated across two otherwise-identical
types, at the cost of the double-meaning above, and of every consumer
needing to remember that `.SplitTransactions` may just be empty (an
unsplit transaction is not a special case, it's a `Transaction` with no
children).

## Questions & answers

**How do I check if a transaction is split?**
`transaction.SplitTransactions.Any()` — an empty collection means it
isn't split.

**Can I create a transaction with splits through this library?**
Not currently supported end-to-end — see the constraint above. The
domain model can represent a split transaction; the write path doesn't
carry that data to the API.

**Why does the child's `Occured` get overwritten in `Transaction.SplitTransactions`?**
Because YNAB's wire format never sets `date` on a subtransaction — it
implicitly inherits the parent transaction's date. The domain layer
makes that explicit by copying `transactionResponse.Occured` onto each
child at construction time, rather than leaving it at its default value.

**Is `IsFullyFormed` enforced anywhere, or just advisory?**
Advisory. It's exposed as data for a caller to check (directly, or via
`AllFullyFormed`) — nothing in the library itself stops you constructing
or using a `Transaction` whose splits aren't fully formed.
