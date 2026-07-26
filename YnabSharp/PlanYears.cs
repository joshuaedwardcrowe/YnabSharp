namespace YnabSharp;

public class PlanYears
{
    public List<int> All { get; set; }
    public List<int> Measurable { get; set; }

    public PlanYears(List<int> all, List<int> measurable)
    {
        All = all;
        Measurable = measurable;
    }
}