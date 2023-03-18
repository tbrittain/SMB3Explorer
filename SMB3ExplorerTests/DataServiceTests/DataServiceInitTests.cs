using Moq;
using SMB3Explorer.Services.SystemInteropWrapper;

namespace SMB3ExplorerTests.DataServiceTests;

public class DataServiceInitTests
{
    [Fact]
    public void DecompressSaveGame_ReturnsFilePath()
    {
        var mockFileStream = new Mock<FileStream>(MockBehavior.Strict, new nint(1), FileAccess.Read);
        var mockSystemIoWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);

        mockSystemIoWrapper.Setup(x => x.FileOpenRead(It.IsAny<string>()))
            .Returns(mockFileStream.Object);
    }
}