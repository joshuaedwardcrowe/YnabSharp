# YnabSharp

A .NET client library for the [YNAB API](https://api.ynab.com) — typed
domain models over YNAB's plans, accounts, categories, and
transactions, with currency handled as `decimal` pounds instead of raw
milliunit integers.

## Install

```
dotnet add package YnabSharp
```

## Quick start

```csharp
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using YnabSharp.Clients;
using YnabSharp.Extensions;
using YnabSharp.Http;

var services = new ServiceCollection()
    .AddYnab()
    .BuildServiceProvider();

var httpClientBuilder = services
    .GetRequiredService<YnabHttpClientBuilder>()
    .WithBearerToken(apiKey); // a YNAB personal access token

var plansClient = new PlansClient(httpClientBuilder);

var plan = await plansClient.GetPlan("My Plan"); // or GetPlans() for all of them
var transactions = await plan.GetTransactions();

foreach (var transaction in transactions)
    Console.WriteLine($"{transaction.Occured:d}  {transaction.PayeeName}  {transaction.Amount:C}");

// plan/account/transaction objects chain — each one already carries
// what it needs to go a level deeper, no re-plumbing required.
var accounts = await plan.GetAccounts();
var account = await plan.GetAccount(accounts.First().Id); // now a ConnectedAccount
var accountTransactions = await account.GetTransactions();  // that account's transactions only
```

`transaction.Amount` above is already a `decimal` in pounds — YNAB's
API represents money as integer milliunits on the wire, converted
automatically at the boundary. See
[`docs/concepts/milliunit-currency-conversion.md`](docs/concepts/milliunit-currency-conversion.md)
before writing any code that reads or writes an amount directly, and
[`docs/concepts/connected-domain-objects.md`](docs/concepts/connected-domain-objects.md)
for more on the chaining above.

## Learn more

- [`CONTRIBUTING.md`](CONTRIBUTING.md) — conventions, branching, how to
  propose a change. Read this before touching anything that moves real
  money.
- [`docs/concepts/`](docs/concepts/) — how each subsystem actually
  works today: [milliunit conversion](docs/concepts/milliunit-currency-conversion.md),
  [split transactions](docs/concepts/split-transactions.md),
  [connected domain objects](docs/concepts/connected-domain-objects.md)
  (the `ConnectedPlan`/`ConnectedAccount` pattern used above).
- [`docs/adr/`](docs/adr/) — architectural decisions and why.
- [`docs/reviews/`](docs/reviews/) — past architectural reviews.
- [`docs/ynab-api-coverage.md`](docs/ynab-api-coverage.md) — what this
  library actually covers against the full YNAB API, and what's
  missing.

## YnabSharp.Seeder

A separate, **unpublished** internal tool that writes real data to a
real YNAB plan via the live API — there is no sandbox mode. Not
something a consumer of the `YnabSharp` package needs; see
[`CONTRIBUTING.md`](CONTRIBUTING.md#a-note-on-the-seeder) if you're
working on it directly.
