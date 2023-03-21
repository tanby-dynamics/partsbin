using System.Globalization;

namespace partsbin.BusinessLogic.Helpers;

public static class DecimalExtensions
{
    public static string FormatCompact(this decimal? d, int minDecimalPlaces = 2)
    {
        return d is null 
            ? string.Empty 
            : d.Value.FormatCompact(minDecimalPlaces);
    }

    public static string FormatCompact(this decimal d, int minDecimalPlaces = 2)
    {
        if (d == 0) return $"0.{new string('0', minDecimalPlaces)}";

        var s = d.ToString(CultureInfo.InvariantCulture);
        var indexOfPeriod = s.IndexOf('.');

        if (indexOfPeriod == -1)
        {
            if (minDecimalPlaces > 0)
            {
                s += $".{new string('0', minDecimalPlaces)}";
            }
            
            return s;
        }

        s = s.TrimEnd('0');

        if (minDecimalPlaces <= 0)
        {
            return s.TrimEnd('.');
        }

        var numToPad = minDecimalPlaces - s.Length - (indexOfPeriod + 1);
        
        if (numToPad > 0)
        {
            s += new string('0', numToPad);
        }
        
        return s;
    }

    public static string FormatCurrency(this decimal d, CultureInfo cultureInfo)
    {
        var currency = d.ToString("C", cultureInfo);
        var fullPrecision = d.ToString("G1000");

        return string.Format($"{currency} ({fullPrecision})");
    }
    
}