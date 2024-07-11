using AmarilisLib.Monad;
using System;
using System.Collections.Generic;

public static partial class DictionaryExtensions
{
    /// <summary>
    /// Returns the value associated with the specified key in the dictionary. If the key is not found, returns the specified Left value.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <typeparam name="TRight">The type of the values in the dictionary.</typeparam>
    /// <param name="source">The dictionary to retrieve the value from.</param>
    /// <param name="key">The key of the value to retrieve.</param>
    /// <param name="leftValue">The value to return if the key is not found.</param>
    /// <returns>An Either monad containing the value associated with the specified key or the Left value.</returns>
    public static IEitherMonad<TLeft, TRight> GetValueOrLeft<TKey, TLeft, TRight>(this Dictionary<TKey, TRight> source, TKey key, TLeft leftValue)
    {
        if(source.ContainsKey(key))
            return Either.ReturnRight<TLeft, TRight>(source[key]);
        else
            return Either.ReturnLeft<TLeft, TRight>(leftValue);
    }
    /// <summary>
    /// Returns the value associated with the specified key in the dictionary. If the key is not found, returns the result of the specified Left value selector function.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <typeparam name="TRight">The type of the values in the dictionary.</typeparam>
    /// <param name="source">The dictionary to retrieve the value from.</param>
    /// <param name="key">The key of the value to retrieve.</param>
    /// <param name="leftSelector">A function that generates the Left value if the key is not found.</param>
    /// <returns>An Either monad containing the value associated with the specified key or the Left value.</returns>
    public static IEitherMonad<TLeft, TRight> GetValueOrLeft<TKey, TLeft, TRight>(this Dictionary<TKey, TRight> source, TKey key, Func<TKey, TLeft> leftSelector)
    {
        if(source.ContainsKey(key))
            return Either.ReturnRight<TLeft, TRight>(source[key]);
        else
            return Either.ReturnLeft<TLeft, TRight>(leftSelector(key));
    }
}
