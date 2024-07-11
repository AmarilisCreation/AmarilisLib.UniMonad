using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Try
    {
        private class DoOnFailureCore<T> : ITryMonad<T>
        {
            ITryMonad<T> _self;
            Action<Exception> _action;
            public DoOnFailureCore(ITryMonad<T> self, Action<Exception> action)
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
                    if(result.IsFaulted) _action(result.Exception);
                    return result;
                }
                catch(Exception exception)
                {
                    return TryResult<T>.Failure(exception);
                }
            }
        }
        /// <summary>
        /// Executes a specified action on the exception produced by the try monad if it fails.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="self">The try monad to run.</param>
        /// <param name="action">The action to execute on the exception if the try monad fails.</param>
        /// <returns>A try monad that runs the specified action on the exception if it fails.</returns>
        public static ITryMonad<T> DoOnFailure<T>(this ITryMonad<T> self, Action<Exception> action)
            => new DoOnFailureCore<T>(self, action);
        private class DoOnFailureAsyncCore<T> : ITryMonad<T>
        {
            ITryMonad<T> _self;
            Func<Exception, Task> _action;
            public DoOnFailureAsyncCore(ITryMonad<T> self, Func<Exception, Task> action)
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
                    if(result.IsFaulted) {
                        await _action(result.Exception);
                        cancellationToken.ThrowIfCancellationRequested();
                    }
                    return result;
                }
                catch(Exception exception)
                {
                    return TryResult<T>.Failure(exception);
                }
            }
        }
        /// <summary>
        /// Executes a specified asynchronous action on the exception produced by the try monad if it fails.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="self">The try monad to run.</param>
        /// <param name="action">The asynchronous action to execute on the exception if the try monad fails.</param>
        /// <returns>A try monad that runs the specified asynchronous action on the exception if it fails.</returns>
        public static ITryMonad<T> DoOnFailure<T>(this ITryMonad<T> self, Func<Exception, Task> action)
            => new DoOnFailureAsyncCore<T>(self, action);
    }
}