using KbitSpec.Internal;

namespace KbitSpec.Tests;

public class StreamExtensionsTests
{
    private static readonly Random Random = new();

    [Fact]
    public void TestByte()
    {
        var stream = new MemoryStream();
        var count = stream.WriteBuffer("Hello World"u8);
        Assert.Equal(count, stream.Position);
        stream.Seek(0, SeekOrigin.Begin);
        Assert.Equal("Hello World"u8, stream.ReadBuffer(11));
        Assert.Equal(count, stream.Position);
    }

    [Fact]
    public void TestEof()
    {
        var stream = new MemoryStream();
        Assert.Throws<EndOfStreamException>(() => stream.ReadBuffer(1));
    }

    [Fact]
    public void TestInt8()
    {
        var values = Enumerable.Range(0, 20).Select(_ => (sbyte)Random.Next()).ToList();

        var stream = new MemoryStream();
        var count = 0;
        foreach (var value in values)
        {
            count += stream.WriteInt8(value);
        }
        Assert.Equal(count, stream.Position);
        stream.Seek(0, SeekOrigin.Begin);
        foreach (var value in values)
        {
            Assert.Equal(value, stream.ReadInt8());
        }
        Assert.Equal(count, stream.Position);
    }

    [Fact]
    public void TestUInt8()
    {
        var values = Enumerable.Range(0, 20).Select(_ => (byte)Random.Next()).ToList();

        var stream = new MemoryStream();
        var count = 0;
        foreach (var value in values)
        {
            count += stream.WriteUInt8(value);
        }
        Assert.Equal(count, stream.Position);
        stream.Seek(0, SeekOrigin.Begin);
        foreach (var value in values)
        {
            Assert.Equal(value, stream.ReadUInt8());
        }
        Assert.Equal(count, stream.Position);
    }

    [Fact]
    public void TestInt16()
    {
        var values = Enumerable.Range(0, 20).Select(_ => (short)Random.Next()).ToList();

        var stream = new MemoryStream();
        var count = 0;
        foreach (var value in values)
        {
            count += stream.WriteInt16(value);
        }
        Assert.Equal(count, stream.Position);
        stream.Seek(0, SeekOrigin.Begin);
        foreach (var value in values)
        {
            Assert.Equal(value, stream.ReadInt16());
        }
        Assert.Equal(count, stream.Position);
    }

    [Fact]
    public void TestUInt16()
    {
        var values = Enumerable.Range(0, 20).Select(_ => (ushort)Random.Next()).ToList();

        var stream = new MemoryStream();
        var count = 0;
        foreach (var value in values)
        {
            count += stream.WriteUInt16(value);
        }
        Assert.Equal(count, stream.Position);
        stream.Seek(0, SeekOrigin.Begin);
        foreach (var value in values)
        {
            Assert.Equal(value, stream.ReadUInt16());
        }
        Assert.Equal(count, stream.Position);
    }

    [Fact]
    public void TestInt32()
    {
        var values = Enumerable.Range(0, 20).Select(_ => Random.Next()).ToList();

        var stream = new MemoryStream();
        var count = 0;
        foreach (var value in values)
        {
            count += stream.WriteInt32(value);
        }
        Assert.Equal(count, stream.Position);
        stream.Seek(0, SeekOrigin.Begin);
        foreach (var value in values)
        {
            Assert.Equal(value, stream.ReadInt32());
        }
        Assert.Equal(count, stream.Position);
    }

    [Fact]
    public void TestUInt32()
    {
        var values = Enumerable.Range(0, 20).Select(_ => (uint)Random.Next()).ToList();

        var stream = new MemoryStream();
        var count = 0;
        foreach (var value in values)
        {
            count += stream.WriteUInt32(value);
        }
        Assert.Equal(count, stream.Position);
        stream.Seek(0, SeekOrigin.Begin);
        foreach (var value in values)
        {
            Assert.Equal(value, stream.ReadUInt32());
        }
        Assert.Equal(count, stream.Position);
    }

    [Fact]
    public void TestUtf()
    {
        List<string> values = ["ABC", "DEF", "12345", "67890"];

        var stream = new MemoryStream();
        var count = 0;
        foreach (var value in values)
        {
            count += stream.WriteUtf(value);
        }
        Assert.Equal(count, stream.Position);
        stream.Seek(0, SeekOrigin.Begin);
        foreach (var value in values)
        {
            Assert.Equal(value, stream.ReadUtf());
        }
        Assert.Equal(count, stream.Position);
    }

    [Fact]
    public void TestULeb128()
    {
        var values = Enumerable.Range(0, 20).Select(_ => (uint)Random.Next()).ToList();

        var stream = new MemoryStream();
        var count = 0;
        foreach (var value in values)
        {
            count += stream.WriteULeb128(value);
        }
        Assert.Equal(count, stream.Position);
        stream.Seek(0, SeekOrigin.Begin);
        foreach (var value in values)
        {
            Assert.Equal(value, stream.ReadULeb128());
        }
        Assert.Equal(count, stream.Position);
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
