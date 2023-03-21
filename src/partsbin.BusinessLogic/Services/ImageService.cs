using Microsoft.AspNetCore.Components.Forms;
using partsbin.BusinessLogic.Helpers;
using partsbin.BusinessLogic.Models;

namespace partsbin.BusinessLogic.Services;

public interface IImageService
{
    Task Upload(IBrowserFile file, Part? part = null);
    Task Delete(Image image);
    /// <summary>
    /// Pull the specified image from file storage and return it as a Base64
    /// encoded URL
    /// </summary>
    /// <param name="image"></param>
    /// <returns>The content of the image as a Base64 encoded URL</returns>
    Task<string> GetBase64Url(Image image);
    Task<IEnumerable<Image>> GetAllImagesForPart(Part part);
    Task UpdateImage(Image image);
    Task DuplicateImageIntoPart(Image sourceImage, Part part);
}

public class ImageService : IImageService
{
    private readonly IDbFactory _dbFactory;

    public ImageService(IDbFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public async Task Upload(IBrowserFile file, Part? part = null)
    {
        using var db = await _dbFactory.GetDatabase();
        var images = db.GetCollection<Image>();
        var image = new Image
        {
            FileName = file.Name,
            ContentType = file.ContentType,
            PartId = part?.Id
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

    public async Task<IEnumerable<Image>> GetAllImagesForPart(Part part)
    {
        using var db = await _dbFactory.GetDatabase();

        var images = await db.GetCollection<Image>().Query()
            .Where(x => x.PartId == part.Id)
            .ToListAsync();
        
        return images;
    }

    public async Task UpdateImage(Image image)
    {
        using var db = await _dbFactory.GetDatabase();
        await db.GetCollection<Image>().UpdateAsync(image);
    }

    public async Task DuplicateImageIntoPart(Image sourceImage, Part part)
    {
        using var db = await _dbFactory.GetDatabase();
        
        // Create duplicate image
        var images = db.GetCollection<Image>();
        var duplicateImage = new Image
        {
            PartId = part.Id,
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
