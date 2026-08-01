namespace YnabSharp.Responses.Accounts;

/// <summary>
/// The YNAB API's wire field names for the Account/AccountBase schema, shared
/// between AccountResponse's [JsonPropertyName] attributes and spec-conformance
/// tests so both sides can't drift from each other through a typo. Includes
/// fields not yet on AccountResponse — this documents the full AccountBase
/// schema, not just what the DTO currently exposes.
/// </summary>
public static class AccountFieldNames
{
    public const string Id = "id";
    public const string Name = "name";
    public const string Type = "type";
    public const string OnBudget = "on_budget";
    public const string Closed = "closed";
    public const string Note = "note";
    public const string Balance = "balance";
    public const string ClearedBalance = "cleared_balance";
    public const string UnclearedBalance = "uncleared_balance";
    public const string TransferPayeeId = "transfer_payee_id";
    public const string Deleted = "deleted";
}
