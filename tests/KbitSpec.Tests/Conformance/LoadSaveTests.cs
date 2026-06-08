using KbitSpec.Tests.TestUtils;

namespace KbitSpec.Tests.Conformance;

public class LoadSaveTests
{
    [Theory]
    [InlineData("demo", "demo.kbits")]
    [InlineData("macintosh", "Athens.kbits")]
    [InlineData("macintosh", "Geneva-12.kbits")]
    [InlineData("macintosh", "New-York-14.kbits")]
    public void TestLoadSaveKbits(string fontDir, string fontFileName)
    {
        var loadPath = Path.Combine("assets", fontDir, fontFileName);
        var savePath = Path.Combine(PathUtil.CreateTempDir(), fontFileName);
        var font = KbitFont.LoadKbits(loadPath);
        font.SaveKbits(savePath);
        Assert.Equal(File.ReadAllBytes(loadPath), File.ReadAllBytes(savePath));
    }

    [Theory]
    [InlineData("demo", "demo.kbitx")]
    [InlineData("macintosh", "Athens.kbitx")]
    [InlineData("macintosh", "Geneva-12.kbitx")]
    [InlineData("macintosh", "New-York-14.kbitx")]
    public void TestLoadSaveKbitx(string fontDir, string fontFileName)
    {
        var loadPath = Path.Combine("assets", fontDir, fontFileName);
        var savePath = Path.Combine(PathUtil.CreateTempDir(), fontFileName);
        var font = KbitFont.LoadKbitx(loadPath);
        font.SaveKbitx(savePath);
        Assert.Equal(File.ReadAllText(loadPath).Replace("\r\n", "\n"), File.ReadAllText(savePath));
    }
}
