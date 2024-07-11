using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class RWSExtensions
    {
        /// <summary>
        /// Converts an RWS monad to an Identity monad asynchronously.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The source RWS monad.</param>
        /// <param name="environment">The environment to run the RWS monad with.</param>
        /// <param name="state">The initial state to run the RWS monad with.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An Identity monad that contains the result of the RWS monad.</returns>
        public static async Task<IIdentityMonad<RWSResult<TOutput, TState, TValue>>> ToIdentityAsync<TEnvironment, TOutput, TState, TValue>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> source, TEnvironment environment, TState state, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(environment, state, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return Identity.Return(selfResult);
        }

        /// <summary>
        /// Converts an RWS monad to a Reader monad asynchronously.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The source RWS monad.</param>
        /// <param name="environment">The environment to run the RWS monad with.</param>
        /// <param name="state">The initial state to run the RWS monad with.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A Reader monad that contains the result of the RWS monad.</returns>
        public static async Task<IReaderMonad<TEnvironment, TValue>> ToReaderAsync<TEnvironment, TOutput, TState, TValue>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> source, TEnvironment environment, TState state, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(environment, state, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return Reader.Return<TEnvironment, TValue>(selfResult.Value);
        }

        /// <summary>
        /// Converts an RWS monad to a Writer monad asynchronously.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The source RWS monad.</param>
        /// <param name="environment">The environment to run the RWS monad with.</param>
        /// <param name="state">The initial state to run the RWS monad with.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A Writer monad that contains the result of the RWS monad.</returns>
        public static async Task<IWriterMonad<TOutput, TValue>> ToWriterAsync<TEnvironment, TOutput, TState, TValue>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> source, TEnvironment environment, TState state, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(environment, state, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return Writer.Return<TOutput, TValue>(selfResult.Value)
                .SelectMany(v => Writer.Tell(v, selfResult.Output));
        }

        /// <summary>
        /// Converts an RWS monad to a State monad asynchronously.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The source RWS monad.</param>
        /// <param name="environment">The environment to run the RWS monad with.</param>
        /// <param name="state">The initial state to run the RWS monad with.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A State monad that contains the result of the RWS monad.</returns>
        public static async Task<IStateMonad<TState, TValue>> ToStateAsync<TEnvironment, TOutput, TState, TValue>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> source, TEnvironment environment, TState state, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(environment, state, cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return State.Return<TState, TValue>(selfResult.Value);
        }
    }
}