namespace YnabSharpa.Extensions;

// TODO: Quick Win - Rename to TransactionFilterExtensions
public static class TransactionExtensions
{
    public static IEnumerable<Transaction> FilterToCategories(
        this IEnumerable<Transaction> transactions, params Guid[] categoryIds)
            => transactions.Where(transaction
                => transaction.SplitTransactions
                    .Concat([transaction])
                    .Any(subTransaction => subTransaction.InCategories(categoryIds)));

    public static IEnumerable<Transaction> FilterToCategories(
        this IEnumerable<Transaction> transactions, IEnumerable<Guid> categoryIds) 
            => FilterToCategories(transactions, categoryIds.ToArray());

    public static IEnumerable<Transaction> FilterOutTransfers(
        this IEnumerable<Transaction> transactions)
            => transactions.Where(transaction => !transaction.IsTransfer);
    
    public static IEnumerable<Transaction> FilterOutAutomations(
        this IEnumerable<Transaction> transactions)
             => transactions.Where(transaction => !AutomatedPayeeNames.All.Contains(transaction.PayeeName));
    
    public static IEnumerable<Transaction> FilterOutCategories(
        this IEnumerable<Transaction> transactions, IEnumerable<Guid> categoryIds)
            => transactions.Where(transaction => transaction.CategoryId.HasValue &&
                                                 !categoryIds.Contains(transaction.CategoryId.Value));
    
    public static IEnumerable<Transaction> FilterToInflow(
        this IEnumerable<Transaction> transactions) 
            => transactions.Where(transaction => transaction.Amount > 0);
    
    public static IEnumerable<Transaction> FilterToOutflow(
        this IEnumerable<Transaction> transactions) 
        => transactions.Where(transaction => transaction.Amount < 0);
    
    public static IEnumerable<Transaction> FilterToPayeeNames(
        this IEnumerable<Transaction> transactions, params string[] payeeNames)
            => transactions.Where(t => payeeNames.Contains(t.PayeeName));

    public static IEnumerable<Transaction> FilterFrom(
        this IEnumerable<Transaction> transactions, DateOnly startDate)
    {
        var dateFrom = startDate.ToDateTime(TimeOnly.MinValue);
        return transactions.Where(t => t.Occured >= dateFrom);
    }

    public static IEnumerable<Transaction> FilterTo(
        this IEnumerable<Transaction> transactions, DateOnly stopDate)
    {
        var dateFrom = stopDate.ToDateTime(TimeOnly.MinValue);
        return transactions.Where(t => t.Occured <= dateFrom);
    }

    public static IEnumerable<Transaction> FilterToSpending(this IEnumerable<Transaction> transactions)
        => transactions.Where(transaction =>
            !transaction.IsTransfer && 
            !AutomatedPayeeNames.All.Contains(transaction.PayeeName));
}