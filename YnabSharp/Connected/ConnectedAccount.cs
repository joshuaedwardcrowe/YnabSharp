using YnabSharpa.Clients;
using YnabSharpa.Responses.Accounts;

namespace YnabSharpa.Connected;

public class ConnectedAccount : Account
{
    private readonly TransactionClient _transactionClient;
    private readonly ScheduledTransactionClient _scheduledTransactionsClient;

    public ConnectedAccount(
        TransactionClient transactionClient,
        ScheduledTransactionClient scheduledTransactionsClient,
        AccountResponse accountResponse) : base(accountResponse)
    {
        _transactionClient = transactionClient;
        _scheduledTransactionsClient = scheduledTransactionsClient;
    }

    public async Task<IEnumerable<Transaction>> GetTransactions()
    {
        var transactions = await _transactionClient.GetAll();
        return transactions.Where(transaction => transaction.AccountId == Id);
    }

    public async Task<IEnumerable<ScheduledTransaction>> GetScheduledTransactions()
    {
        var scheduledTransactions = await _scheduledTransactionsClient.GetAll();
        return scheduledTransactions.Where(st => st.AccountId == Id);
    }
}

