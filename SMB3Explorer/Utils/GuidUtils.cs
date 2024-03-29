﻿using System;

namespace SMB3Explorer.Utils;

/// <summary>
/// This is a helper class for the conversion of GUIDs to and from the BLOB format used in the database.
/// </summary>
public static class GuidUtils
{
    public static byte[] ToBlob(this Guid guid)
    {
        var stringRepresentation = guid.ToString("N").ToUpperInvariant();
        var bytes = new byte[16];
        for (var i = 0; i < 16; i++)
        {
            bytes[i] = Convert.ToByte(stringRepresentation.Substring(i * 2, 2), 16);
        }
        
        return bytes;
    }
    
    public static Guid ToGuid(this byte[] bytes)
    {
        var stringRepresentation = BitConverter.ToString(bytes).Replace("-", "");
        var guid = Guid.ParseExact(stringRepresentation, "N");
        
        return guid;
    }
}