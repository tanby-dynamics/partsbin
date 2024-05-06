using partsbin.BusinessLogic.Models;

namespace partsbin.BusinessLogic.Services.EquipmentServices;

public interface IDuplicateEquipment
{
    Task<Equipment> ExecuteAsync(Equipment source);
}

public class DuplicateEquipment : IDuplicateEquipment
{
    private readonly IEquipmentService _equipmentService;
    private readonly IImageService _imageService;
    private readonly IFileService _fileService;

    public DuplicateEquipment(
        IEquipmentService equipmentService,
        IImageService imageService,
        IFileService fileService)
    {
        _equipmentService = equipmentService;
        _imageService = imageService;
        _fileService = fileService;
    }
    
    public async Task<Equipment> ExecuteAsync(Equipment source)
    {
        var duplicate = await _equipmentService.AddEquipment(new Equipment
        {
            EquipmentType = source.EquipmentType,
            EquipmentName = source.EquipmentName,
            Location = source.Location,
            Manufacturer = source.Manufacturer,
            HtmlNotes = source.HtmlNotes,
            Documents = source.Documents.ToList(),
            Suppliers = source.Suppliers.ToList(),
            Notes = source.Notes,
            Quantity = source.Quantity,
            QuantityIsApproximate = source.QuantityIsApproximate
        });

        var sourceImages = await _imageService.GetAllImages(source);
        foreach (var image in sourceImages)
        {
            await _imageService.DuplicateImage(image, duplicate);
        }

        var sourceFiles = await _fileService.GetAllFiles(source);
        foreach (var file in sourceFiles)
        {
            await _fileService.DuplicateFile(file, duplicate);
        }

        return duplicate;
    }
}