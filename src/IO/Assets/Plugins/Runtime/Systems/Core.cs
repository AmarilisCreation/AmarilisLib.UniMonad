using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    /// <summary>
    /// Represents an IO monad that can be run asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the value returned by the monad.</typeparam>
    public interface IIOMonad<T>
    {
        /// <summary>
        /// Runs the monad asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the value returned by the monad.</returns>
        Task<T> RunAsync(CancellationToken cancellationToken);
    }
}
