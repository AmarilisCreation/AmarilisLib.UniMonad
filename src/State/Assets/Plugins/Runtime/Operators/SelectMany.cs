using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class State
    {
        private class SelectManyCore<TState, TValue, TResult> : IStateMonad<TState, TResult>
        {
            private IStateMonad<TState, TValue> _self;
            private Func<TValue, IStateMonad<TState, TResult>> _selector;
            public SelectManyCore(IStateMonad<TState, TValue> self, Func<TValue, IStateMonad<TState, TResult>> selector)
            {
                _self = self;
                _selector = selector;
            }

            async Task<StateResult<TState, TResult>> IStateMonad<TState, TResult>.RunAsync(TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector(selfResult.Value).RunAsync(selfResult.State, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return selectorResult;
            }
        }
        /// <summary>
        /// Projects each value of the state monad into a new monad, and flattens the result.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value in the original monad.</typeparam>
        /// <typeparam name="TResult">The type of the value in the resulting monad.</typeparam>
        /// <param name="self">The original state monad.</param>
        /// <param name="selector">A function to apply to each value of the original monad.</param>
        /// <returns>A state monad with the result of applying the selector and flattening the result.</returns>
        public static IStateMonad<TState, TResult> SelectMany<TState, TValue, TResult>(this IStateMonad<TState, TValue> self, Func<TValue, IStateMonad<TState, TResult>> selector)
            => new SelectManyCore<TState, TValue, TResult>(self, selector);

        private class SelectManyCore<TState, TFirst, TSecond, TResult> : IStateMonad<TState, TResult>
        {
            private IStateMonad<TState, TFirst> _self;
            private Func<TFirst, IStateMonad<TState, TSecond>> _selector;
            private Func<TFirst, TSecond, TResult> _projector;
            public SelectManyCore(IStateMonad<TState, TFirst> self, Func<TFirst, IStateMonad<TState, TSecond>> selector, Func<TFirst, TSecond, TResult> projector)
            {
                _self = self;
                _selector = selector;
                _projector = projector;
            }

            async Task<StateResult<TState, TResult>> IStateMonad<TState, TResult>.RunAsync(TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var firstResult = await _self.RunAsync(state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var secondResult = await _selector(firstResult.Value).RunAsync(firstResult.State, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return new StateResult<TState, TResult>(secondResult.State, _projector(firstResult.Value, secondResult.Value));
            }
        }
        /// <summary>
        /// Projects each value of the state monad into a new monad, flattens the result, and applies a result selector function.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TFirst">The type of the first value.</typeparam>
        /// <typeparam name="TSecond">The type of the second value.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="self">The original state monad.</param>
        /// <param name="selector">A function to apply to each value of the original monad.</param>
        /// <param name="projector">A function to combine the values.</param>
        /// <returns>A state monad with the result of applying the selector and projector functions.</returns>
        public static IStateMonad<TState, TResult> SelectMany<TState, TFirst, TSecond, TResult>(this IStateMonad<TState, TFirst> self, Func<TFirst, IStateMonad<TState, TSecond>> selector, Func<TFirst, TSecond, TResult> projector)
            => new SelectManyCore<TState, TFirst, TSecond, TResult>(self, selector, projector);


        private class SelectManyAsyncCore<TState, TValue, TResult> : IStateMonad<TState, TResult>
        {
            private IStateMonad<TState, TValue> _self;
            private Func<TValue, Task<IStateMonad<TState, TResult>>> _selector;
            public SelectManyAsyncCore(IStateMonad<TState, TValue> self, Func<TValue, Task<IStateMonad<TState, TResult>>> selector)
            {
                _self = self;
                _selector = selector;
            }

            async Task<StateResult<TState, TResult>> IStateMonad<TState, TResult>.RunAsync(TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector(selfResult.Value);
                cancellationToken.ThrowIfCancellationRequested();
                var result = await (selectorResult).RunAsync(selfResult.State, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return result;
            }
        }
        /// <summary>
        /// Asynchronously projects each value of the state monad into a new monad, and flattens the result.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value in the original monad.</typeparam>
        /// <typeparam name="TResult">The type of the value in the resulting monad.</typeparam>
        /// <param name="self">The original state monad.</param>
        /// <param name="selector">A function to asynchronously apply to each value of the original monad.</param>
        /// <returns>A state monad with the result of applying the selector and flattening the result.</returns>
        public static IStateMonad<TState, TResult> SelectMany<TState, TValue, TResult>(this IStateMonad<TState, TValue> self, Func<TValue, Task<IStateMonad<TState, TResult>>> selector)
            => new SelectManyAsyncCore<TState, TValue, TResult>(self, selector);

        private class SelectManyAsyncCore<TState, TFirst, TSecond, TResult> : IStateMonad<TState, TResult>
        {
            private IStateMonad<TState, TFirst> _self;
            private Func<TFirst, IStateMonad<TState, TSecond>> _selector;
            private Func<TFirst, TSecond, Task<TResult>> _projector;
            public SelectManyAsyncCore(IStateMonad<TState, TFirst> self, Func<TFirst, IStateMonad<TState, TSecond>> selector, Func<TFirst, TSecond, Task<TResult>> projector)
            {
                _self = self;
                _selector = selector;
                _projector = projector;
            }

            async Task<StateResult<TState, TResult>> IStateMonad<TState, TResult>.RunAsync(TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var firstResult = await _self.RunAsync(state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var secondResult = await _selector(firstResult.Value).RunAsync(firstResult.State, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var projectorResult = await _projector(firstResult.Value, secondResult.Value);
                return new StateResult<TState, TResult>(secondResult.State, projectorResult);
            }
        }
        /// <summary>
        /// Asynchronously projects each value of the state monad into a new monad, flattens the result, and applies a result selector function.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TFirst">The type of the first value.</typeparam>
        /// <typeparam name="TSecond">The type of the second value.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="self">The original state monad.</param>
        /// <param name="selector">A function to apply to each value of the original monad.</param>
        /// <param name="projector">A function to combine the values asynchronously.</param>
        /// <returns>A state monad with the result of applying the selector and projector functions.</returns>
        public static IStateMonad<TState, TResult> SelectMany<TState, TFirst, TSecond, TResult>(this IStateMonad<TState, TFirst> self, Func<TFirst, IStateMonad<TState, TSecond>> selector, Func<TFirst, TSecond, Task<TResult>> projector)
            => new SelectManyAsyncCore<TState, TFirst, TSecond, TResult>(self, selector, projector);
    }
}
