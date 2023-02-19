using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast.Services;
using partsbin.Models;
using partsbin.Pages;
using partsbin.Services;

namespace partsbin.UiServices;

public interface IPartUiService
{
    Task<Part?> AddPart();
    Task EditPart(Part part);
    Task EditQuantity(Part part);
    Task SelectPartType(Part part);
    Task SelectRange(Part part);
}

public class PartUiService : IPartUiService
{
    private readonly IModalService _modalService;
    private readonly IPartTypeService _partTypeService;

    public PartUiService(IModalService modalService, IPartTypeService partTypeService)
    {
        _modalService = modalService;
        _partTypeService = partTypeService;
    }
    
    public async Task<Part?> AddPart()
    {
        var parameters = new ModalParameters();
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
        if (part.PartType == null) return;

        var partTypes = _partTypeService.GetUniquePartTypes()
            .OrderBy(x => x);
        var result = await ShowSelectStringModal(
            part.PartType,
            partTypes,
            "Select part type");

        if (result.Cancelled || result.Data is null) return;

        part.PartType = result.Data as string;
    }

    public async Task SelectRange(Part part)
    {
        if (part.Range == null) return;

        var ranges = _partTypeService.GetUniqueRanges()
            .OrderBy(x => x);
        var result = await ShowSelectStringModal(
            part.Range, 
            ranges, 
            "Select range");

        if (result.Cancelled || result.Data is null) return;

        part.Range = result.Data as string;
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