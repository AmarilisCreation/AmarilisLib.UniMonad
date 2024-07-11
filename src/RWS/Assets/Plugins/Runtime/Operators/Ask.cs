using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class RWS
    {
        private class AskReturnNoArgumentCore<TEnvironment, TOutput, TState> : IRWSMonad<TEnvironment, TOutput, TState, TEnvironment>
        {
            Task<RWSResult<TOutput, TState, TEnvironment>> IRWSMonad<TEnvironment, TOutput, TState, TEnvironment>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                return Task.FromResult(new RWSResult<TOutput, TState, TEnvironment>(environment, Array.Empty<TOutput>(), state));
            }
        }
        /// <summary>
        /// Creates an RWS monad that retrieves the current environment.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <returns>An RWS monad that retrieves the current environment.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TEnvironment> Ask<TEnvironment, TOutput, TState>()
            => new AskReturnNoArgumentCore<TEnvironment, TOutput, TState>();

        private class AskReturnSelectCore<TEnvironment, TOutput, TState> : IRWSMonad<TEnvironment, TOutput, TState, TEnvironment>
        {
            private Func<TEnvironment, TEnvironment> _selector;
            public AskReturnSelectCore(Func<TEnvironment, TEnvironment> selector)
            {
                _selector = selector;
            }
            Task<RWSResult<TOutput, TState, TEnvironment>> IRWSMonad<TEnvironment, TOutput, TState, TEnvironment>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(new RWSResult<TOutput, TState, TEnvironment>(_selector(environment), Array.Empty<TOutput>(), state));
            }
        }
        /// <summary>
        /// Creates an RWS monad that retrieves the current environment and applies a selector function to it.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="selector">A function to apply to the current environment.</param>
        /// <returns>An RWS monad that retrieves the current environment and applies the selector function.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TEnvironment> Ask<TEnvironment, TOutput, TState>(Func<TEnvironment, TEnvironment> selector)
            => new AskReturnSelectCore<TEnvironment, TOutput, TState>(selector);

        private class AskCore<TEnvironment, TOutput, TState, TValue> : IRWSMonad<TEnvironment, TOutput, TState, TEnvironment>
        {
            public AskCore(IRWSMonad<TEnvironment, TOutput, TState, TValue> self)
            {

            }
            Task<RWSResult<TOutput, TState, TEnvironment>> IRWSMonad<TEnvironment, TOutput, TState, TEnvironment>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(new RWSResult<TOutput, TState, TEnvironment>(environment, Array.Empty<TOutput>(), state));
            }
        }
        /// <summary>
        /// Creates an RWS monad that retrieves the current environment for the given monad.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The RWS monad whose environment to retrieve.</param>
        /// <returns>An RWS monad that retrieves the current environment.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TEnvironment> Ask<TEnvironment, TOutput, TState, TValue>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> self)
            => new AskCore<TEnvironment, TOutput, TState, TValue>(self);

        private class AskSelectCore<TEnvironment, TOutput, TState, TValue> : IRWSMonad<TEnvironment, TOutput, TState, TEnvironment>
        {
            private Func<TEnvironment, TEnvironment> _selector;
            public AskSelectCore(IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Func<TEnvironment, TEnvironment> selector)
            {
                _selector = selector;
            }
            Task<RWSResult<TOutput, TState, TEnvironment>> IRWSMonad<TEnvironment, TOutput, TState, TEnvironment>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(new RWSResult<TOutput, TState, TEnvironment>(_selector(environment), Array.Empty<TOutput>(), state));
            }
        }
        /// <summary>
        /// Creates an RWS monad that retrieves the current environment for the given monad and applies a selector function to it.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The RWS monad whose environment to retrieve.</param>
        /// <param name="selector">A function to apply to the current environment.</param>
        /// <returns>An RWS monad that retrieves the current environment and applies the selector function.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TEnvironment> Ask<TEnvironment, TOutput, TState, TValue>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Func<TEnvironment, TEnvironment> selector)
            => new AskSelectCore<TEnvironment, TOutput, TState, TValue>(self, selector);

        private class AskReturnSelectAsyncCore<TEnvironment, TOutput, TState> : IRWSMonad<TEnvironment, TOutput, TState, TEnvironment>
        {
            private Func<TEnvironment, Task<TEnvironment>> _selector;
            public AskReturnSelectAsyncCore(Func<TEnvironment, Task<TEnvironment>> selector)
            {
                _selector = selector;
            }
            async Task<RWSResult<TOutput, TState, TEnvironment>> IRWSMonad<TEnvironment, TOutput, TState, TEnvironment>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector(environment);
                cancellationToken.ThrowIfCancellationRequested();
                return new RWSResult<TOutput, TState, TEnvironment>(selectorResult, Array.Empty<TOutput>(), state);
            }
        }
        /// <summary>
        /// Creates an RWS monad that retrieves the current environment and applies an asynchronous selector function to it.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="selector">An asynchronous function to apply to the current environment.</param>
        /// <returns>An RWS monad that retrieves the current environment and applies the asynchronous selector function.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TEnvironment> Ask<TEnvironment, TOutput, TState>(Func<TEnvironment, Task<TEnvironment>> selector)
            => new AskReturnSelectAsyncCore<TEnvironment, TOutput, TState>(selector);

        private class AskSelectAsyncCore<TEnvironment, TOutput, TState, TValue> : IRWSMonad<TEnvironment, TOutput, TState, TEnvironment>
        {
            private Func<TEnvironment, Task<TEnvironment>> _selector;
            public AskSelectAsyncCore(IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Func<TEnvironment, Task<TEnvironment>> selector)
            {
                _selector = selector;
            }
            async Task<RWSResult<TOutput, TState, TEnvironment>> IRWSMonad<TEnvironment, TOutput, TState, TEnvironment>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector(environment);
                cancellationToken.ThrowIfCancellationRequested();
                return new RWSResult<TOutput, TState, TEnvironment>(selectorResult, Array.Empty<TOutput>(), state);
            }
        }
        /// <summary>
        /// Creates an RWS monad that retrieves the current environment for the given monad and applies an asynchronous selector function to it.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The RWS monad whose environment to retrieve.</param>
        /// <param name="selector">An asynchronous function to apply to the current environment.</param>
        /// <returns>An RWS monad that retrieves the current environment and applies the asynchronous selector function.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TEnvironment> Ask<TEnvironment, TOutput, TState, TValue>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Func<TEnvironment, Task<TEnvironment>> selector)
            => new AskSelectAsyncCore<TEnvironment, TOutput, TState, TValue>(self, selector);
    }
}
