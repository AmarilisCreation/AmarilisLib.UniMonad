using AmarilisLib.Monad;

public static partial class ArrayExtensions
{
    /// <summary>
    /// Attempts to retrieve the element at the specified index from the array and returns an Option monad containing the result.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    /// <param name="source">The array to retrieve the element from.</param>
    /// <param name="index">The zero-based index of the element to retrieve.</param>
    /// <returns>An Option monad containing the element if the index is valid, otherwise None.</returns>
    public static IOptionMonad<T> OptionalElementAt<T>(this T[] source, int index)
    {
        var length = source.Length;
        if(index >= 0 && index < length)
            return Option.Return(source[index]);
        return Option.None<T>();
    }
}
