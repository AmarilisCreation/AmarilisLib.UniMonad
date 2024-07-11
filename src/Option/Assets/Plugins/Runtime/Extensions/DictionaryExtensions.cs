using AmarilisLib.Monad;
using System.Collections.Generic;

public static partial class DictionaryExtensions
{
    /// <summary>
    /// Attempts to retrieve the value associated with the specified key from the dictionary and returns an Option monad containing the result.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
    /// <param name="source">The dictionary to retrieve the value from.</param>
    /// <param name="key">The key whose value to retrieve.</param>
    /// <returns>An Option monad containing the value associated with the specified key if found, otherwise None.</returns>
    public static IOptionMonad<TValue> OptionalGetValue<TKey, TValue>(this Dictionary<TKey, TValue> source, TKey key)
    {
        if(source.ContainsKey(key))
            return Option.Return(source[key]);
        return Option.None<TValue>();
    }
}
