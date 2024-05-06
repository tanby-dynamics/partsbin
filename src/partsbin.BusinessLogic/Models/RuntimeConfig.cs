namespace partsbin.BusinessLogic.Models;

public class RuntimeConfig
{
    public required string CultureName { get; set; } = "en-US";
    public required bool HomeShowPartTypes { get; set; }
    public required bool HomeShowManufacturers { get; set; }
    public required bool HomeShowLocations { get; set; }
    public required bool HomeShowEquipmentTypes { get; set; }
    public required bool HomeShowEquipmentNames { get; set; }

    public bool NoHomeSectionsShown => !HomeShowPartTypes
                                       && !HomeShowManufacturers
                                       && !HomeShowLocations
                                       && !HomeShowEquipmentTypes
                                       && !HomeShowEquipmentNames;
}