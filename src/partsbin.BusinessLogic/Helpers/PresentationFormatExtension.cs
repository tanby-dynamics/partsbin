namespace partsbin.BusinessLogic.Helpers;

public static class PresentationFormatExtension
{
    public static string? PresentationFormat(this string? value)
    {
        return value.HasContent() ? value : "---";
    }
}