using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Either
    {
        private class SelectManyCore<TLeft, TRight, TResultRight> : IEitherMonad<TLeft, TResultRight>
        {
            private IEitherMonad<TLeft, TRight> _self;
            private Func<TRight, IEitherMonad<TLeft, TResultRight>> _selector;
            public SelectManyCore(IEitherMonad<TLeft, TRight> self, Func<TRight, IEitherMonad<TLeft, TResultRight>> selector)
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
                var rightResult = await _selector(selfResult.Right).RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return rightResult;
            }
        }
        /// <summary>
        /// Projects each element of a monad to another monad and flattens the resulting monads into one monad.
        /// </summary>
        /// <typeparam name="TLeft">The type of the Left value.</typeparam>
        /// <typeparam name="TRight">The type of the Right value.</typeparam>
        /// <typeparam name="TResultRight">The type of the resulting Right value.</typeparam>
        /// <param name="self">The source monad.</param>
        /// <param name="selector">A transform function to apply to each Right value.</param>
        /// <returns>An Either monad containing the projected result.</returns>
        public static IEitherMonad<TLeft, TResultRight> SelectMany<TLeft, TRight, TResultRight>(this IEitherMonad<TLeft, TRight> self, Func<TRight, IEitherMonad<TLeft, TResultRight>> selector)
            => new SelectManyCore<TLeft, TRight, TResultRight>(self, selector);

        private class SelectManyCore<TLeft, TFirstRight, TSecondResult, TResultRight> : IEitherMonad<TLeft, TResultRight>
        {
            private IEitherMonad<TLeft, TFirstRight> _self;
            private Func<TFirstRight, IEitherMonad<TLeft, TSecondResult>> _selector;
            private Func<TFirstRight, TSecondResult, TResultRight> _projector;
            public SelectManyCore(IEitherMonad<TLeft, TFirstRight> self, Func<TFirstRight, IEitherMonad<TLeft, TSecondResult>> selector, Func<TFirstRight, TSecondResult, TResultRight> projector)
            {
                _self = self;
                _selector = selector;
                _projector = projector;
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
                var secondResult = await _selector(selfResult.Right).RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(secondResult.IsLeft)
                {
                    var leftResult = await ReturnLeft<TLeft, TResultRight>(secondResult.Left).RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return leftResult;
                }
                var rightResult = await ReturnRight<TLeft, TResultRight>(_projector(selfResult.Right, secondResult.Right)).RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return rightResult;
            }
        }
        /// <summary>
        /// Projects each element of a monad to another monad, flattens the resulting monads into one monad, and applies a result selector function.
        /// </summary>
        /// <typeparam name="TLeft">The type of the Left value.</typeparam>
        /// <typeparam name="TFirstRight">The type of the first Right value.</typeparam>
        /// <typeparam name="TSecondResult">The type of the second result value.</typeparam>
        /// <typeparam name="TResultRight">The type of the resulting Right value.</typeparam>
        /// <param name="self">The source monad.</param>
        /// <param name="selector">A transform function to apply to each first Right value.</param>
        /// <param name="projector">A transform function to apply to the result of the first transformation.</param>
        /// <returns>An Either monad containing the projected result.</returns>
        public static IEitherMonad<TLeft, TResultRight> SelectMany<TLeft, TFirstRight, TSecondResult, TResultRight>(this IEitherMonad<TLeft, TFirstRight> self, Func<TFirstRight, IEitherMonad<TLeft, TSecondResult>> selector, Func<TFirstRight, TSecondResult, TResultRight> projector)
            => new SelectManyCore<TLeft, TFirstRight, TSecondResult, TResultRight>(self, selector, projector);

        private class SelectManyAsyncCore<TLeft, TRight, TResultRight> : IEitherMonad<TLeft, TResultRight>
        {
            private IEitherMonad<TLeft, TRight> _self;
            private Func<TRight, Task<IEitherMonad<TLeft, TResultRight>>> _selector;
            public SelectManyAsyncCore(IEitherMonad<TLeft, TRight> self, Func<TRight, Task<IEitherMonad<TLeft, TResultRight>>> selector)
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
                var rightResult = await (await _selector(selfResult.Right)).RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return rightResult;
            }
        }
        /// <summary>
        /// Projects each element of a monad to another monad asynchronously and flattens the resulting monads into one monad.
        /// </summary>
        /// <typeparam name="TLeft">The type of the Left value.</typeparam>
        /// <typeparam name="TRight">The type of the Right value.</typeparam>
        /// <typeparam name="TResultRight">The type of the resulting Right value.</typeparam>
        /// <param name="self">The source monad.</param>
        /// <param name="selector">An asynchronous transform function to apply to each Right value.</param>
        /// <returns>An Either monad containing the projected result.</returns>
        public static IEitherMonad<TLeft, TResultRight> SelectMany<TLeft, TRight, TResultRight>(this IEitherMonad<TLeft, TRight> self, Func<TRight, Task<IEitherMonad<TLeft, TResultRight>>> selector)
            => new SelectManyAsyncCore<TLeft, TRight, TResultRight>(self, selector);

        private class SelectManyAsyncCore<TLeft, TFirstRight, TSecondResult, TResultRight> : IEitherMonad<TLeft, TResultRight>
        {
            private IEitherMonad<TLeft, TFirstRight> _self;
            private Func<TFirstRight, IEitherMonad<TLeft, TSecondResult>> _selector;
            private Func<TFirstRight, TSecondResult, Task<TResultRight>> _projector;
            public SelectManyAsyncCore(IEitherMonad<TLeft, TFirstRight> self, Func<TFirstRight, IEitherMonad<TLeft, TSecondResult>> selector, Func<TFirstRight, TSecondResult, Task<TResultRight>> projector)
            {
                _self = self;
                _selector = selector;
                _projector = projector;
            }
            async Task<EitherResult<TLeft, TResultRight>> IEitherMonad<TLeft, TResultRight>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(selfResult.IsLeft) {

                    var leftResult = await ReturnLeft<TLeft, TResultRight>(selfResult.Left).RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return leftResult;
                }
                var secondResult = await _selector(selfResult.Right).RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(secondResult.IsLeft) {
                    var leftResult = await ReturnLeft<TLeft, TResultRight>(secondResult.Left).RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return leftResult;
                }
                var rightResult = await ReturnRight<TLeft, TResultRight>(await _projector(selfResult.Right, secondResult.Right)).RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return rightResult;
            }
        }
        /// <summary>
        /// Projects each element of a monad to another monad, flattens the resulting monads into one monad, and applies an asynchronous result selector function.
        /// </summary>
        /// <typeparam name="TLeft">The type of the Left value.</typeparam>
        /// <typeparam name="TFirstRight">The type of the first Right value.</typeparam>
        /// <typeparam name="TSecondResult">The type of the second result value.</typeparam>
        /// <typeparam name="TResultRight">The type of the resulting Right value.</typeparam>
        /// <param name="self">The source monad.</param>
        /// <param name="selector">A transform function to apply to each first Right value.</param>
        /// <param name="projector">An asynchronous transform function to apply to the result of the first transformation.</param>
        /// <returns>An Either monad containing the projected result.</returns>
        public static IEitherMonad<TLeft, TResultRight> SelectMany<TLeft, TFirstRight, TSecondResult, TResultRight>(this IEitherMonad<TLeft, TFirstRight> self, Func<TFirstRight, IEitherMonad<TLeft, TSecondResult>> selector, Func<TFirstRight, TSecondResult, Task<TResultRight>> projector)
            => new SelectManyAsyncCore<TLeft, TFirstRight, TSecondResult, TResultRight>(self, selector, projector);
    }
}
