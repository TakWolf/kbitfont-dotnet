namespace KbitSpec.Util;

internal static class Kbits
{
    public static readonly byte[] MagicNumber = "KBnPbits"u8.ToArray();

    public const uint SpecVersion = 1;

    public static readonly byte[] BlockTypeName = "name"u8.ToArray();
    public static readonly byte[] BlockTypeChar = "char"u8.ToArray();
    public static readonly byte[] BlockTypeFin = "fin."u8.ToArray();
}
