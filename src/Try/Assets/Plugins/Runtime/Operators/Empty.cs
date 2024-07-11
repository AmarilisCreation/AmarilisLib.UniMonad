using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Try
    {
        private class EmptyCore<T> : ITryMonad<T>
        {
            public EmptyCore()
            {
            }
            Task<TryResult<T>> ITryMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(TryResult<T>.Success(default));
            }
        }
        /// <summary>
        /// Creates an empty try monad that always succeeds with the default value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <returns>An empty try monad that always succeeds with the default value.</returns>
        public static ITryMonad<T> Empty<T>()
            => new EmptyCore<T>();
    }
}