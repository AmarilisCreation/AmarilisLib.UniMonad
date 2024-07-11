using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class TryExtensions
    {
        /// <summary>
        /// Converts a try monad to an asynchronous identity monad.
        /// </summary>
        /// <typeparam name="T">The type of the value in the monad.</typeparam>
        /// <param name="source">The try monad to convert.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is an identity monad containing the try result.</returns>
        public static async Task<IIdentityMonad<TryResult<T>>> ToIdentityAsync<T>(this ITryMonad<T> source, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            return Identity.Return(selfResult);
        }
        /// <summary>
        /// Converts a try monad to an asynchronous option monad.
        /// </summary>
        /// <typeparam name="T">The type of the value in the monad.</typeparam>
        /// <param name="source">The try monad to convert.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is an option monad containing the value if the try result is successful, or none if it failed.</returns>
        public static async Task<IOptionMonad<T>> ToOptionAsync<T>(this ITryMonad<T> source, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if(selfResult.IsSucceeded) return Option.Return(selfResult.Value);
            return Option.None<T>();
        }

        /// <summary>
        /// Converts a try monad to an asynchronous either monad.
        /// </summary>
        /// <typeparam name="TLeft">The type of the left value in the either monad.</typeparam>
        /// <typeparam name="TRight">The type of the right value in the either monad.</typeparam>
        /// <param name="source">The try monad to convert.</param>
        /// <param name="leftValue">The value to use as the left value if the try result failed.</param>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is an either monad containing the right value if the try result is successful, or the left value if it failed.</returns>
        public static async Task<IEitherMonad<TLeft, TRight>> ToEitherAsync<TLeft, TRight>(this ITryMonad<TRight> source, TLeft leftValue, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var selfResult = await source.RunAsync(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if(selfResult.IsSucceeded) return Either.ReturnRight<TLeft, TRight>(selfResult.Value);
            return Either.ReturnLeft<TLeft, TRight>(leftValue);
        }
    }
}