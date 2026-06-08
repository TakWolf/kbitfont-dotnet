namespace KbitSpec.Tests.Conformance;

public class ParseDumpTests
{
    [Theory]
    [InlineData("demo", "demo.kbits")]
    [InlineData("macintosh", "Athens.kbits")]
    [InlineData("macintosh", "Geneva-12.kbits")]
    [InlineData("macintosh", "New-York-14.kbits")]
    public void TestParseDumpKbits(string fontDir, string fontFileName)
    {
        var data = File.ReadAllBytes(Path.Combine("assets", fontDir, fontFileName));
        var font = KbitFont.ParseKbits(data);
        Assert.Equal(data, font.DumpKbitsToBytes());
    }

    [Theory]
    [InlineData("demo", "demo.kbitx")]
    [InlineData("macintosh", "Athens.kbitx")]
    [InlineData("macintosh", "Geneva-12.kbitx")]
    [InlineData("macintosh", "New-York-14.kbitx")]
    public void TestParseDumpKbitx(string fontDir, string fontFileName)
    {
        var data = File.ReadAllText(Path.Combine("assets", fontDir, fontFileName));
        var font = KbitFont.ParseKbitx(data);
        Assert.Equal(data.Replace("\r\n", "\n"), font.DumpKbitxToString());
    }
}
