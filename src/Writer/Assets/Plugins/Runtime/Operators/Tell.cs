using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Writer
    {
        private class TellCore<TOutput> : IWriterMonad<TOutput, Unit>
        {
            private TOutput _output;
            public TellCore(TOutput output)
            {
                _output = output;
            }
            Task<WriterResult<TOutput, Unit>> IWriterMonad<TOutput, Unit>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(new WriterResult<TOutput, Unit>(Unit.Default, new TOutput[1] { _output }));
            }
        }
        /// <summary>
        /// Creates a writer monad that produces the specified output and returns a unit value.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <param name="output">The output to produce.</param>
        /// <returns>A writer monad that produces the specified output and returns a unit value.</returns>
        public static IWriterMonad<TOutput, Unit> Tell<TOutput>(TOutput output)
            => new TellCore<TOutput>(output);

        private class TellCore<TOutput, TValue> : IWriterMonad<TOutput, TValue>
        {
            private TValue _value;
            private TOutput _output;
            public TellCore(TValue value, TOutput output)
            {
                _value = value;
                _output = output;
            }
            Task<WriterResult<TOutput, TValue>> IWriterMonad<TOutput, TValue>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(new WriterResult<TOutput, TValue>(_value, new[] { _output }));
            }
        }
        /// <summary>
        /// Creates a writer monad that produces the specified output and returns the specified value.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="value">The value to return.</param>
        /// <param name="output">The output to produce.</param>
        /// <returns>A writer monad that produces the specified output and returns the specified value.</returns>
        public static IWriterMonad<TOutput, TValue> Tell<TOutput, TValue>(TValue value, TOutput output)
            => new TellCore<TOutput, TValue>(value, output);

        private class TellEnumerableCore<TOutput, TValue> : IWriterMonad<TOutput, TValue>
        {
            private TValue _value;
            private IEnumerable<TOutput> _outputs;
            public TellEnumerableCore(TValue value, IEnumerable<TOutput> outputs)
            {
                _value = value;
                _outputs = outputs;
            }
            Task<WriterResult<TOutput, TValue>> IWriterMonad<TOutput, TValue>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(new WriterResult<TOutput, TValue>(_value, _outputs));
            }
        }
        /// <summary>
        /// Creates a writer monad that produces the specified outputs and returns the specified value.
        /// </summary>
        /// <typeparam name="TOutput">The type of the outputs.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="value">The value to return.</param>
        /// <param name="outputs">The outputs to produce.</param>
        /// <returns>A writer monad that produces the specified outputs and returns the specified value.</returns>
        public static IWriterMonad<TOutput, TValue> Tell<TOutput, TValue>(TValue value, IEnumerable<TOutput> outputs)
            => new TellEnumerableCore<TOutput, TValue>(value, outputs);
    }
}
