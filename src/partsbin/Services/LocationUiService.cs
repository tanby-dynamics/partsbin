using Blazored.Modal;
using Blazored.Modal.Services;
using partsbin.BusinessLogic.Services;

namespace partsbin.Services;

public interface ILocationUiService
{
    Task<string> SelectLocation(string location);
}

public class LocationUiService : ILocationUiService
{
    private readonly IModalService _modalService;
    private readonly IPartFieldService _partFieldService;
    private readonly ISelectStringUiService _selectStringUiService;
    
    public LocationUiService(
        IModalService modalService, 
        IPartFieldService partFieldService,
        ISelectStringUiService selectStringUiService)
    {
        _modalService = modalService;
        _partFieldService = partFieldService;
        _selectStringUiService = selectStringUiService;
    }

    public async Task<string> SelectLocation(string location)
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
}