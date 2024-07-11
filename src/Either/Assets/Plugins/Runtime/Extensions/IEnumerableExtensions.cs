using AmarilisLib.Monad;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static partial class IEnumerableExtensions
{
    /// <summary>
    /// Returns the element at the specified index in the sequence. If the index is out of range, returns the specified Left value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <typeparam name="TRight">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The sequence to retrieve the element from.</param>
    /// <param name="index">The zero-based index of the element to retrieve.</param>
    /// <param name="leftValue">The value to return if the index is out of range.</param>
    /// <returns>An Either monad containing the element at the specified index or the Left value.</returns>
    public static IEitherMonad<TLeft, TRight> ElementAtOrLeft<TLeft, TRight>(this IEnumerable<TRight> source, int index, TLeft leftValue)
    {
        var length = source.Count();
        if(index >= 0 && index < length)
            return Either.ReturnRight<TLeft, TRight>(source.ElementAt(index));
        else
            return Either.ReturnLeft<TLeft, TRight>(leftValue);
    }
    /// <summary>
    /// Returns the element at the specified index in the sequence. If the index is out of range, returns the result of the specified Left value selector function.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <typeparam name="TRight">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The sequence to retrieve the element from.</param>
    /// <param name="index">The zero-based index of the element to retrieve.</param>
    /// <param name="leftSelector">A function that generates the Left value if the index is out of range.</param>
    /// <returns>An Either monad containing the element at the specified index or the Left value.</returns>
    public static IEitherMonad<TLeft, TRight> ElementAtOrLeft<TLeft, TRight>(this IEnumerable<TRight> source, int index, Func<int, TLeft> leftSelector)
    {
        var length = source.Count();
        if(index >= 0 && index < length)
            return Either.ReturnRight<TLeft, TRight>(source.ElementAt(index));
        else
            return Either.ReturnLeft<TLeft, TRight>(leftSelector(index));
    }
    /// <summary>
    /// Returns the first element of the sequence. If the sequence is empty, returns the specified Left value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <typeparam name="TRight">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The sequence to retrieve the first element from.</param>
    /// <param name="leftValue">The value to return if the sequence is empty.</param>
    /// <returns>An Either monad containing the first element or the Left value.</returns>
    public static IEitherMonad<TLeft, TRight> FirstOrLeft<TLeft, TRight>(this IEnumerable<TRight> source, TLeft leftValue)
    {
        if(source.Count() != 0)
            return Either.ReturnRight<TLeft, TRight>(source.First());
        else
            return Either.ReturnLeft<TLeft, TRight>(leftValue);
    }
    /// <summary>
    /// Returns the first element of the sequence that satisfies a condition. If no such element is found, returns the specified Left value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <typeparam name="TRight">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The sequence to retrieve the element from.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="leftValue">The value to return if no element satisfies the condition.</param>
    /// <returns>An Either monad containing the first element that satisfies the condition or the Left value.</returns>
    public static IEitherMonad<TLeft, TRight> FirstOrLeft<TLeft, TRight>(this IEnumerable<TRight> source, Func<TRight, bool> predicate, TLeft leftValue)
    {
        if(source.Count(predicate) != 0)
            return Either.ReturnRight<TLeft, TRight>(source.First(predicate));
        else
            return Either.ReturnLeft<TLeft, TRight>(leftValue);
    }
    /// <summary>
    /// Returns the last element of the sequence. If the sequence is empty, returns the specified Left value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <typeparam name="TRight">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The sequence to retrieve the last element from.</param>
    /// <param name="leftValue">The value to return if the sequence is empty.</param>
    /// <returns>An Either monad containing the last element or the Left value.</returns>
    public static IEitherMonad<TLeft, TRight> LastOrLeft<TLeft, TRight>(this IEnumerable<TRight> source, TLeft leftValue)
    {
        if(source.Count() != 0)
            return Either.ReturnRight<TLeft, TRight>(source.Last());
        else
            return Either.ReturnLeft<TLeft, TRight>(leftValue);
    }
    /// <summary>
    /// Returns the last element of the sequence that satisfies a condition. If no such element is found, returns the specified Left value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <typeparam name="TRight">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The sequence to retrieve the element from.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="leftValue">The value to return if no element satisfies the condition.</param>
    /// <returns>An Either monad containing the last element that satisfies the condition or the Left value.</returns>
    public static IEitherMonad<TLeft, TRight> LastOrLeft<TLeft, TRight>(this IEnumerable<TRight> source, Func<TRight, bool> predicate, TLeft leftValue)
    {
        if(source.Count(predicate) != 0)
            return Either.ReturnRight<TLeft, TRight>(source.Last(predicate));
        else
            return Either.ReturnLeft<TLeft, TRight>(leftValue);
    }
    /// <summary>
    /// Returns the only element of the sequence that satisfies a specified condition, and throws an exception if more than one such element exists. If no such element is found, returns the specified Left value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <typeparam name="TRight">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The sequence to retrieve the element from.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <param name="leftValue">The value to return if no element satisfies the condition.</param>
    /// <returns>An Either monad containing the single element that satisfies the condition or the Left value.</returns>
    public static IEitherMonad<TLeft, TRight> SingleOrLeft<TLeft, TRight>(this IEnumerable<TRight> source, Func<TRight, bool> predicate, TLeft leftValue)
    {
        try
        {
            return Either.ReturnRight<TLeft, TRight>(source.Single(predicate));
        }
        catch
        {
            return Either.ReturnLeft<TLeft, TRight>(leftValue);
        }
    }
}
