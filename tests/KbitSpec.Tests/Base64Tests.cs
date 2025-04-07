using KbitSpec.Internal;

namespace KbitSpec.Tests;

public class Base64Tests
{
    [Fact]
    public void TestBase64()
    {
        var plain = "Hello World"u8.ToArray();
        var encoded = "SGVsbG8gV29ybGQ"u8.ToArray();
        Assert.True(encoded.SequenceEqual(Base64.EncodeNoPadding(plain)));
        Assert.True(plain.SequenceEqual(Base64.DecodeNoPadding(encoded)));
    }
}
