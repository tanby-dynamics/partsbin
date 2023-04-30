using Microsoft.AspNetCore.Components.Forms;
using partsbin.BusinessLogic.Helpers;
using partsbin.BusinessLogic.Models;

namespace partsbin.BusinessLogic.Services;

public interface IImageService
{
    Task Upload(IBrowserFile file, Part part);
    Task Upload(IBrowserFile file, Equipment equipment);
    Task Delete(Image image);
    /// <summary>
    /// Pull the specified image from file storage and return it as a Base64
    /// encoded URL
    /// </summary>
    /// <param name="image"></param>
    /// <returns>The content of the image as a Base64 encoded URL</returns>
    Task<string> GetBase64Url(Image image);
    Task<IEnumerable<Image>> GetAllImages(Part part);
    Task<IEnumerable<Image>> GetAllImages(Equipment equipment);
    Task UpdateImage(Image image);
    Task DuplicateImage(Image sourceImage, Part part);
    Task DuplicateImage(Image sourceImage, Equipment equipment);
}

public class ImageService : IImageService
{
    private readonly IDbFactory _dbFactory;

    public ImageService(IDbFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public Task Upload(IBrowserFile file, Part part)
    {
        return Upload(file, part: part, equipment: null);
    }

    public Task Upload(IBrowserFile file, Equipment equipment)
    {
        return Upload(file, part: null, equipment: equipment);
    }

    private async Task Upload(IBrowserFile file, Part? part = null, Equipment? equipment = null)
    {
        using var db = await _dbFactory.GetDatabase();
        var images = db.GetCollection<Image>();
        var image = new Image
        {
            FileName = file.Name,
            ContentType = file.ContentType,
            PartId = part?.Id,
            EquipmentId = equipment?.Id
        };
        await images.InsertAsync(image);

        // This is sad but db.FileStorage.UploadAsync seems to read from the stream synchronously and fall over,
        // so it can't just take `file.OpenReadStream` directly. So we stream it into a MemoryStream first.
        var stream = new MemoryStream();
        await file.OpenReadStream(file.Size).CopyToAsync(stream);
        stream.Seek(0, SeekOrigin.Begin);
        
        // Upload the file into storage
        var fileInfo = await db.FileStorage.UploadAsync(image.Id.ToString(), image.FileName, stream);

        // Set the FileId for the image
        image.FileId = fileInfo.Id;
        await images.UpdateAsync(image);
    }

    public async Task Delete(Image image)
    {
        using var db = await _dbFactory.GetDatabase();

        if (image.FileId.HasContent())
        {
            await db.FileStorage.DeleteAsync(image.FileId);
        }
        await db.GetCollection<Image>().DeleteAsync(image.Id);
    }

    public async Task<string> GetBase64Url(Image image)
    {
        if (string.IsNullOrEmpty(image.FileId)) return string.Empty;

        using var db = await _dbFactory.GetDatabase();
        using var stream = new MemoryStream();

        await db.FileStorage.DownloadAsync(image.FileId, stream);
        
        var buffer = stream.ToArray();
        var base64 = Convert.ToBase64String(buffer);

        return $"data:{image.ContentType};base64,{base64}";
    }

    public Task<IEnumerable<Image>> GetAllImages(Part part)
    {
        return GetAllImages(part: part, equipment: null);
    }

    public Task<IEnumerable<Image>> GetAllImages(Equipment equipment)
    {
        return GetAllImages(part: null, equipment: equipment);
    }

    private async Task<IEnumerable<Image>> GetAllImages(Part? part, Equipment? equipment)
    {
        using var db = await _dbFactory.GetDatabase();
        var partId = part?.Id ?? -1;
        var equipmentId = equipment?.Id ?? -1;

        var images = await db.GetCollection<Image>().Query()
            .Where(x => x.PartId == partId || x.EquipmentId == equipmentId)
            .ToListAsync();
        
        return images;
    }

    public async Task UpdateImage(Image image)
    {
        using var db = await _dbFactory.GetDatabase();
        await db.GetCollection<Image>().UpdateAsync(image);
    }

    public Task DuplicateImage(Image sourceImage, Part part)
    {
        return DuplicateImage(sourceImage, part, null);
    }

    public Task DuplicateImage(Image sourceImage, Equipment equipment)
    {
        return DuplicateImage(sourceImage, null, equipment);
    }

    private async Task DuplicateImage(Image sourceImage, Part? part, Equipment? equipment)
    {
        using var db = await _dbFactory.GetDatabase();
        
        // Create duplicate image
        var images = db.GetCollection<Image>();
        var duplicateImage = new Image
        {
            PartId = part?.Id,
            EquipmentId = equipment?.Id,
            FileName = sourceImage.FileName,
            ContentType = sourceImage.ContentType,
            Notes = sourceImage.Notes,
            HtmlNotes = sourceImage.HtmlNotes
        };
        await images.InsertAsync(duplicateImage);
        
        // Copy file data into a new file
        var stream = new MemoryStream();
        await db.FileStorage.DownloadAsync(sourceImage.FileId, stream);
        stream.Seek(0, SeekOrigin.Begin);
        var fileInfo = await db.FileStorage.UploadAsync(
            sourceImage.Id.ToString(), 
            sourceImage.FileName, 
            stream);
        
        // Update duplicate with new file's ID
        duplicateImage.FileId = fileInfo.Id;
        await images.UpdateAsync(duplicateImage);
    }
}
