using System.Diagnostics;
using System.Windows;
using Moq;
using SMB3Explorer.Services.SystemInteropWrapper;
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

        var mockSystemInteropWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);
        
        mockSystemInteropWrapper.Setup(m => m.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<MessageBoxButton>(), It.IsAny<MessageBoxImage>()))
            .Returns(MessageBoxResult.OK);
 
        mockSystemInteropWrapper.Setup(m => m.SetClipboardText(It.IsAny<string>()));
        
        mockSystemInteropWrapper.Setup(m => m.StartProcess(It.IsAny<ProcessStartInfo>()))
            .Returns(It.IsAny<Process>());
        
        mockSystemInteropWrapper.SetupGet(m => m.MessageBoxText)
            .Returns(userFriendlyMessage);

        // Act
        DefaultExceptionHandler.HandleException(mockSystemInteropWrapper.Object, userFriendlyMessage, exception);

        // Assert
        // Ensure that the user-friendly message is displayed in the message box
        var messageBoxText = mockSystemInteropWrapper.Object.MessageBoxText;
        Assert.Contains(userFriendlyMessage, messageBoxText);
    }

    [Fact]
    public void HandleException_DisplaysMessageBoxAndCopiesStackTraceToClipboard_WhenExceptionIsNotNull()
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

        var mockSystemInteropWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);
        
        mockSystemInteropWrapper.Setup(x => x.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<MessageBoxButton>(),
                It.IsAny<MessageBoxImage>()))
            .Returns(MessageBoxResult.OK);
        
        mockSystemInteropWrapper.Setup(x => x.SetClipboardText(It.IsAny<string>()));
        
        mockSystemInteropWrapper.Setup(m => m.StartProcess(It.IsAny<ProcessStartInfo>()))
            .Returns(It.IsAny<Process>());

        // Act
        DefaultExceptionHandler.HandleException(mockSystemInteropWrapper.Object, userFriendlyMessage, exception);

        // Assert
        mockSystemInteropWrapper.Verify(x => x.ShowMessageBox(
            It.Is<string>(message => message.Contains(userFriendlyMessage)),
            It.IsAny<string>(),
            MessageBoxButton.OKCancel,
            MessageBoxImage.Error), Times.Once);

        mockSystemInteropWrapper.Verify(
            x => x.SetClipboardText(It.Is<string>(text => text.Contains($"{exception.Message}{exception.StackTrace}"))), Times.Once);
    }

    [Fact]
    public void HandleException_OpensGitHubIssuePage_WhenUserClicksOK()
    {
        // Arrange
        var exception = new Exception("Test Exception");
        
        var mockSystemInteropWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);

        mockSystemInteropWrapper.Setup(x => x.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<MessageBoxButton>(),
                It.IsAny<MessageBoxImage>()))
            .Returns(MessageBoxResult.OK);
        
        mockSystemInteropWrapper.Setup(x => x.SetClipboardText(It.IsAny<string>()));
        
        mockSystemInteropWrapper.Setup(m => m.StartProcess(It.IsAny<ProcessStartInfo>()))
            .Returns(It.IsAny<Process>());

        // Act
        DefaultExceptionHandler.HandleException(mockSystemInteropWrapper.Object, "Test Message", exception);

        // Assert
        mockSystemInteropWrapper.Verify(
            m => m.StartProcess(It.Is<ProcessStartInfo>(p =>
                p.Arguments.Contains(DefaultExceptionHandler.GithubNewIssueUrl))), Times.Once);
    }
}