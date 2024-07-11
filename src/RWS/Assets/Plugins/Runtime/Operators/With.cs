using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class RWS
    {
        private class WithCore<TEnvironment, TOutput, TState, TValue> : IRWSMonad<TEnvironment, TOutput, TState, TValue>
        {
            private IRWSMonad<TEnvironment, TOutput, TState, TValue> _self;
            private Func<TState, TState> _selector;
            public WithCore(IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Func<TState, TState> selector)
            {
                _self = self;
                _selector = selector;
            }

            async Task<RWSResult<TOutput, TState, TValue>> IRWSMonad<TEnvironment, TOutput, TState, TValue>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(environment, state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return new RWSResult<TOutput, TState, TValue>(selfResult.Value, Array.Empty<TOutput>(), _selector(selfResult.State));
            }
        }
        /// <summary>
        /// Modifies the state of the RWS monad using the specified selector function.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The RWS monad whose state to modify.</param>
        /// <param name="selector">The function to modify the state.</param>
        /// <returns>An RWS monad with the modified state.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TValue> With<TEnvironment, TOutput, TState, TValue>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Func<TState, TState> selector)
            => new WithCore<TEnvironment, TOutput, TState, TValue>(self, selector);

        private class WithAsyncCore<TEnvironment, TOutput, TState, TValue> : IRWSMonad<TEnvironment, TOutput, TState, TValue>
        {
            private IRWSMonad<TEnvironment, TOutput, TState, TValue> _self;
            private Func<TState, Task<TState>> _selector;
            public WithAsyncCore(IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Func<TState, Task<TState>> selector)
            {
                _self = self;
                _selector = selector;
            }

            async Task<RWSResult<TOutput, TState, TValue>> IRWSMonad<TEnvironment, TOutput, TState, TValue>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(environment, state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector(selfResult.State);
                cancellationToken.ThrowIfCancellationRequested();
                return new RWSResult<TOutput, TState, TValue>(selfResult.Value, Array.Empty<TOutput>(), selectorResult);
            }
        }
        /// <summary>
        /// Modifies the state of the RWS monad asynchronously using the specified selector function.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The RWS monad whose state to modify.</param>
        /// <param name="selector">The asynchronous function to modify the state.</param>
        /// <returns>An RWS monad with the modified state.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TValue> With<TEnvironment, TOutput, TState, TValue>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Func<TState, Task<TState>> selector)
            => new WithAsyncCore<TEnvironment, TOutput, TState, TValue>(self, selector);
    }
}
