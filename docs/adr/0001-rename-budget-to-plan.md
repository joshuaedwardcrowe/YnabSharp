# 0001. Rename Budget to Plan throughout the public API

Status: Accepted
Date: 2026-07-26

## Context

YNAB's API renamed its top-level resource from `Budgets` to `Plans`.
The vendored OpenAPI spec (v1.86.0) has no `Budget` schema or `/budgets/`
path left at all — every path is `/plans/{plan_id}/...`, schemas are
`PlanSummary`/`PlanDetail`, and the list/single response wrapper keys are
`plans`/`plan`. `/budgets/` still resolves today only as a
backward-compatible alias. Separately, YNAB renamed their own Budget tab
to "Plan" in the live app
([announcement](https://www.ynab.com/blog/budget-tab-breakdown)), so
this isn't just wire vocabulary drifting from the product — the product
itself uses "Plan" now too.

YnabSharp's public API (`Budget`, `BudgetsClient`, `ConnectedBudget`,
`BudgetYears`, and the `YnabSharp.Responses.Budgets` namespace) still
says "Budget" throughout. Tracked as issue #51. `BudgetsClient.GetBudget(Guid)`
also has a pre-existing duplicated-URL-segment bug (issue #4) on the
same line being touched by this rename.

## Decision

Rename the public API from `Budget` vocabulary to `Plan` vocabulary:
`Budget` → `Plan`, `BudgetYears` → `PlanYears`, `ConnectedBudget` →
`ConnectedPlan`, `BudgetsClient` → `PlansClient` (`GetBudgets()` →
`GetPlans()`, `GetBudget(...)` → `GetPlan(...)`), and the
`YnabSharp.Responses.Budgets` namespace/DTOs → `YnabSharp.Responses.Plans`
(`BudgetResponse` → `PlanResponse`, wrapper JSON keys `budgets`/`budget`
→ `plans`/`plan`). The `YnabApiPath.Budgets` wire constant becomes
`YnabApiPath.Plans = "plans"`. This closes #51 outright rather than
leaving the library's public vocabulary permanently out of step with
the API and product it wraps. The `GetBudget(Guid)` duplicated-segment
bug (#4) is fixed in the same change, since it's the same line.

Package ID and repository name (`YnabSharp`) are unaffected — only
in-library type names change.

## Alternatives considered

**Wire-level fix only** — keep the public C# names (`Budget`,
`BudgetsClient`, `ConnectedBudget`) and only repoint the internal HTTP
path/JSON keys at `/plans/`. This was the initial plan, since it avoids
a breaking change on a package with a stable version number. Rejected:
it leaves the public API's vocabulary permanently diverged from both
the API and the product it wraps, which only gets more confusing (and
more expensive to fix) as more consumers show up. If a breaking change
is warranted, better to take it now than defer it indefinitely.

**Keep both names via aliases/back-compat shims** — e.g. keep `Budget`
as an obsolete type alias for `Plan`. Rejected: doubles the public
surface for a rename that's a straightforward mechanical migration for
consumers, and this repo already avoids compatibility shims as a matter
of course.

## Consequences

This is a breaking change to the public API shape. No version of
`YnabSharp` has actually been published yet (no git tags, no GitHub
Releases exist despite `YnabSharp.csproj` currently reading `1.1.0`),
so nothing real breaks today — but per `CONTRIBUTING.md`'s "standard
semver" policy, whenever this library is first published, that release
owes a major version. `YnabSharp.Seeder` (not published, but shares the
solution) is updated in the same change so the solution keeps
compiling; it isn't a separate compatibility concern since it never
ships independently.

Anyone reading old issues, PRs, or docs that reference `Budget`-named
types should understand those refer to what is now `Plan`.
