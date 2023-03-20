using Microsoft.JSInterop;

namespace partsbin.Services;

public interface IFileService
{
    Task DownloadFile(string fileName, string dataUrl);
}

public class FileService : IFileService
{
    private readonly IJSRuntime _jsRuntime;

    public FileService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }
    
    public async Task DownloadFile(string fileName, string dataUrl)
    {
        await _jsRuntime.InvokeAsync<object>("downloadFile", fileName, dataUrl);
    }
}