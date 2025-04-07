using System.Buffers.Binary;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("KbitSpec.Tests")]
namespace KbitSpec.Internal;

internal static class StreamExtensions
{
    public static byte[] ReadBuffer(this Stream stream, long count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);
        if (count == 0)
        {
            return [];
        }
        var buffer = new byte[count];
        var numRead = stream.ReadAtLeast(buffer, buffer.Length);
        if (numRead != buffer.Length)
        {
            var copy = new byte[numRead];
            Buffer.BlockCopy(buffer, 0, copy, 0, numRead);
            buffer = copy;
        }
        return buffer;
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

    public static short ReadInt16(this Stream stream) => BinaryPrimitives.ReadInt16BigEndian(stream.ReadBuffer(2));

    public static int ReadInt32(this Stream stream) => BinaryPrimitives.ReadInt32BigEndian(stream.ReadBuffer(4));

    public static byte ReadUInt8(this Stream stream)
    {
        var b = stream.ReadByte();
        if (b == -1)
        {
            throw new EndOfStreamException("Unable to read beyond the end of the stream.");
        }
        return (byte)b;
    }

    public static ushort ReadUInt16(this Stream stream) => BinaryPrimitives.ReadUInt16BigEndian(stream.ReadBuffer(2));

    public static uint ReadUInt32(this Stream stream) => BinaryPrimitives.ReadUInt32BigEndian(stream.ReadBuffer(4));

    public static string ReadUtf(this Stream stream)
    {
        var count = stream.ReadUInt16();
        return Encoding.UTF8.GetString(stream.ReadBuffer(count));
    }

    public static uint ReadULeb128(this Stream stream)
    {
        var value = 0u;
        var shift = 0;
        while (true)
        {
            var data = stream.ReadUInt8();
            value |= (uint)((data & 0x7F) << shift);
            if ((data & 0x80) == 0)
            {
                break;
            }
            shift += 7;
        }
        return value;
    }

    public static List<List<byte>> ReadBitmap(this Stream stream)
    {
        var bitmap = new List<List<byte>>();
        var height = stream.ReadULeb128();
        var width = stream.ReadULeb128();
        var repeatCount = 0;
        byte? repeatColor = null;
        foreach (var _ in Enumerable.Range(0, (int)height))
        {
            var bitmapRow = new List<byte>();
            foreach (var __ in Enumerable.Range(0, (int)width))
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

    public static int WriteBuffer(this Stream stream, ReadOnlySpan<byte> values)
    {
        stream.Write(values);
        return values.Length;
    }

    public static int WriteInt8(this Stream stream, sbyte value)
    {
        stream.WriteByte((byte)value);
        return 1;
    }

    public static int WriteInt16(this Stream stream, short value)
    {
        var buffer = new byte[2];
        BinaryPrimitives.WriteInt16BigEndian(buffer, value);
        return stream.WriteBuffer(buffer);
    }

    public static int WriteInt32(this Stream stream, int value)
    {
        var buffer = new byte[4];
        BinaryPrimitives.WriteInt32BigEndian(buffer, value);
        return stream.WriteBuffer(buffer);
    }

    public static int WriteUInt8(this Stream stream, byte value)
    {
        stream.WriteByte(value);
        return 1;
    }

    public static int WriteUInt16(this Stream stream, ushort value)
    {
        var buffer = new byte[2];
        BinaryPrimitives.WriteUInt16BigEndian(buffer, value);
        return stream.WriteBuffer(buffer);
    }

    public static int WriteUInt32(this Stream stream, uint value)
    {
        var buffer = new byte[4];
        BinaryPrimitives.WriteUInt32BigEndian(buffer, value);
        return stream.WriteBuffer(buffer);
    }

    public static int WriteUtf(this Stream stream, string value)
    {
        var data = Encoding.UTF8.GetBytes(value);
        return stream.WriteUInt16((ushort)data.Length) + stream.WriteBuffer(data);
    }

    public static int WriteULeb128(this Stream stream, uint value)
    {
        var count = 0;
        while (value >= 0x80)
        {
            count += stream.WriteUInt8((byte)((value | 0x80) & 0xFF));
            value >>= 7;
        }
        count += stream.WriteUInt8((byte)(value & 0xFF));
        return count;
    }

    private static int WriteBitmapRuns(this Stream stream, byte[] noRepeatColors, int repeatCount, byte? repeatColor)
    {
        var count = 0;
        if (noRepeatColors.Length > 0)
        {
            var n = noRepeatColors.Length;
            var offset = 0;
            while (n >= 992)
            {
                count += stream.WriteUInt8(0xFF);
                count += stream.WriteBuffer(noRepeatColors.AsSpan(offset, 992));
                offset += 992;
                n -= 992;
            }
            if (n >= 32)
            {
                var m = n >> 5;
                count += stream.WriteUInt8((byte)(0xE0 | m));
                m <<= 5;
                count += stream.WriteBuffer(noRepeatColors.AsSpan(offset, m));
                offset += m;
                n -= m;
            }
            if (n > 1)
            {
                count += stream.WriteUInt8((byte)(0xC0 | n));
                count += stream.WriteBuffer(noRepeatColors.AsSpan(offset, n));
            }
            if (n == 1)
            {
                var color = noRepeatColors[offset];
                switch (color)
                {
                    case 0x00:
                        count += stream.WriteUInt8(0x01);
                        break;
                    case 0xFF:
                        count += stream.WriteUInt8(0x41);
                        break;
                    default:
                        count += stream.WriteUInt8(0x81);
                        count += stream.WriteUInt8(color);
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
                count += stream.WriteUInt8((byte)(basic | 0x3F));
                if (pat)
                {
                    count += stream.WriteUInt8(repeatColor!.Value);
                }
                repeatCount -= 992;
            }
            if (repeatCount >= 32)
            {
                var m = repeatCount >> 5;
                count += stream.WriteUInt8((byte)(basic | 0x20 | m));
                m <<= 5;
                if (pat)
                {
                    count += stream.WriteUInt8(repeatColor!.Value);
                }
                repeatCount -= m;
            }
            if (repeatCount > 0)
            {
                count += stream.WriteUInt8((byte)(basic | repeatCount));
                if (pat)
                {
                    count += stream.WriteUInt8(repeatColor!.Value);
                }
            }
        }
        return count;
    }

    public static int WriteBitmap(this Stream stream, List<List<byte>> bitmap)
    {
        var count = 0;
        var height = bitmap.Count;
        var width = height > 0 ? bitmap.Select(bitmapRow => bitmapRow.Count).Max() : 0;
        count += stream.WriteULeb128((uint)height);
        count += stream.WriteULeb128((uint)width);
        var noRepeatColors = new List<byte>();
        var repeatCount = 0;
        byte? repeatColor = null;
        foreach (var bitmapRow in bitmap)
        {
            var bitmapRowCopyed = new List<byte>(bitmapRow);
            if (bitmapRowCopyed.Count < width)
            {
                bitmapRowCopyed.AddRange(Enumerable.Repeat((byte)0x00, width - bitmapRowCopyed.Count));
            }
            foreach (var color in bitmapRowCopyed)
            {
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
                    count += stream.WriteBitmapRuns(noRepeatColors.ToArray(), repeatCount, repeatColor!.Value);
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
        count += stream.WriteBitmapRuns(noRepeatColors.ToArray(), repeatCount, repeatColor);
        return count;
    }
}
