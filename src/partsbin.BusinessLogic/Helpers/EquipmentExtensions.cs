using System.Text;
using partsbin.BusinessLogic.Models;

namespace partsbin.BusinessLogic.Helpers;

public static class EquipmentExtensions
{
    /// <summary>
    /// Clones every property of this piece of equipment, except for the ID
    /// </summary>
    /// <returns>A deep clone of this piece of equipment</returns>
    public static Equipment DeepClone(this Equipment source)
    {
        return new Equipment
        {
            EquipmentType = source.EquipmentType,
            EquipmentName = source.EquipmentName,
            Location = source.Location,
            Manufacturer = source.Manufacturer,
            HtmlNotes = source.HtmlNotes,
            Documents = source.Documents.ToList(),  // this might be a problem
            Suppliers = source.Suppliers.ToList(),
            Notes = source.Notes,
            Quantity = source.Quantity,
            QuantityIsApproximate = source.QuantityIsApproximate
        };
    }
    
    public static string GetDescription(this Equipment equipment)
    {
        var sb = new StringBuilder();
            
        if (equipment.EquipmentType.HasContent()) sb.Append(equipment.EquipmentType + " - ");
        if (equipment.EquipmentName.HasContent()) sb.Append(equipment.EquipmentName + " - ");
        if (equipment.ModelNumber.HasContent()) sb.Append(equipment.ModelNumber + " - ");

        if (sb.Length == 0) return "---";
            
        sb.Remove(sb.Length - 3, 3);
            
        return sb.ToString();
    }

    public static string GetFormattedQuantity(this Equipment equipment)
    {
        return $"{equipment.Quantity} {(equipment.Quantity != 1 ? "items" : "item")}";
    }
}