using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class RWS
    {
        private class DoCore<TEnvironment, TOutput, TState, TValue> : IRWSMonad<TEnvironment, TOutput, TState, TValue>
        {
            private IRWSMonad<TEnvironment, TOutput, TState, TValue> _self;
            private Action<RWSResult<TOutput, TState, TValue>> _action;
            public DoCore(IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Action<RWSResult<TOutput, TState, TValue>> action)
            {
                _self = self;
                _action = action;
            }
            async Task<RWSResult<TOutput, TState, TValue>> IRWSMonad<TEnvironment, TOutput, TState, TValue>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(environment, state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                _action(selfResult);
                return selfResult;
            }
        }
        /// <summary>
        /// Performs the specified action on the result produced by the RWS monad.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The RWS monad whose result to act on.</param>
        /// <param name="action">The action to perform on the result.</param>
        /// <returns>An RWS monad that performs the specified action on its result.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TValue> Do<TEnvironment, TOutput, TState, TValue>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Action<RWSResult<TOutput, TState, TValue>> action)
            => new DoCore<TEnvironment, TOutput, TState, TValue>(self, action);

        private class DoAsyncCore<TEnvironment, TOutput, TState, TValue> : IRWSMonad<TEnvironment, TOutput, TState, TValue>
        {
            private IRWSMonad<TEnvironment, TOutput, TState, TValue> _self;
            private Func<RWSResult<TOutput, TState, TValue>, Task> _action;
            public DoAsyncCore(IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Func<RWSResult<TOutput, TState, TValue>, Task> action)
            {
                _self = self;
                _action = action;
            }
            async Task<RWSResult<TOutput, TState, TValue>> IRWSMonad<TEnvironment, TOutput, TState, TValue>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(environment, state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                await _action(selfResult);
                cancellationToken.ThrowIfCancellationRequested();
                return selfResult;
            }
        }
        /// <summary>
        /// Asynchronously performs the specified action on the result produced by the RWS monad.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The RWS monad whose result to act on.</param>
        /// <param name="action">The asynchronous action to perform on the result.</param>
        /// <returns>An RWS monad that performs the specified asynchronous action on its result.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TValue> Do<TEnvironment, TOutput, TState, TValue>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Func<RWSResult<TOutput, TState, TValue>, Task> action)
            => new DoAsyncCore<TEnvironment, TOutput, TState, TValue>(self, action);
    }
}
