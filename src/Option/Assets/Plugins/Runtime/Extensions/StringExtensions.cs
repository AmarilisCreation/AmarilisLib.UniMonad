using AmarilisLib.Monad;
using System;

public static partial class StringExtensions
{
    /// <summary>
    /// Attempts to parse the string into an integer and returns an Option monad containing the result.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <returns>An Option monad containing the parsed integer if successful, otherwise None.</returns>
    public static IOptionMonad<int> OptionalParseInt(this string source)
    {
        if(int.TryParse(source, out var result))
            return Option.Return(result);
        return Option.None<int>();
    }
    /// <summary>
    /// Attempts to parse the string into a long integer and returns an Option monad containing the result.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <returns>An Option monad containing the parsed long integer if successful, otherwise None.</returns>
    public static IOptionMonad<long> OptionalParseLong(this string source)
    {
        if(long.TryParse(source, out var result))
            return Option.Return(result);
        return Option.None<long>();
    }
    /// <summary>
    /// Attempts to parse the string into a float and returns an Option monad containing the result.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <returns>An Option monad containing the parsed float if successful, otherwise None.</returns>
    public static IOptionMonad<float> OptionalParseFloat(this string source)
    {
        if(float.TryParse(source, out var result))
            return Option.Return(result);
        return Option.None<float>();
    }
    /// <summary>
    /// Attempts to parse the string into a double and returns an Option monad containing the result.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <returns>An Option monad containing the parsed double if successful, otherwise None.</returns>
    public static IOptionMonad<double> OptionalParseDouble(this string source)
    {
        if(double.TryParse(source, out var result))
            return Option.Return(result);
        return Option.None<double>();
    }
    /// <summary>
    /// Attempts to parse the string into a boolean and returns an Option monad containing the result.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <returns>An Option monad containing the parsed boolean if successful, otherwise None.</returns>
    public static IOptionMonad<bool> OptionalParseBool(this string source)
    {
        if(bool.TryParse(source, out var result))
            return Option.Return(result);
        return Option.None<bool>();
    }
    /// <summary>
    /// Attempts to parse the string into a DateTime and returns an Option monad containing the result.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <returns>An Option monad containing the parsed DateTime if successful, otherwise None.</returns>
    public static IOptionMonad<DateTime> OptionalParseDateTime(this string source)
    {
        if(DateTime.TryParse(source, out var result))
            return Option.Return(result);
        return Option.None<DateTime>();
    }
    /// <summary>
    /// Attempts to parse the string into an enum of type T and returns an Option monad containing the result.
    /// </summary>
    /// <typeparam name="T">The enum type to parse the string into.</typeparam>
    /// <param name="source">The string to parse.</param>
    /// <returns>An Option monad containing the parsed enum if successful, otherwise None.</returns>
    public static IOptionMonad<T> OptionalParseEnum<T>(this string source)
    {
        try
        {
            return Option.Return((T)Enum.Parse(typeof(T), source));
        }
        catch
        {
            return Option.None<T>();
        }
    }
}
