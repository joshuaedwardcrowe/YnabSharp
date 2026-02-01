namespace YnabSharpa.Exceptions;

public class YnabException : Exception
{
    public YnabExceptionCode Code { get; }
    
    public YnabException(YnabExceptionCode code, string message) : base(message)
    {
        Code = code;
    }
}