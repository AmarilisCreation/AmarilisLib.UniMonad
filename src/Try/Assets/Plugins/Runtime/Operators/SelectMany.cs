using System;
using System.Threading.Tasks;
using System.Threading;

namespace AmarilisLib.Monad
{
    public static partial class Try
    {
        private class SelectManyCore<T, TResult> : ITryMonad<TResult>
        {
            ITryMonad<T> _self;
            Func<T, ITryMonad<TResult>> _selector;
            public SelectManyCore(ITryMonad<T> self, Func<T, ITryMonad<TResult>> selector)
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
                    var resultValue = await _selector(selfResult.Value).RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return TryResult<TResult>.Success(resultValue.Value);
                }
                catch(Exception exception)
                {
                    return TryResult<TResult>.Failure(exception);
                }
            }
        }
        /// <summary>
        /// Projects each element of a try monad into a new form and flattens the resulting monads into one monad.
        /// </summary>
        /// <typeparam name="T">The type of the value in the initial monad.</typeparam>
        /// <typeparam name="TResult">The type of the value in the resulting monad.</typeparam>
        /// <param name="self">The try monad to project and flatten.</param>
        /// <param name="selector">A transform function to apply to each value.</param>
        /// <returns>A try monad whose elements are the result of invoking the transform function on each element of the source.</returns>
        public static ITryMonad<TResult> SelectMany<T, TResult>(this ITryMonad<T> self, Func<T, ITryMonad<TResult>> selector)
            => new SelectManyCore<T, TResult>(self, selector);

        private class SelectManyCore<TFirst, TSecond, TResult> : ITryMonad<TResult>
        {
            ITryMonad<TFirst> _self;
            Func<TFirst, ITryMonad<TSecond>> _selector;
            Func<TFirst, TSecond, TResult> _projector;
            public SelectManyCore(ITryMonad<TFirst> self, Func<TFirst, ITryMonad<TSecond>> selector, Func<TFirst, TSecond, TResult> projector)
            {
                _self = self;
                _selector = selector;
                _projector = projector;
            }
            async Task<TryResult<TResult>> ITryMonad<TResult>.RunAsync(CancellationToken cancellationToken)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var selfResult = await _self.RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    if(selfResult.IsFaulted) return TryResult<TResult>.Failure(selfResult.Exception);
                    var secondResult = await _selector(selfResult.Value).RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    if(secondResult.IsFaulted) return TryResult<TResult>.Failure(secondResult.Exception);
                    return TryResult<TResult>.Success(_projector(selfResult.Value, secondResult.Value));
                }
                catch(Exception exception)
                {
                    return TryResult<TResult>.Failure(exception);
                }
            }
        }
        /// <summary>
        /// Projects each element of a try monad into a new form and flattens the resulting monads into one monad, using a projection function to combine results.
        /// </summary>
        /// <typeparam name="TFirst">The type of the value in the initial monad.</typeparam>
        /// <typeparam name="TSecond">The type of the value in the intermediate monad.</typeparam>
        /// <typeparam name="TResult">The type of the value in the resulting monad.</typeparam>
        /// <param name="self">The try monad to project and flatten.</param>
        /// <param name="selector">A transform function to apply to each value.</param>
        /// <param name="projector">A function to combine the intermediate and final results.</param>
        /// <returns>A try monad whose elements are the result of invoking the transform function on each element of the source and combining the results with the projection function.</returns>
        public static ITryMonad<TResult> SelectMany<TFirst, TSecond, TResult>(this ITryMonad<TFirst> self, Func<TFirst, ITryMonad<TSecond>> selector, Func<TFirst, TSecond, TResult> projector)
            => new SelectManyCore<TFirst, TSecond, TResult>(self, selector, projector);
        private class SelectManyAsyncCore<T, TResult> : ITryMonad<TResult>
        {
            ITryMonad<T> _self;
            Func<T, Task<ITryMonad<TResult>>> _selector;
            public SelectManyAsyncCore(ITryMonad<T> self, Func<T, Task<ITryMonad<TResult>>> selector)
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
                    var resultValue = await (await _selector(selfResult.Value)).RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return TryResult<TResult>.Success(resultValue.Value);
                }
                catch(Exception exception)
                {
                    return TryResult<TResult>.Failure(exception);
                }
            }
        }
        /// <summary>
        /// Projects each element of a try monad into a new form and flattens the resulting monads into one monad asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the value in the initial monad.</typeparam>
        /// <typeparam name="TResult">The type of the value in the resulting monad.</typeparam>
        /// <param name="self">The try monad to project and flatten.</param>
        /// <param name="selector">A transform function to apply to each value asynchronously.</param>
        /// <returns>A try monad whose elements are the result of invoking the asynchronous transform function on each element of the source.</returns>
        public static ITryMonad<TResult> SelectMany<T, TResult>(this ITryMonad<T> self, Func<T, Task<ITryMonad<TResult>>> selector)
            => new SelectManyAsyncCore<T, TResult>(self, selector);

        private class SelectManyAsyncCore<TFirst, TSecond, TResult> : ITryMonad<TResult>
        {
            ITryMonad<TFirst> _self;
            Func<TFirst, ITryMonad<TSecond>> _selector;
            Func<TFirst, TSecond, Task<TResult>> _projector;
            public SelectManyAsyncCore(ITryMonad<TFirst> self, Func<TFirst, ITryMonad<TSecond>> selector, Func<TFirst, TSecond, Task<TResult>> projector)
            {
                _self = self;
                _selector = selector;
                _projector = projector;
            }
            async Task<TryResult<TResult>> ITryMonad<TResult>.RunAsync(CancellationToken cancellationToken)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var selfResult = await _self.RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    if(selfResult.IsFaulted) return TryResult<TResult>.Failure(selfResult.Exception);
                    var secondResult = await _selector(selfResult.Value).RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    if(secondResult.IsFaulted) return TryResult<TResult>.Failure(secondResult.Exception);
                    var result = await _projector(selfResult.Value, secondResult.Value);
                    cancellationToken.ThrowIfCancellationRequested();
                    return TryResult<TResult>.Success(result);
                }
                catch(Exception exception)
                {
                    return TryResult<TResult>.Failure(exception);
                }
            }
        }
        /// <summary>
        /// Projects each element of a try monad into a new form and flattens the resulting monads into one monad asynchronously, using a projection function to combine results.
        /// </summary>
        /// <typeparam name="TFirst">The type of the value in the initial monad.</typeparam>
        /// <typeparam name="TSecond">The type of the value in the intermediate monad.</typeparam>
        /// <typeparam name="TResult">The type of the value in the resulting monad.</typeparam>
        /// <param name="self">The try monad to project and flatten.</param>
        /// <param name="selector">A transform function to apply to each value.</param>
        /// <param name="projector">A function to combine the intermediate and final results asynchronously.</param>
        /// <returns>A try monad whose elements are the result of invoking the asynchronous transform function on each element of the source and combining the results with the asynchronous projection function.</returns>
        public static ITryMonad<TResult> SelectMany<TFirst, TSecond, TResult>(this ITryMonad<TFirst> self, Func<TFirst, ITryMonad<TSecond>> selector, Func<TFirst, TSecond, Task<TResult>> projector)
            => new SelectManyAsyncCore<TFirst, TSecond, TResult>(self, selector, projector);
    }
}