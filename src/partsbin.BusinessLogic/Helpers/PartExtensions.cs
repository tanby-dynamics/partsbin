using System.Text;
using partsbin.BusinessLogic.Models;

namespace partsbin.BusinessLogic.Helpers;

public static class PartExtensions
{
    public static string GetDescription(this Part part)
    {
            var sb = new StringBuilder();
                
            if (part.PartType.HasContent()) sb.Append(part.PartType + " - ");
            if (part.Range.HasContent()) sb.Append(part.Range + " - ");
            if (part.PartName.HasContent()) sb.Append(part.PartName + " - ");
            if (part.Value is not null) sb.Append(part.GetFormattedValue() + " - ");
            if (part.PartNumber.HasContent()) sb.Append(part.PartNumber + " - ");

            if (sb.Length == 0) return "---";
                
            sb.Remove(sb.Length - 3, 3);
                
            return sb.ToString();
        }

    public static string GetFormattedQuantity(this Part part)
    {
        return $"{part.Quantity} {(part.Quantity != 1 ? "units" : "unit")}";
    }

    public static string GetFormattedValue(this Part part)
    {
        return part.Value is null
            ? "---"
            : $"{part.Value.FormatCompact(0)}{part.ValueUnit}";
    }
}