namespace KbitSpec.Tests;

public class KbitFontTests
{
    [Fact]
    public void TestCopy()
    {
        var font1 = KbitFont.LoadKbitx(Path.Combine("assets", "demo", "demo.kbitx"));
        var font2 = font1.Copy();

        Assert.Equal(font1, font2);
        Assert.NotSame(font1, font2);
        Assert.Same(font1.Props, font2.Props);
        Assert.Same(font1.Names, font2.Names);
        Assert.Same(font1.Characters, font2.Characters);
        Assert.Same(font1.NamedGlyphs, font2.NamedGlyphs);
        Assert.Same(font1.KernPairs, font2.KernPairs);
    }

    [Fact]
    public void TestDeepCopy()
    {
        var font1 = KbitFont.LoadKbitx(Path.Combine("assets", "demo", "demo.kbitx"));
        var font2 = font1.DeepCopy();

        Assert.Equal(font1, font2);
        Assert.NotSame(font1, font2);
        Assert.NotSame(font1.Props, font2.Props);
        Assert.NotSame(font1.Names, font2.Names);
        Assert.NotSame(font1.Characters, font2.Characters);
        Assert.NotSame(font1.NamedGlyphs, font2.NamedGlyphs);
        Assert.NotSame(font1.KernPairs, font2.KernPairs);

        foreach (var ((codePoint1, glyph1), (codePoint2, glyph2)) in font1.Characters.Zip(font2.Characters))
        {
            Assert.Equal(codePoint1, codePoint2);
            Assert.NotSame(glyph1, glyph2);
        }

        foreach (var ((glyphName1, glyph1), (glyphName2, glyph2)) in font1.NamedGlyphs.Zip(font2.NamedGlyphs))
        {
            Assert.Equal(glyphName1, glyphName2);
            Assert.NotSame(glyph1, glyph2);
        }
    }

    [Fact]
    public void TestEquals()
    {
        var filePath = Path.Combine("assets", "demo", "demo.kbitx");
        var font1 = KbitFont.LoadKbitx(filePath);
        var font2 = KbitFont.LoadKbitx(filePath);
        Assert.Equal(font1, font2);
    }
}
