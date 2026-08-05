# 0001. Can Claude connect to a YNAB budget via an MCP server?

- **Status:** In Review
- **Spike:** #151
- **Time-box:** not formally set
- **Date:** 2026-08-05

## Verdict

New complexity. Feasible as a feature within YnabSharp, but blocked on
two missing API endpoints and requiring a write-safety pattern this repo
hasn't needed before.

## Recommendation

1. #98 — Categories month-get
2. #86, #87 — Months list/get-by-month
3. New ADR — write-access model (tool annotations + `ElicitAsync`-gated
   writes)
4. `YnabSharp.Mcp` scaffold — read-only tools first
5. Tool packaging + release workflow (`dotnet tool`, separate from the
   library's)
6. Docs/quick-start
7. Gated write tool(s), once the ADR lands

## What was established

**Blocking dependency.** Conversational balance queries ("how much do I
have left in groceries") need Categories month-get and the Months
resource. Neither exists in YnabSharp today.

**MCP has two distinct write-safety mechanisms.** *Tool annotations*
(`ReadOnly`/`Destructive`/`Idempotent`/`OpenWorld`, set via
`McpServerToolCreateOptions` in the C# SDK) are hints to the host — the
spec calls them untrusted unless the server is trusted, not a safety
boundary. *Elicitation* (`elicitation/create`, C# `server.ElicitAsync(...)`,
accept/decline/cancel) is server-controlled and real. Decision: annotate
every tool, but gate every write on `ElicitAsync` — don't rely on host
trust alone.

**Local stdio hosting needs no server infra.**
`AddMcpServer().WithStdioServerTransport().WithTools<T>()` (package:
`ModelContextProtocol`).

**Distribution should be a `dotnet tool`, not `dotnet run`.** For "anyone
gets running quickly," pack `YnabSharp.Mcp` as `PackAsTool=true` via
NuGet, installed with `dotnet tool install -g`. Needs its own
version/release workflow — same precedent as `YnabSharp.Seeder`'s
version not tracking the library's.

## Evidence

MCP protocol spec:
[elicitation](https://modelcontextprotocol.io/specification/2025-06-18/client/elicitation),
[tools](https://modelcontextprotocol.io/specification/2025-06-18/server/tools)
— the "untrusted unless trusted server" annotation warning and the
accept/decline/cancel elicitation flow are both quoted directly from
these pages.

C# SDK (`modelcontextprotocol/csharp-sdk`):
- `docs/concepts/elicitation/elicitation.md` — `server.ElicitAsync(...)`
  signature and write-confirmation pattern.
- `src/ModelContextProtocol.Core/Server/McpServerToolCreateOptions.cs` —
  `ReadOnly`/`Destructive`/`Idempotent`/`OpenWorld` properties.
- `samples/QuickstartWeatherServer/Program.cs` —
  `AddMcpServer().WithStdioServerTransport().WithTools<T>()` hosting
  pattern.

YnabSharp's own `docs/ynab-api-coverage.md` (checked 2026-08-05):
Categories has no month-get; Months resource has nothing implemented.

Existing open issues confirmed via `gh issue view`: #98, #86, #87.

## Open questions

- Exact shape of the `dotnet tool` release workflow — not designed, just
  decided it needs one, separate from the library's `Publish` workflow.
- Whether Claude Desktop/Claude Code's host-level tool-call confirmation
  UX is reliable enough to layer on top of `ElicitAsync`, or whether
  `ElicitAsync` alone is sufficient — not tested against a real host.
- What the write-access ADR should say about scope: does every write
  tool need `ElicitAsync`, or only ones above some risk threshold?

## Out of scope

Remote/HTTP transport (SSE) — stdio covers the single-user local case
this issue scoped to.

OAuth vs. personal access token — YnabSharp already uses PAT via
`WithBearerToken`; not re-evaluated here.

Multi-user/multi-budget support — out of scope per #151's own scope
note (stays a YnabSharp feature, not a separate service).
