namespace partsbin.BusinessLogic.Models
{
    public class Supplier
    {
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string PartUrl { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; } = 0.0m;
        public string Notes { get; set; } = string.Empty;
        public string HtmlNotes { get; set; } = string.Empty;
    }
}

