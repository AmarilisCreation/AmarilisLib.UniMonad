using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    /// <summary>
    /// Represents a monad that encapsulates a computation with read-only environment, write-only output, and mutable state.
    /// </summary>
    /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public interface IRWSMonad<TEnvironment, TOutput, TState, TValue>
    {
        /// <summary>
        /// Runs the computation with the specified environment and state.
        /// </summary>
        /// <param name="environment">The environment to pass to the computation.</param>
        /// <param name="state">The initial state to pass to the computation.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation, containing the result of the computation.</returns>
        Task<RWSResult<TOutput, TState, TValue>> RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Represents the result of a computation in the RWS monad, including the value, output, and state.
    /// </summary>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public struct RWSResult<TOutput, TState, TValue>
    {
        /// <summary>
        /// Gets the value produced by the computation.
        /// </summary>
        public TValue Value { private set; get; }

        /// <summary>
        /// Gets the output produced by the computation.
        /// </summary>
        public IEnumerable<TOutput> Output { private set; get; }

        /// <summary>
        /// Gets the state after the computation.
        /// </summary>
        public TState State { private set; get; }

        /// <summary>
        /// Initializes a new instance of the RWSResult struct with the specified value, output, and state.
        /// </summary>
        /// <param name="value">The value produced by the computation.</param>
        /// <param name="output">The output produced by the computation.</param>
        /// <param name="state">The state after the computation.</param>
        public RWSResult(TValue value, IEnumerable<TOutput> output, TState state)
        {
            Value = value;
            Output = output;
            State = state;
        }
    }
}
