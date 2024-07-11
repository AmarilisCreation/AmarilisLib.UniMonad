using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class OptionExtensions
    {
        /// <summary>
        /// Converts an Option monad to an Identity monad asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The source Option monad.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An Identity monad that contains the result of the Option monad.</returns>
        public static async Task<IIdentityMonad<OptionResult<T>>> ToIdentityAsync<T>(this IOptionMonad<T> source, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return Identity.Return(selfResult);
        }

        /// <summary>
        /// Converts an Option monad to an Either monad asynchronously.
        /// </summary>
        /// <typeparam name="TLeft">The type of the left value.</typeparam>
        /// <typeparam name="TRight">The type of the right value.</typeparam>
        /// <param name="source">The source Option monad.</param>
        /// <param name="leftValue">The left value to use if the Option is Nothing.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An Either monad that contains the result of the Option monad.</returns>
        public static async Task<IEitherMonad<TLeft, TRight>> ToEitherAsync<TLeft, TRight>(this IOptionMonad<TRight> source, TLeft leftValue, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if(selfResult.IsJust) return Either.ReturnRight<TLeft, TRight>(selfResult.Value);
            return Either.ReturnLeft<TLeft, TRight>(leftValue);
        }

        /// <summary>
        /// Converts an option monad to a try monad asynchronously. If the option monad is none, the specified exception is thrown.
        /// </summary>
        /// <typeparam name="T">The type of the value in the monad.</typeparam>
        /// <typeparam name="TException">The type of the exception to throw if the option monad is none.</typeparam>
        /// <param name="source">The option monad to convert.</param>
        /// <param name="exception">The exception to throw if the option monad is none.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a try monad containing the value if the option monad is just, or the specified exception if it is none.</returns>
        public static async Task<ITryMonad<T>> ToTryAsync<T, TException>(this IOptionMonad<T> source, TException exception, CancellationToken cancellationToken = default)
            where TException : Exception
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if(selfResult.IsJust) return Try.Return(selfResult.Value);
            return Try.Throw<T>(exception);
        }
    }
}