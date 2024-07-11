using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    /// <summary>
    /// Represents a state monad that performs an asynchronous computation and returns a state and a value.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public interface IStateMonad<TState, TValue>
    {
        /// <summary>
        /// Runs the asynchronous computation and returns the state and value.
        /// </summary>
        /// <param name="state">The initial state.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the result of the computation, including the state and value.</returns>
        Task<StateResult<TState, TValue>> RunAsync(TState state, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Represents the result of a state monad computation, including the state and value.
    /// </summary>
    /// <typeparam name="TState">The type of the state.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public struct StateResult<TState, TValue>
    {
        /// <summary>
        /// Gets the resulting state.
        /// </summary>
        public TState State { get; private set; }

        /// <summary>
        /// Gets the resulting value.
        /// </summary>
        public TValue Value { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StateResult{TState, TValue}"/> struct.
        /// </summary>
        /// <param name="state">The resulting state.</param>
        /// <param name="value">The resulting value.</param>
        public StateResult(TState state, TValue value)
        {
            Value = value;
            State = state;
        }
    }
}
