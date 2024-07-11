using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Either
    {
        private class SelectCore<TLeft, TRight, TResultRight> : IEitherMonad<TLeft, TResultRight>
        {
            private IEitherMonad<TLeft, TRight> _self;
            private Func<TRight, TResultRight> _selector;
            public SelectCore(IEitherMonad<TLeft, TRight> self, Func<TRight, TResultRight> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<EitherResult<TLeft, TResultRight>> IEitherMonad<TLeft, TResultRight>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(selfResult.IsLeft)
                {
                    var leftResult = await ReturnLeft<TLeft, TResultRight>(selfResult.Left).RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return leftResult;
                }
                else
                {
                    var rightResult = await ReturnRight<TLeft, TResultRight>(_selector(selfResult.Right)).RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return rightResult;
                }
            }
        }
        /// <summary>
        /// Projects each element of a monad into a new form.
        /// </summary>
        /// <typeparam name="TLeft">The type of the Left value.</typeparam>
        /// <typeparam name="TRight">The type of the Right value.</typeparam>
        /// <typeparam name="TResultRight">The type of the resulting Right value.</typeparam>
        /// <param name="self">The source monad.</param>
        /// <param name="selector">A transform function to apply to each Right value.</param>
        /// <returns>An Either monad containing the projected result.</returns>
        public static IEitherMonad<TLeft, TResultRight> Select<TLeft, TRight, TResultRight>(this IEitherMonad<TLeft, TRight> self, Func<TRight, TResultRight> selector)
            => new SelectCore<TLeft, TRight, TResultRight>(self, selector);

        private struct SelectAsyncCore<TLeft, TRight, TResultRight> : IEitherMonad<TLeft, TResultRight>
        {
            private IEitherMonad<TLeft, TRight> _self;
            private Func<TRight, Task<TResultRight>> _selector;
            public SelectAsyncCore(IEitherMonad<TLeft, TRight> self, Func<TRight, Task<TResultRight>> selector)
            {
                _self = self;
                _selector = selector;
            }

            async Task<EitherResult<TLeft, TResultRight>> IEitherMonad<TLeft, TResultRight>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(selfResult.IsLeft)
                {
                    var leftResult = await ReturnLeft<TLeft, TResultRight>(selfResult.Left).RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return leftResult;
                }
                var rightResult = await ReturnRight<TLeft, TResultRight>(await _selector(selfResult.Right)).RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return rightResult;
            }
        }
        /// <summary>
        /// Projects each element of a monad into a new form asynchronously.
        /// </summary>
        /// <typeparam name="TLeft">The type of the Left value.</typeparam>
        /// <typeparam name="TRight">The type of the Right value.</typeparam>
        /// <typeparam name="TResultRight">The type of the resulting Right value.</typeparam>
        /// <param name="self">The source monad.</param>
        /// <param name="selector">An asynchronous transform function to apply to each Right value.</param>
        /// <returns>An Either monad containing the projected result.</returns>
        public static IEitherMonad<TLeft, TResultRight> Select<TLeft, TRight, TResultRight>(this IEitherMonad<TLeft, TRight> self, Func<TRight, Task<TResultRight>> selector)
            => new SelectAsyncCore<TLeft, TRight, TResultRight>(self, selector);
    }
}
