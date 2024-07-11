using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class RWS
    {
        private class GetReturnNoArgumentCore<TEnvironment, TOutput, TState> : IRWSMonad<TEnvironment, TOutput, TState, TState>
        {
            Task<RWSResult<TOutput, TState, TState>> IRWSMonad<TEnvironment, TOutput, TState, TState>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(new RWSResult<TOutput, TState, TState>(state, Array.Empty<TOutput>(), state));
            }
        }
        /// <summary>
        /// Creates an RWS monad that retrieves the current state.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <returns>An RWS monad that retrieves the current state.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TState> Get<TEnvironment, TOutput, TState>()
            => new GetReturnNoArgumentCore<TEnvironment, TOutput, TState>();

        private class GetReturnSelectCore<TEnvironment, TOutput, TState> : IRWSMonad<TEnvironment, TOutput, TState, TState>
        {
            private Func<TState, TState> _selector;
            public GetReturnSelectCore(Func<TState, TState> selector)
            {
                _selector = selector;
            }
            Task<RWSResult<TOutput, TState, TState>> IRWSMonad<TEnvironment, TOutput, TState, TState>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(new RWSResult<TOutput, TState, TState>(state, Array.Empty<TOutput>(), _selector(state)));
            }
        }
        /// <summary>
        /// Creates an RWS monad that retrieves the current state and applies a selector function to it.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="selector">A function to apply to the current state.</param>
        /// <returns>An RWS monad that retrieves the current state and applies the selector function.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TState> Get<TEnvironment, TOutput, TState>(Func<TState, TState> selector)
            => new GetReturnSelectCore<TEnvironment, TOutput, TState>(selector);

        private class GetReturnSelectAsyncCore<TEnvironment, TOutput, TState> : IRWSMonad<TEnvironment, TOutput, TState, TState>
        {
            private Func<TState, Task<TState>> _selector;
            public GetReturnSelectAsyncCore(Func<TState, Task<TState>> selector)
            {
                _selector = selector;
            }
            async Task<RWSResult<TOutput, TState, TState>> IRWSMonad<TEnvironment, TOutput, TState, TState>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector(state);
                cancellationToken.ThrowIfCancellationRequested();
                return new RWSResult<TOutput, TState, TState>(state, Array.Empty<TOutput>(), selectorResult);
            }
        }
        /// <summary>
        /// Creates an RWS monad that retrieves the current state and applies an asynchronous selector function to it.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="selector">An asynchronous function to apply to the current state.</param>
        /// <returns>An RWS monad that retrieves the current state and applies the asynchronous selector function.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TState> Get<TEnvironment, TOutput, TState>(Func<TState, Task<TState>> selector)
            => new GetReturnSelectAsyncCore<TEnvironment, TOutput, TState>(selector);
    }
}
