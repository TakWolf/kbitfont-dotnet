namespace KbitSpec;

public class KbitFont
{
    public static KbitFont ParseKbits(Stream stream)
    {
        // TODO
        return new KbitFont();
    }

    public static KbitFont ParseKbits(byte[] buffer)
    {
        using var stream = new MemoryStream(buffer);
        return ParseKbits(stream);
    }

    public static KbitFont LoadKbits(string path)
    {
        using var stream = File.OpenRead(path);
        return ParseKbits(stream);
    }

    public static KbitFont ParseKbitx(TextReader reader)
    {
        // TODO
        return new KbitFont();
    }

    public static KbitFont ParseKbitx(string text)
    {
        using var reader = new StringReader(text);
        return ParseKbitx(reader);
    }

    public static KbitFont LoadKbitx(string path)
    {
        using var reader = new StreamReader(path);
        return ParseKbitx(reader);
    }

    public KbitProps Props;
    public KbitNames Names;
    public SortedDictionary<int, KbitGlyph> Characters;
    public SortedDictionary<string, KbitGlyph> NamedGlyphs;
    public SortedDictionary<KbitKernKey, int> KernPairs;

    public KbitFont(
        KbitProps? props = null,
        KbitNames? names = null,
        SortedDictionary<int, KbitGlyph>? characters = null,
        SortedDictionary<string, KbitGlyph>? namedGlyphs = null,
        SortedDictionary<KbitKernKey, int>? kernPairs = null)
    {
        Props = props ?? new KbitProps();
        Names = names ?? new KbitNames();
        Characters = characters ?? new SortedDictionary<int, KbitGlyph>();
        NamedGlyphs = namedGlyphs ?? new SortedDictionary<string, KbitGlyph>();
        KernPairs = kernPairs ?? new SortedDictionary<KbitKernKey, int>();
    }

    public void DumpKbits(Stream stream)
    {
        // TODO
    }

    public byte[] DumpKbitsToBytes()
    {
        using var stream = new MemoryStream();
        DumpKbits(stream);
        return stream.ToArray();
    }

    public void SaveKbits(string path)
    {
        using var stream = File.OpenWrite(path);
        DumpKbits(stream);
    }

    public void DumpKbitx(TextWriter writer)
    {
        // TODO
    }

    public string DumpKbitxToString()
    {
        using var writer = new StringWriter();
        DumpKbitx(writer);
        return writer.ToString();
    }

    public void SaveKbitx(string path)
    {
        using var writer = new StreamWriter(path);
        DumpKbitx(writer);
    }
}
