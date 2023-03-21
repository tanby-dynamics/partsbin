using System.Net;
using Microsoft.AspNetCore.Mvc;
using partsbin.Helpers;
using partsbin.Services;
using partsbin.Services.DTOs;
using partsbin.Services.Services;

namespace partsbin.Controllers;

[ApiController]
[Route("api/file")]
public class FileController : Controller
{
    private readonly IDbFactory _dbFactory;

    public FileController(IDbFactory dbFactory)
    {
        _dbFactory = dbFactory;
    }

    /// <summary>
    /// Gets the requested file
    /// </summary>
    /// <param name="id">File ID</param>
    /// <returns>The file</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileResult))]
    public async Task<IActionResult> Get(string id)
    {
        using var db = await _dbFactory.GetDatabase();
        var file = await db.FileStorage.FindByIdAsync(id);
        var stream = file.OpenRead();
        var memoryStream = new MemoryStream();

        await stream.CopyToAsync(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);

        return File(
            fileStream: memoryStream, 
            contentType: file.MimeType, 
            fileDownloadName: file.Filename);
    }

    [HttpGet("list")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ListFileDto>))]
    public async Task<IActionResult> List()
    {
        using var db = await _dbFactory.GetDatabase();
        var files = await db.FileStorage.FindAllAsync();
        var response = files
            .Select(f => new ListFileDto
            {
                Id = f.Id,
                FileName = f.Filename,
                MimeType = f.MimeType,
                Length = f.Length
            })
            .ToList();
        
        return Ok(response);
    }
}