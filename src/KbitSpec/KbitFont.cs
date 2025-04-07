namespace KbitSpec;

public class KbitFont
{
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
}
