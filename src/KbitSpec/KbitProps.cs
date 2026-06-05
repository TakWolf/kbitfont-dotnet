namespace KbitSpec;

public class KbitProps
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
}
