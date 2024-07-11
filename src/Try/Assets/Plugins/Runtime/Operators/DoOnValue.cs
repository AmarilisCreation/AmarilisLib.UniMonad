using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Try
    {
        private class DoOnValueCore<T> : ITryMonad<T>
        {
            ITryMonad<T> _self;
            Action<T> _action;
            public DoOnValueCore(ITryMonad<T> self, Action<T> action)
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
                    if(result.IsSucceeded) _action(result.Value);
                    return result;
                }
                catch(Exception exception)
                {
                    return TryResult<T>.Failure(exception);
                }
            }
        }
        /// <summary>
        /// Executes a specified action on the value produced by the try monad if it succeeds.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="self">The try monad to run.</param>
        /// <param name="action">The action to execute on the value if the try monad succeeds.</param>
        /// <returns>A try monad that runs the specified action on the value if it succeeds.</returns>
        public static ITryMonad<T> DoOnValue<T>(this ITryMonad<T> self, Action<T> action)
            => new DoOnValueCore<T>(self, action);
        private class DoOnValueAsyncCore<T> : ITryMonad<T>
        {
            ITryMonad<T> _self;
            Func<T, Task> _action;
            public DoOnValueAsyncCore(ITryMonad<T> self, Func<T, Task> action)
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
                    if(result.IsSucceeded) {
                        await _action(result.Value);
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
        /// Executes a specified asynchronous action on the value produced by the try monad if it succeeds.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="self">The try monad to run.</param>
        /// <param name="action">The asynchronous action to execute on the value if the try monad succeeds.</param>
        /// <returns>A try monad that runs the specified asynchronous action on the value if it succeeds.</returns>
        public static ITryMonad<T> DoOnValue<T>(this ITryMonad<T> self, Func<T, Task> action)
            => new DoOnValueAsyncCore<T>(self, action);
    }
}