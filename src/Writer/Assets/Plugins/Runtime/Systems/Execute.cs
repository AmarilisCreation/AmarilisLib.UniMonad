using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Writer
    {
        /// <summary>
        /// Executes the writer monad, running the underlying computation.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The writer monad to execute.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        public static async Task ExecuteAsync<TOutput, TValue>(this IWriterMonad<TOutput, TValue> self, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await self.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
        }
        /// <summary>
        /// Executes the writer monad, running the underlying computation and invoking the specified action with the resulting value.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The writer monad to execute.</param>
        /// <param name="onValue">An action to invoke with the resulting value.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        public static async Task ExecuteAsync<TOutput, TValue>(this IWriterMonad<TOutput, TValue> self, Action<TValue> onValue, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await self.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            onValue(selfResult.Value);
        }
        /// <summary>
        /// Executes the writer monad, running the underlying computation and invoking the specified actions with the resulting value and output.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The writer monad to execute.</param>
        /// <param name="onValue">An action to invoke with the resulting value.</param>
        /// <param name="onOutput">An action to invoke with the resulting output.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        public static async Task ExecuteAsync<TOutput, TValue>(this IWriterMonad<TOutput, TValue> self, Action<TValue> onValue, Action<IEnumerable<TOutput>> onOutput, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await self.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            onValue(selfResult.Value);
            onOutput(selfResult.Output);
        }
    }
}
