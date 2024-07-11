using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class IO
    {
        private class SelectManyCore<T, TResult> : IIOMonad<TResult>
        {
            private IIOMonad<T> _self;
            private Func<T, IIOMonad<TResult>> _selector;
            public SelectManyCore(IIOMonad<T> self, Func<T, IIOMonad<TResult>> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<TResult> IIOMonad<TResult>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _selector(selfResult).RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return result;
            }
        }
        /// <summary>
        /// Projects each element of a monad to another monad and flattens the resulting monads into a single monad.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the first monad.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by the resulting monad.</typeparam>
        /// <param name="self">The first monad.</param>
        /// <param name="selector">A transform function to apply to each value of the first monad.</param>
        /// <returns>A monad whose values are the result of invoking the transform function on each value of the first monad.</returns>
        public static IIOMonad<TResult> SelectMany<T, TResult>(this IIOMonad<T> self, Func<T, IIOMonad<TResult>> selector)
            => new SelectManyCore<T, TResult>(self, selector);

        private class SelectManyCore<TFirst, TSecond, TResult> : IIOMonad<TResult>
        {
            private IIOMonad<TFirst> _self;
            private Func<TFirst, IIOMonad<TSecond>> _selector;
            private Func<TFirst, TSecond, TResult> _projector;
            public SelectManyCore(IIOMonad<TFirst> self, Func<TFirst, IIOMonad<TSecond>> selector, Func<TFirst, TSecond, TResult> projector)
            {
                _self = self;
                _selector = selector;
                _projector = projector;
            }
            async Task<TResult> IIOMonad<TResult>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var secondResult = await _selector(selfResult).RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return _projector(selfResult, secondResult);
            }
        }
        /// <summary>
        /// Projects each element of a monad to another monad, flattens the resulting monads into a single monad, and applies a result selector function to the final result.
        /// </summary>
        /// <typeparam name="TFirst">The type of the value returned by the first monad.</typeparam>
        /// <typeparam name="TSecond">The type of the value returned by the second monad.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by the resulting monad.</typeparam>
        /// <param name="self">The first monad.</param>
        /// <param name="selector">A transform function to apply to each value of the first monad.</param>
        /// <param name="projector">A transform function to apply to the values of the second monad and the result of the first transform function.</param>
        /// <returns>A monad whose values are the result of invoking the transform functions on each value of the first monad.</returns>
        public static IIOMonad<TResult> SelectMany<TFirst, TSecond, TResult>(this IIOMonad<TFirst> self, Func<TFirst, IIOMonad<TSecond>> selector, Func<TFirst, TSecond, TResult> projector)
            => new SelectManyCore<TFirst, TSecond, TResult>(self, selector, projector);

        private class SelectManyAsyncCore<T, TResult> : IIOMonad<TResult>
        {
            private IIOMonad<T> _self;
            private Func<T, Task<IIOMonad<TResult>>> _selector;
            public SelectManyAsyncCore(IIOMonad<T> self, Func<T, Task<IIOMonad<TResult>>> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<TResult> IIOMonad<TResult>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector(selfResult);
                cancellationToken.ThrowIfCancellationRequested();
                var result = await selectorResult.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return result;
            }
        }
        /// <summary>
        /// Projects each element of a monad to an asynchronously-created monad and flattens the resulting monads into a single monad.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the first monad.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by the resulting monad.</typeparam>
        /// <param name="self">The first monad.</param>
        /// <param name="selector">A transform function to apply to each value of the first monad to asynchronously create a monad.</param>
        /// <returns>A monad whose values are the result of invoking the transform function on each value of the first monad.</returns>
        public static IIOMonad<TResult> SelectMany<T, TResult>(this IIOMonad<T> self, Func<T, Task<IIOMonad<TResult>>> selector)
            => new SelectManyAsyncCore<T, TResult>(self, selector);

        private class SelectManyAsyncCore<TFirst, TSecond, TResult> : IIOMonad<TResult>
        {
            private IIOMonad<TFirst> _self;
            private Func<TFirst, IIOMonad<TSecond>> _selector;
            private Func<TFirst, TSecond, Task<TResult>> _projector;
            public SelectManyAsyncCore(IIOMonad<TFirst> self, Func<TFirst, IIOMonad<TSecond>> selector, Func<TFirst, TSecond, Task<TResult>> projector)
            {
                _self = self;
                _selector = selector;
                _projector = projector;
            }
            async Task<TResult> IIOMonad<TResult>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var secondResult = await _selector(selfResult).RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _projector(selfResult, secondResult);
                cancellationToken.ThrowIfCancellationRequested();
                return result;
            }
        }
        /// <summary>
        /// Projects each element of a monad to another monad, flattens the resulting monads into a single monad, and applies an asynchronous result selector function to the final result.
        /// </summary>
        /// <typeparam name="TFirst">The type of the value returned by the first monad.</typeparam>
        /// <typeparam name="TSecond">The type of the value returned by the second monad.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by the resulting monad.</typeparam>
        /// <param name="self">The first monad.</param>
        /// <param name="selector">A transform function to apply to each value of the first monad.</param>
        /// <param name="projector">An asynchronous transform function to apply to the values of the second monad and the result of the first transform function.</param>
        /// <returns>A monad whose values are the result of invoking the transform functions on each value of the first monad.</returns>
        public static IIOMonad<TResult> SelectMany<TFirst, TSecond, TResult>(this IIOMonad<TFirst> self, Func<TFirst, IIOMonad<TSecond>> selector, Func<TFirst, TSecond, Task<TResult>> projector)
            => new SelectManyAsyncCore<TFirst, TSecond, TResult>(self, selector, projector);
    }
}
