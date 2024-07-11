using AmarilisLib.Monad;
using System;

public static partial class StringExtensions
{
    /// <summary>
    /// Tries to parse the string as an integer.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <returns>A try monad containing the parsed integer or an exception if parsing fails.</returns>
    public static ITryMonad<int> TryParseInt(this string source)
    {
        try
        {
            return Try.Return(int.Parse(source));
        }
        catch(Exception exception)
        {
            return Try.Throw<int>(exception);
        }
    }
    /// <summary>
    /// Tries to parse the string as a long.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <returns>A try monad containing the parsed long or an exception if parsing fails.</returns>
    public static ITryMonad<long> TryParseLong(this string source)
    {
        try
        {
            return Try.Return(long.Parse(source));
        }
        catch(Exception exception)
        {
            return Try.Throw<long>(exception);
        }
    }
    /// <summary>
    /// Tries to parse the string as a float.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <returns>A try monad containing the parsed float or an exception if parsing fails.</returns>
    public static ITryMonad<float> TryParseFloat(this string source)
    {
        try
        {
            return Try.Return(float.Parse(source));
        }
        catch(Exception exception)
        {
            return Try.Throw<float>(exception);
        }
    }
    /// <summary>
    /// Tries to parse the string as a double.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <returns>A try monad containing the parsed double or an exception if parsing fails.</returns>
    public static ITryMonad<double> TryParseDouble(this string source)
    {
        try
        {
            return Try.Return(double.Parse(source));
        }
        catch(Exception exception)
        {
            return Try.Throw<double>(exception);
        }
    }
    /// <summary>
    /// Tries to parse the string as a boolean.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <returns>A try monad containing the parsed boolean or an exception if parsing fails.</returns>
    public static ITryMonad<bool> TryParseBool(this string source)
    {
        try
        {
            return Try.Return(bool.Parse(source));
        }
        catch(Exception exception)
        {
            return Try.Throw<bool>(exception);
        }
    }
    /// <summary>
    /// Tries to parse the string as a DateTime.
    /// </summary>
    /// <param name="source">The string to parse.</param>
    /// <returns>A try monad containing the parsed DateTime or an exception if parsing fails.</returns>
    public static ITryMonad<DateTime> TryParseDateTime(this string source)
    {
        try
        {
            return Try.Return(DateTime.Parse(source));
        }
        catch(Exception exception)
        {
            return Try.Throw<DateTime>(exception);
        }
    }
    /// <summary>
    /// Tries to parse the string as an enum of type T.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    /// <param name="source">The string to parse.</param>
    /// <returns>A try monad containing the parsed enum value or an exception if parsing fails.</returns>
    public static ITryMonad<T> TryParseEnum<T>(this string source)
    {
        try
        {
            return Try.Return((T)Enum.Parse(typeof(T), source));
        }
        catch(Exception exception)
        {
            return Try.Throw<T>(exception);
        }
    }
}
