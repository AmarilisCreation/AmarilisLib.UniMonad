using System;
using System.Threading.Tasks;
using System.Threading;

namespace AmarilisLib.Monad
{
    public static partial class Try
    {
        private class SelectCore<T, TResult> : ITryMonad<TResult>
        {
            ITryMonad<T> _self;
            Func<T, TResult> _selector;
            public SelectCore(ITryMonad<T> self, Func<T, TResult> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<TryResult<TResult>> ITryMonad<TResult>.RunAsync(CancellationToken cancellationToken)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var selfResult = await _self.RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    if(selfResult.IsFaulted) return TryResult<TResult>.Failure(selfResult.Exception);
                    var resultValue = _selector(selfResult.Value);
                    return TryResult<TResult>.Success(resultValue);
                }
                catch(Exception exception)
                {
                    return TryResult<TResult>.Failure(exception);
                }
            }
        }
        /// <summary>
        /// Projects each element of a try monad into a new form.
        /// </summary>
        /// <typeparam name="T">The type of the value in the initial monad.</typeparam>
        /// <typeparam name="TResult">The type of the value in the resulting monad.</typeparam>
        /// <param name="self">The try monad to project.</param>
        /// <param name="selector">A transform function to apply to each value.</param>
        /// <returns>A try monad whose elements are the result of invoking the transform function on each element of the source.</returns>
        public static ITryMonad<TResult> Select<T, TResult>(this ITryMonad<T> self, Func<T, TResult> selector)
            => new SelectCore<T, TResult>(self, selector);
        private class SelectAsyncCore<T, TResult> : ITryMonad<TResult>
        {
            ITryMonad<T> _self;
            Func<T, Task<TResult>> _selector;
            public SelectAsyncCore(ITryMonad<T> self, Func<T, Task<TResult>> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<TryResult<TResult>> ITryMonad<TResult>.RunAsync(CancellationToken cancellationToken)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var selfResult = await _self.RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    if(selfResult.IsFaulted) return TryResult<TResult>.Failure(selfResult.Exception);
                    cancellationToken.ThrowIfCancellationRequested();
                    var resultValue = await _selector(selfResult.Value);
                    cancellationToken.ThrowIfCancellationRequested();
                    return TryResult<TResult>.Success(resultValue);
                }
                catch(Exception exception)
                {
                    return TryResult<TResult>.Failure(exception);
                }
            }
        }
        /// <summary>
        /// Projects each element of a try monad into a new form asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the value in the initial monad.</typeparam>
        /// <typeparam name="TResult">The type of the value in the resulting monad.</typeparam>
        /// <param name="self">The try monad to project.</param>
        /// <param name="selector">A transform function to apply to each value asynchronously.</param>
        /// <returns>A try monad whose elements are the result of invoking the asynchronous transform function on each element of the source.</returns>
        public static ITryMonad<TResult> Select<T, TResult>(this ITryMonad<T> self, Func<T, Task<TResult>> selector)
            => new SelectAsyncCore<T, TResult>(self, selector);
    }
}