using System;
using System.Threading.Tasks;
using System.Threading;

namespace AmarilisLib.Monad
{
    public static partial class Try
    {
        private class ThrowCore<T> : ITryMonad<T>
        {
            Exception _exception;
            public ThrowCore(Exception exception)
            {
                _exception = exception;
            }
            Task<TryResult<T>> ITryMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(TryResult<T>.Failure(_exception));
            }
        }
        /// <summary>
        /// Creates a try monad that always fails with the specified exception.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="exception">The exception to throw.</param>
        /// <returns>A try monad that always fails with the specified exception.</returns>
        public static ITryMonad<T> Throw<T>(Exception exception)
            => new ThrowCore<T>(exception);
    }
}