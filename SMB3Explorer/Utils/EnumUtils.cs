using System;
using System.ComponentModel;
using System.Reflection;

namespace SMB3Explorer.Utils;

public static class EnumUtils
{
    public static string GetEnumDescription<T>(this T value) where T : Enum
    {
        var type = value.GetType();
        var member = type.GetMember(value.ToString());
        var attribute = member[0].GetCustomAttribute<DescriptionAttribute>(false);

        return attribute?.Description ?? value.ToString();
    }
}