using System;
namespace partsbin.Data
{
    public class Part
    {
        public string? PartType { get; set; } = ""; // eg "Development board"
        public string? Range { get; set; } = "";    // eg "ESP8266"
        public string? PartName { get; set; }           // eg "WiFi Mini ESP8266"
        public string? PackageType { get; set; }    // eg. "DIP"
        public decimal? Value { get; set; }         // eg "0.1"
        public string? ValueUnit { get; set; }      // eg "uF"
        public string? PartNumber { get; set; }     // eg "XC3802"
        public int Quantity { get; set; }           // eg 4
        public string Location { get; set; } = "";  // eg "Shelf 4 Rack 2 Tray 6"
        public string? Manufacturer { get; set; }   // eg "Jaycar"
        public List<PartDocument> Documents { get; set; } = new List<PartDocument>();
        public List<Supplier> Suppliers { get; set; } = new List<Supplier>();

        public string Description => PartName ?? Range ?? PartType ?? "part";
    }
    public class PartDocument
    {
        public string Description { get; set; } = "";
        public string Url { get; set; } = "";
    }
    public class Supplier
    {
        public string Name { get; set; } = "";
        public string Url { get; set; } = "";
    }
}

