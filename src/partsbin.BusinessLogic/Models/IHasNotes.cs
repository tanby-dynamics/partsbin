namespace partsbin.BusinessLogic.Models;

public interface IHasNotes
{
    string Notes { get; set; }
    string HtmlNotes { get; set; }
}