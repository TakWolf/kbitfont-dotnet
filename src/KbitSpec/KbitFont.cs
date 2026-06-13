using System.Xml;
using KbitSpec.Errors;
using KbitSpec.Utils;

namespace KbitSpec;

public class KbitFont : ICopyable<KbitFont>, IEquatable<KbitFont>
{
    public static KbitFont ParseKbits(Stream stream)
    {
        if (stream.ReadMagicNumber() != Kbits.MagicNumber)
        {
            throw new KbitsException("Bad magic number.");
        }
        if (stream.ReadUInt32() != Kbits.SpecVersion)
        {
            throw new KbitsException("Bad spec version.");
        }

        var font = new KbitFont();
        font.Props.EmAscent = stream.ReadInt32();
        font.Props.EmDescent = stream.ReadInt32();
        font.Props.LineAscent = stream.ReadInt32();
        font.Props.LineDescent = stream.ReadInt32();
        font.Props.LineGap = stream.ReadInt32();
        font.Props.XHeight = stream.ReadInt32();
        font.Props.CapHeight = font.Props.XHeight;
        font.Props.NewGlyphWidth = font.Props.EmAscent + font.Props.EmDescent;

        while (true)
        {
            var blockType = stream.ReadBlockType();
            if (blockType == Kbits.BlockTypeName)
            {
                if (stream.ReadUInt32() != Kbits.SpecVersion)
                {
                    throw new KbitsException("Bad spec version.");
                }

                var nameId = stream.ReadInt32();
                var value = stream.ReadUtf();
                font.Names[nameId] = value;
            }
            else if (blockType == Kbits.BlockTypeChar)
            {
                if (stream.ReadUInt32() != Kbits.SpecVersion)
                {
                    throw new KbitsException("Bad spec version.");
                }

                var codePoint = stream.ReadInt32();
                var advance = stream.ReadInt32();
                var x = stream.ReadInt32();
                var y = stream.ReadInt32();
                var height = (int)stream.ReadUInt32();

                var bitmap = new List<List<byte>>(height);
                for (var i = 0; i < height; i++)
                {
                    var width = (int)stream.ReadUInt32();
                    var bitmapRow = new List<byte>(width);
                    for (var j = 0; j < width; j++)
                    {
                        bitmapRow.Add(stream.ReadUInt8());
                    }
                    bitmap.Add(bitmapRow);
                }

                font.Characters[codePoint] = new KbitGlyph(x, y, advance, bitmap);
            }
            else if (blockType == Kbits.BlockTypeFin)
            {
                break;
            }
            else
            {
                throw new KbitsException($"Bad block type: {blockType}");
            }
        }

        return font;
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
        var document = new XmlDocument();
        document.Load(reader);
        var root = document.DocumentElement;
        if (root?.Name is not Kbitx.TagRoot)
        {
            throw new KbitxException($"Unknown root: {root?.Name}");
        }

        var font = new KbitFont();
        foreach (XmlNode node in root.ChildNodes)
        {
            switch (node.Name)
            {
                case Kbitx.TagProp:
                    {
                        var value = Kbitx.GetAttrInt(node, Kbitx.AttrValue);
                        if (value is null)
                        {
                            continue;
                        }

                        var name = Kbitx.GetAttrString(node, Kbitx.AttrId);
                        switch (name)
                        {
                            case Kbitx.PropEmAscent:
                                font.Props.EmAscent = value.Value;
                                break;
                            case Kbitx.PropEmDescent:
                                font.Props.EmDescent = value.Value;
                                break;
                            case Kbitx.PropLineAscent:
                                font.Props.LineAscent = value.Value;
                                break;
                            case Kbitx.PropLineDescent:
                                font.Props.LineDescent = value.Value;
                                break;
                            case Kbitx.PropLineGap:
                                font.Props.LineGap = value.Value;
                                break;
                            case Kbitx.PropXHeight:
                                font.Props.XHeight = value.Value;
                                break;
                            case Kbitx.PropCapHeight:
                                font.Props.CapHeight = value.Value;
                                break;
                            case Kbitx.PropNewGlyphWidth:
                                font.Props.NewGlyphWidth = value.Value;
                                break;
                        }
                        break;
                    }
                case Kbitx.TagName:
                    {
                        var nameId = Kbitx.GetAttrInt(node, Kbitx.AttrId);
                        var value = Kbitx.GetAttrString(node, Kbitx.AttrValue);
                        if (nameId is not null && value is not null)
                        {
                            font.Names[nameId.Value] = value;
                        }
                        break;
                    }
                case Kbitx.TagGlyph:
                    {
                        var codePoint = Kbitx.GetAttrInt(node, Kbitx.AttrUnicode);
                        var glyphName = Kbitx.GetAttrString(node, Kbitx.AttrName);
                        if (codePoint is null && glyphName is null)
                        {
                            continue;
                        }

                        var x = Kbitx.GetAttrInt(node, Kbitx.AttrX) ?? 0;
                        var y = Kbitx.GetAttrInt(node, Kbitx.AttrY) ?? 0;
                        var advance = Kbitx.GetAttrInt(node, Kbitx.AttrAdvance) ?? 0;
                        var data = Kbitx.GetAttrString(node, Kbitx.AttrData);

                        List<List<byte>>? bitmap = null;
                        if (data is not null)
                        {
                            using var stream = new MemoryStream(Base64.DecodeNoPadding(data));
                            bitmap = stream.ReadBitmap();
                        }

                        var glyph = new KbitGlyph(x, y, advance, bitmap);
                        if (codePoint is not null)
                        {
                            font.Characters[codePoint.Value] = glyph;
                        }
                        else if (glyphName is not null)
                        {
                            font.NamedGlyphs[glyphName] = glyph;
                        }
                        break;
                    }
                case Kbitx.TagKern:
                    {
                        var offset = Kbitx.GetAttrInt(node, Kbitx.AttrOffset);
                        if (offset is null)
                        {
                            continue;
                        }

                        var leftCodePoint = Kbitx.GetAttrInt(node, Kbitx.AttrLeftUnicode);
                        var leftGlyphName = Kbitx.GetAttrString(node, Kbitx.AttrLeftName);
                        var rightCodePoint = Kbitx.GetAttrInt(node, Kbitx.AttrRightUnicode);
                        var rightGlyphName = Kbitx.GetAttrString(node, Kbitx.AttrRightName);

                        (KbitGlyphKey, KbitGlyphKey)? keys = (leftCodePoint, leftGlyphName, rightCodePoint, rightGlyphName) switch
                        {
                            (not null, _, not null, _) => (leftCodePoint.Value, rightCodePoint.Value),
                            (_, not null, _, not null) => (leftGlyphName, rightGlyphName),
                            (not null, _, _, not null) => (leftCodePoint.Value, rightGlyphName),
                            (_, not null, not null, _) => (leftGlyphName, rightCodePoint.Value),
                            _ => null
                        };
                        if (keys is not null)
                        {
                            font.KernPairs[keys.Value] = offset.Value;
                        }
                        break;
                    }
            }
        }
        return font;
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

    public KbitProps Props { get; set; }
    public KbitNames Names { get; set; }
    public SortedDictionary<int, KbitGlyph> Characters { get; set; }
    public SortedDictionary<string, KbitGlyph> NamedGlyphs { get; set; }
    public SortedDictionary<(KbitGlyphKey, KbitGlyphKey), int> KernPairs { get; set; }

    public KbitFont(
        KbitProps? props = null,
        KbitNames? names = null,
        SortedDictionary<int, KbitGlyph>? characters = null,
        SortedDictionary<string, KbitGlyph>? namedGlyphs = null,
        SortedDictionary<(KbitGlyphKey, KbitGlyphKey), int>? kernPairs = null)
    {
        Props = props ?? new KbitProps();
        Names = names ?? new KbitNames();
        Characters = characters ?? new SortedDictionary<int, KbitGlyph>();
        NamedGlyphs = namedGlyphs ?? new SortedDictionary<string, KbitGlyph>();
        KernPairs = kernPairs ?? new SortedDictionary<(KbitGlyphKey, KbitGlyphKey), int>();
    }

    public void DumpKbits(Stream stream)
    {
        stream.WriteMagicNumber(Kbits.MagicNumber);
        stream.WriteUInt32(Kbits.SpecVersion);

        stream.WriteInt32(Props.EmAscent);
        stream.WriteInt32(Props.EmDescent);
        stream.WriteInt32(Props.LineAscent);
        stream.WriteInt32(Props.LineDescent);
        stream.WriteInt32(Props.LineGap);
        stream.WriteInt32(Props.XHeight);

        foreach (var (nameId, value) in Names)
        {
            stream.WriteBlockType(Kbits.BlockTypeName);
            stream.WriteUInt32(Kbits.SpecVersion);
            stream.WriteInt32(nameId);
            stream.WriteUtf(value);
        }

        foreach (var (codePoint, glyph) in Characters)
        {
            stream.WriteBlockType(Kbits.BlockTypeChar);
            stream.WriteUInt32(Kbits.SpecVersion);
            stream.WriteInt32(codePoint);
            stream.WriteInt32(glyph.Advance);
            stream.WriteInt32(glyph.X);
            stream.WriteInt32(glyph.Y);
            stream.WriteUInt32((uint)glyph.Bitmap.Count);
            foreach (var bitmapRow in glyph.Bitmap)
            {
                stream.WriteUInt32((uint)bitmapRow.Count);
                foreach (var color in bitmapRow)
                {
                    stream.WriteUInt8(color);
                }
            }
        }

        stream.WriteBlockType(Kbits.BlockTypeFin);
    }

    public byte[] DumpKbitsToBytes()
    {
        using var stream = new MemoryStream();
        DumpKbits(stream);
        return stream.ToArray();
    }

    public void SaveKbits(string path)
    {
        using var stream = File.Create(path);
        DumpKbits(stream);
    }

    public void DumpKbitx(TextWriter writer)
    {
        writer.Write(Kbitx.XmlHeader);
        writer.Write(Kbitx.XmlDoctype);
        writer.Write(Kbitx.XmlRootStart);

        Kbitx.WriteXmlTagLine(writer, Kbitx.TagProp, [
            (Kbitx.AttrId, Kbitx.PropEmAscent),
            (Kbitx.AttrValue, Props.EmAscent)
        ]);
        Kbitx.WriteXmlTagLine(writer, Kbitx.TagProp, [
            (Kbitx.AttrId, Kbitx.PropEmDescent),
            (Kbitx.AttrValue, Props.EmDescent)
        ]);
        Kbitx.WriteXmlTagLine(writer, Kbitx.TagProp, [
            (Kbitx.AttrId, Kbitx.PropLineAscent),
            (Kbitx.AttrValue, Props.LineAscent)
        ]);
        Kbitx.WriteXmlTagLine(writer, Kbitx.TagProp, [
            (Kbitx.AttrId, Kbitx.PropLineDescent),
            (Kbitx.AttrValue, Props.LineDescent)
        ]);
        Kbitx.WriteXmlTagLine(writer, Kbitx.TagProp, [
            (Kbitx.AttrId, Kbitx.PropLineGap),
            (Kbitx.AttrValue, Props.LineGap)
        ]);
        Kbitx.WriteXmlTagLine(writer, Kbitx.TagProp, [
            (Kbitx.AttrId, Kbitx.PropXHeight),
            (Kbitx.AttrValue, Props.XHeight)
        ]);
        Kbitx.WriteXmlTagLine(writer, Kbitx.TagProp, [
            (Kbitx.AttrId, Kbitx.PropCapHeight),
            (Kbitx.AttrValue, Props.CapHeight)
        ]);
        Kbitx.WriteXmlTagLine(writer, Kbitx.TagProp, [
            (Kbitx.AttrId, Kbitx.PropNewGlyphWidth),
            (Kbitx.AttrValue, Props.NewGlyphWidth)
        ]);

        foreach (var (nameId, value) in Names)
        {
            Kbitx.WriteXmlTagLine(writer, Kbitx.TagName, [
                (Kbitx.AttrId, nameId),
                (Kbitx.AttrValue, value)
            ]);
        }

        foreach (var (codePoint, glyph) in Characters)
        {
            using var stream = new MemoryStream();
            stream.WriteBitmap(glyph.Bitmap);
            var data = Base64.EncodeNoPadding(stream.ToArray());
            Kbitx.WriteXmlTagLine(writer, Kbitx.TagGlyph, [
                (Kbitx.AttrUnicode, codePoint),
                (Kbitx.AttrX, glyph.X),
                (Kbitx.AttrY, glyph.Y),
                (Kbitx.AttrAdvance, glyph.Advance),
                (Kbitx.AttrData, data)
            ]);
        }

        foreach (var (glyphName, glyph) in NamedGlyphs)
        {
            using var stream = new MemoryStream();
            stream.WriteBitmap(glyph.Bitmap);
            var data = Base64.EncodeNoPadding(stream.ToArray());
            Kbitx.WriteXmlTagLine(writer, Kbitx.TagGlyph, [
                (Kbitx.AttrName, glyphName),
                (Kbitx.AttrX, glyph.X),
                (Kbitx.AttrY, glyph.Y),
                (Kbitx.AttrAdvance, glyph.Advance),
                (Kbitx.AttrData, data)
            ]);
        }

        foreach (var ((left, right), offset) in KernPairs)
        {
            Kbitx.WriteXmlTagLine(writer, Kbitx.TagKern, (left, right) switch
            {
                ({ IsInt: true }, { IsInt: true }) => [
                    (Kbitx.AttrLeftUnicode, left.AsInt()),
                    (Kbitx.AttrRightUnicode, right.AsInt()),
                    (Kbitx.AttrOffset, offset)
                ],
                ({ IsString: true }, { IsString: true }) => [
                    (Kbitx.AttrLeftName, left.AsString()),
                    (Kbitx.AttrRightName, right.AsString()),
                    (Kbitx.AttrOffset, offset)
                ],
                ({ IsInt: true }, { IsString: true }) => [
                    (Kbitx.AttrLeftUnicode, left.AsInt()),
                    (Kbitx.AttrRightName, right.AsString()),
                    (Kbitx.AttrOffset, offset)
                ],
                ({ IsString: true }, { IsInt: true }) => [
                    (Kbitx.AttrLeftName, left.AsString()),
                    (Kbitx.AttrRightUnicode, right.AsInt()),
                    (Kbitx.AttrOffset, offset)
                ],
                _ => throw new InvalidOperationException()
            });
        }

        writer.Write(Kbitx.XmlRootClose);
    }

    public string DumpKbitxToString()
    {
        using var writer = new StringWriter();
        DumpKbitx(writer);
        return writer.ToString();
    }

    public void SaveKbitx(string path)
    {
        using var writer = File.CreateText(path);
        DumpKbitx(writer);
    }

    public KbitFont Copy() => new(
        Props,
        Names,
        Characters,
        NamedGlyphs,
        KernPairs);

    public KbitFont DeepCopy() => new(
        Props.DeepCopy(),
        Names.DeepCopy(),
        CopyUtil.DeepCopySortedDictionary(Characters),
        CopyUtil.DeepCopySortedDictionary(NamedGlyphs),
        new SortedDictionary<(KbitGlyphKey, KbitGlyphKey), int>(KernPairs));

    public bool Equals(KbitFont? other)
    {
        if (other is null)
        {
            return false;
        }
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        return Props.Equals(other.Props) &&
               Names.Equals(other.Names) &&
               EqualUtil.DictionaryEquals(Characters, other.Characters) &&
               EqualUtil.DictionaryEquals(NamedGlyphs, other.NamedGlyphs) &&
               EqualUtil.DictionaryEquals(KernPairs, other.KernPairs);
    }

    public override bool Equals(object? other)
    {
        if (other is null)
        {
            return false;
        }
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        if (other.GetType() != GetType())
        {
            return false;
        }
        return Equals((KbitFont)other);
    }

    public override int GetHashCode() => 0;
}
