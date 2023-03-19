namespace SMB3Explorer.Models.Internal;

public record struct AppDataSummary(int NumberOfFiles, long TotalSize);

public record struct AppDataFailedPurgeResult(string FileName, long Size);