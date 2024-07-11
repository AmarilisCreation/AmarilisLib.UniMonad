using AmarilisLib.Monad;
using System;

public static partial class ArrayExtensions
{
    /// <summary>
    /// Tries to get the element at the specified index from the array.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <param name="source">The array from which to retrieve the element.</param>
    /// <param name="index">The zero-based index of the element to retrieve.</param>
    /// <returns>A try monad containing the element at the specified index, or an exception if the index is out of range.</returns>
    public static ITryMonad<T> TryElementAt<T>(this T[] source, int index)
    {
        try
        {
            return Try.Return(source[index]);
        }
        catch(Exception exception)
        {
            return Try.Throw<T>(exception);
        }
    }
}
