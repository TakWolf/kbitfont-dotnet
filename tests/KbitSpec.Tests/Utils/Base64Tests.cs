using KbitSpec.Utils;

namespace KbitSpec.Tests.Utils;

public class Base64Tests
{
    [Fact]
    public void TestBase64()
    {
        var plain = "Hello World"u8.ToArray();
        var encoded = "SGVsbG8gV29ybGQ";
        Assert.Equal(encoded, Base64.EncodeNoPadding(plain));
        Assert.True(plain.SequenceEqual(Base64.DecodeNoPadding(encoded)));
    }
}
