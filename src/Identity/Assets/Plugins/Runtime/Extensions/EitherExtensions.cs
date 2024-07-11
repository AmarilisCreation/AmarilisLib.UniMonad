using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class EitherExtensions
    {
        /// <summary>
        /// Converts an Either monad to an Identity monad asynchronously.
        /// </summary>
        /// <typeparam name="TLeft">The type of the left value.</typeparam>
        /// <typeparam name="TRight">The type of the right value.</typeparam>
        /// <param name="source">The source Either monad.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An Identity monad that contains the result of the Either monad.</returns>
        public static async Task<IIdentityMonad<EitherResult<TLeft, TRight>>> ToIdentityAsync<TLeft, TRight>(this IEitherMonad<TLeft, TRight> source, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return Identity.Return(selfResult);
        }

        /// <summary>
        /// Converts an Either monad to an Option monad asynchronously.
        /// </summary>
        /// <typeparam name="TLeft">The type of the left value.</typeparam>
        /// <typeparam name="TRight">The type of the right value.</typeparam>
        /// <param name="source">The source Either monad.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An Option monad that contains the right value of the Either monad, or None if the Either is a left value.</returns>
        public static async Task<IOptionMonad<TRight>> ToOptionAsync<TLeft, TRight>(this IEitherMonad<TLeft, TRight> source, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if(selfResult.IsRight) return Option.Return(selfResult.Right);
            return Option.None<TRight>();
        }

        /// <summary>
        /// Converts an either monad to a try monad asynchronously, using a specified selector function to create an exception from the left value if the either monad is left.
        /// </summary>
        /// <typeparam name="TLeft">The type of the left value in the either monad.</typeparam>
        /// <typeparam name="TRight">The type of the right value in the either monad.</typeparam>
        /// <typeparam name="TException">The type of the exception to throw if the either monad is left.</typeparam>
        /// <param name="source">The either monad to convert.</param>
        /// <param name="selector">The function to create an exception from the left value.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a try monad containing the right value if the either monad is right, or an exception if it is left.</returns>
        public static async Task<ITryMonad<TRight>> ToTryAsync<TLeft, TRight, TException>(this IEitherMonad<TLeft, TRight> source, Func<TLeft, TException> selector, CancellationToken cancellationToken = default)
            where TException : Exception
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if(selfResult.IsRight) return Try.Return(selfResult.Right);
            return Try.Throw<TRight>(selector(selfResult.Left));
        }

        /// <summary>
        /// Converts an either monad to a try monad asynchronously, using a specified asynchronous selector function to create an exception from the left value if the either monad is left.
        /// </summary>
        /// <typeparam name="TLeft">The type of the left value in the either monad.</typeparam>
        /// <typeparam name="TRight">The type of the right value in the either monad.</typeparam>
        /// <typeparam name="TException">The type of the exception to throw if the either monad is left.</typeparam>
        /// <param name="source">The either monad to convert.</param>
        /// <param name="selector">The asynchronous function to create an exception from the left value.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a try monad containing the right value if the either monad is right, or an exception if it is left.</returns>
        public static async Task<ITryMonad<TRight>> ToTryAsync<TLeft, TRight, TException>(this IEitherMonad<TLeft, TRight> source, Func<TLeft, Task<TException>> selector, CancellationToken cancellationToken = default)
            where TException : Exception
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if(selfResult.IsRight) return Try.Return(selfResult.Right);
            var selectorResult = await selector(selfResult.Left);
            return Try.Throw<TRight>(selectorResult);
        }
    }
}