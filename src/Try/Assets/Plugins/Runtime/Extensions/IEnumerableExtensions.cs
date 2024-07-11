using AmarilisLib.Monad;
using System;
using System.Collections.Generic;
using System.Linq;

public static partial class IEnumerableExtensions
{
    /// <summary>
    /// Tries to get the element at the specified index from the sequence.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The sequence from which to retrieve the element.</param>
    /// <param name="index">The zero-based index of the element to retrieve.</param>
    /// <returns>A try monad containing the element at the specified index, or an exception if the index is out of range.</returns>
    public static ITryMonad<T> TryElementAt<T>(this IEnumerable<T> source, int index)
    {
        try
        {
            return Try.Return(source.ElementAt(index));
        }
        catch(Exception exception)
        {
            return Try.Throw<T>(exception);
        }
    }
    /// <summary>
    /// Tries to get the first element of the sequence.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The sequence from which to retrieve the first element.</param>
    /// <returns>A try monad containing the first element, or an exception if the sequence is empty.</returns>
    public static ITryMonad<T> TryFirst<T>(this IEnumerable<T> source)
    {
        try
        {
            return Try.Return(source.First());
        }
        catch(Exception exception)
        {
            return Try.Throw<T>(exception);
        }
    }
    /// <summary>
    /// Tries to get the first element of the sequence that satisfies a specified condition.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The sequence from which to retrieve the first element.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A try monad containing the first element that satisfies the condition, or an exception if no such element is found.</returns>
    public static ITryMonad<T> TryFirst<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        try
        {
            return Try.Return(source.First(predicate));
        }
        catch(Exception exception)
        {
            return Try.Throw<T>(exception);
        }
    }
    /// <summary>
    /// Tries to get the last element of the sequence.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The sequence from which to retrieve the last element.</param>
    /// <returns>A try monad containing the last element, or an exception if the sequence is empty.</returns>
    public static ITryMonad<T> TryLast<T>(this IEnumerable<T> source)
    {
        try
        {
            return Try.Return(source.Last());
        }
        catch(Exception exception)
        {
            return Try.Throw<T>(exception);
        }
    }
    /// <summary>
    /// Tries to get the last element of the sequence that satisfies a specified condition.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The sequence from which to retrieve the last element.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A try monad containing the last element that satisfies the condition, or an exception if no such element is found.</returns>
    public static ITryMonad<T> TryLast<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        try
        {
            return Try.Return(source.Last(predicate));
        }
        catch(Exception exception)
        {
            return Try.Throw<T>(exception);
        }
    }
    /// <summary>
    /// Tries to get the only element of the sequence, and throws an exception if there is not exactly one element in the sequence.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The sequence from which to retrieve the single element.</param>
    /// <returns>A try monad containing the single element, or an exception if the sequence does not contain exactly one element.</returns>
    public static ITryMonad<T> TrySingle<T>(this IEnumerable<T> source)
    {
        try
        {
            return Try.Return(source.Single());
        }
        catch(Exception exception)
        {
            return Try.Throw<T>(exception);
        }
    }
    /// <summary>
    /// Tries to get the only element of the sequence that satisfies a specified condition, and throws an exception if more than one such element is found.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the sequence.</typeparam>
    /// <param name="source">The sequence from which to retrieve the single element.</param>
    /// <param name="predicate">A function to test each element for a condition.</param>
    /// <returns>A try monad containing the single element that satisfies the condition, or an exception if more than one such element is found.</returns>
    public static ITryMonad<T> TrySingle<T>(this IEnumerable<T> source, Func<T, bool> predicate)
    {
        try
        {
            return Try.Return(source.Single(predicate));
        }
        catch(Exception exception)
        {
            return Try.Throw<T>(exception);
        }
    }
}
