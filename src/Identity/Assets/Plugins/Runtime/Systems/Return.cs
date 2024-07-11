using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Identity
    {
        private struct ReturnCore<T> : IIdentityMonad<T>
        {
            private T _value;
            public ReturnCore(T value)
            {
                _value = value;
            }
            Task<T> IIdentityMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(_value);
            }
        }
        /// <summary>
        /// Creates an Identity monad that returns the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value to return.</param>
        /// <returns>An Identity monad that returns the specified value.</returns>
        public static IIdentityMonad<T> Return<T>(T value)
            => new ReturnCore<T>(value);
    }
}
