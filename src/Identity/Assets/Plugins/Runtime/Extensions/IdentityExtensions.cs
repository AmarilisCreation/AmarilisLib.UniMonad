using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using UnityEngine;

namespace AmarilisLib.Monad
{
    public static partial class IdentityExtensions
    {
        /// <summary>
        /// Converts an identity monad containing an option monad to an asynchronous option monad.
        /// </summary>
        /// <typeparam name="T">The type of the value in the option monad.</typeparam>
        /// <param name="source">The identity monad containing the option monad to convert.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the option monad contained in the identity monad.</returns>
        public static async Task<IOptionMonad<T>> ToOptionAsync<T>(this IIdentityMonad<IOptionMonad<T>> source, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return selfResult;
        }

        /// <summary>
        /// Converts an identity monad containing an either monad to an asynchronous either monad.
        /// </summary>
        /// <typeparam name="TLeft">The type of the left value in the either monad.</typeparam>
        /// <typeparam name="TRight">The type of the right value in the either monad.</typeparam>
        /// <param name="source">The identity monad containing the either monad to convert.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the either monad contained in the identity monad.</returns>
        public static async Task<IEitherMonad<TLeft, TRight>> ToEitherAsync<TLeft, TRight>(this IIdentityMonad<IEitherMonad<TLeft, TRight>> source, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return selfResult;
        }

        /// <summary>
        /// Converts an identity monad containing a try monad to an asynchronous try monad.
        /// </summary>
        /// <typeparam name="T">The type of the value in the try monad.</typeparam>
        /// <param name="source">The identity monad containing the try monad to convert.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the try monad contained in the identity monad.</returns>
        public static async Task<ITryMonad<T>> ToTryAsync<T>(this IIdentityMonad<ITryMonad<T>> source, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return selfResult;
        }

        /// <summary>
        /// Converts an Identity monad to an IO monad asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="source">The source Identity monad.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An IO monad that contains the result of the Identity monad.</returns>
        public static async Task<IIOMonad<T>> ToIOAsync<T>(this IIdentityMonad<T> source, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return IO.Return(selfResult);
        }

        /// <summary>
        /// Converts an Identity monad to a Reader monad asynchronously.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The source Identity monad.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A Reader monad that contains the result of the Identity monad.</returns>
        public static async Task<IReaderMonad<TEnvironment, TValue>> ToReaderAsync<TEnvironment, TValue>(this IIdentityMonad<TValue> source, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return Reader.Return<TEnvironment, TValue>(selfResult);
        }

        /// <summary>
        /// Converts an Identity monad to a Writer monad asynchronously.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The source Identity monad.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A Writer monad that contains the result of the Identity monad.</returns>
        public static async Task<IWriterMonad<TOutput, TValue>> ToWriterAsync<TOutput, TValue>(this IIdentityMonad<TValue> source, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return Writer.Return<TOutput, TValue>(selfResult);
        }

        /// <summary>
        /// Converts an Identity monad to a State monad asynchronously.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The source Identity monad.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A State monad that contains the result of the Identity monad.</returns>
        public static async Task<IStateMonad<TState, TValue>> ToStateAsync<TState, TValue>(this IIdentityMonad<TValue> source, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return State.Return<TState, TValue>(selfResult);
        }

        /// <summary>
        /// Converts an Identity monad to an RWS monad asynchronously.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The source Identity monad.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>An RWS monad that contains the result of the Identity monad.</returns>
        public static async Task<IRWSMonad<TEnvironment, TOutput, TState, TValue>> ToRWSAsync<TEnvironment, TOutput, TState, TValue>(this IIdentityMonad<TValue> source, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return RWS.Return<TEnvironment, TOutput, TState, TValue>(selfResult);
        }
    }
}