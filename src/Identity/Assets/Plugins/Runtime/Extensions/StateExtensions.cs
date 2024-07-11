using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class StateExtensions
    {
        /// <summary>
        /// Converts a State monad to an Identity monad asynchronously.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The source State monad.</param>
        /// <param name="state">The initial state to run the State monad with.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An Identity monad that contains the result of the State monad.</returns>
        public static async Task<IIdentityMonad<StateResult<TState, TValue>>> ToIdentityAsync<TState, TValue>(this IStateMonad<TState, TValue> source, TState state, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(state, cancellationToken);
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
        /// <param name="state">The initial state to run the Reader monad with.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An RWS monad that contains the result of the Reader monad.</returns>
        public static async Task<IRWSMonad<TEnvironment, TOutput, TState, TValue>> ToRWSAsync<TEnvironment, TOutput, TState, TValue>(this IReaderMonad<TState, TValue> source, TState state, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(state, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return RWS.Return<TEnvironment, TOutput, TState, TValue>(selfResult);
        }
    }
}