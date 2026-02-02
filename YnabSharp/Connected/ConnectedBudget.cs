using YnabSharp.Clients;
using YnabSharp.Mappers;
using YnabSharp.Responses.Budgets;

namespace YnabSharp.Connected;

public class ConnectedBudget : Budget
{
    private readonly AccountClient _accountClient;
    private readonly CategoryClient _categoryClient;
    private readonly TransactionClient _transactionClient;
    private readonly ScheduledTransactionClient _scheduledTransactionsClient;

    public ConnectedBudget(
        AccountClient accountClient,
        CategoryClient categoryClient,
        TransactionClient transactionClient,
        ScheduledTransactionClient scheduledTransactionsClient,
        BudgetResponse budgetResponse) : base(budgetResponse)
    {
        _accountClient = accountClient;
        _categoryClient = categoryClient;
        _transactionClient = transactionClient;
        _scheduledTransactionsClient = scheduledTransactionsClient;
    }

    public Task<IEnumerable<Account>> GetAccounts() =>
        _accountClient.GetAll();
    public Task<ConnectedAccount> GetAccount(Guid id)
        => _accountClient.Get(id);
    public Task<IEnumerable<CategoryGroup>> GetCategoryGroups()
        => _categoryClient.GetAll();
    public Task<IEnumerable<Transaction>> GetTransactions()
        => _transactionClient.GetAll();
    public Task<Transaction> GetTransaction(string id) 
        => _transactionClient.Get(id);
    public Task<IEnumerable<Transaction>> CreateTransactions(IEnumerable<Transaction> transactions) => 
        _transactionClient.Create(transactions);
    public Task<ConnectedAccount> CreateAccount(NewAccount newAccount)
        => _accountClient.Create(newAccount);
    
    public async Task MoveAccountTransactions(ConnectedAccount fromAccount, ConnectedAccount toAccount)
    {
        var transactionsTask = fromAccount.GetTransactions();
        var scheduledTransactionsTask = fromAccount.GetScheduledTransactions();
        
        await Task.WhenAll(transactionsTask, scheduledTransactionsTask);
        
        var transactionsToMove = transactionsTask
            .Result
            .Where(t => t.PayeeName != AutomatedPayeeNames.StartingBalance)
            .ToMovedTransactions(toAccount.Id);

        var scheduledTransactionsToMove = scheduledTransactionsTask
            .Result
            .ToMovedTransactions(toAccount.Id);

        var scheduledTransactionMoveTasks = scheduledTransactionsToMove
            .Select(movedScheduledTransaction =>
                _scheduledTransactionsClient.MoveTransaction(movedScheduledTransaction));
        
        await Task.WhenAll(scheduledTransactionMoveTasks);
        _  = await _transactionClient.Move(transactionsToMove);
    }
    
}