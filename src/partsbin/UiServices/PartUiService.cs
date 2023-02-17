using Blazored.Modal;
using Blazored.Modal.Services;
using Blazored.Toast.Services;
using partsbin.Models;
using partsbin.Pages;

namespace partsbin.UiServices;

public interface IPartUiService
{
    Task AddPart();
}

public class PartUiService : IPartUiService
{
    private readonly IModalService _modalService;
    private readonly IToastService _toastService;

    public PartUiService(IModalService modalService, IToastService toastService)
    {
        _modalService = modalService;
        _toastService = toastService;
    }
    
    public async Task AddPart()
    {
        var parameters = new ModalParameters();
        var modal = _modalService.Show<AddEditPartModal>("Add part", parameters);
        var result = await modal.Result;

        if (result.Cancelled || result.Data is null)
        {
            return;
        }

        var part = result.Data as Part;

        _toastService.ShowSuccess($"Added part '{part?.Description ?? "<null>"}'");
    }
}