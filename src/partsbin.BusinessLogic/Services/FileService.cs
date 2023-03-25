using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using partsbin.BusinessLogic.Helpers;
using partsbin.BusinessLogic.Models;
using File = partsbin.BusinessLogic.Models.File;

namespace partsbin.BusinessLogic.Services;

public interface IFileService
{
    Task DownloadFile(string fileName, string dataUrl);
    Task Upload(IBrowserFile browserFile, Part? part = null);
    Task Delete(File file);
    Task<IEnumerable<File>> GetAllFilesForPart(Part part);
    Task UpdateFile(File file);
    Task DuplicateFileIntoPart(File sourceFile, Part part);

}

public class FileService : IFileService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly IDbFactory _dbFactory;

    public FileService(IJSRuntime jsRuntime, IDbFactory dbFactory)
    {
        _jsRuntime = jsRuntime;
        _dbFactory = dbFactory;
    }
    
    public async Task DownloadFile(string fileName, string dataUrl)
    {
        await _jsRuntime.InvokeAsync<object>("Blazor.downloadFile", fileName, dataUrl);
    }
    
    public async Task Upload(IBrowserFile browserFile, Part? part = null)
    {
        using var db = await _dbFactory.GetDatabase();
        
        // Make new file record
        var files = db.GetCollection<File>();
        var file = new File
        {
            FileName = browserFile.Name,
            ContentType = browserFile.ContentType,
            PartId = part?.Id
        };
        await files.InsertAsync(file);

        // Upload the file into storage
        var stream = new MemoryStream();
        await browserFile
            .OpenReadStream(browserFile.Size)
            .CopyToAsync(stream);
        stream.Seek(0, SeekOrigin.Begin);
        var fileInfo = await db.FileStorage.UploadAsync(
            file.Id.ToString(), 
            file.FileName, 
            stream);

        // Set the FileId for the file
        file.FileId = fileInfo.Id;
        await files.UpdateAsync(file);
    }

    public async Task Delete(File file)
    {
        using var db = await _dbFactory.GetDatabase();

        if (file.FileId.HasContent())
        {
            await db.FileStorage.DeleteAsync(file.FileId);
        }
        await db.GetCollection<File>().DeleteAsync(file.Id);
    }
    
    public async Task<IEnumerable<File>> GetAllFilesForPart(Part part)
    {
        using var db = await _dbFactory.GetDatabase();

        var files = await db.GetCollection<File>().Query()
            .Where(x => x.PartId == part.Id)
            .ToListAsync();
        
        return files;
    }

    public async Task UpdateFile(File file)
    {
        using var db = await _dbFactory.GetDatabase();
        await db.GetCollection<File>().UpdateAsync(file);
    }

    public async Task DuplicateFileIntoPart(File sourceFile, Part part)
    {
        using var db = await _dbFactory.GetDatabase();
        
        // Create duplicate file
        var files = db.GetCollection<File>();
        var duplicateFile = new File
        {
            PartId = part.Id,
            FileName = sourceFile.FileName,
            ContentType = sourceFile.ContentType,
            Notes = sourceFile.Notes,
            HtmlNotes = sourceFile.HtmlNotes
        };
        await files.InsertAsync(duplicateFile);
        
        // Copy file data into a new file
        var stream = new MemoryStream();
        await db.FileStorage.DownloadAsync(sourceFile.FileId, stream);
        stream.Seek(0, SeekOrigin.Begin);
        var fileInfo = await db.FileStorage.UploadAsync(
            sourceFile.Id.ToString(), 
            sourceFile.FileName, 
            stream);
        
        // Update duplicate with new file's ID
        duplicateFile.FileId = fileInfo.Id;
        await files.UpdateAsync(duplicateFile);
    }

}