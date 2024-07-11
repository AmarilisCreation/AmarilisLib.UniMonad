using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Identity
    {
        private class SelectManyCore<T, TResult> : IIdentityMonad<TResult>
        {
            private IIdentityMonad<T> _self;
            private Func<T, IIdentityMonad<TResult>> _selector;
            public SelectManyCore(IIdentityMonad<T> self, Func<T, IIdentityMonad<TResult>> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<TResult> IIdentityMonad<TResult>.RunAsync(CancellationToken cancellationToken)
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
        /// Projects each element of a monad into a new form by incorporating the result of an asynchronous function.
        /// </summary>
        /// <typeparam name="T">The type of the source value.</typeparam>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <param name="self">The source monad.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An Identity monad whose elements are the result of invoking the transform function on each element of the source.</returns>
        public static IIdentityMonad<TResult> SelectMany<T, TResult>(this IIdentityMonad<T> self, Func<T, IIdentityMonad<TResult>> selector)
            => new SelectManyCore<T, TResult>(self, selector);

        private class SelectManyCore<TFirst, TSecond, TResult> : IIdentityMonad<TResult>
        {
            private IIdentityMonad<TFirst> _self;
            private Func<TFirst, IIdentityMonad<TSecond>> _selector;
            private Func<TFirst, TSecond, TResult> _projector;
            public SelectManyCore(IIdentityMonad<TFirst> self, Func<TFirst, IIdentityMonad<TSecond>> selector, Func<TFirst, TSecond, TResult> projector)
            {
                _self = self;
                _selector = selector;
                _projector = projector;
            }
            async Task<TResult> IIdentityMonad<TResult>.RunAsync(CancellationToken cancellationToken)
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
        /// Projects each element of a monad into a new form by incorporating the result of an asynchronous function.
        /// </summary>
        /// <typeparam name="TFirst">The type of the first source value.</typeparam>
        /// <typeparam name="TSecond">The type of the second source value.</typeparam>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <param name="self">The source monad.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <param name="projector">A function to combine elements.</param>
        /// <returns>An Identity monad whose elements are the result of invoking the transform function on each element of the source and combining them using the projector function.</returns>
        public static IIdentityMonad<TResult> SelectMany<TFirst, TSecond, TResult>(this IIdentityMonad<TFirst> self, Func<TFirst, IIdentityMonad<TSecond>> selector, Func<TFirst, TSecond, TResult> projector)
            => new SelectManyCore<TFirst, TSecond, TResult>(self, selector, projector);

        private class SelectManyAsynCore<T, TResult> : IIdentityMonad<TResult>
        {
            private IIdentityMonad<T> _self;
            private Func<T, Task<IIdentityMonad<TResult>>> _selector;
            public SelectManyAsynCore(IIdentityMonad<T> self, Func<T, Task<IIdentityMonad<TResult>>> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<TResult> IIdentityMonad<TResult>.RunAsync(CancellationToken cancellationToken)
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
        /// Projects each element of a monad into a new form by incorporating the result of an asynchronous function.
        /// </summary>
        /// <typeparam name="T">The type of the source value.</typeparam>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <param name="self">The source monad.</param>
        /// <param name="selector">An asynchronous transform function to apply to each element.</param>
        /// <returns>An Identity monad whose elements are the result of invoking the asynchronous transform function on each element of the source.</returns>
        public static IIdentityMonad<TResult> SelectMany<T, TResult>(this IIdentityMonad<T> self, Func<T, Task<IIdentityMonad<TResult>>> selector)
            => new SelectManyAsynCore<T, TResult>(self, selector);

        private class SelectManyAsyncCore<TFirst, TSecond, TResult> : IIdentityMonad<TResult>
        {
            private IIdentityMonad<TFirst> _self;
            private Func<TFirst, IIdentityMonad<TSecond>> _selector;
            private Func<TFirst, TSecond, Task<TResult>> _projector;
            public SelectManyAsyncCore(IIdentityMonad<TFirst> self, Func<TFirst, IIdentityMonad<TSecond>> selector, Func<TFirst, TSecond, Task<TResult>> projector)
            {
                _self = self;
                _selector = selector;
                _projector = projector;
            }
            async Task<TResult> IIdentityMonad<TResult>.RunAsync(CancellationToken cancellationToken)
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
        /// Projects each element of a monad into a new form by incorporating the result of an asynchronous function.
        /// </summary>
        /// <typeparam name="TFirst">The type of the first source value.</typeparam>
        /// <typeparam name="TSecond">The type of the second source value.</typeparam>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <param name="self">The source monad.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <param name="projector">An asynchronous function to combine elements.</param>
        /// <returns>An Identity monad whose elements are the result of invoking the transform function on each element of the source and combining them using the asynchronous projector function.</returns>
        public static IIdentityMonad<TResult> SelectMany<TFirst, TSecond, TResult>(this IIdentityMonad<TFirst> self, Func<TFirst, IIdentityMonad<TSecond>> selector, Func<TFirst, TSecond, Task<TResult>> projector)
            => new SelectManyAsyncCore<TFirst, TSecond, TResult>(self, selector, projector);
    }
}
