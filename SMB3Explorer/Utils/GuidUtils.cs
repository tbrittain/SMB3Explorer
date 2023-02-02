using System;

namespace SMB3Explorer.Utils;

/// <summary>
/// This is a helper class for the conversion of GUIDs to and from the BLOB format used in the database.
/// </summary>
public static class GuidUtils
{
    public static byte[] ToBlob(this Guid guid)
    {
        return guid.ToByteArray();
    }
}