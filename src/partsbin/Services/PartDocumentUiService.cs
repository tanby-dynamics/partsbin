using Blazored.Modal;
using Blazored.Modal.Services;
using partsbin.BusinessLogic.Models;
using partsbin.Shared.Documentation;

namespace partsbin.Services;

public interface IPartDocumentUiService
{
    Task AddDocument(Part part);
    Task EditDocument(Part part, PartDocument document);
    Task AddDocument(Equipment equipment);
    Task EditDocument(Equipment equipment, PartDocument document);
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
        await AddDocument(part, null);
    }

    public async Task AddDocument(Equipment equipment)
    {
        await AddDocument(null, equipment);
    }

    private async Task AddDocument(Part? part, Equipment? equipment)
    {
        var parameters = new ModalParameters();

        if (part is not null) parameters.Add("Part", part);
        if (equipment is not null) parameters.Add("Equipment", equipment);

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
        await EditDocument(part, null, document);
    }

    public async Task EditDocument(Equipment equipment, PartDocument document)
    {
        await EditDocument(null, equipment, document);
    }

    private async Task EditDocument(Part? part, Equipment? equipment, PartDocument document)
    {
        var parameters = new ModalParameters();

        if (part is not null) parameters.Add("Part", part);
        if (equipment is not null) parameters.Add("Equipment", equipment);
        parameters.Add("Document", document);

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