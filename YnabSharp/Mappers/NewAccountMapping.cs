using YnabSharp.Responses.Accounts;
using YnabSharp.Sanitisers;

namespace YnabSharp.Mappers;

public static class NewAccountMapping
{
    public static AccountRequest ToAccountRequest(this NewAccount newAccount)
        => new AccountRequest
        {
            Name = newAccount.Name,
            Type = newAccount.Type,
            Balance = MilliunitConverter.PoundsToMilliunit(newAccount.ClearedBalance)
        };

    public static AccountRequest ToAccountRequest(this Account account)
        => new AccountRequest
        {
            Name = account.Name,
            Type = account.Type,
            Balance = MilliunitConverter.PoundsToMilliunit(account.ClearedBalance)
        };
}