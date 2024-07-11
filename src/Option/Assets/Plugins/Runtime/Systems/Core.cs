using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    /// <summary>
    /// Represents a monad that can produce an optional result asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    public interface IOptionMonad<T>
    {
        /// <summary>
        /// Runs the option monad and produces an option result asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing the option result.</returns>
        Task<OptionResult<T>> RunAsync(CancellationToken cancellationToken);
    }
    /// <summary>
    /// Represents the result of an option monad, which can either be a value or none.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    public struct OptionResult<T>
    {
        /// <summary>
        /// Gets the value of the option result if it is Just.
        /// </summary>
        public T Value { private set; get; }

        /// <summary>
        /// Gets a value indicating whether the option result is None.
        /// </summary>
        public bool IsNone { private set; get; }

        /// <summary>
        /// Gets a value indicating whether the option result is Just.
        /// </summary>
        public bool IsJust { private set; get; }

        /// <summary>
        /// Creates an option result that represents a value.
        /// </summary>
        /// <param name="value">The value of the option result.</param>
        /// <returns>An option result that represents a value.</returns>
        public static OptionResult<T> Just(T value) => new OptionResult<T>() { Value = value, IsNone = false, IsJust = true };

        /// <summary>
        /// Creates an option result that represents none.
        /// </summary>
        /// <returns>An option result that represents none.</returns>
        public static OptionResult<T> None() => new OptionResult<T>() { Value = default, IsNone = true, IsJust = false };
    }
}
