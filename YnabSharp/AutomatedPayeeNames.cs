namespace YnabSharpa;

public static class AutomatedPayeeNames
{
    public const string StartingBalance = "Starting Balance";
    public const string ManualBalanceAdjustment = "Manual Balance Adjustment";
    public const string ReconciliationBalanceAdjustment = "Reconciliation Balance Adjustment";
    
    public static readonly List<string> All =
    [
        StartingBalance,
        ManualBalanceAdjustment,
        ReconciliationBalanceAdjustment
    ];
}