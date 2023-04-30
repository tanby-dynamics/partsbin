namespace partsbin.BusinessLogic.Models;

public class Image : IHasNotes
{
    public int Id { get; set; }
    public int? PartId { get; set; } = null;
    public int? EquipmentId { get; set; } = null;
    public required string FileName { get; init; }
    public required string ContentType { get; set; }
    public string FileId { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public string HtmlNotes { get; set; } = string.Empty;
}