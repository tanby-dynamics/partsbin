using Microsoft.JSInterop;

namespace partsbin.Services;

public interface INavUiService
{
    Task HideNavbar();
}

public class NavUiService : INavUiService
{
    private readonly IJSRuntime _jsRuntime;

    public NavUiService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    [JSInvokable]
    public async Task HideNavbar()
    {
        await _jsRuntime.InvokeVoidAsync("hideNavbar");
    }
}