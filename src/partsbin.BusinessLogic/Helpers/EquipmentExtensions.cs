using System.Text;
using partsbin.BusinessLogic.Models;

namespace partsbin.BusinessLogic.Helpers;

public static class EquipmentExtensions
{
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