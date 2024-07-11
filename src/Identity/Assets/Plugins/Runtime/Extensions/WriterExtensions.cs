using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class WriterExtensions
    {
        /// <summary>
        /// Converts a Writer monad to an Identity monad asynchronously.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The source Writer monad.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An Identity monad that contains the result of the Writer monad.</returns>
        public static async Task<IIdentityMonad<WriterResult<TOutput, TValue>>> ToIdentityAsync<TOutput, TValue>(this IWriterMonad<TOutput, TValue> source, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return Identity.Return(selfResult);
        }

        /// <summary>
        /// Converts a Writer monad to an RWS monad asynchronously.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The source Writer monad.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An RWS monad that contains the result of the Writer monad.</returns>
        public static async Task<IRWSMonad<TEnvironment, TOutput, TState, TValue>> ToRWSAsync<TEnvironment, TOutput, TState, TValue>(this IWriterMonad<TOutput, TValue> source, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return RWS.Return<TEnvironment, TOutput, TState, TValue>(selfResult.Value)
                .SelectMany(v => RWS.Tell<TEnvironment, TOutput, TState, TValue>(v, selfResult.Output));
        }
    }
}