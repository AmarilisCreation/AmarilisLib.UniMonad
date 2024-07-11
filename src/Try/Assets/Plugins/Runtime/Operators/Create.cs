using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Try
    {
        private class CreateCore<T> : ITryMonad<T>
        {
            Func<TryResult<T>> _func;
            public CreateCore(Func<TryResult<T>> func)
            {
                _func = func;
            }
            Task<TryResult<T>> ITryMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                try
                {
                    return Task.Factory.StartNew(
                        _func,
                        cancellationToken,
                        TaskCreationOptions.DenyChildAttach,
                        TaskScheduler.FromCurrentSynchronizationContext());
                }
                catch(Exception exception)
                {
                    return Task.FromResult(TryResult<T>.Failure(exception));
                }
            }
        }
        /// <summary>
        /// Creates a try monad from a synchronous function.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="func">The function to create the try result.</param>
        /// <returns>A try monad created from the provided function.</returns>
        public static ITryMonad<T> Create<T>(Func<TryResult<T>> func)
            => new CreateCore<T>(func);
        private class CreateAsyncCore<T> : ITryMonad<T>
        {
            Func<Task<TryResult<T>>> _func;
            public CreateAsyncCore(Func<Task<TryResult<T>>> func)
            {
                _func = func;
            }
            Task<TryResult<T>> ITryMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                try
                {
                    var task = _func();
                    cancellationToken.Register(() => task.Dispose());
                    return task;
                }
                catch(Exception exception)
                {
                    return Task.FromResult(TryResult<T>.Failure(exception));
                }
            }
        }
        /// <summary>
        /// Creates a try monad from an asynchronous function.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="func">The asynchronous function to create the try result.</param>
        /// <returns>A try monad created from the provided asynchronous function.</returns>
        public static ITryMonad<T> Create<T>(Func<Task<TryResult<T>>> func)
            => new CreateAsyncCore<T>(func);
    }
}