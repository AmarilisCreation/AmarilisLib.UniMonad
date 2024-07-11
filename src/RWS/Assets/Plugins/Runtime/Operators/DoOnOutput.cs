using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class RWS
    {
        private class DoOnOutputCore<TEnvironment, TOutput, TState, TValue> : IRWSMonad<TEnvironment, TOutput, TState, TValue>
        {
            private IRWSMonad<TEnvironment, TOutput, TState, TValue> _self;
            private Action<IEnumerable<TOutput>> _action;
            public DoOnOutputCore(IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Action<IEnumerable<TOutput>> action)
            {
                _self = self;
                _action = action;
            }
            async Task<RWSResult<TOutput, TState, TValue>> IRWSMonad<TEnvironment, TOutput, TState, TValue>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(environment, state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                _action(selfResult.Output);
                return selfResult;
            }
        }
        /// <summary>
        /// Performs the specified action on the output produced by the RWS monad.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The RWS monad whose output to act on.</param>
        /// <param name="action">The action to perform on the output.</param>
        /// <returns>An RWS monad that performs the specified action on its output.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TValue> DoOnOutput<TEnvironment, TOutput, TState, TValue>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Action<IEnumerable<TOutput>> action)
            => new DoOnOutputCore<TEnvironment, TOutput, TState, TValue>(self, action);

        private class DoOnOutputAsyncCore<TEnvironment, TOutput, TState, TValue> : IRWSMonad<TEnvironment, TOutput, TState, TValue>
        {
            private IRWSMonad<TEnvironment, TOutput, TState, TValue> _self;
            private Func<IEnumerable<TOutput>, Task> _action;
            public DoOnOutputAsyncCore(IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Func<IEnumerable<TOutput>, Task> action)
            {
                _self = self;
                _action = action;
            }
            async Task<RWSResult<TOutput, TState, TValue>> IRWSMonad<TEnvironment, TOutput, TState, TValue>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(environment, state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                await _action(selfResult.Output);
                cancellationToken.ThrowIfCancellationRequested();
                return selfResult;
            }
        }
        /// <summary>
        /// Asynchronously performs the specified action on the output produced by the RWS monad.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The RWS monad whose output to act on.</param>
        /// <param name="action">The asynchronous action to perform on the output.</param>
        /// <returns>An RWS monad that performs the specified asynchronous action on its output.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TValue> DoOnOutput<TEnvironment, TOutput, TState, TValue>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Func<IEnumerable<TOutput>, Task> action)
            => new DoOnOutputAsyncCore<TEnvironment, TOutput, TState, TValue>(self, action);
    }
}
