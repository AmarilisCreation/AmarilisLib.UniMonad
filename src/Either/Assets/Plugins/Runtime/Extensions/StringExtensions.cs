using AmarilisLib.Monad;
using System;

public static partial class StringExtensions
{
    /// <summary>
    /// Attempts to parse the string as an integer. If successful, returns the integer in a Right value;
    /// otherwise, returns the specified Left value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <param name="source">The string to parse.</param>
    /// <param name="leftValue">The value to return if the parse fails.</param>
    /// <returns>An Either monad containing the parsed integer or the Left value.</returns>
    public static IEitherMonad<TLeft, int> ParseIntOrLeft<TLeft>(this string source, TLeft leftValue)
    {
        if(int.TryParse(source, out var result))
            return Either.ReturnRight<TLeft, int>(result);
        else
            return Either.ReturnLeft<TLeft, int>(leftValue);
    }
    /// <summary>
    /// Attempts to parse the string as an integer. If successful, returns the integer in a Right value;
    /// otherwise, returns the result of the specified Left value selector function.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <param name="source">The string to parse.</param>
    /// <param name="leftSelector">A function that generates the Left value if the parse fails.</param>
    /// <returns>An Either monad containing the parsed integer or the Left value.</returns>
    public static IEitherMonad<TLeft, int> ParseIntOrLeft<TLeft>(this string source, Func<string, TLeft> leftSelector)
    {
        if(int.TryParse(source, out var result))
            return Either.ReturnRight<TLeft, int>(result);
        else
            return Either.ReturnLeft<TLeft, int>(leftSelector(source));
    }
    /// <summary>
    /// Attempts to parse the string as a long. If successful, returns the long in a Right value;
    /// otherwise, returns the specified Left value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <param name="source">The string to parse.</param>
    /// <param name="leftValue">The value to return if the parse fails.</param>
    /// <returns>An Either monad containing the parsed long or the Left value.</returns>
    public static IEitherMonad<TLeft, long> ParseLongOrLeft<TLeft>(this string source, TLeft leftValue)
    {
        if(long.TryParse(source, out var result))
            return Either.ReturnRight<TLeft, long>(result);
        else
            return Either.ReturnLeft<TLeft, long>(leftValue);
    }
    /// <summary>
    /// Attempts to parse the string as a long. If successful, returns the long in a Right value;
    /// otherwise, returns the result of the specified Left value selector function.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <param name="source">The string to parse.</param>
    /// <param name="leftSelector">A function that generates the Left value if the parse fails.</param>
    /// <returns>An Either monad containing the parsed long or the Left value.</returns>
    public static IEitherMonad<TLeft, long> ParseLongOrLeft<TLeft>(this string source, Func<string, TLeft> leftSelector)
    {
        if(long.TryParse(source, out var result))
            return Either.ReturnRight<TLeft, long>(result);
        else
            return Either.ReturnLeft<TLeft, long>(leftSelector(source));
    }
    /// <summary>
    /// Attempts to parse the string as a float. If successful, returns the float in a Right value;
    /// otherwise, returns the specified Left value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <param name="source">The string to parse.</param>
    /// <param name="leftValue">The value to return if the parse fails.</param>
    /// <returns>An Either monad containing the parsed float or the Left value.</returns>
    public static IEitherMonad<TLeft, float> ParseFloatOrLeft<TLeft>(this string source, TLeft leftValue)
    {
        if(float.TryParse(source, out var result))
            return Either.ReturnRight<TLeft, float>(result);
        else
            return Either.ReturnLeft<TLeft, float>(leftValue);
    }
    /// <summary>
    /// Attempts to parse the string as a float. If successful, returns the float in a Right value;
    /// otherwise, returns the result of the specified Left value selector function.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <param name="source">The string to parse.</param>
    /// <param name="leftSelector">A function that generates the Left value if the parse fails.</param>
    /// <returns>An Either monad containing the parsed float or the Left value.</returns>
    public static IEitherMonad<TLeft, float> ParseFloatOrLeft<TLeft>(this string source, Func<string, TLeft> leftSelector)
    {
        if(float.TryParse(source, out var result))
            return Either.ReturnRight<TLeft, float>(result);
        else
            return Either.ReturnLeft<TLeft, float>(leftSelector(source));
    }
    /// <summary>
    /// Attempts to parse the string as a double. If successful, returns the double in a Right value;
    /// otherwise, returns the specified Left value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <param name="source">The string to parse.</param>
    /// <param name="leftValue">The value to return if the parse fails.</param>
    /// <returns>An Either monad containing the parsed double or the Left value.</returns>
    public static IEitherMonad<TLeft, double> ParseDoubleOrLeft<TLeft>(this string source, TLeft leftValue)
    {
        if(double.TryParse(source, out var result))
            return Either.ReturnRight<TLeft, double>(result);
        else
            return Either.ReturnLeft<TLeft, double>(leftValue);
    }
    /// <summary>
    /// Attempts to parse the string as a double. If successful, returns the double in a Right value;
    /// otherwise, returns the result of the specified Left value selector function.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <param name="source">The string to parse.</param>
    /// <param name="leftSelector">A function that generates the Left value if the parse fails.</param>
    /// <returns>An Either monad containing the parsed double or the Left value.</returns>
    public static IEitherMonad<TLeft, double> ParseDoubleOrLeft<TLeft>(this string source, Func<string, TLeft> leftSelector)
    {
        if(double.TryParse(source, out var result))
            return Either.ReturnRight<TLeft, double>(result);
        else
            return Either.ReturnLeft<TLeft, double>(leftSelector(source));
    }
    /// <summary>
    /// Attempts to parse the string as a boolean. If successful, returns the boolean in a Right value;
    /// otherwise, returns the specified Left value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <param name="source">The string to parse.</param>
    /// <param name="leftValue">The value to return if the parse fails.</param>
    /// <returns>An Either monad containing the parsed boolean or the Left value.</returns>
    public static IEitherMonad<TLeft, bool> ParseBoolOrLeft<TLeft>(this string source, TLeft leftValue)
    {
        if(bool.TryParse(source, out var result))
            return Either.ReturnRight<TLeft, bool>(result);
        else
            return Either.ReturnLeft<TLeft, bool>(leftValue);
    }
    /// <summary>
    /// Attempts to parse the string as a boolean. If successful, returns the boolean in a Right value;
    /// otherwise, returns the result of the specified Left value selector function.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <param name="source">The string to parse.</param>
    /// <param name="leftSelector">A function that generates the Left value if the parse fails.</param>
    /// <returns>An Either monad containing the parsed boolean or the Left value.</returns>
    public static IEitherMonad<TLeft, bool> ParseBoolOrLeft<TLeft>(this string source, Func<string, TLeft> leftSelector)
    {
        if(bool.TryParse(source, out var result))
            return Either.ReturnRight<TLeft, bool>(result);
        else
            return Either.ReturnLeft<TLeft, bool>(leftSelector(source));
    }
    /// <summary>
    /// Attempts to parse the string as a DateTime. If successful, returns the DateTime in a Right value;
    /// otherwise, returns the specified Left value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <param name="source">The string to parse.</param>
    /// <param name="leftValue">The value to return if the parse fails.</param>
    /// <returns>An Either monad containing the parsed DateTime or the Left value.</returns>
    public static IEitherMonad<TLeft, DateTime> ParseDateTimeOrLeft<TLeft>(this string source, TLeft leftValue)
    {
        if(DateTime.TryParse(source, out var result))
            return Either.ReturnRight<TLeft, DateTime>(result);
        else
            return Either.ReturnLeft<TLeft, DateTime>(leftValue);
    }
    /// <summary>
    /// Attempts to parse the string as a DateTime. If successful, returns the DateTime in a Right value;
    /// otherwise, returns the result of the specified Left value selector function.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <param name="source">The string to parse.</param>
    /// <param name="leftSelector">A function that generates the Left value if the parse fails.</param>
    /// <returns>An Either monad containing the parsed DateTime or the Left value.</returns>
    public static IEitherMonad<TLeft, DateTime> ParseDateTimeOrLeft<TLeft>(this string source, Func<string, TLeft> leftSelector)
    {
        if(DateTime.TryParse(source, out var result))
            return Either.ReturnRight<TLeft, DateTime>(result);
        else
            return Either.ReturnLeft<TLeft, DateTime>(leftSelector(source));
    }
    /// <summary>
    /// Attempts to parse the string as an enumeration of type TRight. If successful, returns the enumeration in a Right value;
    /// otherwise, returns the specified Left value.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <typeparam name="TRight">The type of the enumeration.</typeparam>
    /// <param name="source">The string to parse.</param>
    /// <param name="leftValue">The value to return if the parse fails.</param>
    /// <returns>An Either monad containing the parsed enumeration or the Left value.</returns>
    public static IEitherMonad<TLeft, TRight> ParseEnumOrLeft<TLeft, TRight>(this string source, TLeft leftValue)
    {
        try
        {
            return Either.ReturnRight<TLeft, TRight>((TRight)Enum.Parse(typeof(TRight), source));
        }
        catch
        {
            return Either.ReturnLeft<TLeft, TRight>(leftValue);
        }
    }
    /// <summary>
    /// Attempts to parse the string as an enumeration of type TRight. If successful, returns the enumeration in a Right value;
    /// otherwise, returns the result of the specified Left value selector function.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <typeparam name="TRight">The type of the enumeration.</typeparam>
    /// <param name="source">The string to parse.</param>
    /// <param name="leftSelector">A function that generates the Left value if the parse fails.</param>
    /// <returns>An Either monad containing the parsed enumeration or the Left value.</returns>
    public static IEitherMonad<TLeft, TRight> ParseEnumOrLeft<TLeft, TRight>(this string source, Func<string, TLeft> leftSelector)
    {
        try
        {
            return Either.ReturnRight<TLeft, TRight>((TRight)Enum.Parse(typeof(TRight), source));
        }
        catch
        {
            return Either.ReturnLeft<TLeft, TRight>(leftSelector(source));
        }
    }
}
