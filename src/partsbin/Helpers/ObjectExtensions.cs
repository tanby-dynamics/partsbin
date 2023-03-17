using System;
using System.Reflection;

namespace partsbin.Helpers;

public static class ObjectExtensions
{
    public static void Dump(this object? obj)
    {
        if (obj is null) 
        {
            Console.WriteLine("null");
            return;
        }

        var objType = obj.GetType();
        var properties = objType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        Console.WriteLine("{0}:", objType.Name);
        foreach (var property in properties)
        {
            Console.WriteLine("  {0}: {1}", property.Name, property.GetValue(obj));
        }
    }
}