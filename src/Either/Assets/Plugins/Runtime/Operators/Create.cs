using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Either
    {
        private class CreateCore<TLeft, TRight> : IEitherMonad<TLeft, TRight>
        {
            private Func<EitherResult<TLeft, TRight>> _func;
            public CreateCore(Func<EitherResult<TLeft, TRight>> func)
            {
                _func = func;
            }
            Task<EitherResult<TLeft, TRight>> IEitherMonad<TLeft, TRight>.RunAsync(CancellationToken cancellationToken)
            {
                return Task.Factory.StartNew(
                    _func,
                    cancellationToken,
                    TaskCreationOptions.DenyChildAttach,
                    TaskScheduler.FromCurrentSynchronizationContext());
            }
        }
        /// <summary>
        /// Creates an Either monad that produces a value by invoking the specified function.
        /// </summary>
        /// <typeparam name="TLeft">The type of the Left value.</typeparam>
        /// <typeparam name="TRight">The type of the Right value.</typeparam>
        /// <param name="func">The function to invoke to produce the Either result.</param>
        /// <returns>An Either monad that produces the value returned by the specified function.</returns>
        public static IEitherMonad<TLeft, TRight> Create<TLeft, TRight>(Func<EitherResult<TLeft, TRight>> func)
            => new CreateCore<TLeft, TRight>(func);
        private class CreateAsyncCore<TLeft, TRight> : IEitherMonad<TLeft, TRight>
        {
            private Func<Task<EitherResult<TLeft, TRight>>> _func;
            public CreateAsyncCore(Func<Task<EitherResult<TLeft, TRight>>> func)
            {
                _func = func;
            }
            Task<EitherResult<TLeft, TRight>> IEitherMonad<TLeft, TRight>.RunAsync(CancellationToken cancellationToken)
            {
                var task = _func();
                cancellationToken.Register(() => task.Dispose());
                return task;
            }
        }
        /// <summary>
        /// Creates an asynchronous either monad using a provided function.
        /// </summary>
        /// <typeparam name="TLeft">The type of the left value.</typeparam>
        /// <typeparam name="TRight">The type of the right value.</typeparam>
        /// <param name="func">The function to create the either result.</param>
        /// <returns>An asynchronous either monad.</returns>
        public static IEitherMonad<TLeft, TRight> Create<TLeft, TRight>(Func<Task<EitherResult<TLeft, TRight>>> func)
            => new CreateAsyncCore<TLeft, TRight>(func);
    }
}
