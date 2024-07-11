using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class State
    {
        private class SelectCore<TState, TValue, TResult> : IStateMonad<TState, TResult>
        {
            private IStateMonad<TState, TValue> _self;
            private Func<TValue, TResult> _selector;
            public SelectCore(IStateMonad<TState, TValue> self, Func<TValue, TResult> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<StateResult<TState, TResult>> IStateMonad<TState, TResult>.RunAsync(TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return new StateResult<TState, TResult>(result.State, _selector(result.Value));
            }
        }
        /// <summary>
        /// Projects each value of the state monad into a new form using the provided synchronous selector function.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value in the original monad.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="self">The original state monad.</param>
        /// <param name="selector">A function to apply to each value of the original monad.</param>
        /// <returns>A state monad with the result of applying the selector function.</returns>
        public static IStateMonad<TState, TResult> Select<TState, TValue, TResult>(this IStateMonad<TState, TValue> self, Func<TValue, TResult> selector)
            => new SelectCore<TState, TValue, TResult>(self, selector);

        private class SelectAsyncCore<TState, TValue, TResult> : IStateMonad<TState, TResult>
        {
            private IStateMonad<TState, TValue> _self;
            private Func<TValue, Task<TResult>> _selector;
            public SelectAsyncCore(IStateMonad<TState, TValue> self, Func<TValue, Task<TResult>> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<StateResult<TState, TResult>> IStateMonad<TState, TResult>.RunAsync(TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector(result.Value);
                return new StateResult<TState, TResult>(result.State, selectorResult);
            }
        }
        /// <summary>
        /// Projects each value of the state monad into a new form using the provided asynchronous selector function.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value in the original monad.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="self">The original state monad.</param>
        /// <param name="selector">A function to apply to each value of the original monad.</param>
        /// <returns>A state monad with the result of applying the selector function.</returns>
        public static IStateMonad<TState, TResult> Select<TState, TValue, TResult>(this IStateMonad<TState, TValue> self, Func<TValue, Task<TResult>> selector)
            => new SelectAsyncCore<TState, TValue, TResult>(self, selector);
    }
}
