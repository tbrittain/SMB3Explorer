using System.Diagnostics;
using System.Windows;
using Moq;
using SMB3Explorer.Services.SystemIoWrapper;
using SMB3Explorer.Utils;

namespace SMB3ExplorerTests.UtilsTests;

public class DefaultExceptionHandlerTests
{
    [Fact]
    public void HandleException_DisplaysUserFriendlyMessage_WhenCalled()
    {
        // Arrange
        const string userFriendlyMessage = "An error has occurred.";
        var exception = new Exception("Test exception");

        var mockSystemIoWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);
        
        mockSystemIoWrapper.Setup(m => m.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<MessageBoxButton>(), It.IsAny<MessageBoxImage>()))
            .Returns(MessageBoxResult.OK);

        mockSystemIoWrapper.Setup(m => m.StartProcess(It.IsAny<ProcessStartInfo>()))
            .Returns(It.IsAny<Process>());
        
        mockSystemIoWrapper.SetupGet(m => m.MessageBoxText)
            .Returns(userFriendlyMessage);

        // Act
        DefaultExceptionHandler.HandleException(mockSystemIoWrapper.Object, userFriendlyMessage, exception);

        // Assert
        // Ensure that the user-friendly message is displayed in the message box
        var messageBoxText = mockSystemIoWrapper.Object.MessageBoxText;
        Assert.Contains(userFriendlyMessage, messageBoxText);
    }

    [Fact]
    public void HandleException_DisplaysMessageBox_WhenExceptionIsNotNull()
    {
        // Arrange
        const string userFriendlyMessage = "An error occurred";
        var exception = new Exception("This will be overwritten by the exception thrown below");
        try
        {
            // ReSharper disable once ConvertToConstant.Local
            var x = 0;
            // ReSharper disable once IntDivisionByZero
            var y = 1 / x;
        }
        catch (Exception e)
        {
            exception = e;
        }

        var mockSystemIoWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);
        
        mockSystemIoWrapper.Setup(x => x.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<MessageBoxButton>(),
                It.IsAny<MessageBoxImage>()))
            .Returns(MessageBoxResult.OK);

        mockSystemIoWrapper.Setup(m => m.StartProcess(It.IsAny<ProcessStartInfo>()))
            .Returns(It.IsAny<Process>());

        // Act
        DefaultExceptionHandler.HandleException(mockSystemIoWrapper.Object, userFriendlyMessage, exception);

        // Assert
        mockSystemIoWrapper.Verify(x => x.ShowMessageBox(
            It.Is<string>(message => message.Contains(userFriendlyMessage)),
            It.IsAny<string>(),
            MessageBoxButton.OKCancel,
            MessageBoxImage.Error), Times.Once);
    }

    [Fact]
    public void HandleException_OpensGitHubIssuePage_WhenUserClicksOK()
    {
        // Arrange
        var exception = new Exception("Test Exception");
        
        var mockSystemIoWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);

        mockSystemIoWrapper.Setup(x => x.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<MessageBoxButton>(),
                It.IsAny<MessageBoxImage>()))
            .Returns(MessageBoxResult.OK);

        mockSystemIoWrapper.Setup(m => m.StartProcess(It.IsAny<ProcessStartInfo>()))
            .Returns(It.IsAny<Process>());

        // Act
        DefaultExceptionHandler.HandleException(mockSystemIoWrapper.Object, "Test Message", exception);

        // Assert
        mockSystemIoWrapper.Verify(
            m => m.StartProcess(It.Is<ProcessStartInfo>(p =>
                p.FileName.Contains(Logger.LogDirectory))), Times.Once);

        mockSystemIoWrapper.Verify(
            m => m.StartProcess(It.Is<ProcessStartInfo>(p =>
                p.FileName.Contains(DefaultExceptionHandler.GithubNewBugUrl))), Times.Once);
    }
}