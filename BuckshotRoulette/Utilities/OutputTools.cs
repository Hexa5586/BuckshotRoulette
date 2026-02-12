using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace BuckshotRoulette.Simplified.Utilities;

public static class OutputTools
{

    private static readonly TextWriter DefaultWriter = new DebugWriter();

    /// <summary>
    /// Outputs a list to a specified writer, defaults to Debug window.
    /// </summary>
    public static void OutputListThroughMapping<T>(
        List<T> sourceList,
        Func<T, string> mappingFunction,
        string separator = ", ",
        string end = "\n",
        bool containIndex = false,
        bool containBrackets = true,
        TextWriter? writer = null)
    {
        writer ??= DefaultWriter;

        string prefix = containBrackets ? "[" : "";
        string postfix = containBrackets ? "]" : "";

        string result;

        if (containIndex)
        {
            result = string.Join(separator, sourceList.Select((item, i) => $"[{i}]{mappingFunction(item)}"));
        }
        else
        {
            result = string.Join(separator, sourceList.Select(mappingFunction));
        }

        writer.Write($"{prefix}{separator}{result}{separator}{postfix}{end}");
    }

    /// <summary>
    /// Simplified version of OutputListThroughMapping with default parameters for common use cases.
    /// </summary>
    public static void OutputListThroughMapping<T>(List<T> sourceList, Func<T, string> mappingFunction)
    {
        OutputListThroughMapping(sourceList, mappingFunction, ", ", "\n", false, true, writer: null);
    }
}