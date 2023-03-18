using Blazored.Modal;
using Blazored.Modal.Services;
using partsbin.Models;
using partsbin.Services;
using partsbin.Shared;
using HostingEnvironmentExtensions = Microsoft.AspNetCore.Hosting.HostingEnvironmentExtensions;

namespace partsbin.UiServices;

public interface ISupplierUiService
{
    Task AddSupplier(Part part);
    Task EditSupplier(Part part, Supplier supplier);
    Task SelectName(Part part, Supplier supplier);
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
        var parameters = new ModalParameters
        {
            { "Part", part }
        };
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
        var parameters = new ModalParameters
        {
            { "Part", part },
            { "Supplier", supplier }
        };
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

    public async Task SelectName(Part part, Supplier supplier)
    {
        var supplierNamesAndLinks = _supplierService
            .GetNamesAndUrls()
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