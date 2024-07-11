using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Either
    {
        private class DoCore<TLeft, TRight> : IEitherMonad<TLeft, TRight>
        {
            private IEitherMonad<TLeft, TRight> _self;
            private Action<EitherResult<TLeft, TRight>> _action;
            public DoCore(IEitherMonad<TLeft, TRight> self, Action<EitherResult<TLeft, TRight>> action)
            {
                _self = self;
                _action = action;
            }
            async Task<EitherResult<TLeft, TRight>> IEitherMonad<TLeft, TRight>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                _action(result);
                return result;
            }
        }
        /// <summary>
        /// Creates an Either monad that performs an action on the result.
        /// </summary>
        /// <typeparam name="TLeft">The type of the Left value.</typeparam>
        /// <typeparam name="TRight">The type of the Right value.</typeparam>
        /// <param name="self">The source monad.</param>
        /// <param name="action">The action to perform on the result.</param>
        /// <returns>An Either monad that performs the specified action on the result.</returns>
        public static IEitherMonad<TLeft, TRight> Do<TLeft, TRight>(this IEitherMonad<TLeft, TRight> self, Action<EitherResult<TLeft, TRight>> action)
            => new DoCore<TLeft, TRight>(self, action);

        private class DoAsyncCore<TLeft, TRight> : IEitherMonad<TLeft, TRight>
        {
            private IEitherMonad<TLeft, TRight> _self;
            private Func<EitherResult<TLeft, TRight>, Task> _action;
            public DoAsyncCore(IEitherMonad<TLeft, TRight> self, Func<EitherResult<TLeft, TRight>, Task> action)
            {
                _self = self;
                _action = action;
            }
            async Task<EitherResult<TLeft, TRight>> IEitherMonad<TLeft, TRight>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                await _action(result);
                cancellationToken.ThrowIfCancellationRequested();
                return result;
            }
        }
        /// <summary>
        /// Creates an Either monad that performs an asynchronous action on the result.
        /// </summary>
        /// <typeparam name="TLeft">The type of the Left value.</typeparam>
        /// <typeparam name="TRight">The type of the Right value.</typeparam>
        /// <param name="self">The source monad.</param>
        /// <param name="action">The asynchronous action to perform on the result.</param>
        /// <returns>An Either monad that performs the specified asynchronous action on the result.</returns>
        public static IEitherMonad<TLeft, TRight> Do<TLeft, TRight>(this IEitherMonad<TLeft, TRight> self, Func<EitherResult<TLeft, TRight>, Task> action)
            => new DoAsyncCore<TLeft, TRight>(self, action);
    }
}
