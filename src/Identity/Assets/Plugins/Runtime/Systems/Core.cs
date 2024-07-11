using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    /// <summary>
    /// Represents a monad that performs an identity operation asynchronously.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    public interface IIdentityMonad<T>
    {
        /// <summary>
        /// Executes the identity operation asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the value.</returns>
        Task<T> RunAsync(CancellationToken cancellationToken);
    }
}
