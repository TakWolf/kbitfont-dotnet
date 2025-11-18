namespace KbitSpec.Errors;

public class KbitException : Exception
{
    public KbitException(string message) : base(message)
    {
    }
}
