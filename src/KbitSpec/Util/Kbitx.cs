using System.Xml;

namespace KbitSpec.Util;

internal static class Kbitx
{
    public const string TagRoot = "kbits";
    public const string TagProp = "prop";
    public const string TagName = "name";
    public const string TagGlyph = "g";
    public const string TagKern = "k";

    public const string AttrId = "id";
    public const string AttrValue = "value";
    public const string AttrUnicode = "u";
    public const string AttrName = "n";
    public const string AttrX = "x";
    public const string AttrY = "y";
    public const string AttrAdvance = "w";
    public const string AttrData = "d";
    public const string AttrLeftUnicode = "lu";
    public const string AttrLeftName = "ln";
    public const string AttrRightUnicode = "ru";
    public const string AttrRightName = "rn";
    public const string AttrOffset = "o";

    public const string PropEmAscent = "emAscent";
    public const string PropEmDescent = "emDescent";
    public const string PropLineAscent = "lineAscent";
    public const string PropLineDescent = "lineDescent";
    public const string PropLineGap = "lineGap";
    public const string PropXHeight = "xHeight";
    public const string PropCapHeight = "capHeight";

    public const string XmlHeader = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n";
    public const string XmlDoctype = $"<!DOCTYPE {TagRoot} PUBLIC \"-//Kreative//DTD BitsNPicasBitmap 1.0//EN\" \"http://www.kreativekorp.com/dtd/kbitx.dtd\">\n";
    public const string XmlRootStart = $"<{TagRoot}>\n";
    public const string XmlRootClose = $"</{TagRoot}>\n";

    public static string? GetAttrString(XmlNode node, string key)
    {
        var value = node.Attributes?[key]?.Value;
        if (value is not null)
        {
            value = value.Trim();
            if (!"".Equals(value))
            {
                return value;
            }
        }
        return null;
    }

    public static int? GetAttrInt(XmlNode node, string key)
    {
        var value = GetAttrString(node, key);
        if (value is not null)
        {
            return Convert.ToInt32(value);
        }
        return null;
    }

    public static void WriteXmlTagLine(TextWriter writer, string tag, List<(string, object)> attrs)
    {
        writer.Write("<");
        writer.Write(tag);
        foreach (var (key, value) in attrs)
        {
            var stringValue = value.ToString()!.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&#34;").Replace("'", "&#39;");

            writer.Write(" ");
            writer.Write(key);
            writer.Write("=\"");
            writer.Write(stringValue);
            writer.Write("\"");
        }
        writer.Write("/>\n");
    }
}
