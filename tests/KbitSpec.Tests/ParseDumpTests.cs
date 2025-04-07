namespace KbitSpec.Tests;

public class ParseDumpTests
{
    [Fact]
    public void TestDemoKbits()
    {
        var data = File.ReadAllBytes(Path.Combine("assets", "demo", "demo.kbits"));
        var font = KbitFont.ParseKbits(data);
        Assert.Equal(data, font.DumpKbitsToBytes());
    }

    [Fact]
    public void TestDemoKbitx()
    {
        var data = File.ReadAllText(Path.Combine("assets", "demo", "demo.kbitx"));
        var font = KbitFont.ParseKbitx(data);
        Assert.Equal(data.Replace("\r\n", "\n"), font.DumpKbitxToString());
    }
}
