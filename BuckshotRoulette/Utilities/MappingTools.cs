using System;
using System.Collections.Generic;
using System.Linq;

namespace BuckshotRoulette.Simplified.Utilities;

/// <summary>
/// Provides utility methods for transforming lists using functional mapping.
/// </summary>
public static class MappingTools
{
    /// <summary>
    /// Transforms a source list into a new list of a different type using a mapping function.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of source.</typeparam>
    /// <typeparam name="TResult">The type of the value returned by selector.</typeparam>
    public static List<TResult> MapList<TSource, TResult>(
        IEnumerable<TSource> sourceList,
        Func<TSource, TResult> mappingFunction)
    {
        // LINQ .Select() is the equivalent of Java's .stream().map()
        return sourceList.Select(mappingFunction).ToList();
    }
}