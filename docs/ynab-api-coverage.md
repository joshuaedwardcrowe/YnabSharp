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
| Plans (Budgets) | list, get, settings | list, get | ⚠️ |
| Accounts | list, create, get | list, create, get | ✅ |
| Categories | list, get, update, month-get, month-update, group-create, group-update | list only | ⚠️ |
| Payees | list, create, get, update | nothing | ❌ |
| Payee Locations | list, get, by-payee | nothing | ❌ |
| Months | list, get | nothing | ❌ |
| Money Movements | list, by-month, groups | nothing (new resource, unevaluated) | ❌ |
| Transactions | list, create, bulk-update, import, get, update, delete, by-account/category/payee/month | list, get, create, bulk-update | ⚠️ |
| Scheduled Transactions | list, create, get, update, delete | list, update | ⚠️ |

Every gap above with a `⚠️`/`❌` and real user-facing value has its own
tracked issue — see the issue tracker rather than duplicating specifics
here. This table is for "what's the shape of coverage," not "here's the
backlog."

## Known naming drift

The API's top-level resource was renamed `Budgets` → `Plans` at some
point after YnabSharp was built (`YnabApiPath.Budgets = "budgets"`,
`BudgetsClient`, `Budget`, `ConnectedBudget` all still say "Budgets").
Verified live: both `/v1/budgets` and `/v1/plans` return 401 (not 404)
unauthenticated, so the old name still works as an alias — this isn't
broken today, just worth knowing before assuming the two names are
interchangeable forever.

## How to re-check

```
curl -s https://api.ynab.com/papi/open_api_spec.yaml -o /tmp/live-spec.yaml
diff docs/ynab-openapi-spec.yaml /tmp/live-spec.yaml
```

The scheduled workflow does this automatically and opens an issue if
they differ — this is the manual equivalent, for whenever you want to
check without waiting for the schedule.
