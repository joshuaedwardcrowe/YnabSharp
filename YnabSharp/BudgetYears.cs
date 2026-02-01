namespace YnabSharp;

public class BudgetYears
{
    public List<int> All { get; set; }
    public List<int> Measurable { get; set; }
    
    public BudgetYears(List<int> all, List<int> measurable)
    {
        All = all;
        Measurable = measurable;
    }
}