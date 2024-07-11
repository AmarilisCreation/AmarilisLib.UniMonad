using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    /// <summary>
    /// Represents a writer monad that produces a result and a log of outputs.
    /// </summary>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public interface IWriterMonad<TOutput, TValue>
    {
        /// <summary>
        /// Runs the writer monad computation.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task that represents the result of the writer monad computation.</returns>
        Task<WriterResult<TOutput, TValue>> RunAsync(CancellationToken cancellationToken);
    }

    /// <summary>
    /// Represents the result of a writer monad computation, including the produced value and output.
    /// </summary>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public struct WriterResult<TOutput, TValue>
    {
        /// <summary>
        /// Gets the produced value of the writer monad computation.
        /// </summary>
        public TValue Value { get; private set; }

        /// <summary>
        /// Gets the output log of the writer monad computation.
        /// </summary>
        public IEnumerable<TOutput> Output { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WriterResult{TOutput, TValue}"/> struct.
        /// </summary>
        /// <param name="value">The produced value.</param>
        /// <param name="output">The output log.</param>
        public WriterResult(TValue value, IEnumerable<TOutput> output)
        {
            Value = value;
            Output = output;
        }
    }
}
