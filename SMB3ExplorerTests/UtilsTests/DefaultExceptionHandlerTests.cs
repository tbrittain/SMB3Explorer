using System.Diagnostics;
using System.Windows;
using Moq;
using SMB3Explorer.Services;
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

        var mockSystemInteropWrapper = new Mock<ISystemInteropWrapper>(MockBehavior.Strict);
        mockSystemInteropWrapper.Setup(m => m.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<MessageBoxButton>(), It.IsAny<MessageBoxImage>()))
            .Returns(MessageBoxResult.OK);
 
        mockSystemInteropWrapper.Setup(m => m.SetClipboardText(It.IsAny<string>()));
        mockSystemInteropWrapper.Setup(m => m.StartProcess(It.IsAny<ProcessStartInfo>()))
            .Returns(It.IsAny<Process>());
        mockSystemInteropWrapper.SetupGet(m => m.MessageBoxText)
            .Returns(userFriendlyMessage);

        // Act
        DefaultExceptionHandler.HandleException(userFriendlyMessage, exception, mockSystemInteropWrapper.Object);

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
        var exception = new Exception("Test exception");

        var messageBoxWrapperMock = new Mock<ISystemInteropWrapper>();
        messageBoxWrapperMock.Setup(x => x.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>(),
                It.IsAny<MessageBoxButton>(),
                It.IsAny<MessageBoxImage>()))
            .Returns(MessageBoxResult.OK);

        // Act
        DefaultExceptionHandler.HandleException(userFriendlyMessage, exception, messageBoxWrapperMock.Object);

        // Assert
        messageBoxWrapperMock.Verify(x => x.ShowMessageBox(
            It.Is<string>(message => message.Contains(userFriendlyMessage)),
            It.IsAny<string>(),
            MessageBoxButton.OKCancel,
            MessageBoxImage.Error), Times.Once);

        messageBoxWrapperMock.Verify(
            x => x.SetClipboardText(It.Is<string>(text => text.Contains(exception.StackTrace))), Times.Once);
    }

    [Fact]
    public void HandleException_OpensGitHubIssuePage_WhenUserClicksOK()
    {
        // Arrange
        var wrapper = new Mock<ISystemInteropWrapper>();
        wrapper.Setup(m => m.ShowMessageBox(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<MessageBoxButton>(),
                It.IsAny<MessageBoxImage>()))
            .Returns(MessageBoxResult.OK);

        var exception = new Exception("Test Exception");
        var expectedClipboardText = exception.StackTrace ?? "Unknown error";

        wrapper.Setup(m => m.MessageBoxText)
            .Returns(
                $"Test Message A full stack trace has been copied to your clipboard. {Environment.NewLine}{expectedClipboardText}");

        // Act
        DefaultExceptionHandler.HandleException("Test Message", exception, wrapper.Object);

        // Assert
        wrapper.Verify(
            m => m.StartProcess(It.Is<ProcessStartInfo>(p =>
                p.FileName.Contains(DefaultExceptionHandler.GithubNewIssueUrl))), Times.Once);
    }
}