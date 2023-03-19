using Microsoft.JSInterop;

namespace partsbin.Services;

public interface INavService
{
    Task HideNavbar();
}

public class NavService : INavService
{
    private readonly IJSRuntime _jsRuntime;

    public NavService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    [JSInvokable]
    public async Task HideNavbar()
    {
        await _jsRuntime.InvokeVoidAsync("hideNavbar");
    }
}