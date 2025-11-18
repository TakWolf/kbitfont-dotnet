using KbitSpec.Utils;

namespace KbitSpec.Tests;

public class StreamExtensionsTests
{
    [Fact]
    public void TestBytes()
    {
        var stream = new MemoryStream();
        Assert.Equal(11, stream.WriteBytes("Hello World"u8));
        Assert.Equal(11, stream.Position);
        stream.Seek(0, SeekOrigin.Begin);
        Assert.Equal("Hello World"u8, stream.ReadBytes(11));
        Assert.Equal(11, stream.Position);
    }

    [Fact]
    public void TestEof()
    {
        var stream = new MemoryStream();
        stream.WriteBytes("ABC"u8);
        Assert.Throws<EndOfStreamException>(() => stream.ReadBytes(4));
        stream.Seek(0, SeekOrigin.Begin);
        Assert.Equal("ABC"u8, stream.ReadBytes(4, throwOnEndOfStream: false));
    }

    [Fact]
    public void TestUInt8()
    {
        var stream = new MemoryStream();
        Assert.Equal(1, stream.WriteUInt8(0x00));
        Assert.Equal(1, stream.WriteUInt8(0xFF));
        Assert.Equal(2, stream.Position);
        stream.Seek(0, SeekOrigin.Begin);
        Assert.Equal(0x00, stream.ReadUInt8());
        Assert.Equal(0xFF, stream.ReadUInt8());
        Assert.Equal(2, stream.Position);
    }

    [Fact]
    public void TestInt8()
    {
        var stream = new MemoryStream();
        Assert.Equal(1, stream.WriteInt8(-0x80));
        Assert.Equal(1, stream.WriteInt8(0x7F));
        Assert.Equal(2, stream.Position);
        stream.Seek(0, SeekOrigin.Begin);
        Assert.Equal(-0x80, stream.ReadInt8());
        Assert.Equal(0x7F, stream.ReadInt8());
        Assert.Equal(2, stream.Position);
    }

    [Fact]
    public void TestUInt16()
    {
        var stream = new MemoryStream();
        Assert.Equal(2, stream.WriteUInt16(0x0000));
        Assert.Equal(2, stream.WriteUInt16(0xFFFF));
        Assert.Equal(4, stream.Position);
        stream.Seek(0, SeekOrigin.Begin);
        Assert.Equal(0x0000, stream.ReadUInt16());
        Assert.Equal(0xFFFF, stream.ReadUInt16());
        Assert.Equal(4, stream.Position);
    }

    [Fact]
    public void TestInt16()
    {
        var stream = new MemoryStream();
        Assert.Equal(2, stream.WriteInt16(-0x8000));
        Assert.Equal(2, stream.WriteInt16(0x7FFF));
        Assert.Equal(4, stream.Position);
        stream.Seek(0, SeekOrigin.Begin);
        Assert.Equal(-0x8000, stream.ReadInt16());
        Assert.Equal(0x7FFF, stream.ReadInt16());
        Assert.Equal(4, stream.Position);
    }

    [Fact]
    public void TestUInt32()
    {
        var stream = new MemoryStream();
        Assert.Equal(4, stream.WriteUInt32(0x00000000u));
        Assert.Equal(4, stream.WriteUInt32(0xFFFFFFFFu));
        Assert.Equal(8, stream.Position);
        stream.Seek(0, SeekOrigin.Begin);
        Assert.Equal(0x00000000u, stream.ReadUInt32());
        Assert.Equal(0xFFFFFFFFu, stream.ReadUInt32());
        Assert.Equal(8, stream.Position);
    }

    [Fact]
    public void TestInt32()
    {
        var stream = new MemoryStream();
        Assert.Equal(4, stream.WriteInt32(-0x80000000));
        Assert.Equal(4, stream.WriteInt32(0x7FFFFFFF));
        Assert.Equal(8, stream.Position);
        stream.Seek(0, SeekOrigin.Begin);
        Assert.Equal(-0x80000000, stream.ReadInt32());
        Assert.Equal(0x7FFFFFFF, stream.ReadInt32());
        Assert.Equal(8, stream.Position);
    }

    [Fact]
    public void TestUtf()
    {
        var stream = new MemoryStream();
        Assert.Equal(5, stream.WriteUtf("ABC"));
        Assert.Equal(7, stream.WriteUtf("12345"));
        Assert.Equal(12, stream.Position);
        stream.Seek(0, SeekOrigin.Begin);
        Assert.Equal("ABC", stream.ReadUtf());
        Assert.Equal("12345", stream.ReadUtf());
        Assert.Equal(12, stream.Position);
    }

    [Fact]
    public void TestULeb128()
    {
        var stream = new MemoryStream();
        Assert.Equal(3, stream.WriteULeb128(65535u));
        Assert.Equal(3, stream.WriteULeb128(624485u));
        Assert.Equal(3, stream.WriteBytes([0xFF, 0xFF, 0x03]));
        Assert.Equal(3, stream.WriteBytes([0xE5, 0x8E, 0x26]));
        Assert.Equal(12, stream.Position);
        stream.Seek(0, SeekOrigin.Begin);
        Assert.Equal([0xFF, 0xFF, 0x03], stream.ReadBytes(3));
        Assert.Equal([0xE5, 0x8E, 0x26], stream.ReadBytes(3));
        Assert.Equal(65535u, stream.ReadULeb128());
        Assert.Equal(624485u, stream.ReadULeb128());
        Assert.Equal(12, stream.Position);
    }

    [Fact]
    public void TestBitmap1()
    {
        List<List<byte>> bitmap = [
            [0x00, 0x00, 0xFF, 0xFF, 0x80],
            [0x00, 0x00, 0xFF, 0xFF, 0x80],
            [0x00, 0x00, 0xFF, 0xFF, 0x80]
        ];

        var stream = new MemoryStream();
        var count = stream.WriteBitmap(bitmap);
        Assert.Equal(count, stream.Position);
        stream.Seek(0, SeekOrigin.Begin);
        Assert.Equal(bitmap, stream.ReadBitmap());
        Assert.Equal(count, stream.Position);
    }

    [Fact]
    public void TestBitmap2()
    {
        List<List<byte>> bitmap = [Enumerable.Range(0, 1050).Select(i => (byte)(i % 0xFF)).ToList()];

        var stream = new MemoryStream();
        var count = stream.WriteBitmap(bitmap);
        Assert.Equal(count, stream.Position);
        stream.Seek(0, SeekOrigin.Begin);
        Assert.Equal(bitmap, stream.ReadBitmap());
        Assert.Equal(count, stream.Position);
    }

    [Fact]
    public void TestBitmap3()
    {
        List<List<byte>> bitmap = [Enumerable.Range(0, 1050).Select(_ => (byte)0x00).ToList()];

        var stream = new MemoryStream();
        var count = stream.WriteBitmap(bitmap);
        Assert.Equal(count, stream.Position);
        stream.Seek(0, SeekOrigin.Begin);
        Assert.Equal(bitmap, stream.ReadBitmap());
        Assert.Equal(count, stream.Position);
    }

    [Fact]
    public void TestBitmap4()
    {
        List<List<byte>> bitmap = [Enumerable.Range(0, 1050).Select(_ => (byte)0x80).ToList()];

        var stream = new MemoryStream();
        var count = stream.WriteBitmap(bitmap);
        Assert.Equal(count, stream.Position);
        stream.Seek(0, SeekOrigin.Begin);
        Assert.Equal(bitmap, stream.ReadBitmap());
        Assert.Equal(count, stream.Position);
    }

    [Fact]
    public void TestBitmap5()
    {
        List<List<byte>> bitmap = [Enumerable.Range(0, 1050).Select(_ => (byte)0xFF).ToList()];

        var stream = new MemoryStream();
        var count = stream.WriteBitmap(bitmap);
        Assert.Equal(count, stream.Position);
        stream.Seek(0, SeekOrigin.Begin);
        Assert.Equal(bitmap, stream.ReadBitmap());
        Assert.Equal(count, stream.Position);
    }
}
