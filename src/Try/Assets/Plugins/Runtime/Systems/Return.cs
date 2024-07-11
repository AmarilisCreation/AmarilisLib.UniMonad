using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Try
    {
        private class ReturnCore<T> : ITryMonad<T>
        {
            T _value;
            public ReturnCore(T value)
            {
                _value = value;
            }
            Task<TryResult<T>> ITryMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(TryResult<T>.Success(_value));
            }
        }
        /// <summary>
        /// Creates a try monad that returns a specified value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value to return.</param>
        /// <returns>A try monad that returns the specified value.</returns>
        public static ITryMonad<T> Return<T>(T value)
        {
            return new ReturnCore<T>(value);
        }
    }
}