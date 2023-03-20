using Ionic.Zlib;
using Moq;
using SMB3Explorer.Services.ApplicationContext;
using SMB3Explorer.Services.DataService;
using SMB3Explorer.Services.SystemInteropWrapper;

namespace SMB3ExplorerTests.DataServiceTests;

public class DataServiceInitTests
{
    [Fact]
    public async Task DecompressSaveGame_ReturnsFilePath()
    {
        // Arrange
        var mockFileStream = new Mock<Stream>();
        mockFileStream.Setup(x => x.CanRead).Returns(true);
        mockFileStream.Setup(x => x.CanWrite).Returns(true);

        var mockSystemIoWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);

        mockSystemIoWrapper.Setup(x => x.FileOpenRead(It.IsAny<string>()))
            .Returns(mockFileStream.Object);

        mockSystemIoWrapper.Setup(x => x.GetZlibDecompressionStream(It.IsAny<Stream>()))
            .Returns(new ZlibStream(mockFileStream.Object, CompressionMode.Decompress));

        mockSystemIoWrapper.Setup(x => x.FileCreateStream(It.IsAny<string>()))
            .Returns(mockFileStream.Object);

        var mockApplicationContext = new Mock<IApplicationContext>();

        // Act
        var dataService = new DataService(mockApplicationContext.Object, mockSystemIoWrapper.Object);
        var result = await dataService.DecompressSaveGame("filePath", mockSystemIoWrapper.Object);

        // Assert
        if (result.TryPickT0(out var outputFilePath, out _))
        {
            var baseFilePath = Path.Combine(Path.GetTempPath(), "smb3_explorer_");
            Assert.StartsWith(baseFilePath, outputFilePath);
            Assert.EndsWith(".sqlite", outputFilePath);
        }
        else
        {
            Assert.True(false, "Expected a successful result.");
        }
    }

    [Fact]
    public async Task DecompressSaveGame_ReturnsError_WhenFileOpenReadFails()
    {
        // Arrange
        var mockSystemIoWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);

        mockSystemIoWrapper.Setup(x => x.FileOpenRead(It.IsAny<string>()))
            .Returns((Stream?) null);

        var mockApplicationContext = new Mock<IApplicationContext>();

        // Act
        var dataService = new DataService(mockApplicationContext.Object, mockSystemIoWrapper.Object);
        var result = await dataService.DecompressSaveGame("filePath", mockSystemIoWrapper.Object);

        // Assert
        if (result.TryPickT1(out var error, out _))
        {
            Assert.Equal("Could not open file.", error.Value);
        }
        else
        {
            Assert.True(false, "Expected an error result.");
        }
    }

    // TODO: need to mock the sqlite connection here
    [Fact]
    public async Task EstablishDbConnection_ReturnsSuccess_WhenIsCompressedSaveGameIsFalse()
    {
        // Arrange
        var mockSystemIoWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);

        var mockApplicationContext = new Mock<IApplicationContext>();

        // Act
        var dataService = new DataService(mockApplicationContext.Object, mockSystemIoWrapper.Object);
        var result = await dataService.EstablishDbConnection("filePath", false);

        // Assert
        if (result.TryPickT0(out var success, out _))
        {
            Assert.Equal("filePath", dataService.CurrentFilePath);
        }
        else
        {
            Assert.True(false, "Expected a successful result.");
        }
    }
}