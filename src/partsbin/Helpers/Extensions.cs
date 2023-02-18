using System.Globalization;

namespace partsbin.Helpers;

public static class Extensions
{
    public static string FormatCompact(this decimal? d, int minDecimalPlaces = 2)
    {
        if (d is null) return string.Empty;
        if (d == 0) return $"0.{new string('0', minDecimalPlaces)}";

        var s = d.ToString();

        if (s is null) return string.Empty;
        
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
}