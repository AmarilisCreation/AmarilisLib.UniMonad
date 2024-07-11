using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Writer
    {
        private class ReturnCore<TOutput, TValue> : IWriterMonad<TOutput, TValue>
        {
            private TValue _value;
            public ReturnCore(TValue value)
            {
                _value = value;
            }
            Task<WriterResult<TOutput, TValue>> IWriterMonad<TOutput, TValue>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(new WriterResult<TOutput, TValue>(_value, Array.Empty<TOutput>()));
            }
        }
        /// <summary>
        /// Creates a writer monad that returns the specified value with no output.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="value">The value to be returned by the writer monad.</param>
        /// <returns>A writer monad that returns the specified value with no output.</returns>
        public static IWriterMonad<TOutput, TValue> Return<TOutput, TValue>(TValue value)
            => new ReturnCore<TOutput, TValue>(value);
    }
}
