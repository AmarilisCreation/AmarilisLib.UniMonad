using AmarilisLib.Monad;
using System.Collections.Generic;
using System.Linq;

public static partial class IListExtensions
{
    /// <summary>
    /// Attempts to retrieve the element at the specified index from the list and returns an Option monad containing the result.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the list.</typeparam>
    /// <param name="source">The list to retrieve the element from.</param>
    /// <param name="index">The zero-based index of the element to retrieve.</param>
    /// <returns>An Option monad containing the element if the index is valid, otherwise None.</returns>
    public static IOptionMonad<T> OptionalElementAt<T>(this IList<T> source, int index)
    {
        var length = source.Count;
        if(index >= 0 && index < length)
            return Option.Return(source[index]);
        return Option.None<T>();
    }
}
