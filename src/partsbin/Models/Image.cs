namespace partsbin.Models;

public class Image
{
    public int Id { get; set; }
    public int? PartId { get; set; } = null;
    public required string Filename { get; init; }
    public required string ContentType { get; set; }
    public string FileId { get; set; } = string.Empty;
}