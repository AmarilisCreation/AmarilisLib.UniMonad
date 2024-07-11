using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Try
    {
        private class DoCore<T> : ITryMonad<T>
        {
            ITryMonad<T> _self;
            Action<TryResult<T>> _action;
            public DoCore(ITryMonad<T> self, Action<TryResult<T>> action)
            {
                _self = self;
                _action = action;
            }
            async Task<TryResult<T>> ITryMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                try
                {
                    var result = await _self.RunAsync(cancellationToken);
                    _action(result);
                    return result;
                }
                catch(Exception exception)
                {
                    return TryResult<T>.Failure(exception);
                }
            }
        }
        /// <summary>
        /// Executes a specified action on the result produced by the try monad.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="self">The try monad to run.</param>
        /// <param name="action">The action to execute on the result.</param>
        /// <returns>A try monad that runs the specified action on the result.</returns>
        public static ITryMonad<T> Do<T>(this ITryMonad<T> self, Action<TryResult<T>> action)
            => new DoCore<T>(self, action);
        private class DoAsyncCore<T> : ITryMonad<T>
        {
            ITryMonad<T> _self;
            Func<TryResult<T>, Task> _action;
            public DoAsyncCore(ITryMonad<T> self, Func<TryResult<T>, Task> action)
            {
                _self = self;
                _action = action;
            }
            async Task<TryResult<T>> ITryMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var result = await _self.RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    await _action(result);
                    cancellationToken.ThrowIfCancellationRequested();
                    return result;
                }
                catch(Exception exception)
                {
                    return TryResult<T>.Failure(exception);
                }
            }
        }
        /// <summary>
        /// Executes a specified asynchronous action on the result produced by the try monad.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="self">The try monad to run.</param>
        /// <param name="action">The asynchronous action to execute on the result.</param>
        /// <returns>A try monad that runs the specified asynchronous action on the result.</returns>
        public static ITryMonad<T> Do<T>(this ITryMonad<T> self, Func<TryResult<T>, Task> action)
            => new DoAsyncCore<T>(self, action);
    }
}