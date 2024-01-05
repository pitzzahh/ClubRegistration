using System;
using System.Globalization;

namespace ClubRegistration;

public static class StringExtensions
{
    public static string FirstCharToUpper(this string input) =>
        input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
        };
    
    public static string ToTitleCase(string word) => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(word.ToLower());
    
    public static bool IsNumeric(this string input) => int.TryParse(input, out _);
}