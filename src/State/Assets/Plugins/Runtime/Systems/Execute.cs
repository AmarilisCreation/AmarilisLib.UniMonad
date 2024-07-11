using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class State
    {
        /// <summary>
        /// Executes the state monad and returns the resulting state.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The state monad to execute.</param>
        /// <param name="state">The initial state.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the resulting state.</returns>
        public static async Task<TState> ExecuteAsync<TState, TValue>(this IStateMonad<TState, TValue> self, TState state, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await self.RunAsync(state, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return selfResult.State;
        }
        /// <summary>
        /// Executes the state monad, applies an action to the result, and returns the resulting state.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The state monad to execute.</param>
        /// <param name="state">The initial state.</param>
        /// <param name="onValue">The action to apply to the result.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the resulting state.</returns>
        public static async Task<TState> ExecuteAsync<TState, TValue>(this IStateMonad<TState, TValue> self, TState state, Action<TValue> onValue, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await self.RunAsync(state, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            onValue(selfResult.Value);
            return selfResult.State;
        }
    }
}
