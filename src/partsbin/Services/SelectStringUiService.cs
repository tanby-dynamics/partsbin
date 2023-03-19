using Blazored.Modal;
using Blazored.Modal.Services;
using partsbin.Shared;

namespace partsbin.Services;

public interface ISelectStringUiService
{
    Task<ModalResult> ShowModal(
        string selected,
        IEnumerable<string> selections,
        string title = "Select item");

    Task<ModalResult> ShowTwoColumnModal(
        string selected,
        IEnumerable<TwoColumnEntry> selections,
        string title = "Select item",
        KeyColumn keyColumn = KeyColumn.Col2);
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

    public async Task<ModalResult> ShowTwoColumnModal(
        string selected,
        IEnumerable<TwoColumnEntry> selections,
        string title = "Select item",
        KeyColumn keyColumn = KeyColumn.Col2)
    {
        var parameters = new ModalParameters
        {
            { "SelectedString", selected },
            { "Selections", selections },
            { "KeyColumn", keyColumn }
        };
        var options = new ModalOptions { Position = ModalPosition.Middle };
        var modal = _modalService.Show<SelectTwoColumnModal>(title, parameters, options);

        return await modal.Result;
    }

}
public struct TwoColumnEntry
{
    public string Col1 { get; init; }
    public string Col2 { get; init; }
}

public enum KeyColumn
{
    Col1,
    Col2
}