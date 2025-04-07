using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("KbitSpec.Tests")]
namespace KbitSpec.Internal;

internal static class Base64
{
    public static byte[] EncodeNoPadding(byte[] data)
    {
        var text = Convert.ToBase64String(data);
        text = text.TrimEnd('=');
        return Encoding.ASCII.GetBytes(text);
    }

    public static byte[] DecodeNoPadding(byte[] data)
    {
        var text = Encoding.ASCII.GetString(data);
        text = text.PadRight((int)Math.Ceiling(text.Length / 4.0) * 4, '=');
        return Convert.FromBase64String(text);
    }
}
