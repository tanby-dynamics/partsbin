using System.Globalization;
using Blazored.Modal.Services;
using partsbin.BusinessLogic.Models;

namespace partsbin.Services;

public interface IRuntimeConfigUiService
{
    Task<string> SelectCulture(RuntimeConfig config);
}

public class RuntimeConfigUiService : IRuntimeConfigUiService
{
    private readonly IModalService _modalService;
    private readonly ISelectStringUiService _selectStringUiService;
    private readonly IList<(string displayName, string name)>? _cultures;

    public RuntimeConfigUiService(IModalService modalService,
        ISelectStringUiService selectStringUiService)
    {
        _modalService = modalService;
        _selectStringUiService = selectStringUiService;
        _cultures = CultureInfo.GetCultures(CultureTypes.AllCultures)
            .OrderBy(x => x.DisplayName)
            .Select(x => (displayName: x.DisplayName, nameof: x.Name))
            .ToList();
    }

    public async Task<string> SelectCulture(RuntimeConfig config)
    {
        var result = await _selectStringUiService.ShowTwoColumnModal(
            config.CultureName,
            _cultures!.Select(x => new TwoColumnEntry
            {
                Col1 = x.displayName,
                Col2 = x.name
            }),
            title: "Select culture",
            keyColumn: KeyColumn.Col2);

        return result.Cancelled ? config.CultureName : (string)result.Data!;
    }
}