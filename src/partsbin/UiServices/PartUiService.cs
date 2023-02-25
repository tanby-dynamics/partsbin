using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast.Services;
using partsbin.Models;
using partsbin.Pages;
using partsbin.Services;
using partsbin.Shared;

namespace partsbin.UiServices;

public interface IPartUiService
{
    Task<Part?> AddPart(
        string? partType = null,
        string? location = null);
    Task EditPart(Part part);
    Task EditQuantity(Part part);
    Task SelectPartType(Part part);
    Task SelectRange(Part part);
    Task SelectPartName(Part part);
    Task SelectPackageType(Part part);
    Task SelectValueUnit(Part part);
    Task SelectManufacturer(Part part);
    Task SelectLocation(Part part);
}

public class PartUiService : IPartUiService
{
    private readonly IModalService _modalService;
    private readonly IPartFieldService _partFieldService;

    public PartUiService(IModalService modalService, IPartFieldService partFieldService)
    {
        _modalService = modalService;
        _partFieldService = partFieldService;
    }
    
    public async Task<Part?> AddPart(
        string? partType = null,
        string? location = null)
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
        
        var modal = _modalService.Show<AddEditPartModal>("Add part", parameters);
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
        var result = await ShowSelectStringModal(
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
        var result = await ShowSelectStringModal(
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
        var result = await ShowSelectStringModal(
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
        var result = await ShowSelectStringModal(
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
        var result = await ShowSelectStringModal(
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
        var result = await ShowSelectStringModal(
            part?.Manufacturer ?? string.Empty, 
            manufacturers, 
            "Select manufacturer");

        if (result.Cancelled || result.Data is null) return;

        part!.Manufacturer = result.Data as string;
    }

    public async Task SelectLocation(Part part)
    {
        var locations = _partFieldService.GetUniqueLocations()
            .OrderBy(x => x);
        var result = await ShowSelectStringModal(
            part?.Location ?? string.Empty, 
            locations, 
            "Select location");

        if (result.Cancelled || result.Data is null) return;

        part!.Location = result.Data as string;
    }

    private async Task<ModalResult> ShowSelectStringModal(
        string selected, 
        IEnumerable<string> selections,
        string title = "Select item")
    {
        var parameters = new ModalParameters
        {
            { "SelectedString", selected },
            { "Selections", selections },
        };
        var options = new ModalOptions { Position = ModalPosition.Middle };
        var modal = _modalService.Show<SelectStringModal>(title, parameters, options);
        
        return await modal.Result;
    }
}