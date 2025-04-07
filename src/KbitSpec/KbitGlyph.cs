namespace KbitSpec;

public class KbitGlyph
{
    public int X;
    public int Y;
    public int Advance;
    public List<List<byte>> Bitmap;

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
}
