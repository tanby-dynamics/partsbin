using partsbin.BusinessLogic.Models;

namespace partsbin.BusinessLogic.Services.PartServices;

public interface IDuplicatePart
{
    Task<Part> ExecuteAsync(Part source);
}

public class DuplicatePart : IDuplicatePart
{
    private readonly IPartService _partService;
    private readonly IImageService _imageService;
    private readonly IFileService _fileService;

    public DuplicatePart(
        IPartService partService,
        IImageService imageService,
        IFileService fileService)
    {
        _partService = partService;
        _imageService = imageService;
        _fileService = fileService;
    }
    
    public async Task<Part> ExecuteAsync(Part source)
    {
        var duplicatePart = await _partService.AddPart(new Part
        {
            PartType = source.PartType,
            Range = source.Range,
            Location = source.Location,
            Manufacturer = source.Manufacturer,
            HtmlNotes = source.HtmlNotes,
            Documents = source.Documents.ToList(),
            Suppliers = source.Suppliers.ToList(),
            Notes = source.Notes,
            PackageType = source.PackageType,
            Value = source.Value,
            ValueUnit = source.ValueUnit,
            PartName = source.PartName,
            PartNumber = source.PartNumber,
            Quantity = source.Quantity,
            QuantityIsApproximate = source.QuantityIsApproximate
        });

        var sourceImages = await _imageService.GetAllImages(source);
        foreach (var image in sourceImages)
        {
            await _imageService.DuplicateImage(image, duplicatePart);
        }

        var sourceFiles = await _fileService.GetAllFiles(source);
        foreach (var file in sourceFiles)
        {
            await _fileService.DuplicateFile(file, duplicatePart);
        }

        return duplicatePart;
    }
}