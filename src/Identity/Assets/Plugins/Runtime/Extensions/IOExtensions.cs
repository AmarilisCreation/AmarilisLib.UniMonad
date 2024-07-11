using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class IOExtensions
    {
        /// <summary>
        /// Converts an IO monad to an Identity monad asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The source IO monad.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An Identity monad that contains the result of the IO monad.</returns>
        public static async Task<IIdentityMonad<T>> ToIdentityAsync<T>(this IIOMonad<T> source, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return Identity.Return(selfResult);
        }
    }
}