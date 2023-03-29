using Moq;
using SMB3Explorer.Services.CsvWriterWrapper;
using SMB3Explorer.Services.SystemIoWrapper;
using SMB3Explorer.Utils;
using static SMB3Explorer.Constants.FileExports;

namespace SMB3ExplorerTests.UtilsTests;

public class CsvUtilsTests
{
    [Fact]
    public async Task ExportCsv_ShouldExportCsvFile()
    {
        // Arrange
        var expectedFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SMB3Explorer", "export.csv");
        var records = new List<Foo> { new Foo { Id = 1, Name = "Bar" } }.ToAsyncEnumerable();
        
        var mockSystemIoWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);
        
        mockSystemIoWrapper
            .Setup(x => x.DirectoryExists(BaseExportsDirectory))
            .Returns(true);
        
        mockSystemIoWrapper
            .Setup(x => x.FileExists(expectedFilePath))
            .Returns(false);
        
        mockSystemIoWrapper
            .Setup(x => x.FileCreate(expectedFilePath))
            .Returns(ValueTask.CompletedTask);
        
        mockSystemIoWrapper
            .Setup(x => x.CreateStreamWriter(expectedFilePath))
            .Returns(new StreamWriter(Stream.Null));

        var mockCsvWriterWrapper = new Mock<ICsvWriterWrapper>(MockBehavior.Strict);
        
        mockSystemIoWrapper
            .Setup(x => x.CreateCsvWriter())
            .Returns(mockCsvWriterWrapper.Object);

        mockCsvWriterWrapper
            .Setup(x => x.Initialize(It.IsAny<StreamWriter>()))
            .Verifiable();
        
        mockCsvWriterWrapper
            .Setup(x => x.WriteHeaderAsync<It.IsAnyType>())
            .Returns(Task.CompletedTask)
            .Verifiable();
        
        mockCsvWriterWrapper
            .Setup(x => x.WriteRecordAsync(It.IsAny<It.IsAnyType>()))
            .Returns(Task.CompletedTask)
            .Verifiable();
        
        mockCsvWriterWrapper
            .Setup(x => x.DisposeAsync())
            .Returns(ValueTask.CompletedTask)
            .Verifiable();

        // Act
        var (actualFilePath, _) = await CsvUtils.ExportCsv(mockSystemIoWrapper.Object, records, "export.csv");

        // Assert
        actualFilePath.Should().Be(expectedFilePath);
        mockSystemIoWrapper.VerifyAll();
    }
    
    [Fact]
    public async Task ExportCsv_ShouldCreateDirectoryIfNotExists()
    {
        // Arrange
        var expectedFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SMB3Explorer", "export.csv");
        var records = new List<Foo> { new Foo { Id = 1, Name = "Bar" } }.ToAsyncEnumerable();

        var mockSystemIoWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);

        mockSystemIoWrapper
            .Setup(x => x.DirectoryExists(BaseExportsDirectory))
            .Returns(false);

        mockSystemIoWrapper
            .Setup(x => x.DirectoryCreate(BaseExportsDirectory))
            .Verifiable();

        mockSystemIoWrapper
            .Setup(x => x.FileExists(expectedFilePath))
            .Returns(false);

        mockSystemIoWrapper
            .Setup(x => x.FileCreate(expectedFilePath))
            .Returns(ValueTask.CompletedTask);
        
        mockSystemIoWrapper
            .Setup(x => x.CreateStreamWriter(expectedFilePath))
            .Returns(new StreamWriter(Stream.Null));

        var mockCsvWriterWrapper = new Mock<ICsvWriterWrapper>(MockBehavior.Strict);
        
        mockSystemIoWrapper
            .Setup(x => x.CreateCsvWriter())
            .Returns(mockCsvWriterWrapper.Object);

        mockCsvWriterWrapper
            .Setup(x => x.Initialize(It.IsAny<StreamWriter>()))
            .Verifiable();
        
        mockCsvWriterWrapper
            .Setup(x => x.WriteHeaderAsync<It.IsAnyType>())
            .Returns(Task.CompletedTask)
            .Verifiable();
        
        mockCsvWriterWrapper
            .Setup(x => x.WriteRecordAsync(It.IsAny<It.IsAnyType>()))
            .Returns(Task.CompletedTask)
            .Verifiable();
        
        mockCsvWriterWrapper
            .Setup(x => x.DisposeAsync())
            .Returns(ValueTask.CompletedTask)
            .Verifiable();

        // Act
        var (actualFilePath, _) = await CsvUtils.ExportCsv(mockSystemIoWrapper.Object, records, "export.csv");

        // Assert
        actualFilePath.Should().Be(expectedFilePath);
        mockSystemIoWrapper.VerifyAll();
    }
    
    [Fact]
    public async Task ExportCsv_ShouldExportCsvFileWithLimitedRowCount()
    {
        // Arrange
        var expectedFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SMB3Explorer", "export.csv");
        var records = new List<Foo>
        {
            new Foo { Id = 1, Name = "Bar" },
            new Foo { Id = 2, Name = "Baz" },
            new Foo { Id = 3, Name = "Qux" }
        }.ToAsyncEnumerable();

        var mockSystemIoWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);

        mockSystemIoWrapper
            .Setup(x => x.DirectoryExists(BaseExportsDirectory))
            .Returns(true);

        mockSystemIoWrapper
            .Setup(x => x.FileExists(expectedFilePath))
            .Returns(false);

        mockSystemIoWrapper
            .Setup(x => x.FileCreate(expectedFilePath))
            .Returns(ValueTask.CompletedTask);
        
        mockSystemIoWrapper
            .Setup(x => x.CreateStreamWriter(expectedFilePath))
            .Returns(new StreamWriter(Stream.Null));

        var mockCsvWriterWrapper = new Mock<ICsvWriterWrapper>(MockBehavior.Strict);
        
        mockSystemIoWrapper
            .Setup(x => x.CreateCsvWriter())
            .Returns(mockCsvWriterWrapper.Object);

        mockCsvWriterWrapper
            .Setup(x => x.Initialize(It.IsAny<StreamWriter>()))
            .Verifiable();
        
        mockCsvWriterWrapper
            .Setup(x => x.WriteHeaderAsync<It.IsAnyType>())
            .Returns(Task.CompletedTask)
            .Verifiable();
        
        mockCsvWriterWrapper
            .Setup(x => x.WriteRecordAsync(It.IsAny<It.IsAnyType>()))
            .Returns(Task.CompletedTask)
            .Verifiable();
        
        mockCsvWriterWrapper
            .Setup(x => x.DisposeAsync())
            .Returns(ValueTask.CompletedTask)
            .Verifiable();

        // Act
        var (actualFilePath, rowCount) = await CsvUtils.ExportCsv(mockSystemIoWrapper.Object, records, "export.csv", 2);

        // Assert
        actualFilePath.Should().Be(expectedFilePath);
        rowCount.Should().Be(2);
        mockSystemIoWrapper.VerifyAll();
    }
    
    [Fact]
    public async Task ExportCsv_FileExists_DeletesFile()
    {
        // Arrange
        var expectedFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SMB3Explorer", "export.csv");
        var records = new List<Foo> { new Foo { Id = 1, Name = "Bar" } }.ToAsyncEnumerable();

        var mockSystemIoWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);

        mockSystemIoWrapper
            .Setup(x => x.DirectoryExists(BaseExportsDirectory))
            .Returns(true);

        mockSystemIoWrapper
            .Setup(x => x.FileExists(expectedFilePath))
            .Returns(true);

        mockSystemIoWrapper
            .Setup(x => x.FileCreate(expectedFilePath))
            .Returns(ValueTask.CompletedTask);

        mockSystemIoWrapper
            .Setup(x => x.FileDelete(expectedFilePath))
            .Returns(true)
            .Verifiable();
        
        mockSystemIoWrapper
            .Setup(x => x.CreateStreamWriter(expectedFilePath))
            .Returns(new StreamWriter(Stream.Null));

        var mockCsvWriterWrapper = new Mock<ICsvWriterWrapper>(MockBehavior.Strict);
        
        mockSystemIoWrapper
            .Setup(x => x.CreateCsvWriter())
            .Returns(mockCsvWriterWrapper.Object);

        mockCsvWriterWrapper
            .Setup(x => x.Initialize(It.IsAny<StreamWriter>()))
            .Verifiable();
        
        mockCsvWriterWrapper
            .Setup(x => x.WriteHeaderAsync<It.IsAnyType>())
            .Returns(Task.CompletedTask)
            .Verifiable();
        
        mockCsvWriterWrapper
            .Setup(x => x.WriteRecordAsync(It.IsAny<It.IsAnyType>()))
            .Returns(Task.CompletedTask)
            .Verifiable();
        
        mockCsvWriterWrapper
            .Setup(x => x.DisposeAsync())
            .Returns(ValueTask.CompletedTask)
            .Verifiable();

        // Act
        var (actualFilePath, _) = await CsvUtils.ExportCsv(mockSystemIoWrapper.Object, records, "export.csv");

        // Assert
        actualFilePath.Should().Be(expectedFilePath);
        mockSystemIoWrapper.VerifyAll();
    }

    private class Foo
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
