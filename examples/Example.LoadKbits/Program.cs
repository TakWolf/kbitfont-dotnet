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
