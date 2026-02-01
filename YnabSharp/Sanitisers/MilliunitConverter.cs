namespace YnabSharp.Sanitisers;

public static class MilliunitConverter
{
    private const decimal ConversationRate = 1000m;
    
    public static decimal Calculate(int amountInMilliunits) => amountInMilliunits / ConversationRate;

    public static decimal? MilliunitToPounds(int? amountInMilliunits)
    {
        if (amountInMilliunits.HasValue)
        {
            return Calculate(amountInMilliunits.Value);
        }

        return null;
    }

    public static int PoundsToMilliunit(decimal amountInPounds)
    {
        var conversion = amountInPounds * ConversationRate;
        return (int)conversion;
    }
}