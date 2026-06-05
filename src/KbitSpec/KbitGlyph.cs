namespace KbitSpec;

public class KbitGlyph
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
}
