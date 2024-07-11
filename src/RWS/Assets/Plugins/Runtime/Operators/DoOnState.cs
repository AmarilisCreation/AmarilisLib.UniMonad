using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class RWS
    {
        private class DoOnStateCore<TEnvironment, TOutput, TState, TValue> : IRWSMonad<TEnvironment, TOutput, TState, TValue>
        {
            private IRWSMonad<TEnvironment, TOutput, TState, TValue> _self;
            private Action<TState> _action;
            public DoOnStateCore(IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Action<TState> action)
            {
                _self = self;
                _action = action;
            }
            async Task<RWSResult<TOutput, TState, TValue>> IRWSMonad<TEnvironment, TOutput, TState, TValue>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(environment, state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                _action(selfResult.State);
                return selfResult;
            }
        }
        /// <summary>
        /// Performs the specified action on the state produced by the RWS monad.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The RWS monad whose state to act on.</param>
        /// <param name="action">The action to perform on the state.</param>
        /// <returns>An RWS monad that performs the specified action on its state.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TValue> DoOnState<TEnvironment, TOutput, TState, TValue>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Action<TState> action)
            => new DoOnStateCore<TEnvironment, TOutput, TState, TValue>(self, action);

        private class DoOnStateAsyncCore<TEnvironment, TOutput, TState, TValue> : IRWSMonad<TEnvironment, TOutput, TState, TValue>
        {
            private IRWSMonad<TEnvironment, TOutput, TState, TValue> _self;
            private Func<TState, Task> _action;
            public DoOnStateAsyncCore(IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Func<TState, Task> action)
            {
                _self = self;
                _action = action;
            }
            async Task<RWSResult<TOutput, TState, TValue>> IRWSMonad<TEnvironment, TOutput, TState, TValue>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(environment, state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                await _action(selfResult.State);
                cancellationToken.ThrowIfCancellationRequested();
                return selfResult;
            }
        }
        /// <summary>
        /// Asynchronously performs the specified action on the state produced by the RWS monad.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The RWS monad whose state to act on.</param>
        /// <param name="action">The asynchronous action to perform on the state.</param>
        /// <returns>An RWS monad that performs the specified asynchronous action on its state.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TValue> DoOnState<TEnvironment, TOutput, TState, TValue>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Func<TState, Task> action)
            => new DoOnStateAsyncCore<TEnvironment, TOutput, TState, TValue>(self, action);
    }
}
