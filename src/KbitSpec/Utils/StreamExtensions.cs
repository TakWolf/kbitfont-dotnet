using System.Buffers;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

[assembly: InternalsVisibleTo("KbitSpec.Tests")]
namespace KbitSpec.Utils;

internal static class StreamExtensions
{
    public static byte[] ReadBytes(this Stream stream, int size, bool throwOnEndOfStream = true)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(size);
        if (size == 0)
        {
            return [];
        }

        var buffer = new byte[size];
        var numRead = stream.ReadAtLeast(buffer, size, throwOnEndOfStream);
        if (numRead != size)
        {
            var newBuffer = new byte[numRead];
            Buffer.BlockCopy(buffer, 0, newBuffer, 0, numRead);
            buffer = newBuffer;
        }
        return buffer;
    }

    public static byte ReadUInt8(this Stream stream)
    {
        var b = stream.ReadByte();
        if (b == -1)
        {
            throw new EndOfStreamException("Unable to read beyond the end of the stream.");
        }
        return (byte)b;
    }

    public static sbyte ReadInt8(this Stream stream)
    {
        var b = stream.ReadByte();
        if (b == -1)
        {
            throw new EndOfStreamException("Unable to read beyond the end of the stream.");
        }
        return (sbyte)b;
    }

    public static ushort ReadUInt16(this Stream stream)
    {
        Span<byte> span = stackalloc byte[2];
        stream.ReadExactly(span);
        return BinaryPrimitives.ReadUInt16BigEndian(span);
    }

    public static short ReadInt16(this Stream stream)
    {
        Span<byte> span = stackalloc byte[2];
        stream.ReadExactly(span);
        return BinaryPrimitives.ReadInt16BigEndian(span);
    }

    public static uint ReadUInt32(this Stream stream)
    {
        Span<byte> span = stackalloc byte[4];
        stream.ReadExactly(span);
        return BinaryPrimitives.ReadUInt32BigEndian(span);
    }

    public static int ReadInt32(this Stream stream)
    {
        Span<byte> span = stackalloc byte[4];
        stream.ReadExactly(span);
        return BinaryPrimitives.ReadInt32BigEndian(span);
    }

    public static string ReadUtf(this Stream stream)
    {
        var size = stream.ReadUInt16();
        var buffer = ArrayPool<byte>.Shared.Rent(size);
        try
        {
            stream.ReadExactly(buffer, 0, size);
            return Encoding.UTF8.GetString(buffer, 0, size);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    public static uint ReadULeb128(this Stream stream)
    {
        var value = 0u;
        var shift = 0;
        for (var i = 0; i < 5; i++)
        {
            var data = stream.ReadUInt8();
            value |= (uint)((data & 0x7F) << shift);
            if ((data & 0x80) == 0)
            {
                return value;
            }
            shift += 7;
        }
        throw new InvalidDataException("ULeb128 too long.");
    }

    public static List<List<byte>> ReadBitmap(this Stream stream)
    {
        var height = (int)stream.ReadULeb128();
        var width = (int)stream.ReadULeb128();
        var repeatCount = 0;
        byte? repeatColor = null;
        var bitmap = new List<List<byte>>(height);
        for (var y = 0; y < height; y++)
        {
            var bitmapRow = new List<byte>(width);
            for (var x = 0; x < width; x++)
            {
                if (repeatCount <= 0)
                {
                    var data = stream.ReadUInt8();
                    repeatCount = data & 0x1F;
                    if ((data & 0x20) != 0)
                    {
                        repeatCount <<= 5;
                    }
                    var colorType = data & 0xC0;
                    repeatColor = colorType switch
                    {
                        0x00 => 0x00,
                        0x40 => 0xFF,
                        0x80 => stream.ReadUInt8(),
                        0xC0 => null,
                        _ => repeatColor
                    };
                }
                repeatCount -= 1;
                var color = repeatColor ?? stream.ReadUInt8();
                bitmapRow.Add(color);
            }
            bitmap.Add(bitmapRow);
        }
        return bitmap;
    }

    public static int WriteBytes(this Stream stream, ReadOnlySpan<byte> buffer)
    {
        if (buffer.IsEmpty)
        {
            return 0;
        }

        stream.Write(buffer);
        return buffer.Length;
    }

    public static int WriteUInt8(this Stream stream, byte value)
    {
        stream.WriteByte(value);
        return 1;
    }

    public static int WriteInt8(this Stream stream, sbyte value)
    {
        stream.WriteByte((byte)value);
        return 1;
    }

    public static int WriteUInt16(this Stream stream, ushort value)
    {
        Span<byte> span = stackalloc byte[2];
        BinaryPrimitives.WriteUInt16BigEndian(span, value);
        return stream.WriteBytes(span);
    }

    public static int WriteInt16(this Stream stream, short value)
    {
        Span<byte> span = stackalloc byte[2];
        BinaryPrimitives.WriteInt16BigEndian(span, value);
        return stream.WriteBytes(span);
    }

    public static int WriteUInt32(this Stream stream, uint value)
    {
        Span<byte> span = stackalloc byte[4];
        BinaryPrimitives.WriteUInt32BigEndian(span, value);
        return stream.WriteBytes(span);
    }

    public static int WriteInt32(this Stream stream, int value)
    {
        Span<byte> span = stackalloc byte[4];
        BinaryPrimitives.WriteInt32BigEndian(span, value);
        return stream.WriteBytes(span);
    }

    public static int WriteUtf(this Stream stream, string value)
    {
        if (value.Length == 0)
        {
            return stream.WriteUInt16(0);
        }

        var buffer = ArrayPool<byte>.Shared.Rent(Encoding.UTF8.GetMaxByteCount(value.Length));
        try
        {
            var size = Encoding.UTF8.GetBytes(value, buffer);
            stream.WriteUInt16((ushort)size);
            stream.Write(buffer, 0, size);
            return 2 + size;
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    public static int WriteULeb128(this Stream stream, uint value)
    {
        var size = 0;
        while (value >= 0x80)
        {
            size += stream.WriteUInt8((byte)((value | 0x80) & 0xFF));
            value >>= 7;
        }
        size += stream.WriteUInt8((byte)(value & 0xFF));
        return size;
    }

    private static int WriteBitmapRuns(this Stream stream, List<byte> noRepeatColors, int repeatCount, byte? repeatColor)
    {
        var size = 0;
        var colors = CollectionsMarshal.AsSpan(noRepeatColors);
        if (colors.Length > 0)
        {
            var n = colors.Length;
            var offset = 0;
            while (n >= 992)
            {
                size += stream.WriteUInt8(0xFF);
                size += stream.WriteBytes(colors.Slice(offset, 992));
                offset += 992;
                n -= 992;
            }
            if (n >= 32)
            {
                var m = n >> 5;
                size += stream.WriteUInt8((byte)(0xE0 | m));
                m <<= 5;
                size += stream.WriteBytes(colors.Slice(offset, m));
                offset += m;
                n -= m;
            }
            if (n > 1)
            {
                size += stream.WriteUInt8((byte)(0xC0 | n));
                size += stream.WriteBytes(colors.Slice(offset, n));
            }
            if (n == 1)
            {
                var color = colors[offset];
                switch (color)
                {
                    case 0x00:
                        size += stream.WriteUInt8(0x01);
                        break;
                    case 0xFF:
                        size += stream.WriteUInt8(0x41);
                        break;
                    default:
                        size += stream.WriteUInt8(0x81);
                        size += stream.WriteUInt8(color);
                        break;
                }
            }
        }
        if (repeatCount > 0)
        {
            byte basic = repeatColor switch
            {
                0x00 => 0x00,
                0xFF => 0x40,
                _ => 0x80
            };
            var pat = repeatColor != 0x00 && repeatColor != 0xFF;
            while (repeatCount >= 992)
            {
                size += stream.WriteUInt8((byte)(basic | 0x3F));
                if (pat)
                {
                    size += stream.WriteUInt8(repeatColor!.Value);
                }
                repeatCount -= 992;
            }
            if (repeatCount >= 32)
            {
                var m = repeatCount >> 5;
                size += stream.WriteUInt8((byte)(basic | 0x20 | m));
                m <<= 5;
                if (pat)
                {
                    size += stream.WriteUInt8(repeatColor!.Value);
                }
                repeatCount -= m;
            }
            if (repeatCount > 0)
            {
                size += stream.WriteUInt8((byte)(basic | repeatCount));
                if (pat)
                {
                    size += stream.WriteUInt8(repeatColor!.Value);
                }
            }
        }
        return size;
    }

    public static int WriteBitmap(this Stream stream, List<List<byte>> bitmap)
    {
        var size = 0;
        var height = bitmap.Count;
        var width = 0;
        if (height > 0)
        {
            foreach (var bitmapRow in bitmap)
            {
                width = Math.Max(bitmapRow.Count, width);
            }
        }
        size += stream.WriteULeb128((uint)height);
        size += stream.WriteULeb128((uint)width);
        var noRepeatColors = new List<byte>();
        var repeatCount = 0;
        byte? repeatColor = null;
        foreach (var bitmapRow in bitmap)
        {
            for (var x = 0; x < width; x++)
            {
                var color = x < bitmapRow.Count ? bitmapRow[x] : (byte)0x00;
                if (repeatCount <= 0)
                {
                    repeatCount = 1;
                    repeatColor = color;
                }
                else if (repeatColor == color)
                {
                    repeatCount += 1;
                }
                else if (repeatCount == 1)
                {
                    noRepeatColors.Add(repeatColor!.Value);
                    repeatColor = color;
                }
                else
                {
                    size += stream.WriteBitmapRuns(noRepeatColors, repeatCount, repeatColor!.Value);
                    noRepeatColors.Clear();
                    repeatCount = 1;
                    repeatColor = color;
                }
            }
        }
        if (repeatCount == 1)
        {
            noRepeatColors.Add(repeatColor!.Value);
            repeatCount = 0;
        }
        size += stream.WriteBitmapRuns(noRepeatColors, repeatCount, repeatColor);
        return size;
    }
}
