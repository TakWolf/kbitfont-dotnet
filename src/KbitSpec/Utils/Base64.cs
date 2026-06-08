using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("KbitSpec.Tests")]
namespace KbitSpec.Utils;

internal static class Base64
{
    public static string EncodeNoPadding(byte[] data)
    {
        var text = Convert.ToBase64String(data);
        text = text.TrimEnd('=');
        return text;
    }

    public static byte[] DecodeNoPadding(string text)
    {
        text = text.PadRight((text.Length + 3) / 4 * 4, '=');
        return Convert.FromBase64String(text);
    }
}
