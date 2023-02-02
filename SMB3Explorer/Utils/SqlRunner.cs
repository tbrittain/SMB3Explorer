using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace SMB3Explorer.Utils;

/// <summary>
/// Enum mapping to SQL file embedded resources in the Resources/Sql folder.
/// </summary>
public enum SqlFile
{
    [Description("GetAvailableTables.sql")]
    GetAvailableTables,
}

public static class SqlRunner
{
    /// <summary>
    /// Abstracts the process of getting a SQL file from the embedded resources.
    /// </summary>
    /// <param name="file">A SQL file in the embedded resources mapped by the <see cref="SqlFile"/> enum</param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static string GetSqlCommand(SqlFile file)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"SMB3Explorer.Resources.Sql.{GetSqlFileName(file)}";

        using var stream = assembly.GetManifestResourceStream(resourceName);
        using var reader = new StreamReader(stream ?? throw new InvalidOperationException("Invalid resource name"));
        
        var result = reader.ReadToEnd();
        return result;
    }
    
    // We can generalize this to get any enum description later if we need to
    private static string GetSqlFileName(SqlFile sqlFile)
    {
        var type = sqlFile.GetType();
        var memberInfo = type.GetMember(sqlFile.ToString());
        var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
        var description = ((DescriptionAttribute)attributes[0]).Description;
        return description;
    }
}