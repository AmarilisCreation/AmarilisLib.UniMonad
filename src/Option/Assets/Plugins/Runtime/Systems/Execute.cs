using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Option
    {
        /// <summary>
        /// Executes the Option monad asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="self">The Option monad to execute.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task ExecuteAsync<T>(this IOptionMonad<T> self, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await self.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
        }
        /// <summary>
        /// Executes the Option monad asynchronously and performs an action if the result is Just.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="self">The Option monad to execute.</param>
        /// <param name="onJust">The action to perform if the result is Just.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task ExecuteAsync<T>(this IOptionMonad<T> self, Action<T> onJust, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await self.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if(selfResult.IsJust)
            {
                onJust(selfResult.Value);
            }
        }
        /// <summary>
        /// Executes the Option monad asynchronously and performs actions based on whether the result is Just or None.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="self">The Option monad to execute.</param>
        /// <param name="onJust">The action to perform if the result is Just.</param>
        /// <param name="onNone">The action to perform if the result is None.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task ExecuteAsync<T>(this IOptionMonad<T> self, Action<T> onJust, Action onNone, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await self.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if(selfResult.IsJust)
            {
                onJust(selfResult.Value);
            }
            else
            {
                onNone();
            }
        }
    }
}
