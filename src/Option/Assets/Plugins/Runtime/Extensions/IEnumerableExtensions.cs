using AmarilisLib.Monad;
using System;
using System.Collections.Generic;
using System.Linq;

public static partial class IEnumerableExtensions
{
    /// <summary>
    /// Attempts to retrieve the element at the specified index from the enumerable and returns an Option monad containing the result.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
    /// <param name="source">The enumerable to retrieve the element from.</param>
    /// <param name="index">The zero-based index of the element to retrieve.</param>
    /// <returns>An Option monad containing the element if the index is valid, otherwise None.</returns>
    public static IOptionMonad<T> OptionalElementAt<T>(this IEnumerable<T> source, int index)
    {
        var length = source.Count();
        if(index >= 0 && index < length)
            return Option.Return(source.ElementAt(index));
        return Option.None<T>();
    }
    /// <summary>
    /// Attempts to retrieve the first element from the enumerable and returns an Option monad containing the result.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
    /// <param name="source">The enumerable to retrieve the first element from.</param>
    /// <returns>An Option monad containing the first element if the enumerable is not empty, otherwise None.</returns>
    public static IOptionMonad<T> OptionalFirst<T>(this IEnumerable<T> source)
    {
        if(source.Count() != 0)
            return Option.Return(source.First());
        return Option.None<T>();
    }
    /// <summary>
    /// Attempts to retrieve the first element that matches the predicate from the enumerable and returns an Option monad containing the result.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
    /// <param name="source">The enumerable to retrieve the first element from.</param>
    /// <param name="predicate">The predicate to match elements against.</param>
    /// <returns>An Option monad containing the first element that matches the predicate if any, otherwise None.</returns>
    public static IOptionMonad<T> OptionalFirst<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        if(source.Count(predicate) != 0)
            return Option.Return(source.First(predicate));
        return Option.None<T>();
    }
    /// <summary>
    /// Attempts to retrieve the last element from the enumerable and returns an Option monad containing the result.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
    /// <param name="source">The enumerable to retrieve the last element from.</param>
    /// <returns>An Option monad containing the last element if the enumerable is not empty, otherwise None.</returns>
    public static IOptionMonad<T> OptionalLast<T>(this IEnumerable<T> source)
    {
        if(source.Count() != 0)
            return Option.Return(source.Last());
        return Option.None<T>();
    }
    /// <summary>
    /// Attempts to retrieve the last element that matches the predicate from the enumerable and returns an Option monad containing the result.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
    /// <param name="source">The enumerable to retrieve the last element from.</param>
    /// <param name="predicate">The predicate to match elements against.</param>
    /// <returns>An Option monad containing the last element that matches the predicate if any, otherwise None.</returns>
    public static IOptionMonad<T> OptionalLast<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        if(source.Count(predicate) != 0)
            return Option.Return(source.Last(predicate));
        return Option.None<T>();
    }
    /// <summary>
    /// Attempts to retrieve the single element from the enumerable and returns an Option monad containing the result.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
    /// <param name="source">The enumerable to retrieve the single element from.</param>
    /// <returns>An Option monad containing the single element if found, otherwise None.</returns>

    public static IOptionMonad<T> OptionalSingle<T>(this IEnumerable<T> source)
    {
        try
        {
            return Option.Return(source.Single());
        }
        catch
        {
            return Option.None<T>();
        }
    }
    /// <summary>
    /// Attempts to retrieve the single element that matches the predicate from the enumerable and returns an Option monad containing the result.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the enumerable.</typeparam>
    /// <param name="source">The enumerable to retrieve the single element from.</param>
    /// <param name="predicate">The predicate to match the element against.</param>
    /// <returns>An Option monad containing the single element that matches the predicate if found, otherwise None.</returns>
    public static IOptionMonad<T> OptionalSingle<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        try
        {
            return Option.Return(source.Single(predicate));
        }
        catch
        {
            return Option.None<T>();
        }
    }
}
