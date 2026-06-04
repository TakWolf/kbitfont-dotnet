namespace KbitSpec.Tests;

public class KbitGlyphKeyTests
{
    [Fact]
    public void TestValue()
    {
        {
            var value = new KbitGlyphKey(42);
            Assert.True(value.IsInt);
            Assert.False(value.IsString);
            Assert.Equal(42, value.AsInt());
            Assert.Throws<InvalidOperationException>(() => value.AsString());
        }
        {
            var value = new KbitGlyphKey("hello");
            Assert.True(value.IsString);
            Assert.False(value.IsInt);
            Assert.Equal("hello", value.AsString());
            Assert.Throws<InvalidOperationException>(() => value.AsInt());
        }
        {
            Assert.Throws<ArgumentNullException>(() => new KbitGlyphKey(null!));
        }
        {
            KbitGlyphKey value = default;
            Assert.True(value.IsInt);
            Assert.Equal(0, value.AsInt());
        }
        {
            var value = new KbitGlyphKey(42);
            var (intValue, stringValue) = value;
            Assert.Equal(42, intValue);
            Assert.Null(stringValue);
        }
        {
            var value = new KbitGlyphKey("hello");
            var (intValue, stringValue) = value;
            Assert.Equal("hello", stringValue);
            Assert.Null(intValue);
        }
        {
            KbitGlyphKey value = 42;
            Assert.True(value.IsInt);
            Assert.Equal(42, value.AsInt());
        }
        {
            KbitGlyphKey value = "hello";
            Assert.True(value.IsString);
            Assert.Equal("hello", value.AsString());
        }
        {
            var value = (int)new KbitGlyphKey(42);
            Assert.Equal(42, value);
        }
        {
            var value = (string)new KbitGlyphKey("hello");
            Assert.Equal("hello", value);
        }
        {
            Assert.Throws<InvalidOperationException>(() => (string)new KbitGlyphKey(42));
            Assert.Throws<InvalidOperationException>(() => (int)new KbitGlyphKey("hello"));
        }
    }

    [Theory]
    [InlineData(1, 2, -1)]
    [InlineData(2, 1, 1)]
    [InlineData(1, 1, 0)]
    [InlineData("a", "b", -1)]
    [InlineData("b", "a", 1)]
    [InlineData("a", "a", 0)]
    [InlineData(1, "a", -1)]
    [InlineData("a", 1, 1)]
    public void TestCompareTo(KbitGlyphKey left, KbitGlyphKey right, int expected)
    {
        Assert.Equal(expected, left.CompareTo(right));
    }

    [Fact]
    public void TestEquals()
    {
        {
            var a = new KbitGlyphKey(42);
            var b = new KbitGlyphKey(42);
            Assert.True(a.Equals(b));
            Assert.True(a.Equals((object)b));
            Assert.True(a == b);
            Assert.False(a != b);
        }
        {
            var a = new KbitGlyphKey(1);
            var b = new KbitGlyphKey(42);
            Assert.False(a.Equals(b));
            Assert.False(a.Equals((object)b));
            Assert.False(a == b);
            Assert.True(a != b);
        }
        {
            var a = new KbitGlyphKey("hello");
            var b = new KbitGlyphKey("hello");
            Assert.True(a.Equals(b));
            Assert.True(a.Equals((object)b));
            Assert.True(a == b);
            Assert.False(a != b);
        }
        {
            var a = new KbitGlyphKey("hello");
            var b = new KbitGlyphKey("world");
            Assert.False(a.Equals(b));
            Assert.False(a.Equals((object)b));
            Assert.False(a == b);
            Assert.True(a != b);
        }
        {
            var a = new KbitGlyphKey(42);
            var b = new KbitGlyphKey("hello");
            Assert.False(a.Equals(b));
            Assert.False(a.Equals((object)b));
            Assert.False(a == b);
            Assert.True(a != b);
        }
        {
            var a = new KbitGlyphKey("hello");
            var b = new KbitGlyphKey(42);
            Assert.False(a.Equals(b));
            Assert.False(a.Equals((object)b));
            Assert.False(a == b);
            Assert.True(a != b);
        }
    }

    [Fact]
    public void TestGetHashCode()
    {
        {
            var a = new KbitGlyphKey(42);
            var b = new KbitGlyphKey(42);
            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }
        {
            var a = new KbitGlyphKey("hello");
            var b = new KbitGlyphKey("hello");
            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }
        {
            var a = new KbitGlyphKey(42);
            var b = new KbitGlyphKey("hello");
            Assert.NotEqual(a.GetHashCode(), b.GetHashCode());
        }
    }

    [Fact]
    public void TestToString()
    {
        {
            var value = new KbitGlyphKey(42);
            Assert.Equal("42", value.ToString());
        }
        {
            var value = new KbitGlyphKey("hello");
            Assert.Equal("hello", value.ToString());
        }
    }
}
