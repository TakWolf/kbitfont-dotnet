using System.Buffers.Binary;

namespace KbitSpec.Utils;

internal static class Kbits
{
    public static readonly ulong MagicNumber = BinaryPrimitives.ReadUInt64LittleEndian("KBnPbits"u8);

    public const uint SpecVersion = 1;

    public static readonly uint BlockTypeName = BinaryPrimitives.ReadUInt32LittleEndian("name"u8);
    public static readonly uint BlockTypeChar = BinaryPrimitives.ReadUInt32LittleEndian("char"u8);
    public static readonly uint BlockTypeFin = BinaryPrimitives.ReadUInt32LittleEndian("fin."u8);
}
