using Blazored.Modal;
using Blazored.Modal.Services;
using partsbin.Shared;

namespace partsbin.UiServices;

public interface ISelectStringUiService
{
    Task<ModalResult> ShowModal(
        string selected,
        IEnumerable<string> selections,
        string title = "Select item");
}

public class SelectStringUiService : ISelectStringUiService
{
    private readonly IModalService _modalService;

    public SelectStringUiService(IModalService modalService)
    {
        _modalService = modalService;
    }

    public async Task<ModalResult> ShowModal(
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