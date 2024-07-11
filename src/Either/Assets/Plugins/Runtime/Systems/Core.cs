using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    /// <summary>
    /// Represents an Either monad with Left and Right values.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <typeparam name="TRight">The type of the Right value.</typeparam>
    public interface IEitherMonad<TLeft, TRight>
    {
        /// <summary>
        /// Runs the monad asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains an <see cref="EitherResult{TLeft, TRight}"/>.</returns>
        Task<EitherResult<TLeft, TRight>> RunAsync(CancellationToken cancellationToken);
    }

    /// <summary>
    /// Represents the result of an Either monad, which can be either Left or Right.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <typeparam name="TRight">The type of the Right value.</typeparam>
    public struct EitherResult<TLeft, TRight>
    {
        /// <summary>
        /// Gets the Left value.
        /// </summary>
        public TLeft Left { get; private set; }

        /// <summary>
        /// Gets the Right value.
        /// </summary>
        public TRight Right { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the result is a Left value.
        /// </summary>
        public bool IsLeft { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the result is a Right value.
        /// </summary>
        public bool IsRight { get; private set; }

        /// <summary>
        /// Creates an Either result representing a Left value.
        /// </summary>
        /// <param name="value">The Left value.</param>
        /// <returns>An Either result containing the Left value.</returns>
        public static EitherResult<TLeft, TRight> ReturnLeft(TLeft value) => new EitherResult<TLeft, TRight> { Left = value, IsLeft = true, IsRight = false };

        /// <summary>
        /// Creates an Either result representing a Right value.
        /// </summary>
        /// <param name="value">The Right value.</param>
        /// <returns>An Either result containing the Right value.</returns>
        public static EitherResult<TLeft, TRight> ReturnRight(TRight value) => new EitherResult<TLeft, TRight> { Right = value, IsLeft = false, IsRight = true };
    }
}
