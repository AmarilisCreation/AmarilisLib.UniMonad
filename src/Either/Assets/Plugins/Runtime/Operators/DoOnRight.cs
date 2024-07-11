using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Either
    {
        private class DoOnRightCore<TLeft, TRight> : IEitherMonad<TLeft, TRight>
        {
            private IEitherMonad<TLeft, TRight> _self;
            private Action<TRight> _action;
            public DoOnRightCore(IEitherMonad<TLeft, TRight> self, Action<TRight> action)
            {
                _self = self;
                _action = action;
            }
            async Task<EitherResult<TLeft, TRight>> IEitherMonad<TLeft, TRight>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(result.IsRight) _action(result.Right);
                return result;
            }
        }
        /// <summary>
        /// Creates an Either monad that performs an action on the Right value if it exists.
        /// </summary>
        /// <typeparam name="TLeft">The type of the Left value.</typeparam>
        /// <typeparam name="TRight">The type of the Right value.</typeparam>
        /// <param name="self">The source monad.</param>
        /// <param name="action">The action to perform on the Right value.</param>
        /// <returns>An Either monad that performs the specified action on the Right value.</returns>
        public static IEitherMonad<TLeft, TRight> DoOnRight<TLeft, TRight>(this IEitherMonad<TLeft, TRight> self, Action<TRight> action)
            => new DoOnRightCore<TLeft, TRight>(self, action);
        private class DoOnRightAsyncCore<TLeft, TRight> : IEitherMonad<TLeft, TRight>
        {
            private IEitherMonad<TLeft, TRight> _self;
            private Func<TRight, Task> _action;
            public DoOnRightAsyncCore(IEitherMonad<TLeft, TRight> self, Func<TRight, Task> action)
            {
                _self = self;
                _action = action;
            }
            async Task<EitherResult<TLeft, TRight>> IEitherMonad<TLeft, TRight>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(result.IsRight)
                {
                    await _action(result.Right);
                    cancellationToken.ThrowIfCancellationRequested();
                }
                return result;
            }
        }
        /// <summary>
        /// Creates an Either monad that performs an asynchronous action on the Right value if it exists.
        /// </summary>
        /// <typeparam name="TLeft">The type of the Left value.</typeparam>
        /// <typeparam name="TRight">The type of the Right value.</typeparam>
        /// <param name="self">The source monad.</param>
        /// <param name="action">The asynchronous action to perform on the Right value.</param>
        /// <returns>An Either monad that performs the specified asynchronous action on the Right value.</returns>
        public static IEitherMonad<TLeft, TRight> DoOnRight<TLeft, TRight>(this IEitherMonad<TLeft, TRight> self, Func<TRight, Task> action)
            => new DoOnRightAsyncCore<TLeft, TRight>(self, action);
    }
}
