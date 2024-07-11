using System;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class State
    {
        private class IfCore<TState, TValue> : IStateMonad<TState, TValue>
        {
            private IStateMonad<TState, TValue> _self;
            private IStateMonad<TState, TValue> _elseSource;
            private Func<StateResult<TState, TValue>, bool> _selector;
            public IfCore(IStateMonad<TState, TValue> self, IStateMonad<TState, TValue> elseSource, Func<StateResult<TState, TValue>, bool> selector)
            {
                _self = self;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<StateResult<TState, TValue>> IStateMonad<TState, TValue>.RunAsync(TState state, System.Threading.CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(_selector(result)) return result;
                var elseResult = await _elseSource.RunAsync(state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return elseResult;
            }
        }
        /// <summary>
        /// Conditionally executes one of two state monads based on a selector function applied to the result of the first monad.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The initial state monad.</param>
        /// <param name="elseSource">The state monad to execute if the selector returns false.</param>
        /// <param name="selector">A function to determine which state monad to execute based on the result of the first monad.</param>
        /// <returns>A state monad that conditionally executes one of two state monads.</returns>
        public static IStateMonad<TState, TValue> If<TState, TValue>(this IStateMonad<TState, TValue> self, IStateMonad<TState, TValue> elseSource, Func<StateResult<TState, TValue>, bool> selector)
            => new IfCore<TState, TValue>(self, elseSource, selector);

        private class IfStaticCore<TState, TValue> : IStateMonad<TState, TValue>
        {
            private IStateMonad<TState, TValue> _thenSource;
            private IStateMonad<TState, TValue> _elseSource;
            private Func<bool> _selector;
            public IfStaticCore(IStateMonad<TState, TValue> thenSource, IStateMonad<TState, TValue> elseSource, Func<bool> selector)
            {
                _thenSource = thenSource;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<StateResult<TState, TValue>> IStateMonad<TState, TValue>.RunAsync(TState state, System.Threading.CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if(_selector())
                {
                    var thenResult = await _thenSource.RunAsync(state, cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return thenResult;
                }
                var elseResult = await _elseSource.RunAsync(state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return elseResult;
            }
        }
        /// <summary>
        /// Conditionally executes one of two state monads based on a static selector function.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="thenSource">The state monad to execute if the selector returns true.</param>
        /// <param name="elseSource">The state monad to execute if the selector returns false.</param>
        /// <param name="selector">A static function to determine which state monad to execute.</param>
        /// <returns>A state monad that conditionally executes one of two state monads.</returns>
        public static IStateMonad<TState, TValue> If<TState, TValue>(IStateMonad<TState, TValue> thenSource, IStateMonad<TState, TValue> elseSource, Func<bool> selector)
            => new IfStaticCore<TState, TValue>(thenSource, elseSource, selector);

        private class IfAsyncCore<TState, TValue> : IStateMonad<TState, TValue>
        {
            private IStateMonad<TState, TValue> _self;
            private IStateMonad<TState, TValue> _elseSource;
            private Func<StateResult<TState, TValue>, Task<bool>> _selector;
            public IfAsyncCore(IStateMonad<TState, TValue> self, IStateMonad<TState, TValue> elseSource, Func<StateResult<TState, TValue>, Task<bool>> selector)
            {
                _self = self;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<StateResult<TState, TValue>> IStateMonad<TState, TValue>.RunAsync(TState state, System.Threading.CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector(result);
                cancellationToken.ThrowIfCancellationRequested();
                if(selectorResult) return result;
                var elseResult = await _elseSource.RunAsync(state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return elseResult;
            }
        }
        /// <summary>
        /// Conditionally executes one of two state monads based on an asynchronous selector function applied to the result of the first monad.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The initial state monad.</param>
        /// <param name="elseSource">The state monad to execute if the selector returns false.</param>
        /// <param name="selector">An asynchronous function to determine which state monad to execute based on the result of the first monad.</param>
        /// <returns>A state monad that conditionally executes one of two state monads.</returns>
        public static IStateMonad<TState, TValue> If<TState, TValue>(this IStateMonad<TState, TValue> self, IStateMonad<TState, TValue> elseSource, Func<StateResult<TState, TValue>, Task<bool>> selector)
            => new IfAsyncCore<TState, TValue>(self, elseSource, selector);

        private class IfStaticAsyncCore<TState, TValue> : IStateMonad<TState, TValue>
        {
            private IStateMonad<TState, TValue> _thenSource;
            private IStateMonad<TState, TValue> _elseSource;
            private Task<Func<bool>> _selector;
            public IfStaticAsyncCore(IStateMonad<TState, TValue> thenSource, IStateMonad<TState, TValue> elseSource, Task<Func<bool>> selector)
            {
                _thenSource = thenSource;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<StateResult<TState, TValue>> IStateMonad<TState, TValue>.RunAsync(TState state, System.Threading.CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector;
                cancellationToken.ThrowIfCancellationRequested();
                if(selectorResult()) {
                    var thenResult =  await _thenSource.RunAsync(state, cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return thenResult;
                }
                var elseResult = await _elseSource.RunAsync(state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return elseResult;
            }
        }
        /// <summary>
        /// Conditionally executes one of two state monads based on an asynchronous selector function.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="thenSource">The state monad to execute if the selector returns true.</param>
        /// <param name="elseSource">The state monad to execute if the selector returns false.</param>
        /// <param name="selector">An asynchronous function to determine which state monad to execute.</param>
        /// <returns>A state monad that conditionally executes one of two state monads.</returns>
        public static IStateMonad<TState, TValue> If<TState, TValue>(IStateMonad<TState, TValue> thenSource, IStateMonad<TState, TValue> elseSource, Task<Func<bool>> selector)
            => new IfStaticAsyncCore<TState, TValue>(thenSource, elseSource, selector);
    }
}
