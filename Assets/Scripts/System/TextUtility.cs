using System;
using System.Text.RegularExpressions;

public static class TextUtility
{
    public static string ParseHexCodesToUnicode(string rawText)
    {
        if (string.IsNullOrEmpty(rawText))
            return rawText;

        return Regex.Replace(rawText, @"0x([0-9A-Fa-f]{1,8})", match =>
        {
            try
            {
                string hexValue = match.Groups[1].Value;
                int codePoint = Convert.ToInt32(hexValue, 16);
                return char.ConvertFromUtf32(codePoint);
            }
            catch (Exception)
            {
                return match.Value;
            }
        });
    }
}