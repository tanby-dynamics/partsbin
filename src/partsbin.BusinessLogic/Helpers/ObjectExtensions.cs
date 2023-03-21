using System.Collections;
using System.Reflection;

namespace partsbin.BusinessLogic.Helpers;

public static class ObjectExtensions
{
    public static void Dump(this object? obj)
    {
        switch (obj)
        {
            case null:
                Console.WriteLine("null");
                return;
            case string or int or double or decimal:
                Console.WriteLine(obj);
                return;
        }

        Console.Write("{0}:", obj.GetType().Name);

        if (obj is IEnumerable)
        {
            Console.WriteLine(" - IEnumerable:");
            foreach (var item in (IEnumerable)obj)
            {
                if (item is null) Console.WriteLine("  null");
                else DumpProperties(item);
            }

            return;
        }
        
        Console.WriteLine();
        DumpProperties(obj);
    }

    private static void DumpProperties(object item)
    {
        var properties = item
            .GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance);
        
        foreach (var property in properties)
        {
            Console.WriteLine($"  {property.Name}: {property.GetValue(item)}");
        }
    }
}