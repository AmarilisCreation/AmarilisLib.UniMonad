using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Reader
    {
        private class ReturnCore<TEnvironment, TValue> : IReaderMonad<TEnvironment, TValue>
        {
            private TValue _value;
            public ReturnCore(TValue value)
            {
                _value = value;
            }
            public Task<TValue> RunAsync(TEnvironment environment, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(_value);
            }
        }
        /// <summary>
        /// Creates a Reader monad that returns the specified value.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="value">The value to return.</param>
        /// <returns>A Reader monad that returns the specified value.</returns>
        public static IReaderMonad<TEnvironment, TValue> Return<TEnvironment, TValue>(TValue value)
            => new ReturnCore<TEnvironment, TValue>(value);
    }
}
