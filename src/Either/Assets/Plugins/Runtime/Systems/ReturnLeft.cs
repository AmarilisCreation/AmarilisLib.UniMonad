using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Either
    {
        private struct LeftCore<TLeft, TRight> : IEitherMonad<TLeft, TRight>
        {
            private TLeft _value;
            public LeftCore(TLeft value)
            {
                _value = value;
            }
            Task<EitherResult<TLeft, TRight>> IEitherMonad<TLeft, TRight>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(EitherResult<TLeft, TRight>.ReturnLeft(_value));
            }
        }
        /// <summary>
        /// Creates an Either monad that represents a Left value.
        /// </summary>
        /// <typeparam name="TLeft">The type of the Left value.</typeparam>
        /// <typeparam name="TRight">The type of the Right value.</typeparam>
        /// <param name="value">The Left value.</param>
        /// <returns>An Either monad containing the Left value.</returns>
        public static IEitherMonad<TLeft, TRight> ReturnLeft<TLeft, TRight>(TLeft value) => new LeftCore<TLeft, TRight>(value);
    }
}
