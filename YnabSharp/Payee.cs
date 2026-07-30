using YnabSharp.Responses.Payees;

namespace YnabSharp;

public class Payee(PayeeResponse payeeResponse)
{
    public Guid Id => payeeResponse.Id;
    public string Name => payeeResponse.Name;
    public Guid? TransferAccountId => payeeResponse.TransferAccountId;
    public bool Deleted => payeeResponse.Deleted;
}
