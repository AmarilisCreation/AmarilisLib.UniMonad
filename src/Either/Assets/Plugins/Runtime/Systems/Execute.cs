using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Either
    {
        /// <summary>
        /// Executes the Either monad asynchronously.
        /// </summary>
        /// <typeparam name="TLeft">The type of the Left value.</typeparam>
        /// <typeparam name="TRight">The type of the Right value.</typeparam>
        /// <param name="self">The Either monad to execute.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous execution.</returns>
        public static async Task ExecuteAsync<TLeft, TRight>(this IEitherMonad<TLeft, TRight> self, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await self.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
        }
        /// <summary>
        /// Executes the Either monad asynchronously and performs the specified action if the result is Right.
        /// </summary>
        /// <typeparam name="TLeft">The type of the Left value.</typeparam>
        /// <typeparam name="TRight">The type of the Right value.</typeparam>
        /// <param name="self">The Either monad to execute.</param>
        /// <param name="onRight">The action to perform if the result is Right.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous execution.</returns>
        public static async Task ExecuteAsync<TLeft, TRight>(this IEitherMonad<TLeft, TRight> self, Action<TRight> onRight, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await self.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if(selfResult.IsRight)
            {
                onRight(selfResult.Right);
            }
        }
        /// <summary>
        /// Executes the Either monad asynchronously and performs the specified actions based on the result.
        /// </summary>
        /// <typeparam name="TLeft">The type of the Left value.</typeparam>
        /// <typeparam name="TRight">The type of the Right value.</typeparam>
        /// <param name="self">The Either monad to execute.</param>
        /// <param name="onRight">The action to perform if the result is Right.</param>
        /// <param name="onLeft">The action to perform if the result is Left.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous execution.</returns>
        public static async Task ExecuteAsync<TLeft, TRight>(this IEitherMonad<TLeft, TRight> self, Action<TRight> onRight, Action<TLeft> onLeft, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await self.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if(selfResult.IsRight)
            {
                onRight(selfResult.Right);
            }
            else
            {
                onLeft(selfResult.Left);
            }
        }
    }
}
