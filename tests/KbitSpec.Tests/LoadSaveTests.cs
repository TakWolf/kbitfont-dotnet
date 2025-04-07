namespace KbitSpec.Tests;

public class LoadSaveTests
{
    [Fact]
    public void TestDemoKbits()
    {
        var loadPath = Path.Combine("assets", "demo", "demo.kbits");
        var savePath = Path.Combine(PathUtils.CreateTempDir(), "demo.kbits");
        var font = KbitFont.LoadKbits(loadPath);
        font.SaveKbits(savePath);
        Assert.Equal(File.ReadAllBytes(loadPath), File.ReadAllBytes(savePath));
    }

    [Fact]
    public void TestDemoKbitx()
    {
        var loadPath = Path.Combine("assets", "demo", "demo.kbitx");
        var savePath = Path.Combine(PathUtils.CreateTempDir(), "demo.kbitx");
        var font = KbitFont.LoadKbitx(loadPath);
        font.SaveKbitx(savePath);
        Assert.Equal(File.ReadAllText(loadPath).Replace("\r\n", "\n"), File.ReadAllText(savePath));
    }

    [Fact]
    public void TestAthensKbits()
    {
        var loadPath = Path.Combine("assets", "macintosh", "Athens.kbits");
        var savePath = Path.Combine(PathUtils.CreateTempDir(), "Athens.kbits");
        var font = KbitFont.LoadKbits(loadPath);
        font.SaveKbits(savePath);
        Assert.Equal(File.ReadAllBytes(loadPath), File.ReadAllBytes(savePath));
    }

    [Fact]
    public void TestAthensKbitx()
    {
        var loadPath = Path.Combine("assets", "macintosh", "Athens.kbitx");
        var savePath = Path.Combine(PathUtils.CreateTempDir(), "Athens.kbitx");
        var font = KbitFont.LoadKbitx(loadPath);
        font.SaveKbitx(savePath);
        Assert.Equal(File.ReadAllText(loadPath).Replace("\r\n", "\n"), File.ReadAllText(savePath));
    }

    [Fact]
    public void TestGeneva12Kbits()
    {
        var loadPath = Path.Combine("assets", "macintosh", "Geneva-12.kbits");
        var savePath = Path.Combine(PathUtils.CreateTempDir(), "Geneva-12.kbits");
        var font = KbitFont.LoadKbits(loadPath);
        font.SaveKbits(savePath);
        Assert.Equal(File.ReadAllBytes(loadPath), File.ReadAllBytes(savePath));
    }

    [Fact]
    public void TestGeneva12Kbitx()
    {
        var loadPath = Path.Combine("assets", "macintosh", "Geneva-12.kbitx");
        var savePath = Path.Combine(PathUtils.CreateTempDir(), "Geneva-12.kbitx");
        var font = KbitFont.LoadKbitx(loadPath);
        font.SaveKbitx(savePath);
        Assert.Equal(File.ReadAllText(loadPath).Replace("\r\n", "\n"), File.ReadAllText(savePath));
    }

    [Fact]
    public void TestNewYork14Kbits()
    {
        var loadPath = Path.Combine("assets", "macintosh", "New-York-14.kbits");
        var savePath = Path.Combine(PathUtils.CreateTempDir(), "New-York-14.kbits");
        var font = KbitFont.LoadKbits(loadPath);
        font.SaveKbits(savePath);
        Assert.Equal(File.ReadAllBytes(loadPath), File.ReadAllBytes(savePath));
    }

    [Fact]
    public void TestNewYork14Kbitx()
    {
        var loadPath = Path.Combine("assets", "macintosh", "New-York-14.kbitx");
        var savePath = Path.Combine(PathUtils.CreateTempDir(), "New-York-14.kbitx");
        var font = KbitFont.LoadKbitx(loadPath);
        font.SaveKbitx(savePath);
        Assert.Equal(File.ReadAllText(loadPath).Replace("\r\n", "\n"), File.ReadAllText(savePath));
    }
}
