namespace SMB3Explorer.Utils;

public static class SafeParseDouble
{
    public static double Parse(string value)
    {
        return double.TryParse(value, out var result)
            ? result
            : 0;
    }
}