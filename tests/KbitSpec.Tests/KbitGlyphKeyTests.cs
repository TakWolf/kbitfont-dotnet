namespace KbitSpec.Tests;

public class KbitGlyphKeyTests
{
    [Fact]
    public void TestValue1()
    {
        var value = new KbitGlyphKey(42);
        Assert.True(value.IsInt);
        Assert.False(value.IsString);
        Assert.Equal(42, value.AsInt());
        Assert.Throws<InvalidOperationException>(() => value.AsString());
    }

    [Fact]
    public void TestValue2()
    {
        var value = new KbitGlyphKey("hello");
        Assert.True(value.IsString);
        Assert.False(value.IsInt);
        Assert.Equal("hello", value.AsString());
        Assert.Throws<InvalidOperationException>(() => value.AsInt());
    }

    [Fact]
    public void TestValue3()
    {
        Assert.Throws<ArgumentNullException>(() => new KbitGlyphKey(null!));
    }

    [Fact]
    public void TestValue4()
    {
        KbitGlyphKey value = default;
        Assert.True(value.IsInt);
        Assert.Equal(0, value.AsInt());
    }

    [Fact]
    public void TestValue5()
    {
        var value = new KbitGlyphKey(42);
        var (intValue, stringValue) = value;
        Assert.Equal(42, intValue);
        Assert.Null(stringValue);
    }

    [Fact]
    public void TestValue6()
    {
        var value = new KbitGlyphKey("hello");
        var (intValue, stringValue) = value;
        Assert.Equal("hello", stringValue);
        Assert.Null(intValue);
    }

    [Fact]
    public void TestValue7()
    {
        KbitGlyphKey value = 42;
        Assert.True(value.IsInt);
        Assert.Equal(42, value.AsInt());
    }

    [Fact]
    public void TestValue8()
    {
        KbitGlyphKey value = "hello";
        Assert.True(value.IsString);
        Assert.Equal("hello", value.AsString());
    }

    [Fact]
    public void TestValue9()
    {
        var value = (int)new KbitGlyphKey(42);
        Assert.Equal(42, value);
    }

    [Fact]
    public void TestValue10()
    {
        var value = (string)new KbitGlyphKey("hello");
        Assert.Equal("hello", value);
    }

    [Fact]
    public void TestValue11()
    {
        Assert.Throws<InvalidOperationException>(() => (string)new KbitGlyphKey(42));
        Assert.Throws<InvalidOperationException>(() => (int)new KbitGlyphKey("hello"));
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
    public void TestEquals1()
    {
        var a = new KbitGlyphKey(42);
        var b = new KbitGlyphKey(42);
        Assert.True(a.Equals(b));
        Assert.True(a.Equals((object)b));
        Assert.True(a == b);
        Assert.False(a != b);
    }

    [Fact]
    public void TestEquals2()
    {
        var a = new KbitGlyphKey(1);
        var b = new KbitGlyphKey(42);
        Assert.False(a.Equals(b));
        Assert.False(a.Equals((object)b));
        Assert.False(a == b);
        Assert.True(a != b);
    }

    [Fact]
    public void TestEquals3()
    {
        var a = new KbitGlyphKey("hello");
        var b = new KbitGlyphKey("hello");
        Assert.True(a.Equals(b));
        Assert.True(a.Equals((object)b));
        Assert.True(a == b);
        Assert.False(a != b);
    }

    [Fact]
    public void TestEquals4()
    {
        var a = new KbitGlyphKey("hello");
        var b = new KbitGlyphKey("world");
        Assert.False(a.Equals(b));
        Assert.False(a.Equals((object)b));
        Assert.False(a == b);
        Assert.True(a != b);
    }

    [Fact]
    public void TestEquals5()
    {
        var a = new KbitGlyphKey(42);
        var b = new KbitGlyphKey("hello");
        Assert.False(a.Equals(b));
        Assert.False(a.Equals((object)b));
        Assert.False(a == b);
        Assert.True(a != b);
    }

    [Fact]
    public void TestEquals6()
    {
        var a = new KbitGlyphKey("hello");
        var b = new KbitGlyphKey(42);
        Assert.False(a.Equals(b));
        Assert.False(a.Equals((object)b));
        Assert.False(a == b);
        Assert.True(a != b);
    }

    [Fact]
    public void TestGetHashCode1()
    {
        var a = new KbitGlyphKey(42);
        var b = new KbitGlyphKey(42);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void TestGetHashCode2()
    {
        var a = new KbitGlyphKey("hello");
        var b = new KbitGlyphKey("hello");
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void TestGetHashCode3()
    {
        var a = new KbitGlyphKey(42);
        var b = new KbitGlyphKey("hello");
        Assert.NotEqual(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void TestToString1()
    {
        var value = new KbitGlyphKey(42);
        Assert.Equal("42", value.ToString());
    }

    [Fact]
    public void TestToString2()
    {
        var value = new KbitGlyphKey("hello");
        Assert.Equal("hello", value.ToString());
    }
}
