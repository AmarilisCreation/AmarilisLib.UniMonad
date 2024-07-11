using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Either
    {
        private class DoOnLeftCore<TLeft, TRight> : IEitherMonad<TLeft, TRight>
        {
            private IEitherMonad<TLeft, TRight> _self;
            private Action<TLeft> _action;
            public DoOnLeftCore(IEitherMonad<TLeft, TRight> self, Action<TLeft> action)
            {
                _self = self;
                _action = action;
            }
            async Task<EitherResult<TLeft, TRight>> IEitherMonad<TLeft, TRight>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(result.IsLeft) _action(result.Left);
                return result;
            }
        }
        /// <summary>
        /// Creates an Either monad that performs an action on the Left value if it exists.
        /// </summary>
        /// <typeparam name="TLeft">The type of the Left value.</typeparam>
        /// <typeparam name="TRight">The type of the Right value.</typeparam>
        /// <param name="self">The source monad.</param>
        /// <param name="action">The action to perform on the Left value.</param>
        /// <returns>An Either monad that performs the specified action on the Left value.</returns>
        public static IEitherMonad<TLeft, TRight> DoOnLeft<TLeft, TRight>(this IEitherMonad<TLeft, TRight> self, Action<TLeft> action)
            => new DoOnLeftCore<TLeft, TRight>(self, action);

        private class DoOnLeftAsyncCore<TLeft, TRight> : IEitherMonad<TLeft, TRight>
        {
            private IEitherMonad<TLeft, TRight> _self;
            private Func<TLeft, Task> _action;
            public DoOnLeftAsyncCore(IEitherMonad<TLeft, TRight> self, Func<TLeft, Task> action)
            {
                _self = self;
                _action = action;
            }
            async Task<EitherResult<TLeft, TRight>> IEitherMonad<TLeft, TRight>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(result.IsLeft) {
                    await _action(result.Left);
                    cancellationToken.ThrowIfCancellationRequested();
                }
                return result;
            }
        }
        /// <summary>
        /// Creates an Either monad that performs an asynchronous action on the Left value if it exists.
        /// </summary>
        /// <typeparam name="TLeft">The type of the Left value.</typeparam>
        /// <typeparam name="TRight">The type of the Right value.</typeparam>
        /// <param name="self">The source monad.</param>
        /// <param name="action">The asynchronous action to perform on the Left value.</param>
        /// <returns>An Either monad that performs the specified asynchronous action on the Left value.</returns>
        public static IEitherMonad<TLeft, TRight> DoOnLeft<TLeft, TRight>(this IEitherMonad<TLeft, TRight> self, Func<TLeft, Task> action)
            => new DoOnLeftAsyncCore<TLeft, TRight>(self, action);
    }
}
