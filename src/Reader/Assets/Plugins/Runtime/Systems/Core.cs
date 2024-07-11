using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    /// <summary>
    /// Represents a Reader monad which encapsulates a computation that reads from a shared environment.
    /// </summary>
    /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public interface IReaderMonad<TEnvironment, TValue>
    {
        /// <summary>
        /// Executes the computation using the given environment and cancellation token.
        /// </summary>
        /// <param name="environment">The environment to use in the computation.</param>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the result of the computation.</returns>
        Task<TValue> RunAsync(TEnvironment environment, CancellationToken cancellationToken);
    }
}
