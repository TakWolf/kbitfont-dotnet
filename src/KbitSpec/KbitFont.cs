using System.Text;
using System.Xml;
using KbitSpec.Error;
using KbitSpec.Util;

namespace KbitSpec;

public class KbitFont
{
    public static KbitFont ParseKbits(Stream stream)
    {
        if (!Kbits.MagicNumber.SequenceEqual(stream.ReadBytes(8)))
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

        while (true)
        {
            var blockType = stream.ReadBytes(4);
            if (Kbits.BlockTypeName.SequenceEqual(blockType))
            {
                if (stream.ReadUInt32() != Kbits.SpecVersion)
                {
                    throw new KbitsException("Bad spec version.");
                }
                var nameId = stream.ReadInt32();
                var value = stream.ReadUtf();
                font.Names[nameId] = value;
            }
            else if (Kbits.BlockTypeChar.SequenceEqual(blockType))
            {
                if (stream.ReadUInt32() != Kbits.SpecVersion)
                {
                    throw new KbitsException("Bad spec version.");
                }
                var codePoint = stream.ReadInt32();
                var advance = stream.ReadInt32();
                var x = stream.ReadInt32();
                var y = stream.ReadInt32();
                var bitmap = new List<List<byte>>();
                foreach (var _ in Enumerable.Range(0, (int)stream.ReadUInt32()))
                {
                    var bitmapRow = new List<byte>();
                    foreach (var __ in Enumerable.Range(0, (int)stream.ReadUInt32()))
                    {
                        bitmapRow.Add(stream.ReadUInt8());
                    }
                    bitmap.Add(bitmapRow);
                }
                font.Characters[codePoint] = new KbitGlyph(x, y, advance, bitmap);
            }
            else if (Kbits.BlockTypeFin.SequenceEqual(blockType))
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
        if (!Kbitx.TagRoot.Equals(root?.Name))
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
                            using var stream = new MemoryStream(Base64.DecodeNoPadding(Encoding.ASCII.GetBytes(data)));
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
                        if (leftCodePoint is not null)
                        {
                            if (rightCodePoint is not null)
                            {
                                font.KernPairs[new KbitKernKey(leftCodePoint, rightCodePoint)] = offset.Value;
                            }
                            else if (rightGlyphName is not null)
                            {
                                font.KernPairs[new KbitKernKey(leftCodePoint, rightGlyphName)] = offset.Value;
                            }
                        }
                        else if (leftGlyphName is not null)
                        {
                            if (rightCodePoint is not null)
                            {
                                font.KernPairs[new KbitKernKey(leftGlyphName, rightCodePoint)] = offset.Value;
                            }
                            else if (rightGlyphName is not null)
                            {
                                font.KernPairs[new KbitKernKey(leftGlyphName, rightGlyphName)] = offset.Value;
                            }
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
        stream.WriteBytes(Kbits.MagicNumber);
        stream.WriteUInt32(Kbits.SpecVersion);

        stream.WriteInt32(Props.EmAscent);
        stream.WriteInt32(Props.EmDescent);
        stream.WriteInt32(Props.LineAscent);
        stream.WriteInt32(Props.LineDescent);
        stream.WriteInt32(Props.LineGap);
        stream.WriteInt32(Props.XHeight);

        foreach (var (nameId, value) in Names)
        {
            stream.WriteBytes(Kbits.BlockTypeName);
            stream.WriteUInt32(Kbits.SpecVersion);
            stream.WriteInt32(nameId);
            stream.WriteUtf(value);
        }

        foreach (var (codePoint, glyph) in Characters)
        {
            stream.WriteBytes(Kbits.BlockTypeChar);
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

        stream.WriteBytes(Kbits.BlockTypeFin);
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
            var data = Encoding.ASCII.GetString(Base64.EncodeNoPadding(stream.ToArray()));
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
            var data = Encoding.ASCII.GetString(Base64.EncodeNoPadding(stream.ToArray()));
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
            switch (left)
            {
                case int when right is int:
                    Kbitx.WriteXmlTagLine(writer, Kbitx.TagKern, [
                        (Kbitx.AttrLeftUnicode, left),
                        (Kbitx.AttrRightUnicode, right),
                        (Kbitx.AttrOffset, offset)
                    ]);
                    break;
                case int when right is string:
                    Kbitx.WriteXmlTagLine(writer, Kbitx.TagKern, [
                        (Kbitx.AttrLeftUnicode, left),
                        (Kbitx.AttrRightName, right),
                        (Kbitx.AttrOffset, offset)
                    ]);
                    break;
                case string when right is int:
                    Kbitx.WriteXmlTagLine(writer, Kbitx.TagKern, [
                        (Kbitx.AttrLeftName, left),
                        (Kbitx.AttrRightUnicode, right),
                        (Kbitx.AttrOffset, offset)
                    ]);
                    break;
                case string when right is string:
                    Kbitx.WriteXmlTagLine(writer, Kbitx.TagKern, [
                        (Kbitx.AttrLeftName, left),
                        (Kbitx.AttrRightName, right),
                        (Kbitx.AttrOffset, offset)
                    ]);
                    break;
            }
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
        using var writer = new StreamWriter(path);
        DumpKbitx(writer);
    }
}
