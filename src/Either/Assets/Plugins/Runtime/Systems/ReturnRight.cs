using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Either
    {
        private struct RightCore<TLeft, TRight> : IEitherMonad<TLeft, TRight>
        {
            private TRight _value;
            public RightCore(TRight value)
            {
                _value = value;
            }
            public Task<EitherResult<TLeft, TRight>> RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(EitherResult<TLeft, TRight>.ReturnRight(_value));
            }
        }
        /// <summary>
        /// Creates an Either monad that represents a Right value.
        /// </summary>
        /// <typeparam name="TLeft">The type of the Left value.</typeparam>
        /// <typeparam name="TRight">The type of the Right value.</typeparam>
        /// <param name="value">The Right value.</param>
        /// <returns>An Either monad containing the Right value.</returns>
        public static IEitherMonad<TLeft, TRight> ReturnRight<TLeft, TRight>(TRight value) => new RightCore<TLeft, TRight>(value);
    }
}
