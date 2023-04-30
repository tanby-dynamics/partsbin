using Blazored.Modal;
using Blazored.Modal.Services;
using partsbin.BusinessLogic.Models;
using partsbin.BusinessLogic.Services;
using partsbin.Shared.Equipment;

namespace partsbin.Services;

public interface IEquipmentUiService
{
    Task<Equipment?> AddEquipment(
        string? equipmentType = null,
        string? location = null,
        string? manufacturer = null);
    Task EditEquipment(Equipment equipment);
    Task EditQuantity(Equipment equipment);
    Task SelectEquipmentType(Equipment equipment);
    Task SelectEquipmentName(Equipment equipment);
    Task SelectManufacturer(Equipment equipment);
    Task SelectLocation(Equipment equipment);
    Task SelectModelNumber(Equipment equipment);
}

public class EquipmentUiService : IEquipmentUiService
{
    private readonly IModalService _modalService;
    private readonly IEquipmentFieldService _equipmentFieldService;
    private readonly ISelectStringUiService _selectStringUiService;
    private readonly IConfirmUiService _confirmUiService;

    public EquipmentUiService(
        IModalService modalService, 
        IEquipmentFieldService equipmentFieldService, 
        ISelectStringUiService selectStringUiService,
        IConfirmUiService confirmUiService)
    {
        _modalService = modalService;
        _equipmentFieldService = equipmentFieldService;
        _selectStringUiService = selectStringUiService;
        _confirmUiService = confirmUiService;
    }
    
    public async Task<Equipment?> AddEquipment(
        string? equipmentType = null,
        string? location = null,
        string? manufacturer = null)
    {
        var parameters = new ModalParameters();
        
        if (equipmentType is not null)
        {
            parameters.Add("EquipmentType", equipmentType);
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
        var modal = _modalService.Show<AddEditEquipmentModal>("Add equipment", parameters, modalOptions);
        var result = await modal.Result;

        if (result.Cancelled || result.Data is null)
        {
            return null;
        }

        return result.Data as Equipment;
    }

    public async Task EditEquipment(Equipment equipment)
    {
        var parameters = new ModalParameters { { "Equipment", equipment } };
        var modal = _modalService.Show<AddEditEquipmentModal>("Edit equipment", parameters);
        
        await modal.Result;
    }

    public async Task EditQuantity(Equipment equipment)
    {
        var parameters = new ModalParameters { { "Equipment", equipment } };
        var modal = _modalService.Show<UpdateEquipmentQuantityModal>("Update quantity", parameters);

        await modal.Result;
    }

    public async Task SelectEquipmentType(Equipment equipment)
    {
        var equipmentTypes = (await _equipmentFieldService.GetUniqueEquipmentTypes())
            .OrderBy(x => x);
        var result = await _selectStringUiService.ShowModal(
            equipment?.EquipmentType ?? string.Empty,
            equipmentTypes,
            "Select equipment type");

        if (result.Cancelled || result.Data is null) return;

        equipment!.EquipmentType = (string)result.Data;
    }


    public async Task SelectEquipmentName(Equipment equipment)
    {
        var equipmentNames = (await _equipmentFieldService.GetUniqueEquipmentNames())
            .OrderBy(x => x);
        var result = await _selectStringUiService.ShowModal(
            equipment?.EquipmentName ?? string.Empty,
            equipmentNames,
            "Select equipment name");

        if (result.Cancelled || result.Data is null) return;

        equipment!.EquipmentType = (string)result.Data;
    }

    public async Task SelectManufacturer(Equipment equipment)
    {
        var manufacturers = (await _equipmentFieldService.GetUniqueManufacturers())
            .OrderBy(x => x);
        var result = await _selectStringUiService.ShowModal(
            equipment?.Manufacturer ?? string.Empty, 
            manufacturers, 
            "Select manufacturer");

        if (result.Cancelled || result.Data is null) return;

        equipment!.Manufacturer = (string)result.Data;
    }

    public async Task SelectLocation(Equipment equipment)
    {
        var locations = (await _equipmentFieldService.GetUniqueLocations())
            .OrderBy(x => x);
        var result = await _selectStringUiService.ShowModal(
            equipment?.Location ?? string.Empty, 
            locations, 
            "Select location");

        if (result.Cancelled || result.Data is null) return;

        equipment!.Location = (string)result.Data;
    }

    public async Task SelectModelNumber(Equipment equipment)
    {
        var modelNumbers = (await _equipmentFieldService.GetUniqueModelNumbers())
            .OrderBy(x => x);
        var result = await _selectStringUiService.ShowModal(
            equipment?.ModelNumber ?? string.Empty, 
            modelNumbers, 
            "Select model number");

        if (result.Cancelled || result.Data is null) return;

        equipment!.ModelNumber = (string)result.Data;
    }
}