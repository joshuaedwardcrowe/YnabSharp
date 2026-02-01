using YnabSharpa.Responses.Accounts;

namespace YnabSharpa.Mappers;

public static class NewAccountMapping
{
    public static AccountRequest ToAccountRequest(this NewAccount newAccount)
        => new AccountRequest
        {
            Name = newAccount.Name,
            Type = newAccount.Type,
            Balance = newAccount.Balance
        };

}