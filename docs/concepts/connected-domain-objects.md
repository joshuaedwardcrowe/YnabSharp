# Connected domain objects

## Premise

Most response-wrapping domain types in this library (`Category`,
`CategoryGroup`, `ScheduledTransaction`, ...) are pure read views over a
response DTO: construct one, read its properties, done. `Budget` and
`Account` are the two exceptions — `ConnectedBudget : Budget` and
`ConnectedAccount : Account` (`Connected/`) add live behaviour (fetch
this budget's transactions, create an account, move transactions
between two accounts) by holding onto the actual API clients needed to
do it themselves.

## Problem

Fetching, say, "this budget's categories" needs a `CategoryClient`
scoped to the right budget path, built with the same shared
bearer-token `HttpClient` builder every other client uses. Without
something assembling that once, per budget, every caller would have to
reconstruct that wiring by hand each time they wanted a related object
— "get this budget's categories" would otherwise mean manually building
a `CategoryClient` with the correct path segment themselves.

## Solution

**Entry point:** `BudgetsClient.GetBudgets()`/`GetBudget(...)` calls the
top-level `/budgets` endpoint, then for *each* `BudgetResponse` manually
constructs the four budget-scoped clients a `ConnectedBudget` needs —
`AccountClient`, `CategoryClient`, `TransactionClient`,
`ScheduledTransactionClient` — all built with the same
`ynabBudgetApiPath` (`"budgets/{budgetId}"`) and the same
`YnabHttpClientBuilder`/`ITransactionFactory` list `BudgetsClient` itself
holds:

```csharp
private IEnumerable<ConnectedBudget> ConvertBudgetResponsesToWrappers(IEnumerable<BudgetResponse> budgetResponses)
{
    foreach (var budgetResponse in budgetResponses)
    {
        var ynabBudgetApiPath = $"{YnabApiPath.Budgets}/{budgetResponse.Id}";

        var accountClient = new AccountClient(_httpClientBuilder, ynabBudgetApiPath, _transactionFactories);
        var categoryClient = new CategoryClient(_httpClientBuilder, ynabBudgetApiPath);
        var transactionClient = new TransactionClient(_httpClientBuilder, ynabBudgetApiPath, _transactionFactories);
        var scheduledTransactionClient = new ScheduledTransactionClient(_httpClientBuilder, ynabBudgetApiPath);

        yield return new ConnectedBudget(accountClient, categoryClient, transactionClient, scheduledTransactionClient, budgetResponse);
    }
}
```

`ConnectedBudget` is a `Budget` (same read-only properties) that also
exposes `GetAccounts()`, `GetAccount(id)`, `GetCategoryGroups()`,
`GetTransactions()`, `GetTransaction(id)`, `CreateTransactions(...)`,
`CreateAccount(...)`, and `MoveAccountTransactions(from, to)` — each just
delegating to the client it was handed at construction.

The same pattern repeats one level down. `AccountClient.Get`/`Create`
don't return a plain `Account` — they return a `ConnectedAccount`, and
to build one, `AccountClient` itself constructs a `TransactionClient` and
`ScheduledTransactionClient` (again from the same path/factories it
holds) and hands them to `new ConnectedAccount(...)`.
`ConnectedAccount` then exposes `GetTransactions()`/
`GetScheduledTransactions()`, filtered down to just that account's `Id`.

### What this looks like from the outside

The point of building the object graph this way is that a consumer
never touches `AccountClient`, `TransactionClient`, or a budget/account
path segment directly — each object handed back already carries what it
needs to go one level deeper:

```csharp
var budgetsClient = new BudgetsClient(httpClientBuilder);

var budget = await budgetsClient.GetBudget("My Budget"); // ConnectedBudget
var account = await budget.GetAccount(accountId);        // ConnectedAccount
var transactions = await account.GetTransactions();      // that account's transactions only
```

Three calls, three different clients doing the actual HTTP work behind
the scenes (`BudgetsClient` → `AccountClient` → `TransactionClient`),
and the caller never constructs any of them or repeats a budget/account
ID — each step's return value *is* the next step's starting point.

None of this wiring goes through the DI container at the point of use.
`AddYnab()` (`Extensions/ServiceCollectionExtensions.cs`) only registers
what's genuinely a singleton for the app's lifetime — `YnabHttpClientBuilder`
and the `ITransactionFactory` chain. Everything scoped to one specific
budget or account is constructed by hand, by the client one level up,
because its constructor arguments (the budget- or account-specific path
segment) aren't known until a specific response comes back over the
wire.

`ConnectedBudget.MoveAccountTransactions` is a good example of what this
buys you: it composes calls across three of its held clients
(`GetTransactions()`/`GetScheduledTransactions()` on the *source*
`ConnectedAccount`, then `MoveTransaction(...)`/`Move(...)` on its own
`ScheduledTransactionClient`/`TransactionClient`) and applies one
business rule while doing it — it filters out any transaction whose
payee is `AutomatedPayeeNames.StartingBalance` before moving one, since a
starting-balance entry shouldn't follow the funds to a new account.

## Constraints & tradeoffs

**Manual object-graph construction instead of DI resolution for
anything budget/account-scoped.** Keeps the container simple — only
genuine app-lifetime singletons are registered — at the cost of every
budget-scoped client needing to carry exactly the constructor parameters
(`ynabBudgetApiPath`, the `ITransactionFactory` list) required to build
its own children by hand, and of that wiring being duplicated between
`BudgetsClient.ConvertBudgetResponsesToWrappers` and
`AccountClient.ConvertAccountResponseToConnectedAccount` rather than
centralised in one place.

**`Connected*` types inherit from their plain counterpart, rather than
wrapping it.** A `ConnectedAccount` "is a" `Account`, so anywhere an
`Account` is expected a `ConnectedAccount` also works — but not the
reverse. A plain `Account` returned from `ConnectedBudget.GetAccounts()`
(the bulk list call) can't make further calls itself; only the
single-item `GetAccount(id)` returns a `ConnectedAccount`. A caller has
to know which method they called to know whether the object in hand can
act on its own behalf.

## Questions & answers

**Why does `GetAccounts()` return `Account`, but `GetAccount(id)` returns `ConnectedAccount`?**
`AccountClient.GetAll()` maps straight from the bulk response to
`new Account(a)`, without building the per-account clients a
`ConnectedAccount` needs — only the single-item lookup (`Get`/`Create`)
goes through `ConvertAccountResponseToConnectedAccount`. If you need to
act on an account you got from `GetAccounts()`, fetch it again via
`GetAccount(id)`.

**Is a `ConnectedBudget`/`ConnectedAccount` safe to hold onto and reuse?**
Its held clients don't carry any per-request state beyond the fixed
budget/account path and the shared `YnabHttpClientBuilder`, so reuse is
fine — but note `YnabHttpClientBuilder.Build()` calls
`IHttpClientFactory.CreateClient()` fresh on every single call, so
there's no connection-reuse benefit to holding onto one either way.

**Where do I add a new budget-scoped operation?**
Add the method to the relevant `*Client` first, then expose it on
`ConnectedBudget`/`ConnectedAccount` as a thin delegation — following the
existing pattern (e.g. `GetCategoryGroups()`), rather than putting logic
directly on the `Connected*` type itself.
