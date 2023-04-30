using Blazored.Modal;
using Blazored.Modal.Services;
using partsbin.BusinessLogic.Services;

namespace partsbin.Services;

public interface ILocationUiService
{
    Task<string> SelectPartLocation(string location);
    Task<string> SelectEquipmentLocation(string location);
}

public class LocationUiService : ILocationUiService
{
    private readonly IModalService _modalService;
    private readonly IPartFieldService _partFieldService;
    private readonly IEquipmentFieldService _equipmentFieldService;
    private readonly ISelectStringUiService _selectStringUiService;
    
    public LocationUiService(
        IModalService modalService, 
        IPartFieldService partFieldService,
        IEquipmentFieldService equipmentFieldService,
        ISelectStringUiService selectStringUiService)
    {
        _modalService = modalService;
        _partFieldService = partFieldService;
        _equipmentFieldService = equipmentFieldService;
        _selectStringUiService = selectStringUiService;
    }

    public async Task<string> SelectPartLocation(string location)
    {
        var locations = (await _partFieldService.GetUniqueLocations())
            .OrderBy(x => x);
        var result = await _selectStringUiService.ShowModal(
            location ?? string.Empty,
            locations,
            "Select location");
        
        if (result.Cancelled || result.Data is null) return string.Empty;

        return (string)result.Data;
    }

    public async Task<string> SelectEquipmentLocation(string location)
    {
        var locations = (await _equipmentFieldService.GetUniqueLocations())
            .OrderBy(x => x);
        var result = await _selectStringUiService.ShowModal(
            location ?? string.Empty,
            locations,
            "Select location");
        
        if (result.Cancelled || result.Data is null) return string.Empty;

        return (string)result.Data;
    }
}