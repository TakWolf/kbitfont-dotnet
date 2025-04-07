namespace KbitSpec;

public readonly struct KbitKernKey : IComparable<KbitKernKey>
{
    private static void CheckType(object value)
    {
        if (value is not int && value is not string)
        {
            throw new ArgumentException("Can only be 'int' or 'string'.", nameof(value));
        }
    }

    private static int Compare(object objA, object objB)
    {
        return objA switch
        {
            int intA when objB is int intB => intA.CompareTo(intB),
            int when objB is string => -1,
            int => throw new ArgumentException("Can only be 'int' or 'string'.", nameof(objB)),
            string when objB is int => 1,
            string stringA when objB is string stringB => string.Compare(stringA, stringB, StringComparison.Ordinal),
            string => throw new ArgumentException("Can only be 'int' or 'string'.", nameof(objB)),
            _ => throw new ArgumentException("Can only be 'int' or 'string'.", nameof(objA))
        };
    }

    public readonly object Left;
    public readonly object Right;

    public KbitKernKey(object left, object right)
    {
        CheckType(left);
        CheckType(right);
        Left = left;
        Right = right;
    }

    public int LeftAsInt => (int)Left;

    public string LeftAsString => (string)Left;

    public int RightAsInt => (int)Right;

    public string RightAsString => (string)Right;

    int IComparable<KbitKernKey>.CompareTo(KbitKernKey other)
    {
        var result = Compare(Left, other.Left);
        if (result == 0)
        {
            result = Compare(Right, other.Right);
        }
        return result;
    }

    public void Deconstruct(out object left, out object right)
    {
        left = Left;
        right = Right;
    }
}
