using SMB3Explorer.Utils;

namespace SMB3ExplorerTests.UtilsTests;

public class GuidUtilsTests
{
    [Fact]
    public void ToBlob_ReturnsByteArray()
    {
        // Arrange
        var guid = new Guid("b83810eb-1937-421b-bbf9-17bb5a5a1a5d");

        // Act
        var result = guid.ToBlob();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<byte[]>();
        result.Length.Should().Be(16);
    }

    [Fact]
    public void ToGuid_ReturnsGuid()
    {
        // Arrange
        var bytes = new byte[]
        {
            184, 56, 16, 235, 25, 55, 66, 27, 187, 249, 23, 187, 90, 90, 26, 93
        };

        // Act
        var result = bytes.ToGuid();

        // Assert
        result.Should().NotBe(Guid.Empty);
        result.Should().Be(new Guid("b83810eb-1937-421b-bbf9-17bb5a5a1a5d"));
    }

    [Fact]
    public void ToBlob_ReturnsSameGuid_WhenToGuidCalledOnResult()
    {
        // Arrange
        var guid = new Guid("b83810eb-1937-421b-bbf9-17bb5a5a1a5d");

        // Act
        var result = guid.ToBlob();
        var guidResult = result.ToGuid();

        // Assert
        guidResult.Should().Be(guid);
    }

    [Fact]
    public void ToGuid_ReturnsSameByteArray_WhenToBlobCalledOnResult()
    {
        // Arrange
        var bytes = new byte[]
        {
            184, 56, 16, 235, 25, 55, 66, 27, 187, 249, 23, 187, 90, 90, 26, 93
        };

        // Act
        var result = bytes.ToGuid();
        var bytesResult = result.ToBlob();

        // Assert
        bytesResult.Should().BeEquivalentTo(bytes);
    }
}