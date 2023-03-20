using Blazored.Modal;
using Blazored.Modal.Services;
using partsbin.Shared;

namespace partsbin.Services;

public enum ConfirmResult
{
    Confirmed,
    Cancelled
}

public interface IConfirmUiService
{
    Task<ConfirmResult> Confirm(string title, string caption = "Are you sure?");
}

public class ConfirmUiService : IConfirmUiService
{
    private readonly IModalService _modalService;

    public ConfirmUiService(IModalService modalService)
    {
        _modalService = modalService;
    }

    public async Task<ConfirmResult> Confirm(string title, string caption = "Are you sure")
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

        return result.Confirmed ? ConfirmResult.Confirmed : ConfirmResult.Cancelled;
    }

}