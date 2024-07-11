using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class IO
    {
        /// <summary>
        /// Executes the IO monad asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the monad.</typeparam>
        /// <param name="self">The monad to execute.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous execution.</returns>
        public static async Task ExecuteAsync<T>(this IIOMonad<T> self, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await self.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
        }
        /// <summary>
        /// Executes the IO monad asynchronously and performs the specified action on the result value.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the monad.</typeparam>
        /// <param name="self">The monad to execute.</param>
        /// <param name="onValue">The action to perform on the monad's result value.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous execution.</returns>
        public static async Task ExecuteAsync<T>(this IIOMonad<T> self, Action<T> onValue, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await self.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            onValue(selfResult);
        }
    }
}
