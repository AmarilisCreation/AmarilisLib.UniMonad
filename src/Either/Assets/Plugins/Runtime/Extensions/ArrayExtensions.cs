using AmarilisLib.Monad;
using System;
using System.Linq;

public static partial class ArrayExtensions
{
    /// <summary>
    /// Returns the element at the specified index in the array. If the index is out of range, returns the specified Left value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <typeparam name="TRight">The type of the elements in the array.</typeparam>
    /// <param name="source">The array to retrieve the element from.</param>
    /// <param name="index">The zero-based index of the element to retrieve.</param>
    /// <param name="leftValue">The value to return if the index is out of range.</param>
    /// <returns>An Either monad containing the element at the specified index or the Left value.</returns>
    public static IEitherMonad<TLeft, TRight> ElementAtOrLeft<TLeft, TRight>(this TRight[] source, int index, TLeft leftValue)
    {
        var length = source.Length;
        if(index >= 0 && index < length)
            return Either.ReturnRight<TLeft, TRight>(source[index]);
        else
            return Either.ReturnLeft<TLeft, TRight>(leftValue);
    }
    /// <summary>
    /// Returns the element at the specified index in the array. If the index is out of range, returns the result of the specified Left value selector function.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <typeparam name="TRight">The type of the elements in the array.</typeparam>
    /// <param name="source">The array to retrieve the element from.</param>
    /// <param name="index">The zero-based index of the element to retrieve.</param>
    /// <param name="leftSelector">A function that generates the Left value if the index is out of range.</param>
    /// <returns>An Either monad containing the element at the specified index or the Left value.</returns>
    public static IEitherMonad<TLeft, TRight> ElementAtOrLeft<TLeft, TRight>(this TRight[] source, int index, Func<int, TLeft> leftSelector)
    {
        var length = source.Length;
        if(index >= 0 && index < length)
            return Either.ReturnRight<TLeft, TRight>(source[index]);
        else
            return Either.ReturnLeft<TLeft, TRight>(leftSelector(index));
    }
}
