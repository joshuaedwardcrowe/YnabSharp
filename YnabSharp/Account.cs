using YnabSharp.Responses.Accounts;
using YnabSharp.Sanitisers;

namespace YnabSharp;

public class Account(AccountResponse response)
{
    public Guid Id => response.Id;
    public string Name => response.Name;
    public AccountType Type => response.Type;
    public decimal ClearedBalance => MilliunitConverter.Calculate(response.ClearedBalance);
    public bool Closed => response.Closed;
    public bool OnBudget => response.OnBudget;
    public decimal Balance => MilliunitConverter.Calculate(response.Balance);
    public decimal UnclearedBalance => MilliunitConverter.Calculate(response.UnclearedBalance);
    public Guid? TransferPayeeId => response.TransferPayeeId;
    public bool Deleted => response.Deleted;
}