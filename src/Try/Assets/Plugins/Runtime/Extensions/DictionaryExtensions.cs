using AmarilisLib.Monad;
using System;
using System.Collections.Generic;

public static partial class DictionaryExtensions
{
    /// <summary>
    /// Tries to get the value associated with the specified key from the dictionary.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="source">The dictionary from which to retrieve the value.</param>
    /// <param name="key">The key whose value to get.</param>
    /// <returns>A try monad containing the value associated with the specified key, or an exception if the key is not found.</returns>
    public static ITryMonad<TValue> TryGetValue<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key)
    {
        try
        {
            return Try.Return(source[key]);
        }
        catch(Exception exception)
        {
            return Try.Throw<TValue>(exception);
        }
    }
}
