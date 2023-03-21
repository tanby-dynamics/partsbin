namespace partsbin.BusinessLogic.Helpers;

public static class HasContentExtension
{
    public static bool HasContent(this string? value) => !string.IsNullOrWhiteSpace(value);
    public static bool HasContent(this decimal? value) => value is not null && value != 0.0m;
}