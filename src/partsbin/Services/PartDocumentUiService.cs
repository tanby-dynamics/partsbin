using Blazored.Modal;
using Blazored.Modal.Services;
using partsbin.BusinessLogic.Models;
using partsbin.Shared.Documentation;

namespace partsbin.Services;

public interface IPartDocumentUiService
{
    Task AddDocument(Part part);
    Task EditDocument(Part part, PartDocument document);
}

public class PartDocumentUiService : IPartDocumentUiService
{
    private readonly IModalService _modalService;

    public PartDocumentUiService(IModalService modalService)
    {
        _modalService = modalService;
    }
    
    public async Task AddDocument(Part part)
    {
        var parameters = new ModalParameters
        {
            { "Part", part }
        };
        var modalOptions = new ModalOptions
        {
            DisableBackgroundCancel = true
        };
        var modal = _modalService.Show<AddEditPartDocumentModal>(
            "Add documentation", 
            parameters, 
            modalOptions);

        await modal.Result;
    }

    public async Task EditDocument(Part part, PartDocument document)
    {
        var parameters = new ModalParameters
        {
            { "Part", part },
            { "Document", document }
        };
        var modalOptions = new ModalOptions()
        {
            DisableBackgroundCancel = true
        };
        var modal = _modalService.Show<AddEditPartDocumentModal>(
            "Edit document",
            parameters,
            modalOptions);

        await modal.Result;
    }
}