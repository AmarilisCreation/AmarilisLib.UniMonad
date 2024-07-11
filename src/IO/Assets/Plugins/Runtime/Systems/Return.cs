using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class IO
    {
        private struct ReturnCore<T> : IIOMonad<T>
        {
            private T _value;
            public ReturnCore(T value)
            {
                _value = value;
            }
            Task<T> IIOMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(_value);
            }
        }
        /// <summary>
        /// Creates a monad that returns the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the monad.</typeparam>
        /// <param name="value">The value to be returned by the monad.</param>
        /// <returns>An IO monad that returns the specified value.</returns>
        public static IIOMonad<T> Return<T>(T value)
            => new ReturnCore<T>(value);
    }
}
