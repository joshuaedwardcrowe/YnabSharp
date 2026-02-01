using YnabSharpa.Sanitisers;

namespace YnabSharpa;

public class NewAccount(string name, AccountType type, int balance)
{
    public string Name = name;
    public AccountType Type = type;
    public decimal Balance => MilliunitConverter.PoundsToMilliunit(balance);
}