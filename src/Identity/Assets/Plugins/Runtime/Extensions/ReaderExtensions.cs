using System.Threading;
using System.Threading.Tasks;
using static UnityEngine.Random;

namespace AmarilisLib.Monad
{
    public static partial class ReaderExtensions
    {
        /// <summary>
        /// Converts a Reader monad to an Identity monad asynchronously.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The source Reader monad.</param>
        /// <param name="environment">The environment to run the Reader monad with.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An Identity monad that contains the result of the Reader monad.</returns>
        public static async Task<IIdentityMonad<TValue>> ToIdentityAsync<TEnvironment, TValue>(this IReaderMonad<TEnvironment, TValue> source, TEnvironment environment, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(environment, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return Identity.Return(selfResult);
        }

        /// <summary>
        /// Converts a Reader monad to an RWS monad asynchronously.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The source Reader monad.</param>
        /// <param name="environment">The environment to run the Reader monad with.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An RWS monad that contains the result of the Reader monad.</returns>
        public static async Task<IRWSMonad<TEnvironment, TOutput, TState, TValue>> ToRWSAsync<TEnvironment, TOutput, TState, TValue>(this IReaderMonad<TEnvironment, TValue> source, TEnvironment environment, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(environment, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return RWS.Return<TEnvironment, TOutput, TState, TValue>(selfResult);
        }
    }
}