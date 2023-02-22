using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace partsbin.Helpers;

/// <summary>
/// https://www.puresourcecode.com/dotnet/blazor/create-a-blazor-component-for-quill/
/// </summary>
public static class QuillHelper
{
    private const string CreateQuillFnName = "QuillFunctions.createQuill";
    private const string GetTextFnName = "QuillFunctions.getQuillText";
    private const string GetHtmlFnName = "QuillFunctions.getQuillHTML";
    private const string GetContentFnName = "QuillFunctions.getQuillContent";
    private const string LoadQuillContentFnName = "QuillFunctions.loadQuillContent";
    private const string EnableQuillEditorFnName = "QuillFunctions.enableQuillEditor";

    public static ValueTask<object> CreateQuill(
        IJSRuntime jsRuntime,
        ElementReference quillElement,
        ElementReference toolbar,
        bool readOnly,
        string placeholder,
        string theme,
        string debugLevel)
    {
        return jsRuntime.InvokeAsync<object>(
            CreateQuillFnName,
            quillElement,
            toolbar,
            readOnly,
            placeholder,
            theme,
            debugLevel);
    }

    public static ValueTask<string> GetText(
        IJSRuntime jsRuntime,
        ElementReference quillElement)
    {
        return jsRuntime.InvokeAsync<string>(
            GetTextFnName,
            quillElement);
    }

    public static ValueTask<string> GetHtml(
        IJSRuntime jsRuntime,
        ElementReference quillElement)
    {
        return jsRuntime.InvokeAsync<string>(
            GetHtmlFnName,
            quillElement);
    }

    public static ValueTask<string> GetContent(
        IJSRuntime jsRuntime,
        ElementReference quillElement)
    {
        return jsRuntime.InvokeAsync<string>(
            GetContentFnName,
            quillElement);
    }

    public static ValueTask<object> LoadQuillContent(
        IJSRuntime jsRuntime,
        ElementReference quillElement,
        string content)
    {
        return jsRuntime.InvokeAsync<object>(
            LoadQuillContentFnName,
            quillElement, content);
    }

    public static ValueTask<object> EnableQuillEditor(
        IJSRuntime jsRuntime,
        ElementReference quillElement,
        bool mode)
    {
        return jsRuntime.InvokeAsync<object>(
            EnableQuillEditorFnName,
            quillElement, mode);
    }
}