namespace partsbin.BusinessLogic.Models;

/// <summary>
/// Equipment is very similar to Part, however it is for equipment
/// such as multimeters, screwdriver sets, power supplies, cables, etc.
/// </summary>
public class Equipment
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }
    public string EquipmentType { get; set; } = string.Empty;
    public string EquipmentName { get; set; } = string.Empty;
    public string Manufacturer { get; set; } = string.Empty;
    public string ModelNumber { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public bool QuantityIsApproximate { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public string HtmlNotes { get; set; } = string.Empty;
    public List<PartDocument> Documents { get; set; } = new List<PartDocument>();
    public List<Supplier> Suppliers { get; set; } = new List<Supplier>();
}