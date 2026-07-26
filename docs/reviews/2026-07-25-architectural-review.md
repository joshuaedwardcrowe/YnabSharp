# Architectural review — 2026-07-25

## Summary

- **47 findings** filed as GitHub issues: **9 high**, **21 medium** (1
  closed immediately as already-resolved, see below), **17 low**.
- Every finding carries all three label axes (type/area/severity) per
  [`CONTRIBUTING.md`](../../CONTRIBUTING.md) — the full list is always
  queryable directly from GitHub:
  [findings from this review](https://github.com/joshuaedwardcrowe/YnabSharp/issues?q=is%3Aissue+created%3A2026-07-25).
- This review is what the [`CONTRIBUTING.md`](../../CONTRIBUTING.md)
  operating model, [ADRs](../adr/), and [concept docs](../concepts/) in
  this repo were built in direct response to.

## Methodology

Findings came from parallel review passes across independent dimensions
(HTTP/auth layer, currency-conversion correctness, domain model shape,
Seeder write-path safety, test coverage), rather than one linear
read-through — the same shape as the `repo-operating-model` skill
(`.claude/skills/repo-operating-model/`) now codifies. Every
**high**-severity finding was personally re-verified against the actual
source before being filed, with extra scrutiny on anything touching
currency conversion or the Seeder's write path — this library moves
real money, so a wrong finding here isn't just embarrassing, it's
actively dangerous to trust.

## Findings

<!-- generated from `gh issue list` at review-doc write time — see the
live issue list linked above for current status, since issues can be
closed/re-triaged after this doc is written. -->

### High

| # | Finding | Type | Area |
|---|---|---|---|
| [#2](https://github.com/joshuaedwardcrowe/YnabSharp/issues/2) | Creating an account inflates the starting balance ~1000x — pounds→milliunits conversion applied twice | bug | domain |
| [#3](https://github.com/joshuaedwardcrowe/YnabSharp/issues/3) | YnabHttpClientBuilder is a DI singleton with a mutable bearer-token field — credential-bleed risk | bug | client |
| [#4](https://github.com/joshuaedwardcrowe/YnabSharp/issues/4) | BudgetsClient.GetBudget(Guid) constructs a broken, duplicated URL | bug | client |
| [#5](https://github.com/joshuaedwardcrowe/YnabSharp/issues/5) | YnabException discards the HTTP status code entirely | bug | client |
| [#6](https://github.com/joshuaedwardcrowe/YnabSharp/issues/6) | Network failures and non-JSON error bodies bypass YnabException entirely | bug | client |
| [#7](https://github.com/joshuaedwardcrowe/YnabSharp/issues/7) | PayeeName/CategoryName are typed non-nullable but are legitimately null on ordinary transactions | bug | domain |
| [#8](https://github.com/joshuaedwardcrowe/YnabSharp/issues/8) | No working safety net before the Seeder writes real, irreversible data to a real YNAB budget | bug | seeder |
| [#9](https://github.com/joshuaedwardcrowe/YnabSharp/issues/9) | A partial failure mid-seed strands real, untracked accounts with no rollback | bug | seeder |
| [#10](https://github.com/joshuaedwardcrowe/YnabSharp/issues/10) | Seeder step ordering is inverted from what the folder structure promises | tech-debt | seeder |

### Medium

| # | Finding | Type | Area |
|---|---|---|---|
| [#11](https://github.com/joshuaedwardcrowe/YnabSharp/issues/11) | PoundsToMilliunit truncates instead of rounding, and the outbound conversion has zero tests | bug | domain |
| [#12](https://github.com/joshuaedwardcrowe/YnabSharp/issues/12) | Budget.GetYears().All silently omits the budget's most recent active year | bug | domain |
| [#13](https://github.com/joshuaedwardcrowe/YnabSharp/issues/13) | GroupByFlags() filters on FlagName, silently dropping color-only flagged transactions | bug | domain |
| [#14](https://github.com/joshuaedwardcrowe/YnabSharp/issues/14) | AccountType/FlagColor strict enum deserialization breaks the entire response on any unrecognized value | bug | domain |
| [#15](https://github.com/joshuaedwardcrowe/YnabSharp/issues/15) | Category.GoalTarget left in raw milliunits while sibling goal fields are converted to pounds | bug | domain |
| [#16](https://github.com/joshuaedwardcrowe/YnabSharp/issues/16) | Category domain type omits Budgeted/Available amounts entirely | feature | domain |
| [#17](https://github.com/joshuaedwardcrowe/YnabSharp/issues/17) | CategoryGoal.cs is dead, duplicate, and unit-inconsistent code | tech-debt | domain |
| [#18](https://github.com/joshuaedwardcrowe/YnabSharp/issues/18) | Record types wrapping IEnumerable&lt;T&gt; get illusory value equality | tech-debt | domain |
| [#19](https://github.com/joshuaedwardcrowe/YnabSharp/issues/19) | Transaction dates are DateTime while the rest of the codebase correctly uses DateOnly | tech-debt | domain |
| [#20](https://github.com/joshuaedwardcrowe/YnabSharp/issues/20) | Month grouping keys depend on ambient thread culture and don't sort chronologically | bug | domain |
| [#21](https://github.com/joshuaedwardcrowe/YnabSharp/issues/21) | Category-group filters hardcode one personal budget's category names | tech-debt | domain |
| [#22](https://github.com/joshuaedwardcrowe/YnabSharp/issues/22) | NewAccount breaks the immutable-projection pattern and can't represent fractional balances | tech-debt | domain |
| [#23](https://github.com/joshuaedwardcrowe/YnabSharp/issues/23) | No CancellationToken support anywhere in the HTTP layer | tech-debt | client |
| [#24](https://github.com/joshuaedwardcrowe/YnabSharp/issues/24) | No handling for YNAB's rate limiting (429, 200 requests/hour) | bug | client |
| [#25](https://github.com/joshuaedwardcrowe/YnabSharp/issues/25) | Path segments are unescaped, stringly-typed interpolation with no validation | tech-debt | client |
| [#26](https://github.com/joshuaedwardcrowe/YnabSharp/issues/26) | No interfaces on any client class — hard for consumers to mock | tech-debt | client |
| [#27](https://github.com/joshuaedwardcrowe/YnabSharp/issues/27) | Zero test coverage across everything that touches money and the network | tech-debt | tooling |
| ~~[#28](https://github.com/joshuaedwardcrowe/YnabSharp/issues/28)~~ | ~~No CI whatsoever~~ — **closed at filing time**, resolved by the CI workflow added in the same session's operating-model PR before this issue needed triage | tech-debt | tooling |
| [#29](https://github.com/joshuaedwardcrowe/YnabSharp/issues/29) | The published NuGet package carries a CLI-framework dependency nothing in the library uses | tech-debt | client |
| [#30](https://github.com/joshuaedwardcrowe/YnabSharp/issues/30) | Seeded transactions are never linked to real categories; account type never varies (verified) | bug | seeder |
| [#31](https://github.com/joshuaedwardcrowe/YnabSharp/issues/31) | KitCli version skew across the three projects | tech-debt | tooling |

### Low

| # | Finding | Type | Area |
|---|---|---|---|
| [#32](https://github.com/joshuaedwardcrowe/YnabSharp/issues/32) | TransactionFlowSanitiser is unused dead code | tech-debt | domain |
| [#33](https://github.com/joshuaedwardcrowe/YnabSharp/issues/33) | Mixed record/mutable-class style within Collections/ with no apparent rule | tech-debt | domain |
| [#34](https://github.com/joshuaedwardcrowe/YnabSharp/issues/34) | Currency-specific naming (Pounds) on math that's actually currency-agnostic | tech-debt | domain |
| [#35](https://github.com/joshuaedwardcrowe/YnabSharp/issues/35) | FilterTo's local variable is misleadingly named dateFrom | tech-debt | domain |
| [#36](https://github.com/joshuaedwardcrowe/YnabSharp/issues/36) | The transaction-factory strategy pattern is self-documented as incomplete | tech-debt | domain |
| [#37](https://github.com/joshuaedwardcrowe/YnabSharp/issues/37) | SplitTransactions.IsFullyFormed treats the optional Memo field as required | bug | domain |
| [#38](https://github.com/joshuaedwardcrowe/YnabSharp/issues/38) | Domain wrapper classes have no value equality, inconsistent with the record-based collections they compose with | tech-debt | domain |
| [#39](https://github.com/joshuaedwardcrowe/YnabSharp/issues/39) | Client constructor style has drifted between primary constructors and classic explicit constructors | tech-debt | client |
| [#40](https://github.com/joshuaedwardcrowe/YnabSharp/issues/40) | JsonSerializerOptions rebuilt per client instance instead of shared | tech-debt | client |
| [#41](https://github.com/joshuaedwardcrowe/YnabSharp/issues/41) | HttpResponseMessage is never disposed | tech-debt | client |
| [#42](https://github.com/joshuaedwardcrowe/YnabSharp/issues/42) | Two DTOs skip the required modifier their siblings all use (compiler-confirmed) | bug | client |
| [#43](https://github.com/joshuaedwardcrowe/YnabSharp/issues/43) | Naming typo: BuilUriPath is missing a 'd' | tech-debt | client |
| [#44](https://github.com/joshuaedwardcrowe/YnabSharp/issues/44) | README is a two-line placeholder | docs | tooling |
| [#45](https://github.com/joshuaedwardcrowe/YnabSharp/issues/45) | Published NuGet package description is the unedited placeholder | docs | tooling |
| [#46](https://github.com/joshuaedwardcrowe/YnabSharp/issues/46) | No publish/release automation or documented process | tech-debt | tooling |
| [#47](https://github.com/joshuaedwardcrowe/YnabSharp/issues/47) | Aspirational, empty ADR solution folder | tech-debt | tooling |
| [#48](https://github.com/joshuaedwardcrowe/YnabSharp/issues/48) | Unaddressed compiler warnings in the Seeder | tech-debt | seeder |

## Next review

Gated on ≥1,000 lines of `.cs` source changed **and** ≥6 months
elapsed since this review — see
[`.claude/skills/repo-operating-model/SKILL.md`](../../.claude/skills/repo-operating-model/SKILL.md#1-check-whether-a-full-review-is-actually-due).
