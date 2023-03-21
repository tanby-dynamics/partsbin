using System.Text;
using partsbin.Services.Models;

namespace partsbin.Logic.Helpers;

public static class PartExtensions
{
    /// <summary>
    /// Clones every property of this part, except for the ID
    /// Note that the gallery is not duplicated as it lives outside of the Part
    /// </summary>
    /// <returns>A deep clone of this part</returns>
    public static Part DeepClone(this Part source)
    {
        return new Part
        {
            PartType = source.PartType,
            Range = source.Range,
            Location = source.Location,
            Manufacturer = source.Manufacturer,
            HtmlNotes = source.HtmlNotes,
            Documents = source.Documents.ToList(),  // this might be a problem
            Suppliers = source.Suppliers.ToList(),
            Notes = source.Notes,
            PackageType = source.PackageType,
            Value = source.Value,
            ValueUnit = source.ValueUnit,
            PartName = source.PartName,
            PartNumber = source.PartNumber,
            Quantity = source.Quantity
        };
    }
    
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