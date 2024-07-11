using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    /// <summary>
    /// Defines an asynchronous try monad.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    public interface ITryMonad<T>
    {
        /// <summary>
        /// Runs the try monad asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the try result.</returns>
        Task<TryResult<T>> RunAsync(CancellationToken cancellationToken);
    }

    /// <summary>
    /// Represents the result of a try monad computation.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    public struct TryResult<T>
    {
        /// <summary>
        /// Gets the value of the result.
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// Gets the exception if the computation failed.
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the computation failed.
        /// </summary>
        public bool IsFaulted { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the computation succeeded.
        /// </summary>
        public bool IsSucceeded { get; private set; }

        /// <summary>
        /// Creates a successful try result.
        /// </summary>
        /// <param name="value">The value of the result.</param>
        /// <returns>A successful try result.</returns>
        public static TryResult<T> Success(T value) => new TryResult<T> { Value = value, Exception = null, IsFaulted = false, IsSucceeded = true };

        /// <summary>
        /// Creates a failed try result.
        /// </summary>
        /// <param name="exception">The exception that caused the failure.</param>
        /// <returns>A failed try result.</returns>
        public static TryResult<T> Failure(Exception exception) => new TryResult<T> { Value = default, Exception = exception, IsFaulted = true, IsSucceeded = false };
    }
}
