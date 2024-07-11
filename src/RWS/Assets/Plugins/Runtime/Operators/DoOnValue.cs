using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class RWS
    {
        private class DoOnValueCore<TEnvironment, TOutput, TState, TValue> : IRWSMonad<TEnvironment, TOutput, TState, TValue>
        {
            private IRWSMonad<TEnvironment, TOutput, TState, TValue> _self;
            private Action<TValue> _action;
            public DoOnValueCore(IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Action<TValue> action)
            {
                _self = self;
                _action = action;
            }
            async Task<RWSResult<TOutput, TState, TValue>> IRWSMonad<TEnvironment, TOutput, TState, TValue>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(environment, state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                _action(selfResult.Value);
                return selfResult;
            }
        }
        /// <summary>
        /// Performs the specified action on the value produced by the RWS monad.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The RWS monad whose value to act on.</param>
        /// <param name="action">The action to perform on the value.</param>
        /// <returns>An RWS monad that performs the specified action on its value.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TValue> DoOnValue<TEnvironment, TOutput, TState, TValue>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Action<TValue> action)
            => new DoOnValueCore<TEnvironment, TOutput, TState, TValue>(self, action);

        private class DoOnValueAsyncCore<TEnvironment, TOutput, TState, TValue> : IRWSMonad<TEnvironment, TOutput, TState, TValue>
        {
            private IRWSMonad<TEnvironment, TOutput, TState, TValue> _self;
            private Func<TValue, Task> _action;
            public DoOnValueAsyncCore(IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Func<TValue, Task> action)
            {
                _self = self;
                _action = action;
            }
            async Task<RWSResult<TOutput, TState, TValue>> IRWSMonad<TEnvironment, TOutput, TState, TValue>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(environment, state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                await _action(selfResult.Value);
                cancellationToken.ThrowIfCancellationRequested();
                return selfResult;
            }
        }
        /// <summary>
        /// Asynchronously performs the specified action on the value produced by the RWS monad.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The RWS monad whose value to act on.</param>
        /// <param name="action">The asynchronous action to perform on the value.</param>
        /// <returns>An RWS monad that performs the specified asynchronous action on its value.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TValue> DoOnValue<TEnvironment, TOutput, TState, TValue>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Func<TValue, Task> action)
            => new DoOnValueAsyncCore<TEnvironment, TOutput, TState, TValue>(self, action);
    }
}
