namespace YnabSharpa.Sanitisers;

public static class TransactionFlowSanitiser
{
    /// <summary>
    /// Ingore the inflow or outflow defined negative/positive amount value.
    /// </summary>
    /// <param name="amount"></param>
    /// <returns></returns>
    public static decimal Sanitise(decimal amount) => Math.Abs(amount);
}