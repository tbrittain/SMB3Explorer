using Moq;
using SMB3Explorer.Services.CsvWriterWrapper;
using SMB3Explorer.Services.SystemInteropWrapper;
using SMB3Explorer.Utils;

namespace SMB3ExplorerTests.UtilsTests;

public class CsvUtilsTests
{
    [Fact]
    public async Task ExportCsv_ShouldExportCsvFile()
    {
        // Arrange
        var expectedFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SMB3Explorer", "export.csv");
        var records = new List<Foo> { new Foo { Id = 1, Name = "Bar" } }.ToAsyncEnumerable();
        
        var mockSystemInteropWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);
        
        mockSystemInteropWrapper
            .Setup(x => x.DirectoryExists(CsvUtils.DefaultDirectory))
            .Returns(true);
        
        mockSystemInteropWrapper
            .Setup(x => x.FileExists(expectedFilePath))
            .Returns(false);
        
        mockSystemInteropWrapper
            .Setup(x => x.FileCreate(expectedFilePath))
            .Returns(ValueTask.CompletedTask);
        
        mockSystemInteropWrapper
            .Setup(x => x.CreateStreamWriter(expectedFilePath))
            .Returns(new StreamWriter(Stream.Null));

        var mockCsvWriterWrapper = new Mock<ICsvWriterWrapper>(MockBehavior.Strict);
        
        mockSystemInteropWrapper
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
        var (actualFilePath, _) = await CsvUtils.ExportCsv(mockSystemInteropWrapper.Object, records, "export.csv");

        // Assert
        actualFilePath.Should().Be(expectedFilePath);
        mockSystemInteropWrapper.VerifyAll();
    }
    
    [Fact]
    public async Task ExportCsv_ShouldCreateDirectoryIfNotExists()
    {
        // Arrange
        var expectedFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SMB3Explorer", "export.csv");
        var records = new List<Foo> { new Foo { Id = 1, Name = "Bar" } }.ToAsyncEnumerable();

        var mockSystemInteropWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);

        mockSystemInteropWrapper
            .Setup(x => x.DirectoryExists(CsvUtils.DefaultDirectory))
            .Returns(false);

        mockSystemInteropWrapper
            .Setup(x => x.DirectoryCreate(CsvUtils.DefaultDirectory))
            .Verifiable();

        mockSystemInteropWrapper
            .Setup(x => x.FileExists(expectedFilePath))
            .Returns(false);

        mockSystemInteropWrapper
            .Setup(x => x.FileCreate(expectedFilePath))
            .Returns(ValueTask.CompletedTask);
        
        mockSystemInteropWrapper
            .Setup(x => x.CreateStreamWriter(expectedFilePath))
            .Returns(new StreamWriter(Stream.Null));

        var mockCsvWriterWrapper = new Mock<ICsvWriterWrapper>(MockBehavior.Strict);
        
        mockSystemInteropWrapper
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
        var (actualFilePath, _) = await CsvUtils.ExportCsv(mockSystemInteropWrapper.Object, records, "export.csv");

        // Assert
        actualFilePath.Should().Be(expectedFilePath);
        mockSystemInteropWrapper.VerifyAll();
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

        var mockSystemInteropWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);

        mockSystemInteropWrapper
            .Setup(x => x.DirectoryExists(CsvUtils.DefaultDirectory))
            .Returns(true);

        mockSystemInteropWrapper
            .Setup(x => x.FileExists(expectedFilePath))
            .Returns(false);

        mockSystemInteropWrapper
            .Setup(x => x.FileCreate(expectedFilePath))
            .Returns(ValueTask.CompletedTask);
        
        mockSystemInteropWrapper
            .Setup(x => x.CreateStreamWriter(expectedFilePath))
            .Returns(new StreamWriter(Stream.Null));

        var mockCsvWriterWrapper = new Mock<ICsvWriterWrapper>(MockBehavior.Strict);
        
        mockSystemInteropWrapper
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
        var (actualFilePath, rowCount) = await CsvUtils.ExportCsv(mockSystemInteropWrapper.Object, records, "export.csv", 2);

        // Assert
        actualFilePath.Should().Be(expectedFilePath);
        rowCount.Should().Be(2);
        mockSystemInteropWrapper.VerifyAll();
    }
    
    [Fact]
    public async Task ExportCsv_FileExists_DeletesFile()
    {
        // Arrange
        var expectedFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SMB3Explorer", "export.csv");
        var records = new List<Foo> { new Foo { Id = 1, Name = "Bar" } }.ToAsyncEnumerable();

        var mockSystemInteropWrapper = new Mock<ISystemIoWrapper>(MockBehavior.Strict);

        mockSystemInteropWrapper
            .Setup(x => x.DirectoryExists(CsvUtils.DefaultDirectory))
            .Returns(true);

        mockSystemInteropWrapper
            .Setup(x => x.FileExists(expectedFilePath))
            .Returns(true);

        mockSystemInteropWrapper
            .Setup(x => x.FileCreate(expectedFilePath))
            .Returns(ValueTask.CompletedTask);

        mockSystemInteropWrapper
            .Setup(x => x.FileDelete(expectedFilePath))
            .Returns(true)
            .Verifiable();
        
        mockSystemInteropWrapper
            .Setup(x => x.CreateStreamWriter(expectedFilePath))
            .Returns(new StreamWriter(Stream.Null));

        var mockCsvWriterWrapper = new Mock<ICsvWriterWrapper>(MockBehavior.Strict);
        
        mockSystemInteropWrapper
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
        var (actualFilePath, _) = await CsvUtils.ExportCsv(mockSystemInteropWrapper.Object, records, "export.csv");

        // Assert
        actualFilePath.Should().Be(expectedFilePath);
        mockSystemInteropWrapper.VerifyAll();
    }

    private class Foo
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
