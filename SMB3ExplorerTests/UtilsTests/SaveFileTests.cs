using System.Windows;
using Microsoft.Win32;
using Moq;
using SMB3Explorer.Services.SystemIoWrapper;
using SMB3Explorer.Utils;

namespace SMB3ExplorerTests.UtilsTests;

public class SaveFileTests
{
    [Fact]
    public void GetUserProvidedFile_ReturnsFilePath()
    {
        // Arrange
        var mockSystemIoWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);
        
        mockSystemIoWrapper
            .Setup(x => x.ShowOpenFileDialog(It.IsAny<OpenFileDialog>()))
            .Returns(true);

        // Act
        var result = SaveFile.GetUserProvidedFile(Environment.SpecialFolder.MyDocuments.ToString(), mockSystemIoWrapper.Object);

        // Assert
        result.Should().NotBeNull();
        result.TryPickT0(out var filePath, out _).Should().BeTrue();
        filePath.Should().NotBeNull();
    }
    
    [Fact]
    public void GetUserProvidedFile_ReturnsNone()
    {
        // Arrange
        var mockSystemIoWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);
        
        mockSystemIoWrapper
            .Setup(x => x.ShowOpenFileDialog(It.IsAny<OpenFileDialog>()))
            .Returns(false);

        // Act
        var result = SaveFile.GetUserProvidedFile(Environment.SpecialFolder.MyDocuments.ToString(), mockSystemIoWrapper.Object);

        // Assert
        result.Should().NotBeNull();
        result.TryPickT1(out var none, out _).Should().BeTrue();
        none.Should().NotBeNull();
    }

    [Fact]
    public void GetSaveFilePath_ReturnsAutomaticallyDetectedFilePath()
    {
        // Arrange
        var mockSystemIoWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);
        
        mockSystemIoWrapper
            .Setup(x => x.DirectoryExists(SaveFile.BaseGameDirectoryPath))
            .Returns(true);

        var steamDirectory = Path.Combine(SaveFile.BaseGameDirectoryPath, "123456");

        mockSystemIoWrapper
            .Setup(x => x.DirectoryGetDirectories(SaveFile.BaseGameDirectoryPath))
            .Returns(new[] {steamDirectory});

        var saveFilePath =
            Path.Combine(steamDirectory, SaveFile.DefaultSaveFileName);
        
        mockSystemIoWrapper
            .Setup(x => x.FileExists(saveFilePath))
            .Returns(true);
        
        // Act
        var result = SaveFile.GetSaveFilePath(mockSystemIoWrapper.Object);
        
        // Assert
        result.Should().NotBeNull();
        result.TryPickT0(out var filePath, out _).Should().BeTrue();
        filePath.Should().BeEquivalentTo(saveFilePath);
    }

    [Fact]
    public void GetSaveFilePath_PromptsUserSelection_WhenDefaultDirectoryDoesNotExist()
    {
        // Arrange
        var mockSystemIoWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);
        
        mockSystemIoWrapper
            .Setup(x => x.DirectoryExists(SaveFile.BaseGameDirectoryPath))
            .Returns(false);

        mockSystemIoWrapper.Setup(x =>
                x.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>()))
            .Returns(MessageBoxResult.Yes);

        mockSystemIoWrapper.Setup(x => x.ShowOpenFileDialog(It.IsAny<OpenFileDialog>()))
            .Returns(true);
        
        // Act
        var result = SaveFile.GetSaveFilePath(mockSystemIoWrapper.Object);
        
        // Assert
        result.Should().NotBeNull();
        result.TryPickT0(out var filePath, out _).Should().BeTrue();
        filePath.Should().NotBeNull();
    }

    [Fact]
    public void GetSaveFilePath_ReturnsNone_WhenDefaultDirectoryDoesNotExistAndUserCancels()
    {
        // Arrange
        var mockSystemIoWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);
        
        mockSystemIoWrapper
            .Setup(x => x.DirectoryExists(SaveFile.BaseGameDirectoryPath))
            .Returns(false);

        mockSystemIoWrapper.Setup(x =>
                x.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>()))
            .Returns(MessageBoxResult.No);
        
        // Act
        var result = SaveFile.GetSaveFilePath(mockSystemIoWrapper.Object);
        
        // Assert
        result.Should().NotBeNull();
        result.TryPickT1(out var none, out _).Should().BeTrue();
        none.Should().NotBeNull();
    }
    
    [Fact]
    public void GetSaveFilePath_PromptsUserSelection_WhenDefaultDirectoryExistsButNoSaveFileExists()
    {
        // Arrange
        var mockSystemIoWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);
        
        mockSystemIoWrapper
            .Setup(x => x.DirectoryExists(SaveFile.BaseGameDirectoryPath))
            .Returns(true);

        var steamDirectory = Path.Combine(SaveFile.BaseGameDirectoryPath, "123456");

        mockSystemIoWrapper
            .Setup(x => x.DirectoryGetDirectories(SaveFile.BaseGameDirectoryPath))
            .Returns(new[] {steamDirectory});

        var saveFilePath =
            Path.Combine(steamDirectory, SaveFile.DefaultSaveFileName);
        
        mockSystemIoWrapper
            .Setup(x => x.FileExists(saveFilePath))
            .Returns(false);

        mockSystemIoWrapper.Setup(x =>
                x.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>()))
            .Returns(MessageBoxResult.Yes);

        mockSystemIoWrapper.Setup(x => x.ShowOpenFileDialog(It.IsAny<OpenFileDialog>()))
            .Returns(true);
        
        // Act
        var result = SaveFile.GetSaveFilePath(mockSystemIoWrapper.Object);
        
        // Assert
        result.Should().NotBeNull();
        result.TryPickT0(out var filePath, out _).Should().BeTrue();
        filePath.Should().NotBeNull();
    }
    
    [Fact]
    public void GetSaveFilePath_ReturnsNone_WhenDefaultDirectoryExistsButNoSaveFileExistsAndUserCancels()
    {
        // Arrange
        var mockSystemIoWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);
        
        mockSystemIoWrapper
            .Setup(x => x.DirectoryExists(SaveFile.BaseGameDirectoryPath))
            .Returns(true);

        var steamDirectory = Path.Combine(SaveFile.BaseGameDirectoryPath, "123456");

        mockSystemIoWrapper
            .Setup(x => x.DirectoryGetDirectories(SaveFile.BaseGameDirectoryPath))
            .Returns(new[] {steamDirectory});

        var saveFilePath =
            Path.Combine(steamDirectory, SaveFile.DefaultSaveFileName);
        
        mockSystemIoWrapper
            .Setup(x => x.FileExists(saveFilePath))
            .Returns(false);

        mockSystemIoWrapper.Setup(x =>
                x.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>()))
            .Returns(MessageBoxResult.No);
        
        // Act
        var result = SaveFile.GetSaveFilePath(mockSystemIoWrapper.Object);
        
        // Assert
        result.Should().NotBeNull();
        result.TryPickT1(out var none, out _).Should().BeTrue();
        none.Should().NotBeNull();
    }
    
    [Fact]
    public void GetSaveFilePath_PromptsUserSelection_WhenDefaultDirectoryExistsButNoSteamDirectoryExists()
    {
        // Arrange
        var mockSystemIoWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);
        
        mockSystemIoWrapper
            .Setup(x => x.DirectoryExists(SaveFile.BaseGameDirectoryPath))
            .Returns(true);

        mockSystemIoWrapper
            .Setup(x => x.DirectoryGetDirectories(SaveFile.BaseGameDirectoryPath))
            .Returns(Array.Empty<string>());

        mockSystemIoWrapper.Setup(x =>
                x.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>()))
            .Returns(MessageBoxResult.Yes);

        mockSystemIoWrapper.Setup(x => x.ShowOpenFileDialog(It.IsAny<OpenFileDialog>()))
            .Returns(true);
        
        // Act
        var result = SaveFile.GetSaveFilePath(mockSystemIoWrapper.Object);
        
        // Assert
        result.Should().NotBeNull();
        result.TryPickT0(out var filePath, out _).Should().BeTrue();
        filePath.Should().NotBeNull();
    }
    
    [Fact]
    public void GetSaveFilePath_ReturnsNone_WhenDefaultDirectoryExistsButNoSteamDirectoryExistsAndUserCancels()
    {
        // Arrange
        var mockSystemIoWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);
        
        mockSystemIoWrapper
            .Setup(x => x.DirectoryExists(SaveFile.BaseGameDirectoryPath))
            .Returns(true);

        mockSystemIoWrapper
            .Setup(x => x.DirectoryGetDirectories(SaveFile.BaseGameDirectoryPath))
            .Returns(Array.Empty<string>());

        mockSystemIoWrapper.Setup(x =>
                x.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>()))
            .Returns(MessageBoxResult.No);
        
        // Act
        var result = SaveFile.GetSaveFilePath(mockSystemIoWrapper.Object);
        
        // Assert
        result.Should().NotBeNull();
        result.TryPickT1(out var none, out _).Should().BeTrue();
        none.Should().NotBeNull();
    }

    [Fact]
    public void GetSaveFilePath_PromptsUserSelection_WhenDefaultDirectoryExistsAndMultipleSteamDirectoriesExist()
    {
        // Arrange
        var mockSystemIoWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);
        
        mockSystemIoWrapper
            .Setup(x => x.DirectoryExists(SaveFile.BaseGameDirectoryPath))
            .Returns(true);

        var steamDirectory1 = Path.Combine(SaveFile.BaseGameDirectoryPath, "123456");
        var steamDirectory2 = Path.Combine(SaveFile.BaseGameDirectoryPath, "654321");

        mockSystemIoWrapper
            .Setup(x => x.DirectoryGetDirectories(SaveFile.BaseGameDirectoryPath))
            .Returns(new[] {steamDirectory1, steamDirectory2});

        var saveFilePath1 =
            Path.Combine(steamDirectory1, SaveFile.DefaultSaveFileName);
        
        mockSystemIoWrapper
            .Setup(x => x.FileExists(saveFilePath1))
            .Returns(false);

        var saveFilePath2 =
            Path.Combine(steamDirectory2, SaveFile.DefaultSaveFileName);
        
        mockSystemIoWrapper
            .Setup(x => x.FileExists(saveFilePath2))
            .Returns(false);

        mockSystemIoWrapper.Setup(x =>
                x.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>()))
            .Returns(MessageBoxResult.Yes);

        mockSystemIoWrapper.Setup(x => x.ShowOpenFileDialog(It.IsAny<OpenFileDialog>()))
            .Returns(true);
        
        // Act
        var result = SaveFile.GetSaveFilePath(mockSystemIoWrapper.Object);
        
        // Assert
        result.Should().NotBeNull();
        result.TryPickT0(out var filePath, out _).Should().BeTrue();
        filePath.Should().NotBeNull();
    }
    
    [Fact]
    public void GetSaveFilePath_ReturnsNone_WhenDefaultDirectoryExistsAndMultipleSteamDirectoriesExistAndUserCancels()
    {
        // Arrange
        var mockSystemIoWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);
        
        mockSystemIoWrapper
            .Setup(x => x.DirectoryExists(SaveFile.BaseGameDirectoryPath))
            .Returns(true);

        var steamDirectory1 = Path.Combine(SaveFile.BaseGameDirectoryPath, "123456");
        var steamDirectory2 = Path.Combine(SaveFile.BaseGameDirectoryPath, "654321");

        mockSystemIoWrapper
            .Setup(x => x.DirectoryGetDirectories(SaveFile.BaseGameDirectoryPath))
            .Returns(new[] {steamDirectory1, steamDirectory2});

        var saveFilePath1 =
            Path.Combine(steamDirectory1, SaveFile.DefaultSaveFileName);
        
        mockSystemIoWrapper
            .Setup(x => x.FileExists(saveFilePath1))
            .Returns(false);

        var saveFilePath2 =
            Path.Combine(steamDirectory2, SaveFile.DefaultSaveFileName);
        
        mockSystemIoWrapper
            .Setup(x => x.FileExists(saveFilePath2))
            .Returns(false);

        mockSystemIoWrapper.Setup(x =>
                x.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>()))
            .Returns(MessageBoxResult.No);
        
        // Act
        var result = SaveFile.GetSaveFilePath(mockSystemIoWrapper.Object);
        
        // Assert
        result.Should().NotBeNull();
        result.TryPickT1(out var none, out _).Should().BeTrue();
        none.Should().NotBeNull();
    }
}