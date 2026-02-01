using YnabSharp.Http;
using YnabSharp.Mappers;
using YnabSharp.Requests.ScheduledTransactions;
using YnabSharp.Responses.ScheduledTransactions;

namespace YnabSharp.Clients;

public class ScheduledTransactionClient(YnabHttpClientBuilder builder, string ynabBudgetApiPath) : YnabApiClient
{
    public async Task<IEnumerable<ScheduledTransaction>> GetAll()
    {
        var response = await Get<GetScheduledTransactionsResponseData>(string.Empty);
        return response.Data.ScheduledTransactions.Select(st => new ScheduledTransaction(st));
    }

    public async Task<ScheduledTransaction> MoveTransaction(MovedScheduledTransaction movedScheduledTransaction)
    {
        var requestUrl = $"{movedScheduledTransaction.Id}";
        var request = new UpdateScheduledTransactionRequest(movedScheduledTransaction.ToScheduledTransactionRequest());
        var response = await Put<UpdateScheduledTransactionRequest, GetScheduledTransactionResponseData>(requestUrl, request);
        return new ScheduledTransaction(response.Data.ScheduledTransaction);
    }
    
    protected override HttpClient GetHttpClient() => builder.Build(ynabBudgetApiPath,  YnabApiPath.ScheduledTransactions);
}
