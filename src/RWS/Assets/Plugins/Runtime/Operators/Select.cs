using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class RWS
    {
        private class SelectCore<TEnvironment, TOutput, TState, TValue, TResult> : IRWSMonad<TEnvironment, TOutput, TState, TResult>
        {
            private IRWSMonad<TEnvironment, TOutput, TState, TValue> _self;
            private Func<TValue, TResult> _selector;
            public SelectCore(IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Func<TValue, TResult> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<RWSResult<TOutput, TState, TResult>> IRWSMonad<TEnvironment, TOutput, TState, TResult>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(environment, state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return new RWSResult<TOutput, TState, TResult>(_selector(selfResult.Value), selfResult.Output, selfResult.State ?? state);
            }
        }
        /// <summary>
        /// Projects each element of the RWS monad into a new form using a specified selector function.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value in the source RWS monad.</typeparam>
        /// <typeparam name="TResult">The type of the value in the resulting RWS monad.</typeparam>
        /// <param name="self">The source RWS monad.</param>
        /// <param name="selector">A function to project each value into a new form.</param>
        /// <returns>An RWS monad containing the projected values.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TResult> Select<TEnvironment, TOutput, TState, TValue, TResult>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Func<TValue, TResult> selector)
            => new SelectCore<TEnvironment, TOutput, TState, TValue, TResult>(self, selector);

        private class SelectAsyncCore<TEnvironment, TOutput, TState, TValue, TResult> : IRWSMonad<TEnvironment, TOutput, TState, TResult>
        {
            private IRWSMonad<TEnvironment, TOutput, TState, TValue> _self;
            private Func<TValue, Task<TResult>> _selector;
            public SelectAsyncCore(IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Func<TValue, Task<TResult>> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<RWSResult<TOutput, TState, TResult>> IRWSMonad<TEnvironment, TOutput, TState, TResult>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(environment, state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector(selfResult.Value);
                cancellationToken.ThrowIfCancellationRequested();
                return new RWSResult<TOutput, TState, TResult>(selectorResult, selfResult.Output, selfResult.State ?? state);
            }
        }
        /// <summary>
        /// Projects each element of the RWS monad into a new form using a specified asynchronous selector function.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value in the source RWS monad.</typeparam>
        /// <typeparam name="TResult">The type of the value in the resulting RWS monad.</typeparam>
        /// <param name="self">The source RWS monad.</param>
        /// <param name="selector">An asynchronous function to project each value into a new form.</param>
        /// <returns>An RWS monad containing the projected values.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TResult> Select<TEnvironment, TOutput, TState, TValue, TResult>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Func<TValue, Task<TResult>> selector)
            => new SelectAsyncCore<TEnvironment, TOutput, TState, TValue, TResult>(self, selector);
    }
}
