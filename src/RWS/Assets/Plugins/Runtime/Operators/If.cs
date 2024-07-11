using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class RWS
    {
        private class IfCore<TEnvironment, TOutput, TState, TValue> : IRWSMonad<TEnvironment, TOutput, TState, TValue>
        {
            private IRWSMonad<TEnvironment, TOutput, TState, TValue> _self;
            private IRWSMonad<TEnvironment, TOutput, TState, TValue> _elseSource;
            private Func<RWSResult<TOutput, TState, TValue>, bool> _selector;
            public IfCore(IRWSMonad<TEnvironment, TOutput, TState, TValue> self, IRWSMonad<TEnvironment, TOutput, TState, TValue> elseSource, Func<RWSResult<TOutput, TState, TValue>, bool> selector)
            {
                _self = self;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<RWSResult<TOutput, TState, TValue>> IRWSMonad<TEnvironment, TOutput, TState, TValue>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(environment, state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(_selector(result))
                {
                    return result;
                }
                return await _elseSource.RunAsync(environment, state, cancellationToken);
            }
        }
        /// <summary>
        /// Conditionally executes one of two RWS monads based on the result of a selector function.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The RWS monad to execute if the selector returns true.</param>
        /// <param name="elseSource">The RWS monad to execute if the selector returns false.</param>
        /// <param name="selector">A function that determines which monad to execute based on the result of the self monad.</param>
        /// <returns>An RWS monad that conditionally executes one of the two specified monads.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TValue> If<TEnvironment, TOutput, TState, TValue>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> self, IRWSMonad<TEnvironment, TOutput, TState, TValue> elseSource, Func<RWSResult<TOutput, TState, TValue>, bool> selector)
            => new IfCore<TEnvironment, TOutput, TState, TValue>(self, elseSource, selector);

        private class IfStaticCore<TEnvironment, TOutput, TState, TValue> : IRWSMonad<TEnvironment, TOutput, TState, TValue>
        {
            private IRWSMonad<TEnvironment, TOutput, TState, TValue> _thenSource;
            private IRWSMonad<TEnvironment, TOutput, TState, TValue> _elseSource;
            private Func<bool> _selector;
            public IfStaticCore(IRWSMonad<TEnvironment, TOutput, TState, TValue> thenSource, IRWSMonad<TEnvironment, TOutput, TState, TValue> elseSource, Func<bool> selector)
            {
                _thenSource = thenSource;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<RWSResult<TOutput, TState, TValue>> IRWSMonad<TEnvironment, TOutput, TState, TValue>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if(_selector())
                {
                    var thenResult = await _thenSource.RunAsync(environment, state, cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return thenResult;
                }
                var elseResult = await _elseSource.RunAsync(environment, state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return elseResult;
            }
        }
        /// <summary>
        /// Conditionally executes one of two RWS monads based on the result of a static selector function.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="thenSource">The RWS monad to execute if the selector returns true.</param>
        /// <param name="elseSource">The RWS monad to execute if the selector returns false.</param>
        /// <param name="selector">A static function that determines which monad to execute.</param>
        /// <returns>An RWS monad that conditionally executes one of the two specified monads.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TValue> If<TEnvironment, TOutput, TState, TValue>(IRWSMonad<TEnvironment, TOutput, TState, TValue> thenSource, IRWSMonad<TEnvironment, TOutput, TState, TValue> elseSource, Func<bool> selector)
            => new IfStaticCore<TEnvironment, TOutput, TState, TValue>(thenSource, elseSource, selector);

        private class IfAsyncCore<TEnvironment, TOutput, TState, TValue> : IRWSMonad<TEnvironment, TOutput, TState, TValue>
        {
            private IRWSMonad<TEnvironment, TOutput, TState, TValue> _self;
            private IRWSMonad<TEnvironment, TOutput, TState, TValue> _elseSource;
            private Func<RWSResult<TOutput, TState, TValue>, Task<bool>> _selector;
            public IfAsyncCore(IRWSMonad<TEnvironment, TOutput, TState, TValue> self, IRWSMonad<TEnvironment, TOutput, TState, TValue> elseSource, Func<RWSResult<TOutput, TState, TValue>, Task<bool>> selector)
            {
                _self = self;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<RWSResult<TOutput, TState, TValue>> IRWSMonad<TEnvironment, TOutput, TState, TValue>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(environment, state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector(result);
                if(selectorResult) return result;
                var elseResult = await _elseSource.RunAsync(environment, state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return elseResult;
            }
        }
        /// <summary>
        /// Conditionally executes one of two RWS monads based on the result of an asynchronous selector function.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The RWS monad to execute if the selector returns true.</param>
        /// <param name="elseSource">The RWS monad to execute if the selector returns false.</param>
        /// <param name="selector">An asynchronous function that determines which monad to execute based on the result of the self monad.</param>
        /// <returns>An RWS monad that conditionally executes one of the two specified monads.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TValue> If<TEnvironment, TOutput, TState, TValue>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> self, IRWSMonad<TEnvironment, TOutput, TState, TValue> elseSource, Func<RWSResult<TOutput, TState, TValue>, Task<bool>> selector)
            => new IfAsyncCore<TEnvironment, TOutput, TState, TValue>(self, elseSource, selector);

        private class IfStaticAsyncCore<TEnvironment, TOutput, TState, TValue> : IRWSMonad<TEnvironment, TOutput, TState, TValue>
        {
            private IRWSMonad<TEnvironment, TOutput, TState, TValue> _thenSource;
            private IRWSMonad<TEnvironment, TOutput, TState, TValue> _elseSource;
            private Task<Func<bool>> _selector;
            public IfStaticAsyncCore(IRWSMonad<TEnvironment, TOutput, TState, TValue> thenSource, IRWSMonad<TEnvironment, TOutput, TState, TValue> elseSource, Task<Func<bool>> selector)
            {
                _thenSource = thenSource;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<RWSResult<TOutput, TState, TValue>> IRWSMonad<TEnvironment, TOutput, TState, TValue>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector;
                cancellationToken.ThrowIfCancellationRequested();
                if(selectorResult()) {
                    var thenResult = await _thenSource.RunAsync(environment, state, cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return thenResult;
                }
                var elseResult = await _elseSource.RunAsync(environment, state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return elseResult;
            }
        }
        /// <summary>
        /// Conditionally executes one of two RWS monads based on the result of a static asynchronous selector function.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="thenSource">The RWS monad to execute if the selector returns true.</param>
        /// <param name="elseSource">The RWS monad to execute if the selector returns false.</param>
        /// <param name="selector">A static asynchronous function that determines which monad to execute.</param>
        /// <returns>An RWS monad that conditionally executes one of the two specified monads.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TValue> If<TEnvironment, TOutput, TState, TValue>(IRWSMonad<TEnvironment, TOutput, TState, TValue> thenSource, IRWSMonad<TEnvironment, TOutput, TState, TValue> elseSource, Task<Func<bool>> selector)
            => new IfStaticAsyncCore<TEnvironment, TOutput, TState, TValue>(thenSource, elseSource, selector);
    }
}
