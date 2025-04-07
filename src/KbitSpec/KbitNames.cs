namespace KbitSpec;

public class KbitNames : SortedDictionary<int, string>
{
    private const int NameIdCopyright = 0;
    private const int NameIdFamily = 1;
    private const int NameIdStyle = 2;
    private const int NameIdUniqueId = 3;
    private const int NameIdFamilyAndStyle = 4;
    private const int NameIdVersion = 5;
    private const int NameIdPostscript = 6;
    private const int NameIdTrademark = 7;
    private const int NameIdManufacturer = 8;
    private const int NameIdDesigner = 9;
    private const int NameIdDescription = 10;
    private const int NameIdVendorUrl = 11;
    private const int NameIdDesignerUrl = 12;
    private const int NameIdLicenseDescription = 13;
    private const int NameIdLicenseUrl = 14;
    private const int NameIdWindowsFamily = 16;
    private const int NameIdWindowsStyle = 17;
    private const int NameIdMacosFamilyAndStyle = 18;
    private const int NameIdSampleText = 19;
    private const int NameIdPostscriptCid = 20;
    private const int NameIdWwsFamily = 21;
    private const int NameIdWwsStyle = 22;

    private string? GetValue(int key) => TryGetValue(key, out var value) ? value : null;

    private void SetValue(int key, string? value)
    {
        if (value is null)
        {
            Remove(key);
        }
        else
        {
            this[key] = value;
        }
    }

    public string? Copyright
    {
        get => GetValue(NameIdCopyright);
        set => SetValue(NameIdCopyright, value);
    }

    public string? Family
    {
        get => GetValue(NameIdFamily);
        set => SetValue(NameIdFamily, value);
    }

    public string? Style
    {
        get => GetValue(NameIdStyle);
        set => SetValue(NameIdStyle, value);
    }

    public string? UniqueId
    {
        get => GetValue(NameIdUniqueId);
        set => SetValue(NameIdUniqueId, value);
    }

    public string? FamilyAndStyle
    {
        get => GetValue(NameIdFamilyAndStyle);
        set => SetValue(NameIdFamilyAndStyle, value);
    }

    public string? Version
    {
        get => GetValue(NameIdVersion);
        set => SetValue(NameIdVersion, value);
    }

    public string? Postscript
    {
        get => GetValue(NameIdPostscript);
        set => SetValue(NameIdPostscript, value);
    }

    public string? Trademark
    {
        get => GetValue(NameIdTrademark);
        set => SetValue(NameIdTrademark, value);
    }

    public string? Manufacturer
    {
        get => GetValue(NameIdManufacturer);
        set => SetValue(NameIdManufacturer, value);
    }

    public string? Designer
    {
        get => GetValue(NameIdDesigner);
        set => SetValue(NameIdDesigner, value);
    }

    public string? Description
    {
        get => GetValue(NameIdDescription);
        set => SetValue(NameIdDescription, value);
    }

    public string? VendorUrl
    {
        get => GetValue(NameIdVendorUrl);
        set => SetValue(NameIdVendorUrl, value);
    }

    public string? DesignerUrl
    {
        get => GetValue(NameIdDesignerUrl);
        set => SetValue(NameIdDesignerUrl, value);
    }

    public string? LicenseDescription
    {
        get => GetValue(NameIdLicenseDescription);
        set => SetValue(NameIdLicenseDescription, value);
    }

    public string? LicenseUrl
    {
        get => GetValue(NameIdLicenseUrl);
        set => SetValue(NameIdLicenseUrl, value);
    }

    public string? WindowsFamily
    {
        get => GetValue(NameIdWindowsFamily);
        set => SetValue(NameIdWindowsFamily, value);
    }

    public string? WindowsStyle
    {
        get => GetValue(NameIdWindowsStyle);
        set => SetValue(NameIdWindowsStyle, value);
    }

    public string? MacosFamilyAndStyle
    {
        get => GetValue(NameIdMacosFamilyAndStyle);
        set => SetValue(NameIdMacosFamilyAndStyle, value);
    }

    public string? SampleText
    {
        get => GetValue(NameIdSampleText);
        set => SetValue(NameIdSampleText, value);
    }

    public string? PostscriptCid
    {
        get => GetValue(NameIdPostscriptCid);
        set => SetValue(NameIdPostscriptCid, value);
    }

    public string? WwsFamily
    {
        get => GetValue(NameIdWwsFamily);
        set => SetValue(NameIdWwsFamily, value);
    }

    public string? WwsStyle
    {
        get => GetValue(NameIdWwsStyle);
        set => SetValue(NameIdWwsStyle, value);
    }
}
