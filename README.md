# YnabSharp

A .NET client library for the [YNAB API](https://api.ynab.com) — typed
domain models over YNAB's budgets, accounts, categories, and
transactions, with currency handled as `decimal` pounds instead of raw
milliunit integers.

## Install

```
dotnet add package YnabSharp
```

## Quick start

```csharp
using Microsoft.Extensions.DependencyInjection;
using YnabSharp.Clients;
using YnabSharp.Extensions;
using YnabSharp.Http;

var services = new ServiceCollection()
    .AddYnab()
    .BuildServiceProvider();

var httpClientBuilder = services
    .GetRequiredService<YnabHttpClientBuilder>()
    .WithBearerToken(apiKey); // a YNAB personal access token

var budgetsClient = new BudgetsClient(httpClientBuilder);

var budget = await budgetsClient.GetBudget("My Budget"); // or GetBudgets() for all of them
var transactions = await budget.GetTransactions();

foreach (var transaction in transactions)
    Console.WriteLine($"{transaction.Occured:d}  {transaction.PayeeName}  {transaction.Amount:C}");
```

`transaction.Amount` above is already a `decimal` in pounds — YNAB's
API represents money as integer milliunits on the wire, converted
automatically at the boundary. See
[`docs/concepts/milliunit-currency-conversion.md`](docs/concepts/milliunit-currency-conversion.md)
before writing any code that reads or writes an amount directly.

## Learn more

- [`CONTRIBUTING.md`](CONTRIBUTING.md) — conventions, branching, how to
  propose a change. Read this before touching anything that moves real
  money.
- [`docs/concepts/`](docs/concepts/) — how each subsystem actually
  works today: [milliunit conversion](docs/concepts/milliunit-currency-conversion.md),
  [split transactions](docs/concepts/split-transactions.md),
  [connected domain objects](docs/concepts/connected-domain-objects.md)
  (the `ConnectedBudget`/`ConnectedAccount` pattern used above).
- [`docs/adr/`](docs/adr/) — architectural decisions and why.
- [`docs/reviews/`](docs/reviews/) — past architectural reviews.
- [`docs/ynab-api-coverage.md`](docs/ynab-api-coverage.md) — what this
  library actually covers against the full YNAB API, and what's
  missing.

## YnabSharp.Seeder

A separate, **unpublished** internal tool that writes real data to a
real YNAB budget via the live API — there is no sandbox mode. Not
something a consumer of the `YnabSharp` package needs; see
[`CONTRIBUTING.md`](CONTRIBUTING.md#a-note-on-the-seeder) if you're
working on it directly.
