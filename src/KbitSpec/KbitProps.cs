using KbitSpec.Utils;

namespace KbitSpec;

public class KbitProps : ICopyable<KbitProps>, IEquatable<KbitProps>
{
    public int EmAscent { get; set; }
    public int EmDescent { get; set; }
    public int LineAscent { get; set; }
    public int LineDescent { get; set; }
    public int LineGap { get; set; }
    public int XHeight { get; set; }
    public int CapHeight { get; set; }

    public KbitProps(
        int emAscent = 0,
        int emDescent = 0,
        int lineAscent = 0,
        int lineDescent = 0,
        int lineGap = 0,
        int xHeight = 0,
        int capHeight = 0)
    {
        EmAscent = emAscent;
        EmDescent = emDescent;
        LineAscent = lineAscent;
        LineDescent = lineDescent;
        LineGap = lineGap;
        XHeight = xHeight;
        CapHeight = capHeight;
    }

    public int EmHeight => EmAscent + EmDescent;

    public int LineHeight => LineAscent + LineDescent;

    public KbitProps Copy() => new(
        EmAscent,
        EmDescent,
        LineAscent,
        LineDescent,
        LineGap,
        XHeight,
        CapHeight);

    public KbitProps DeepCopy() => Copy();

    public bool Equals(KbitProps? other)
    {
        if (other is null)
        {
            return false;
        }
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        return EmAscent == other.EmAscent &&
               EmDescent == other.EmDescent &&
               LineAscent == other.LineAscent &&
               LineDescent == other.LineDescent &&
               LineGap == other.LineGap &&
               XHeight == other.XHeight &&
               CapHeight == other.CapHeight;
    }

    public override bool Equals(object? other)
    {
        if (other is null)
        {
            return false;
        }
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        if (other.GetType() != GetType())
        {
            return false;
        }
        return Equals((KbitProps)other);
    }

    public override int GetHashCode() => 0;
}
