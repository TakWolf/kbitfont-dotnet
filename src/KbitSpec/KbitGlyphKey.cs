namespace KbitSpec;

public readonly struct KbitGlyphKey : IComparable<KbitGlyphKey>, IEquatable<KbitGlyphKey>
{
    private readonly int _intValue;
    private readonly string? _stringValue;

    public KbitGlyphKey(int value)
    {
        _intValue = value;
        _stringValue = null;
    }

    public KbitGlyphKey(string value)
    {
        ArgumentNullException.ThrowIfNull(value);
        _intValue = 0;
        _stringValue = value;
    }

    public bool IsInt => _stringValue is null;

    public bool IsString => _stringValue is not null;

    public int AsInt() => IsInt ? _intValue : throw new InvalidOperationException("The value is not int.");

    public string AsString() => IsString ? _stringValue! : throw new InvalidOperationException("The value is not string.");

    public static implicit operator KbitGlyphKey(int value) => new(value);

    public static implicit operator KbitGlyphKey(string value) => new(value);

    public static explicit operator int(KbitGlyphKey value) => value.AsInt();

    public static explicit operator string(KbitGlyphKey value) => value.AsString();

    public void Deconstruct(out int? intValue, out string? stringValue)
    {
        if (IsInt)
        {
            intValue = _intValue;
            stringValue = null;
        }
        else
        {
            intValue = null;
            stringValue = _stringValue;
        }
    }

    public int CompareTo(KbitGlyphKey other)
    {
        if (IsInt && other.IsInt)
        {
            return _intValue.CompareTo(other._intValue);
        }
        if (IsString && other.IsString)
        {
            return string.Compare(_stringValue, other._stringValue, StringComparison.Ordinal);
        }
        return IsInt ? -1 : 1;
    }

    public bool Equals(KbitGlyphKey other) => _intValue == other._intValue && _stringValue == other._stringValue;

    public override bool Equals(object? obj) => obj is KbitGlyphKey other && Equals(other);

    public static bool operator ==(KbitGlyphKey left, KbitGlyphKey right) => left.Equals(right);

    public static bool operator !=(KbitGlyphKey left, KbitGlyphKey right) => !(left == right);

    public override int GetHashCode() => IsInt ? HashCode.Combine(1, _intValue) : HashCode.Combine(2, _stringValue);

    public override string ToString() => IsInt ? _intValue.ToString() : _stringValue!;
}
