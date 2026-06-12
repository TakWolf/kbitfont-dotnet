namespace KbitSpec.Tests;

public class KbitPropsTests
{
    [Fact]
    public void TestProps()
    {
        var props = new KbitProps(
            emAscent: 10,
            emDescent: 2,
            lineAscent: 12,
            lineDescent: 4);
        Assert.Equal(12, props.EmHeight);
        Assert.Equal(16, props.LineHeight);
    }

    [Fact]
    public void TestCopy()
    {
        var props1 = new KbitProps(
            emAscent: 1,
            emDescent: 2,
            lineAscent: 3,
            lineDescent: 4,
            lineGap: 5,
            xHeight: 6,
            capHeight: 7,
            newGlyphWidth: 8);
        var props2 = props1.Copy();
        var props3 = props1.DeepCopy();

        Assert.Equal(props1, props2);
        Assert.Equal(props1, props3);
        Assert.NotSame(props1, props2);
        Assert.NotSame(props1, props3);
    }

    [Fact]
    public void TestEquals()
    {
        var props1 = new KbitProps(
            emAscent: 1,
            emDescent: 2,
            lineAscent: 3,
            lineDescent: 4,
            lineGap: 5,
            xHeight: 6,
            capHeight: 7,
            newGlyphWidth: 8);
        var props2 = new KbitProps(
            emAscent: 1,
            emDescent: 2,
            lineAscent: 3,
            lineDescent: 4,
            lineGap: 5,
            xHeight: 6,
            capHeight: 7,
            newGlyphWidth: 8);
        Assert.Equal(props1, props2);
    }
}
