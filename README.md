# KbitFont.NET

[![.NET](https://img.shields.io/badge/dotnet-8.0-mediumpurple)](https://dotnet.microsoft.com)
[![NuGet](https://img.shields.io/nuget/v/KbitFont)](https://www.nuget.org/packages/KbitFont)

KbitFont is a library for parsing [Bits'N'Picas](https://github.com/kreativekorp/bitsnpicas) native save format files (`.kbits` and `.kbitx`).

## Installation

```shell
dotnet add package KbitFont
```

## Usage

### Create

```csharp
using KbitSpec;

var outputsDir = Path.Combine("build");
if (Directory.Exists(outputsDir))
{
    Directory.Delete(outputsDir, true);
}
Directory.CreateDirectory(outputsDir);

var font = new KbitFont();
font.Props.EmAscent = 14;
font.Props.EmDescent = 2;
font.Props.LineAscent = 14;
font.Props.LineDescent = 2;
font.Props.XHeight = 7;
font.Props.CapHeight = 10;

font.Names.Version = "1.0.0";
font.Names.Family = "My Font";
font.Names.Style = "Regular";
font.Names.Manufacturer = "Pixel Font Studio";
font.Names.Designer = "TakWolf";
font.Names.Description = "A pixel font";
font.Names.Copyright = "Copyright (c) TakWolf";
font.Names.LicenseDescription = "This Font Software is licensed under the SIL Open Font License, Version 1.1";
font.Names.VendorUrl = "https://github.com/TakWolf/kbitfont-dotnet";
font.Names.DesignerUrl = "https://takwolf.com";
font.Names.LicenseUrl = "https://openfontlicense.org";

font.Characters[65] = new KbitGlyph(
    x: 0,
    y: 14,
    advance: 8,
    bitmap: [
        [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00],
        [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00],
        [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00],
        [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00],
        [0x00, 0x00, 0x00, 0xFF, 0xFF, 0x00, 0x00, 0x00],
        [0x00, 0x00, 0xFF, 0x00, 0x00, 0xFF, 0x00, 0x00],
        [0x00, 0x00, 0xFF, 0x00, 0x00, 0xFF, 0x00, 0x00],
        [0x00, 0xFF, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x00],
        [0x00, 0xFF, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x00],
        [0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x00],
        [0x00, 0xFF, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x00],
        [0x00, 0xFF, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x00],
        [0x00, 0xFF, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x00],
        [0x00, 0xFF, 0x00, 0x00, 0x00, 0x00, 0xFF, 0x00],
        [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00],
        [0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00]
    ]);

font.NamedGlyphs[".notdef"] = new KbitGlyph(
    x: 0,
    y: 14,
    advance: 8,
    bitmap: [
        [0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF],
        [0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF],
        [0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF],
        [0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF],
        [0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF],
        [0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF],
        [0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF],
        [0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF],
        [0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF],
        [0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF],
        [0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF],
        [0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF],
        [0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF],
        [0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF],
        [0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xFF],
        [0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF]
    ]);

font.SaveKbits(Path.Combine(outputsDir, "my-font.kbits"));
font.SaveKbitx(Path.Combine(outputsDir, "my-font.kbitx"));
```

### Load Kbits

```csharp
using KbitSpec;

var outputsDir = Path.Combine("build");
if (Directory.Exists(outputsDir))
{
    Directory.Delete(outputsDir, true);
}
Directory.CreateDirectory(outputsDir);

var font = KbitFont.LoadKbits(Path.Combine("assets", "macintosh", "Athens.kbits"));
Console.WriteLine($"name: {font.Names.Family}");
Console.WriteLine($"size: {font.Props.EmHeight}");
Console.WriteLine($"ascent: {font.Props.LineAscent}");
Console.WriteLine($"descent: {font.Props.LineDescent}");
Console.WriteLine();
foreach (var (codePoint, glyph) in font.Characters)
{
    Console.WriteLine($"char: {char.ConvertFromUtf32(codePoint)} ({codePoint:X4})");
    Console.WriteLine($"xy: {(glyph.X, glyph.Y)}");
    Console.WriteLine($"dimensions: {glyph.Dimensions}");
    Console.WriteLine($"advance: {glyph.Advance}");
    foreach (var bitmapRow in glyph.Bitmap)
    {
        var text = string.Join("", bitmapRow.Select(color => color <= 127 ? "  " : "██"));
        Console.WriteLine($"{text}*");
    }
    Console.WriteLine();
}
font.SaveKbits(Path.Combine(outputsDir, "Athens.kbits"));
```

### Load Kbitx

```csharp
using KbitSpec;

var outputsDir = Path.Combine("build");
if (Directory.Exists(outputsDir))
{
    Directory.Delete(outputsDir, true);
}
Directory.CreateDirectory(outputsDir);

var font = KbitFont.LoadKbitx(Path.Combine("assets", "macintosh", "Athens.kbitx"));
Console.WriteLine($"name: {font.Names.Family}");
Console.WriteLine($"size: {font.Props.EmHeight}");
Console.WriteLine($"ascent: {font.Props.LineAscent}");
Console.WriteLine($"descent: {font.Props.LineDescent}");
Console.WriteLine();
foreach (var (codePoint, glyph) in font.Characters)
{
    Console.WriteLine($"char: {char.ConvertFromUtf32(codePoint)} ({codePoint:X4})");
    Console.WriteLine($"xy: {(glyph.X, glyph.Y)}");
    Console.WriteLine($"dimensions: {glyph.Dimensions}");
    Console.WriteLine($"advance: {glyph.Advance}");
    foreach (var bitmapRow in glyph.Bitmap)
    {
        var text = string.Join("", bitmapRow.Select(color => color <= 127 ? "  " : "██"));
        Console.WriteLine($"{text}*");
    }
    Console.WriteLine();
}
font.SaveKbitx(Path.Combine(outputsDir, "Athens.kbitx"));
```

## Specifications

### Font Struct

- [Font.java](https://github.com/TakWolf/kbitfont-spec/blob/master/bitsnpicas/src/main/java/com/kreative/bitsnpicas/Font.java)
- [BitmapFont.java](https://github.com/TakWolf/kbitfont-spec/blob/master/bitsnpicas/src/main/java/com/kreative/bitsnpicas/BitmapFont.java)

### Kbits

- [KbitsBitmapFontImporter.java](https://github.com/TakWolf/kbitfont-spec/blob/master/bitsnpicas/src/main/java/com/kreative/bitsnpicas/importer/KbitsBitmapFontImporter.java)
- [KbitsBitmapFontExporter.java](https://github.com/TakWolf/kbitfont-spec/blob/master/bitsnpicas/src/main/java/com/kreative/bitsnpicas/exporter/KbitsBitmapFontExporter.java)

### Kbitx

- [KbitxBitmapFontImporter.java](https://github.com/TakWolf/kbitfont-spec/blob/master/bitsnpicas/src/main/java/com/kreative/bitsnpicas/importer/KbitxBitmapFontImporter.java)
- [KbitxBitmapFontExporter.java](https://github.com/TakWolf/kbitfont-spec/blob/master/bitsnpicas/src/main/java/com/kreative/bitsnpicas/exporter/KbitxBitmapFontExporter.java)

## License

[MIT License](LICENSE)
