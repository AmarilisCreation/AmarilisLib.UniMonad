using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Reader
    {
        private class SelectManyCore<TEnvironment, TValue, TResult> : IReaderMonad<TEnvironment, TResult>
        {
            private IReaderMonad<TEnvironment, TValue> _self;
            private Func<TValue, IReaderMonad<TEnvironment, TResult>> _selector;
            public SelectManyCore(IReaderMonad<TEnvironment, TValue> self, Func<TValue, IReaderMonad<TEnvironment, TResult>> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<TResult> IReaderMonad<TEnvironment, TResult>.RunAsync(TEnvironment environment, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(environment, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _selector(selfResult).RunAsync(environment, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return result;
            }
        }
        /// <summary>
        /// Projects each value of the Reader monad into a new Reader monad and flattens the results into a single Reader monad.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="self">The Reader monad to project.</param>
        /// <param name="selector">A projection function to apply to each value.</param>
        /// <returns>A Reader monad whose values are the result of invoking the projection function on each value of the original Reader monad and flattening the resulting monads.</returns>
        public static IReaderMonad<TEnvironment, TResult> SelectMany<TEnvironment, TValue, TResult>(this IReaderMonad<TEnvironment, TValue> self, Func<TValue, IReaderMonad<TEnvironment, TResult>> selector)
            => new SelectManyCore<TEnvironment, TValue, TResult>(self, selector);

        private class SelectManyCore<TEnvironment, TFirst, TSecond, TResult> : IReaderMonad<TEnvironment, TResult>
        {
            private IReaderMonad<TEnvironment, TFirst> _self;
            private Func<TFirst, IReaderMonad<TEnvironment, TSecond>> _selector;
            private Func<TFirst, TSecond, TResult> _projector;
            public SelectManyCore(IReaderMonad<TEnvironment, TFirst> self, Func<TFirst, IReaderMonad<TEnvironment, TSecond>> selector, Func<TFirst, TSecond, TResult> projector)
            {
                _self = self;
                _selector = selector;
                _projector = projector;
            }
            async Task<TResult> IReaderMonad<TEnvironment, TResult>.RunAsync(TEnvironment environment, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var firstResult = await _self.RunAsync(environment, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var secondResult = await _selector(firstResult).RunAsync(environment, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return _projector(firstResult, secondResult);
            }
        }
        /// <summary>
        /// Projects each value of the Reader monad into a new Reader monad, flattens the results into a single Reader monad, and applies a result selector function to each value and intermediate projection result.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TFirst">The type of the first value.</typeparam>
        /// <typeparam name="TSecond">The type of the second value.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="self">The Reader monad to project.</param>
        /// <param name="selector">A projection function to apply to each value.</param>
        /// <param name="projector">A function to apply to each value and intermediate projection result.</param>
        /// <returns>A Reader monad whose values are the result of invoking the projection function and result selector function on each value of the original Reader monad and flattening the resulting monads.</returns>
        public static IReaderMonad<TEnvironment, TResult> SelectMany<TEnvironment, TFirst, TSecond, TResult>(this IReaderMonad<TEnvironment, TFirst> self, Func<TFirst, IReaderMonad<TEnvironment, TSecond>> selector, Func<TFirst, TSecond, TResult> projector)
            => new SelectManyCore<TEnvironment, TFirst, TSecond, TResult>(self, selector, projector);

        private class SelectManyAsyncCore<TEnvironment, TValue, TResult> : IReaderMonad<TEnvironment, TResult>
        {
            private IReaderMonad<TEnvironment, TValue> _self;
            private Func<TValue, Task<IReaderMonad<TEnvironment, TResult>>> _selector;
            public SelectManyAsyncCore(IReaderMonad<TEnvironment, TValue> self, Func<TValue, Task<IReaderMonad<TEnvironment, TResult>>> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<TResult> IReaderMonad<TEnvironment, TResult>.RunAsync(TEnvironment environment, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var firstResult = await _self.RunAsync(environment, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector(firstResult);
                cancellationToken.ThrowIfCancellationRequested();
                var result = await (selectorResult).RunAsync(environment, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return result;
            }
        }
        /// <summary>
        /// Projects each value of the Reader monad into a new Reader monad using an asynchronous projection function and flattens the results into a single Reader monad.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="self">The Reader monad to project.</param>
        /// <param name="selector">An asynchronous projection function to apply to each value.</param>
        /// <returns>A Reader monad whose values are the result of invoking the asynchronous projection function on each value of the original Reader monad and flattening the resulting monads.</returns>
        public static IReaderMonad<TEnvironment, TResult> SelectMany<TEnvironment, TValue, TResult>(this IReaderMonad<TEnvironment, TValue> self, Func<TValue, Task<IReaderMonad<TEnvironment, TResult>>> selector)
            => new SelectManyAsyncCore<TEnvironment, TValue, TResult>(self, selector);

        private class SelectManyAsyncCore<TEnvironment, TFirst, TSecond, TResult> : IReaderMonad<TEnvironment, TResult>
        {
            private IReaderMonad<TEnvironment, TFirst> _self;
            private Func<TFirst, IReaderMonad<TEnvironment, TSecond>> _selector;
            private Func<TFirst, TSecond, Task<TResult>> _projector;
            public SelectManyAsyncCore(IReaderMonad<TEnvironment, TFirst> self, Func<TFirst, IReaderMonad<TEnvironment, TSecond>> selector, Func<TFirst, TSecond, Task<TResult>> projector)
            {
                _self = self;
                _selector = selector;
                _projector = projector;
            }
            async Task<TResult> IReaderMonad<TEnvironment, TResult>.RunAsync(TEnvironment environment, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var firstResult = await _self.RunAsync(environment, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var secondResult = await _selector(firstResult).RunAsync(environment, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return await _projector(firstResult, secondResult);
            }
        }
        /// <summary>
        /// Projects each value of the Reader monad into a new Reader monad, flattens the results into a single Reader monad, and applies an asynchronous result selector function to each value and intermediate projection result.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TFirst">The type of the first value.</typeparam>
        /// <typeparam name="TSecond">The type of the second value.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="self">The Reader monad to project.</param>
        /// <param name="selector">A projection function to apply to each value.</param>
        /// <param name="projector">An asynchronous function to apply to each value and intermediate projection result.</param>
        /// <returns>A Reader monad whose values are the result of invoking the projection function and asynchronous result selector function on each value of the original Reader monad and flattening the resulting monads.</returns>
        public static IReaderMonad<TEnvironment, TResult> SelectMany<TEnvironment, TFirst, TSecond, TResult>(this IReaderMonad<TEnvironment, TFirst> self, Func<TFirst, IReaderMonad<TEnvironment, TSecond>> selector, Func<TFirst, TSecond, Task<TResult>> projector)
            => new SelectManyAsyncCore<TEnvironment, TFirst, TSecond, TResult>(self, selector, projector);
    }
}
