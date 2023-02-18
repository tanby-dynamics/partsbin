using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast.Services;
using partsbin.Models;
using partsbin.Pages;

namespace partsbin.UiServices;

public interface IPartUiService
{
    Task<Part?> AddPart();
    Task EditPart(Part part);
    Task EditQuantity(Part part);
}

public class PartUiService : IPartUiService
{
    private readonly IModalService _modalService;

    public PartUiService(IModalService modalService)
    {
        _modalService = modalService;
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
}