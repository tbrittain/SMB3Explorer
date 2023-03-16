using Microsoft.Win32;
using Moq;
using SMB3Explorer.Services.SystemInteropWrapper;
using SMB3Explorer.Utils;

namespace SMB3ExplorerTests.UtilsTests;

public class SaveFileTests
{
    [Fact]
    public void GetUserProvidedFile_ReturnsFilePath()
    {
        // Arrange
        var mockSystemInteropWrapper = new Mock<ISystemInteropWrapper>(MockBehavior.Strict);
        
        mockSystemInteropWrapper
            .Setup(x => x.ShowOpenFileDialog(It.IsAny<OpenFileDialog>()))
            .Returns(true);

        // Act
        var result = SaveFile.GetUserProvidedFile(Environment.SpecialFolder.MyDocuments.ToString(), mockSystemInteropWrapper.Object);

        // Assert
        result.Should().NotBeNull();
        result.TryPickT0(out var filePath, out _).Should().BeTrue();
        filePath.Should().NotBeNull();
    }
    
    [Fact]
    public void GetUserProvidedFile_ReturnsNone()
    {
        // Arrange
        var mockSystemInteropWrapper = new Mock<ISystemInteropWrapper>(MockBehavior.Strict);
        
        mockSystemInteropWrapper
            .Setup(x => x.ShowOpenFileDialog(It.IsAny<OpenFileDialog>()))
            .Returns(false);

        // Act
        var result = SaveFile.GetUserProvidedFile(Environment.SpecialFolder.MyDocuments.ToString(), mockSystemInteropWrapper.Object);

        // Assert
        result.Should().NotBeNull();
        result.TryPickT1(out var none, out _).Should().BeTrue();
        none.Should().NotBeNull();
    }

    [Fact]
    public void GetSaveFilePath_ReturnsAutomaticallyDetectedFilePath()
    {
        // Arrange
        var mockSystemInteropWrapper = new Mock<ISystemInteropWrapper>(MockBehavior.Strict);
        
        mockSystemInteropWrapper
            .Setup(x => x.DirectoryExists(SaveFile.BaseGameDirectoryPath))
            .Returns(true);

        var steamDirectory = Path.Combine(SaveFile.BaseGameDirectoryPath, "123456");

        mockSystemInteropWrapper
            .Setup(x => x.DirectoryGetDirectories(SaveFile.BaseGameDirectoryPath))
            .Returns(new[] {steamDirectory});

        var saveFilePath =
            Path.Combine(steamDirectory, SaveFile.DefaultSaveFileName);
        
        mockSystemInteropWrapper
            .Setup(x => x.FileExists(saveFilePath))
            .Returns(true);
        
        // Act
        var result = SaveFile.GetSaveFilePath(mockSystemInteropWrapper.Object);
        
        // Assert
        result.Should().NotBeNull();
        result.TryPickT0(out var filePath, out _).Should().BeTrue();
        filePath.Should().BeEquivalentTo(saveFilePath);
    }
}