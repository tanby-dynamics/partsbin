using Microsoft.AspNetCore.Components.Forms;
using partsbin.Helpers;
using partsbin.Models;
using LiteDB.Async;

namespace partsbin.Services;

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
            Filename = file.Name,
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
        var fileInfo = await db.FileStorage.UploadAsync(image.Id.ToString(), image.Filename, stream);

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
}
