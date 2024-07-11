using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Try
    {
        /// <summary>
        /// Executes the try monad asynchronously, handling any potential exceptions.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="self">The try monad to execute.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task ExecuteAsync<T>(this ITryMonad<T> self, CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(selfResult.IsFaulted)
                {
                    return;
                }
                else
                {
                    return;
                }
            }
            catch
            {
                return;
            }
        }
        /// <summary>
        /// Executes the try monad asynchronously, invoking the specified action on success.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="self">The try monad to execute.</param>
        /// <param name="onSuccess">The action to invoke on success.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task ExecuteAsync<T>(this ITryMonad<T> self, Action<T> onSuccess, CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(selfResult.IsFaulted)
                {
                    throw (selfResult.Exception);
                }
                else
                {
                    onSuccess(selfResult.Value);
                    return;
                }
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }
        /// <summary>
        /// Executes the try monad asynchronously, invoking the specified actions on success or error.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="self">The try monad to execute.</param>
        /// <param name="onSuccess">The action to invoke on success.</param>
        /// <param name="onError">The action to invoke on error.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task ExecuteAsync<T>(this ITryMonad<T> self, Action<T> onSuccess, Action<Exception> onError, CancellationToken cancellationToken = default)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(selfResult.IsFaulted)
                {
                    onError(selfResult.Exception);
                    return;
                }
                else
                {
                    onSuccess(selfResult.Value);
                    return;
                }
            }
            catch(Exception exception)
            {
                onError(exception);
                return;
            }
        }
        /// <summary>
        /// Executes the try monad asynchronously, invoking the specified actions on success or specific error type.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <typeparam name="TException">The type of the exception to handle.</typeparam>
        /// <param name="self">The try monad to execute.</param>
        /// <param name="onSuccess">The action to invoke on success.</param>
        /// <param name="onError">The action to invoke on the specified error type.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public static async Task ExecuteAsync<T, TException>(this ITryMonad<T> self, Action<T> onSuccess, Action<TException> onError, CancellationToken cancellationToken = default) where TException : Exception
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(selfResult.IsFaulted)
                {
                    if(selfResult.Exception is TException)
                    {
                        onError(selfResult.Exception as TException);
                    }
                    return;
                }
                else
                {
                    onSuccess(selfResult.Value);
                    return;
                }
            }
            catch(Exception exception)
            {
                if(exception is TException)
                {
                    onError(exception as TException);
                }
                return;
            }
        }
    }
}