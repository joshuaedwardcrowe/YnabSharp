# MCP connector spike — 2026-08-05

Investigation for [#151](https://github.com/joshuaedwardcrowe/YnabSharp/issues/151), "No way for Claude to connect to a YNAB budget."

## Conclusion

Feasible within YnabSharp (per #151's scope note). Shape: a local stdio MCP server (`YnabSharp.Mcp`), packaged as a .NET global tool, read-only tools by default, writes gated behind protocol-level elicitation — not host trust.

## Blocking dependency

Conversational balance queries ("how much do I have left in groceries") need Categories month-get and the Months resource — neither exists in YnabSharp today. Already tracked: [#98](https://github.com/joshuaedwardcrowe/YnabSharp/issues/98) (Categories month-get), [#86](https://github.com/joshuaedwardcrowe/YnabSharp/issues/86)/[#87](https://github.com/joshuaedwardcrowe/YnabSharp/issues/87) (Months list/get-by-month). Must land first.

## Write-safety mechanism

MCP has two distinct things, easy to conflate:

- *Tool annotations* (`ReadOnly`/`Destructive`/`Idempotent`/`OpenWorld`, set via `McpServerToolCreateOptions` in the C# SDK) are hints to the **host**. The spec calls these untrusted unless the server is trusted — advisory, not a safety boundary.
- *Elicitation* (`elicitation/create`, exposed in C# as `server.ElicitAsync(...)`, with accept/decline/cancel responses) is a real protocol-level mechanism the **server** controls directly.

Decision: annotate every tool correctly, but gate every write tool on an explicit `ElicitAsync` confirm before it acts. Don't rely on host trust alone — we don't control the host.

## Hosting/distribution

`AddMcpServer().WithStdioServerTransport().WithTools<T>()` (package: `ModelContextProtocol`) — no server infra needed. For "anyone gets running quickly," `YnabSharp.Mcp` should pack as a `dotnet tool` (`PackAsTool=true`) via NuGet, not `dotnet run` from a clone. Needs its own version/release workflow, same precedent as `YnabSharp.Seeder`'s version not tracking the library's.

## Next milestone

1. #98 — Categories month-get
2. #86, #87 — Months
3. New ADR — write-access model (annotations + `ElicitAsync`)
4. `YnabSharp.Mcp` scaffold — read-only tools first
5. Tool packaging + release workflow
6. Docs/quick-start
7. Gated write tool(s), post-ADR
