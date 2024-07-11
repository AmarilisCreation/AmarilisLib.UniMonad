using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Identity
    {
        private class SelectCore<T, TResult> : IIdentityMonad<TResult>
        {
            private IIdentityMonad<T> _self;
            private Func<T, TResult> _selector;
            public SelectCore(IIdentityMonad<T> self, Func<T, TResult> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<TResult> IIdentityMonad<TResult>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return _selector(selfResult);
            }
        }
        /// <summary>
        /// Projects each element of a monad into a new form by applying the specified selector function.
        /// </summary>
        /// <typeparam name="T">The type of the source value.</typeparam>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <param name="self">The source monad.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An Identity monad whose elements are the result of invoking the transform function on each element of the source.</returns>
        public static IIdentityMonad<TResult> Select<T, TResult>(this IIdentityMonad<T> self, Func<T, TResult> selector)
            => new SelectCore<T, TResult>(self, selector);

        private class SelectAsyncCore<T, TResult> : IIdentityMonad<TResult>
        {
            private IIdentityMonad<T> _self;
            private Func<T, Task<TResult>> _selector;
            public SelectAsyncCore(IIdentityMonad<T> self, Func<T, Task<TResult>> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<TResult> IIdentityMonad<TResult>.RunAsync(CancellationToken cancellationToken)
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
        /// Projects each element of a monad into a new form by applying the specified asynchronous selector function.
        /// </summary>
        /// <typeparam name="T">The type of the source value.</typeparam>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <param name="self">The source monad.</param>
        /// <param name="selector">An asynchronous transform function to apply to each element.</param>
        /// <returns>An Identity monad whose elements are the result of invoking the asynchronous transform function on each element of the source.</returns>
        public static IIdentityMonad<TResult> Select<T, TResult>(this IIdentityMonad<T> self, Func<T, Task<TResult>> selector)
            => new SelectAsyncCore<T, TResult>(self, selector);
    }
}
