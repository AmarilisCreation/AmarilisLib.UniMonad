using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class RWS
    {
        /// <summary>
        /// Executes the RWS monad and returns the resulting state.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The RWS monad to execute.</param>
        /// <param name="environment">The environment to pass to the monad.</param>
        /// <param name="state">The initial state to pass to the monad.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>The resulting state after executing the monad.</returns>
        public static async Task<TState> ExecuteAsync<TEnvironment, TOutput, TState, TValue>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> self, TEnvironment environment, TState state, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await self.ExecuteAsync(environment, state, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return selfResult;
        }
        /// <summary>
        /// Executes the RWS monad, returns the resulting state, and performs an action on the resulting value.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The RWS monad to execute.</param>
        /// <param name="environment">The environment to pass to the monad.</param>
        /// <param name="state">The initial state to pass to the monad.</param>
        /// <param name="onValue">The action to perform on the resulting value.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>The resulting state after executing the monad.</returns>
        public static async Task<TState> ExecuteAsync<TEnvironment, TOutput, TState, TValue>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> self, TEnvironment environment, TState state, Action<TValue> onValue, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await self.RunAsync(environment, state, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            onValue(selfResult.Value);
            return selfResult.State;
        }
        /// <summary>
        /// Executes the RWS monad, returns the resulting state, and performs actions on the resulting value and output.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The RWS monad to execute.</param>
        /// <param name="environment">The environment to pass to the monad.</param>
        /// <param name="state">The initial state to pass to the monad.</param>
        /// <param name="onValue">The action to perform on the resulting value.</param>
        /// <param name="onOutput">The action to perform on the resulting output.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>The resulting state after executing the monad.</returns>
        public static async Task<TState> ExecuteAsync<TEnvironment, TOutput, TState, TValue>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> self, TEnvironment environment, TState state, Action<TValue> onValue, Action<IEnumerable<TOutput>> onOutput, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await self.RunAsync(environment, state, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            onValue(selfResult.Value);
            onOutput(selfResult.Output);
            return selfResult.State;
        }
        /// <summary>
        /// Executes the RWS monad, returns the resulting state, and performs actions on the resulting value and state.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The RWS monad to execute.</param>
        /// <param name="environment">The environment to pass to the monad.</param>
        /// <param name="state">The initial state to pass to the monad.</param>
        /// <param name="onValue">The action to perform on the resulting value.</param>
        /// <param name="onState">The action to perform on the resulting state.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>The resulting state after executing the monad.</returns>
        public static async Task<TState> ExecuteAsync<TEnvironment, TOutput, TState, TValue>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> self, TEnvironment environment, TState state, Action<TValue> onValue, Action<TState> onState, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await self.RunAsync(environment, state, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            onValue(selfResult.Value);
            onState(selfResult.State);
            return selfResult.State;
        }
        /// <summary>
        /// Executes the RWS monad, returns the resulting state, and performs actions on the resulting value, state, and output.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The RWS monad to execute.</param>
        /// <param name="environment">The environment to pass to the monad.</param>
        /// <param name="state">The initial state to pass to the monad.</param>
        /// <param name="onValue">The action to perform on the resulting value.</param>
        /// <param name="onState">The action to perform on the resulting state.</param>
        /// <param name="onOutput">The action to perform on the resulting output.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>The resulting state after executing the monad.</returns>
        public static async Task<TState> ExecuteAsync<TEnvironment, TOutput, TState, TValue>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> self, TEnvironment environment, TState state, Action<TValue> onValue, Action<TState> onState, Action<IEnumerable<TOutput>> onOutput, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await self.RunAsync(environment, state, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            onValue(selfResult.Value);
            onState(selfResult.State);
            onOutput(selfResult.Output);
            return selfResult.State;
        }
    }
}
