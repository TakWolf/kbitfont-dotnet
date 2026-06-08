namespace KbitSpec.Tests;

public class KbitNamesTests
{
    [Fact]
    public void TestNames()
    {
        var names = new KbitNames();

        names.Copyright = "ID: 0";
        Assert.Equal("ID: 0", names.Copyright);
        Assert.Equal("ID: 0", names[0]);

        names.Family = "ID: 1";
        Assert.Equal("ID: 1", names.Family);
        Assert.Equal("ID: 1", names[1]);

        names.Style = "ID: 2";
        Assert.Equal("ID: 2", names.Style);
        Assert.Equal("ID: 2", names[2]);

        names.UniqueId = "ID: 3";
        Assert.Equal("ID: 3", names.UniqueId);
        Assert.Equal("ID: 3", names[3]);

        names.FamilyAndStyle = "ID: 4";
        Assert.Equal("ID: 4", names.FamilyAndStyle);
        Assert.Equal("ID: 4", names[4]);

        names.Version = "ID: 5";
        Assert.Equal("ID: 5", names.Version);
        Assert.Equal("ID: 5", names[5]);

        names.Postscript = "ID: 6";
        Assert.Equal("ID: 6", names.Postscript);
        Assert.Equal("ID: 6", names[6]);

        names.Trademark = "ID: 7";
        Assert.Equal("ID: 7", names.Trademark);
        Assert.Equal("ID: 7", names[7]);

        names.Manufacturer = "ID: 8";
        Assert.Equal("ID: 8", names.Manufacturer);
        Assert.Equal("ID: 8", names[8]);

        names.Designer = "ID: 9";
        Assert.Equal("ID: 9", names.Designer);
        Assert.Equal("ID: 9", names[9]);

        names.Description = "ID: 10";
        Assert.Equal("ID: 10", names.Description);
        Assert.Equal("ID: 10", names[10]);

        names.VendorUrl = "ID: 11";
        Assert.Equal("ID: 11", names.VendorUrl);
        Assert.Equal("ID: 11", names[11]);

        names.DesignerUrl = "ID: 12";
        Assert.Equal("ID: 12", names.DesignerUrl);
        Assert.Equal("ID: 12", names[12]);

        names.LicenseDescription = "ID: 13";
        Assert.Equal("ID: 13", names.LicenseDescription);
        Assert.Equal("ID: 13", names[13]);

        names.LicenseUrl = "ID: 14";
        Assert.Equal("ID: 14", names.LicenseUrl);
        Assert.Equal("ID: 14", names[14]);

        names.WindowsFamily = "ID: 16";
        Assert.Equal("ID: 16", names.WindowsFamily);
        Assert.Equal("ID: 16", names[16]);

        names.WindowsStyle = "ID: 17";
        Assert.Equal("ID: 17", names.WindowsStyle);
        Assert.Equal("ID: 17", names[17]);

        names.MacosFamilyAndStyle = "ID: 18";
        Assert.Equal("ID: 18", names.MacosFamilyAndStyle);
        Assert.Equal("ID: 18", names[18]);

        names.SampleText = "ID: 19";
        Assert.Equal("ID: 19", names.SampleText);
        Assert.Equal("ID: 19", names[19]);

        names.PostscriptCid = "ID: 20";
        Assert.Equal("ID: 20", names.PostscriptCid);
        Assert.Equal("ID: 20", names[20]);

        names.WwsFamily = "ID: 21";
        Assert.Equal("ID: 21", names.WwsFamily);
        Assert.Equal("ID: 21", names[21]);

        names.WwsStyle = "ID: 22";
        Assert.Equal("ID: 22", names.WwsStyle);
        Assert.Equal("ID: 22", names[22]);
    }

    [Fact]
    public void TestCopy()
    {
        var names1 = new KbitNames();
        names1.Copyright = "ID: 0";
        names1.Family = "ID: 1";
        var names2 = names1.Copy();
        var names3 = names1.DeepCopy();

        Assert.Equal(names1, names2);
        Assert.Equal(names1, names3);
        Assert.NotSame(names1, names2);
        Assert.NotSame(names1, names3);
    }

    [Fact]
    public void TestEquals()
    {
        var names1 = new KbitNames();
        names1.Copyright = "ID: 0";
        names1.Family = "ID: 1";

        var names2 = new KbitNames();
        names2.Copyright = "ID: 0";
        names2.Family = "ID: 1";

        Assert.Equal(names1, names2);
    }
}
