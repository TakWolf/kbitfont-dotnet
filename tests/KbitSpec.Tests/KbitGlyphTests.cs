namespace KbitSpec.Tests;

public class KbitGlyphTests
{
    [Fact]
    public void TestGlyph()
    {
        var glyph = new KbitGlyph(
            x: 1,
            y: 2,
            advance: 3,
            bitmap: [[1, 0, 0, 1]]);
        Assert.Equal(4, glyph.Width);
        Assert.Equal(1, glyph.Height);
        Assert.Equal((4, 1), glyph.Dimensions);
    }

    [Fact]
    public void TestCopy()
    {
        var glyph1 = new KbitGlyph(
            x: 1,
            y: 2,
            advance: 3,
            bitmap: [[1, 0, 0, 1]]);
        var glyph2 = glyph1.Copy();

        Assert.Equal(glyph1, glyph2);
        Assert.NotSame(glyph1, glyph2);
        Assert.Same(glyph1.Bitmap, glyph2.Bitmap);
    }

    [Fact]
    public void TestDeepCopy()
    {
        var glyph1 = new KbitGlyph(
            x: 1,
            y: 2,
            advance: 3,
            bitmap: [[1, 0, 0, 1]]);
        var glyph2 = glyph1.DeepCopy();

        Assert.Equal(glyph1, glyph2);
        Assert.NotSame(glyph1, glyph2);
        Assert.NotSame(glyph1.Bitmap, glyph2.Bitmap);

        foreach (var (bitmapRow1, bitmapRow2) in glyph1.Bitmap.Zip(glyph2.Bitmap))
        {
            Assert.NotSame(bitmapRow1, bitmapRow2);
        }
    }

    [Fact]
    public void TestEquals()
    {
        var glyph1 = new KbitGlyph(
            x: 1,
            y: 2,
            advance: 3,
            bitmap: [[1, 0, 0, 1]]);
        var glyph2 = new KbitGlyph(
            x: 1,
            y: 2,
            advance: 3,
            bitmap: [[1, 0, 0, 1]]);
        Assert.Equal(glyph1, glyph2);
    }
}
