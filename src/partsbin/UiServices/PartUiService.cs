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
        var parameters = new ModalParameters
        {
            { "SelectedString", part.PartType },
            { "Selections", partTypes },
        };
        var options = new ModalOptions { Position = ModalPosition.Middle };
        var modal = _modalService.Show<SelectStringModal>("Select part type", parameters, options);
        var result = await modal.Result;

        if (result.Cancelled || result.Data is null) return;

        part.PartType = result.Data as string;
    }
}