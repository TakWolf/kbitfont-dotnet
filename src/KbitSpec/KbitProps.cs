namespace KbitSpec;

public class KbitProps
{
    public int EmAscent;
    public int EmDescent;
    public int LineAscent;
    public int LineDescent;
    public int LineGap;
    public int XHeight;
    public int CapHeight;

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
