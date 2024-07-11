using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Identity
    {
        /// <summary>
        /// Executes the identity monad asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the value in the monad.</typeparam>
        /// <param name="self">The identity monad to execute.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task ExecuteAsync<T>(this IIdentityMonad<T> self, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await self.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
        }
        /// <summary>
        /// Executes the identity monad asynchronously and performs an action on the resulting value.
        /// </summary>
        /// <typeparam name="T">The type of the value in the monad.</typeparam>
        /// <param name="self">The identity monad to execute.</param>
        /// <param name="onValue">The action to perform on the resulting value.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task ExecuteAsync<T>(this IIdentityMonad<T> self, Action<T> onValue, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await self.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            onValue(selfResult);
        }
    }
}
