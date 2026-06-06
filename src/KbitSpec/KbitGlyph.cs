using KbitSpec.Utils;

namespace KbitSpec;

public class KbitGlyph : ICopyable<KbitGlyph>, IEquatable<KbitGlyph>
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Advance { get; set; }
    public List<List<byte>> Bitmap { get; set; }

    public KbitGlyph(
        int x = 0,
        int y = 0,
        int advance = 0,
        List<List<byte>>? bitmap = null)
    {
        X = x;
        Y = y;
        Advance = advance;
        Bitmap = bitmap ?? [];
    }

    public int Width => Bitmap.Count > 0 ? Bitmap[0].Count : 0;

    public int Height => Bitmap.Count;

    public (int, int) Dimensions => (Width, Height);

    public KbitGlyph Copy() => new(
        X,
        Y,
        Advance,
        Bitmap);

    public KbitGlyph DeepCopy() => new(
        X,
        Y,
        Advance,
        CopyUtil.DeepCopyBitmap(Bitmap));

    public bool Equals(KbitGlyph? other)
    {
        if (other is null)
        {
            return false;
        }
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        return X == other.X &&
               Y == other.Y &&
               Advance == other.Advance &&
               EqualUtil.BitmapEquals(Bitmap, other.Bitmap);
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
        return Equals((KbitGlyph)other);
    }

    public override int GetHashCode() => 0;
}
