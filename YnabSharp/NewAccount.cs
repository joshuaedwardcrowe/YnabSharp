using YnabSharp.Sanitisers;

namespace YnabSharp;

public class NewAccount(string name, AccountType type, int balance)
{
    public string Name = name;
    public AccountType Type = type;
    public decimal Balance => MilliunitConverter.PoundsToMilliunit(balance);
}