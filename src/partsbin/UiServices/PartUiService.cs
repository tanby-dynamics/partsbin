using Blazored.Modal;
using Blazored.Modal.Services;
using partsbin.Models;
using partsbin.Services;
using partsbin.Shared;

namespace partsbin.UiServices;

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
    Task<bool> Confirm(string title, string caption = "Are you sure?");
    Task SelectPartNumber(Part part);
}

public class PartUiService : IPartUiService
{
    private readonly IModalService _modalService;
    private readonly IPartFieldService _partFieldService;
    private readonly ISelectStringUiService _selectStringUiService;

    public PartUiService(IModalService modalService, IPartFieldService partFieldService, ISelectStringUiService selectStringUiService)
    {
        _modalService = modalService;
        _partFieldService = partFieldService;
        _selectStringUiService = selectStringUiService;
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
        var partTypes = _partFieldService.GetUniquePartTypes()
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
        var ranges = _partFieldService.GetUniqueRanges()
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
        var partNames = _partFieldService.GetUniquePartNames()
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
        var packageTypes = _partFieldService.GetUniquePackageTypes()
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
        var valueUnits = _partFieldService.GetUniqueValueUnits()
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
        var manufacturers = _partFieldService.GetUniqueManufacturers()
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
        var partNumbers = _partFieldService.GetUniquePartNumbers()
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
        var locations = _partFieldService.GetUniqueLocations()
            .OrderBy(x => x);
        var result = await _selectStringUiService.ShowModal(
            part?.Location ?? string.Empty, 
            locations, 
            "Select location");

        if (result.Cancelled || result.Data is null) return;

        part!.Location = result.Data as string;
    }


    public async Task<bool> Confirm(string title, string caption = "Are you sure")
    {
        var parameters = new ModalParameters
        {
            { "Caption", caption }
        };
        var options = new ModalOptions
        {
            Position = ModalPosition.Middle
        };
        var modal = _modalService.Show<ConfirmationModal>(
            title,
            parameters,
            options);
        var result = await modal.Result;

        return result.Confirmed;
    }
}