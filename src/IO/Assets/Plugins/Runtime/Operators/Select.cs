using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class IO
    {
        private class SelectCore<T, TResult> : IIOMonad<TResult>
        {
            private IIOMonad<T> _self;
            private Func<T, TResult> _selector;
            public SelectCore(IIOMonad<T> self, Func<T, TResult> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<TResult> IIOMonad<TResult>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return _selector(selfResult);
            }
        }
        /// <summary>
        /// Projects each element of a monad into a new form.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the source monad.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by the resulting monad.</typeparam>
        /// <param name="self">The source monad.</param>
        /// <param name="selector">A transform function to apply to each value of the source monad.</param>
        /// <returns>A monad whose values are the result of invoking the transform function on each value of the source monad.</returns>
        public static IIOMonad<TResult> Select<T, TResult>(this IIOMonad<T> self, Func<T, TResult> selector)
            => new SelectCore<T, TResult>(self, selector);

        private struct SelectAsyncCore<T, TResult> : IIOMonad<TResult>
        {
            private IIOMonad<T> _self;
            private Func<T, Task<TResult>> _selector;
            public SelectAsyncCore(IIOMonad<T> self, Func<T, Task<TResult>> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<TResult> IIOMonad<TResult>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var secondResult = await _selector(selfResult);
                cancellationToken.ThrowIfCancellationRequested();
                var result = await Return(secondResult).RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return result;
            }
        }
        /// <summary>
        /// Projects each element of a monad into a new form asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the source monad.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by the resulting monad.</typeparam>
        /// <param name="self">The source monad.</param>
        /// <param name="selector">An asynchronous transform function to apply to each value of the source monad.</param>
        /// <returns>A monad whose values are the result of invoking the asynchronous transform function on each value of the source monad.</returns>
        public static IIOMonad<TResult> Select<T, TResult>(this IIOMonad<T> self, Func<T, Task<TResult>> selector)
            => new SelectAsyncCore<T, TResult>(self, selector);
    }
}
