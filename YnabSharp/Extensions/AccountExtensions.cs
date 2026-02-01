namespace YnabSharp.Extensions;

public static class AccountExtensions
{
    public static Account? Find(
        this IEnumerable<Account> accounts, string name)
            => accounts.FirstOrDefault(account => account.Name == name);
    
    public static IEnumerable<Account> FilterToTypes(
        this IEnumerable<Account> accounts, params AccountType[] types)
            => accounts.Where(account => types.Contains(account.Type));
}