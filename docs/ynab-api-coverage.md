# YNAB API coverage

What YnabSharp actually implements, against the full YNAB API surface.
This is a living reference, not a point-in-time snapshot — update it
whenever `docs/ynab-openapi-spec.yaml` is re-synced (see
[`.github/workflows/ynab-spec-drift-check.yml`](../.github/workflows/ynab-spec-drift-check.yml),
which flags when the two have drifted, but doesn't update either file
itself).

Vendored spec version: **1.86.0**. Last checked: **2026-07-26**.

## Coverage by resource

✅ = fully covered · ⚠️ = partial · ❌ = not implemented

| Resource | Spec has | YnabSharp has | |
|---|---|---|---|
| User | `GET /user` | nothing | ❌ |
| Plans | list, get, settings | list, get | ⚠️ |
| Accounts | list, create, get | list, create, get | ✅ |
| Categories | list, create, get, update, month-get, month-update, group-create, group-update | list only | ⚠️ |
| Payees | list, create, get, update | list, get | ⚠️ |
| Payee Locations | list, get, by-payee | nothing | ❌ |
| Months | list, get | nothing | ❌ |
| Money Movements | list, by-month, groups, groups-by-month | nothing (new resource, unevaluated) | ❌ |
| Transactions | list, create, bulk-update, import, get, update, delete, by-account/category/payee/month | list, get, create, bulk-update | ⚠️ |
| Scheduled Transactions | list, create, get, update, delete | list, update | ⚠️ |

Every gap above with a `⚠️`/`❌` and real user-facing value has its own
tracked issue — see the issue tracker rather than duplicating specifics
here. This table is for "what's the shape of coverage," not "here's the
backlog."

## What the API cannot do

Distinct from the table above: these aren't gaps in YnabSharp's coverage,
they're ceilings in the YNAB API itself. No amount of implementing will
close them, so they're recorded here rather than raised as issues.

| Limit | Where it shows |
|---|---|
| **No delete for categories or category groups.** The spec has exactly two `delete` operations, both on Transactions (`deleteTransaction`, `deleteScheduledTransaction`). | Anything that consolidates category structure can create and re-point, but can't remove what it replaced. |
| **`hidden` is readable but not writable.** It's a required property on both `CategoryBase` and the category-group schema, but absent from `SaveCategory` — so it can be read back and never set. | Rules out hiding a retired category as a substitute for deleting it. |
| **Money Movements is read-only.** All four operations are `get*`; there is no create, update or delete. | Money already assigned to a category can't be moved between categories through the API. |

Consequence worth stating plainly, because it's the one that bites: a
**merge** — fold category A into category B, then retire A — is not
fully achievable through this API. Creating B and re-pointing
transactions is; moving A's assigned money and then removing A is not.
Callers have to leave the emptied category in place and finish by hand.

## How to re-check

```
curl -s https://api.ynab.com/papi/open_api_spec.yaml -o /tmp/live-spec.yaml
diff docs/ynab-openapi-spec.yaml /tmp/live-spec.yaml
```

The scheduled workflow does this automatically and opens an issue if
they differ — this is the manual equivalent, for whenever you want to
check without waiting for the schedule.

When resolving a drift issue: re-vendor the spec, update the version/
date above and the coverage table if anything relevant changed, and add
a line below. The diff itself tells you *that* something changed; this
log is for the human judgment call on whether it *matters*.

## Sync history

| Date | Spec version | What changed |
|---|---|---|
| 2026-07-26 | — → 1.86.0 | Initial vendoring. Discovered the `Budgets` → `Plans` rename (see above) and the `Money Movements` resource while comparing against YnabSharp's actual coverage — neither is otherwise recorded anywhere. |
