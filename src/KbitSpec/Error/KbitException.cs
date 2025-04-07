namespace KbitSpec.Error;

public class KbitException : Exception
{
    public KbitException(string message) : base(message)
    {
    }
}
