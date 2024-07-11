using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Reader
    {
        /// <summary>
        /// Executes the Reader monad with the specified environment.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The Reader monad to execute.</param>
        /// <param name="environment">The environment to use.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task ExecuteAsync<TEnvironment, TValue>(this IReaderMonad<TEnvironment, TValue> self, TEnvironment environment, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await self.RunAsync(environment, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
        }
        /// <summary>
        /// Executes the Reader monad with the specified environment and performs an action on the result.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The Reader monad to execute.</param>
        /// <param name="environment">The environment to use.</param>
        /// <param name="onValue">The action to perform on the result.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task ExecuteAsync<TEnvironment, TValue>(this IReaderMonad<TEnvironment, TValue> self, TEnvironment environment, Action<TValue> onValue, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await self.RunAsync(environment, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            onValue(selfResult);
        }
    }
}
