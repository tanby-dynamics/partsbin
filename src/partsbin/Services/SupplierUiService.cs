using Blazored.Modal;
using Blazored.Modal.Services;
using partsbin.BusinessLogic.Models;
using partsbin.BusinessLogic.Services;
using partsbin.Shared.Supplier;

namespace partsbin.Services;

public interface ISupplierUiService
{
    Task AddSupplier(Part part);
    Task AddSupplier(Equipment equipment);
    Task EditSupplier(Part part, Supplier supplier);
    Task EditSupplier(Equipment equipment, Supplier supplier);
    Task SelectName(Supplier supplier);
}

public class SupplierUiService : ISupplierUiService
{
    private readonly IModalService _modalService;
    private readonly ISupplierService _supplierService;
    private readonly ISelectStringUiService _selectStringUiService;

    public SupplierUiService(
        IModalService modalService,
        ISupplierService supplierService, 
        ISelectStringUiService selectStringUiService)
    {
        _modalService = modalService;
        _supplierService = supplierService;
        _selectStringUiService = selectStringUiService;
    }
    
    public async Task AddSupplier(Part part)
    {
        await AddSupplier(part, null);
    }

    public async Task AddSupplier(Equipment equipment)
    {
        await AddSupplier(null, equipment);
    }

    private async Task AddSupplier(Part? part, Equipment? equipment)
    {
        var parameters = new ModalParameters();

        if (part is not null) parameters.Add("Part", part);
        if (equipment is not null) parameters.Add("Equipment", equipment);

        var modalOptions = new ModalOptions
        {
            DisableBackgroundCancel = true
        };
        var modal = _modalService.Show<AddEditSupplierModal>(
            "Add supplier", 
            parameters, 
            modalOptions);

        await modal.Result;
    }

    public async Task EditSupplier(Part part, Supplier supplier)
    {
        await EditSupplier(part, null, supplier);
    }

    public async Task EditSupplier(Equipment equipment, Supplier supplier)
    {
        await EditSupplier(null, equipment, supplier);
    }

    private async Task EditSupplier(Part? part, Equipment? equipment, Supplier supplier)
    {
        var parameters = new ModalParameters();

        if (part is not null) parameters.Add("Part", part);
        if (equipment is not null) parameters.Add("Equipment", equipment);
        parameters.Add("Supplier", supplier);

        var modalOptions = new ModalOptions()
        {
            DisableBackgroundCancel = true
        };
        var modal = _modalService.Show<AddEditSupplierModal>(
            "Edit supplier",
            parameters,
            modalOptions);

        await modal.Result;
    }

    public async Task SelectName(Supplier supplier)
    {
        var supplierNamesAndLinks = (await _supplierService.GetNamesAndUrls())
            .Select(x => $"{x.name} - {x.url}")
            .OrderBy(x => x);
        var result = await _selectStringUiService.ShowModal(
            string.Empty,
            supplierNamesAndLinks,
            "Select supplier");

        if (result.Cancelled || result.Data is null) return;

        var data = (string)result.Data;        
        
        supplier.Name = data[0..data.IndexOf(" - ", StringComparison.InvariantCulture)];
        supplier.Url = data[(supplier.Name.Length + 3)..];
    }
}