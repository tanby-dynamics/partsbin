using Blazored.Modal;
using Blazored.Modal.Services;
using partsbin.BusinessLogic.Models;
using partsbin.BusinessLogic.Services;
using partsbin.Shared.Parts;

namespace partsbin.Services;

public interface IPartUiService
{
    Task<Part?> AddPart(
        string? partType = null,
        string? location = null,
        string? manufacturer = null);
    Task EditPart(Part part);
    Task EditQuantity(Part part);
    Task SelectPartType(Part part);
    Task SelectRange(Part part);
    Task SelectPartName(Part part);
    Task SelectPackageType(Part part);
    Task SelectValueUnit(Part part);
    Task SelectManufacturer(Part part);
    Task SelectLocation(Part part);
    Task SelectPartNumber(Part part);
}

public class PartUiService : IPartUiService
{
    private readonly IModalService _modalService;
    private readonly IPartFieldService _partFieldService;
    private readonly ISelectStringUiService _selectStringUiService;
    private readonly IConfirmUiService _confirmUiService;

    public PartUiService(
        IModalService modalService, 
        IPartFieldService partFieldService, 
        ISelectStringUiService selectStringUiService,
        IConfirmUiService confirmUiService)
    {
        _modalService = modalService;
        _partFieldService = partFieldService;
        _selectStringUiService = selectStringUiService;
        _confirmUiService = confirmUiService;
    }
    
    public async Task<Part?> AddPart(
        string? partType = null,
        string? location = null,
        string? manufacturer = null)
    {
        var parameters = new ModalParameters();
        
        if (partType is not null)
        {
            parameters.Add("PartType", partType);
        }

        if (location is not null)
        {
            parameters.Add("Location", location);
        }

        if (manufacturer is not null)
        {
            parameters.Add("Manufacturer", manufacturer);
        }

        var modalOptions = new ModalOptions
        {
            DisableBackgroundCancel = true
        };
        var modal = _modalService.Show<AddEditPartModal>("Add part", parameters, modalOptions);
        var result = await modal.Result;

        if (result.Cancelled || result.Data is null)
        {
            return null;
        }

        return result.Data as Part;
    }

    public async Task EditPart(Part part)
    {
        var parameters = new ModalParameters { { "Part", part } };
        var modal = _modalService.Show<AddEditPartModal>("Edit part", parameters);
        
        await modal.Result;
    }

    public async Task EditQuantity(Part part)
    {
        var parameters = new ModalParameters { { "Part", part } };
        var modal = _modalService.Show<UpdatePartQuantityModal>("Update quantity", parameters);

        await modal.Result;
    }

    public async Task SelectPartType(Part part)
    {
        var partTypes = (await _partFieldService.GetUniquePartTypes())
            .OrderBy(x => x);
        var result = await _selectStringUiService.ShowModal(
            part?.PartType ?? string.Empty,
            partTypes,
            "Select part type");

        if (result.Cancelled || result.Data is null) return;

        part!.PartType = result.Data as string;
    }

    public async Task SelectRange(Part part)
    {
        var ranges = (await _partFieldService.GetUniqueRanges())
            .OrderBy(x => x);
        var result = await _selectStringUiService.ShowModal(
            part?.Range ?? string.Empty, 
            ranges, 
            "Select range");

        if (result.Cancelled || result.Data is null) return;

        part!.Range = result.Data as string;
    }

    public async Task SelectPartName(Part part)
    {
        var partNames = (await _partFieldService.GetUniquePartNames())
            .OrderBy(x => x);
        var result = await _selectStringUiService.ShowModal(
            part?.PartName ?? string.Empty, 
            partNames, 
            "Select part name");

        if (result.Cancelled || result.Data is null) return;

        part!.PartName = result.Data as string;
    }

    public async Task SelectPackageType(Part part)
    {
        var packageTypes = (await _partFieldService.GetUniquePackageTypes())
            .OrderBy(x => x);
        var result = await _selectStringUiService.ShowModal(
            part?.PackageType ?? string.Empty, 
            packageTypes, 
            "Select package type");

        if (result.Cancelled || result.Data is null) return;

        part!.PackageType = result.Data as string;
    }

    public async Task SelectValueUnit(Part part)
    {
        var valueUnits = (await _partFieldService.GetUniqueValueUnits())
            .OrderBy(x => x);
        var result = await _selectStringUiService.ShowModal(
            part?.ValueUnit ?? string.Empty, 
            valueUnits, 
            "Select value unit");

        if (result.Cancelled || result.Data is null) return;

        part!.ValueUnit = result.Data as string;
    }

    public async Task SelectManufacturer(Part part)
    {
        var manufacturers = (await _partFieldService.GetUniqueManufacturers())
            .OrderBy(x => x);
        var result = await _selectStringUiService.ShowModal(
            part?.Manufacturer ?? string.Empty, 
            manufacturers, 
            "Select manufacturer");

        if (result.Cancelled || result.Data is null) return;

        part!.Manufacturer = result.Data as string;
    }

    public async Task SelectPartNumber(Part part)
    {
        var partNumbers = (await _partFieldService.GetUniquePartNumbers())
            .OrderBy(x => x);
        var result = await _selectStringUiService.ShowModal(
            part?.PartNumber ?? string.Empty, 
            partNumbers, 
            "Select part number");

        if (result.Cancelled || result.Data is null) return;

        part!.PartNumber = result.Data as string;
    }

    public async Task SelectLocation(Part part)
    {
        var locations = (await _partFieldService.GetUniqueLocations())
            .OrderBy(x => x);
        var result = await _selectStringUiService.ShowModal(
            part?.Location ?? string.Empty, 
            locations, 
            "Select location");

        if (result.Cancelled || result.Data is null) return;

        part!.Location = result.Data as string;
    }
}