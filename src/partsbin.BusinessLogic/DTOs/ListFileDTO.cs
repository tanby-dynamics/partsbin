namespace partsbin.BusinessLogic.DTOs;

public class ListFileDTO
{
    public required string Id { get; set; }
    public required string FileName { get; set; }
    public required string MimeType { get; set; }
    public required long Length { get; set; }
}
