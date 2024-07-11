using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Either
    {
        private class IfCore<TLeft, TRight> : IEitherMonad<TLeft, TRight>
        {
            private IEitherMonad<TLeft, TRight> _self;
            private IEitherMonad<TLeft, TRight> _elseSource;
            private Func<EitherResult<TLeft, TRight>, bool> _selector;
            public IfCore(IEitherMonad<TLeft, TRight> self, IEitherMonad<TLeft, TRight> elseSource, Func<EitherResult<TLeft, TRight>, bool> selector)
            {
                _self = self;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<EitherResult<TLeft, TRight>> IEitherMonad<TLeft, TRight>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(_selector(result)) return result;
                var elseResult = await _elseSource.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return elseResult;
            }
        }
        /// <summary>
        /// Creates an Either monad that conditionally runs another monad based on the result of the first monad.
        /// </summary>
        /// <typeparam name="TLeft">The type of the Left value.</typeparam>
        /// <typeparam name="TRight">The type of the Right value.</typeparam>
        /// <param name="self">The first monad.</param>
        /// <param name="elseSource">The monad to run if the condition is false.</param>
        /// <param name="selector">The condition to evaluate.</param>
        /// <returns>An Either monad that conditionally runs another monad.</returns>
        public static IEitherMonad<TLeft, TRight> If<TLeft, TRight>(this IEitherMonad<TLeft, TRight> self, IEitherMonad<TLeft, TRight> elseSource, Func<EitherResult<TLeft, TRight>, bool> selector)
            => new IfCore<TLeft, TRight>(self, elseSource, selector);

        private class IfStaticCore<TLeft, TRight> : IEitherMonad<TLeft, TRight>
        {
            private IEitherMonad<TLeft, TRight> _thenSource;
            private IEitherMonad<TLeft, TRight> _elseSource;
            private Func<bool> _selector;
            public IfStaticCore(IEitherMonad<TLeft, TRight> thenSource, IEitherMonad<TLeft, TRight> elseSource, Func<bool> selector)
            {
                _thenSource = thenSource;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<EitherResult<TLeft, TRight>> IEitherMonad<TLeft, TRight>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if(_selector()) {
                    var thenResult = await _thenSource.RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return thenResult;
                }
                var elseResult = await _elseSource.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return elseResult;
            }
        }
        /// <summary>
        /// Creates an Either monad that conditionally runs one of two monads based on a static condition.
        /// </summary>
        /// <typeparam name="TLeft">The type of the Left value.</typeparam>
        /// <typeparam name="TRight">The type of the Right value.</typeparam>
        /// <param name="thenSource">The monad to run if the condition is true.</param>
        /// <param name="elseSource">The monad to run if the condition is false.</param>
        /// <param name="selector">The static condition to evaluate.</param>
        /// <returns>An Either monad that conditionally runs one of two monads.</returns>
        public static IEitherMonad<TLeft, TRight> If<TLeft, TRight>(IEitherMonad<TLeft, TRight> thenSource, IEitherMonad<TLeft, TRight> elseSource, Func<bool> selector)
            => new IfStaticCore<TLeft, TRight>(thenSource, elseSource, selector);

        private class IfAsyncCore<TLeft, TRight> : IEitherMonad<TLeft, TRight>
        {
            private IEitherMonad<TLeft, TRight> _self;
            private IEitherMonad<TLeft, TRight> _elseSource;
            private Func<EitherResult<TLeft, TRight>, Task<bool>> _selector;
            public IfAsyncCore(IEitherMonad<TLeft, TRight> self, IEitherMonad<TLeft, TRight> elseSource, Func<EitherResult<TLeft, TRight>, Task<bool>> selector)
            {
                _self = self;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<EitherResult<TLeft, TRight>> IEitherMonad<TLeft, TRight>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector(result);
                cancellationToken.ThrowIfCancellationRequested();
                if(selectorResult) return result;
                var elseResult = await _elseSource.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return elseResult;
            }
        }
        /// <summary>
        /// Creates an Either monad that conditionally runs another monad based on the asynchronous result of the first monad.
        /// </summary>
        /// <typeparam name="TLeft">The type of the Left value.</typeparam>
        /// <typeparam name="TRight">The type of the Right value.</typeparam>
        /// <param name="self">The first monad.</param>
        /// <param name="elseSource">The monad to run if the condition is false.</param>
        /// <param name="selector">The asynchronous condition to evaluate.</param>
        /// <returns>An Either monad that conditionally runs another monad.</returns>
        public static IEitherMonad<TLeft, TRight> If<TLeft, TRight>(this IEitherMonad<TLeft, TRight> self, IEitherMonad<TLeft, TRight> elseSource, Func<EitherResult<TLeft, TRight>, Task<bool>> selector)
            => new IfAsyncCore<TLeft, TRight>(self, elseSource, selector);

        private class IfStaticAsyncCore<TLeft, TRight> : IEitherMonad<TLeft, TRight>
        {
            private IEitherMonad<TLeft, TRight> _thenSource;
            private IEitherMonad<TLeft, TRight> _elseSource;
            private Task<Func<bool>> _selector;
            public IfStaticAsyncCore(IEitherMonad<TLeft, TRight> thenSource, IEitherMonad<TLeft, TRight> elseSource, Task<Func<bool>> selector)
            {
                _thenSource = thenSource;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<EitherResult<TLeft, TRight>> IEitherMonad<TLeft, TRight>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector;
                if(selectorResult()) {
                    var thenResult = await _thenSource.RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return thenResult;
                }
                var elseResult = await _elseSource.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return elseResult;
            }
        }
        /// <summary>
        /// Creates an Either monad that conditionally runs one of two monads based on an asynchronous static condition.
        /// </summary>
        /// <typeparam name="TLeft">The type of the Left value.</typeparam>
        /// <typeparam name="TRight">The type of the Right value.</typeparam>
        /// <param name="thenSource">The monad to run if the condition is true.</param>
        /// <param name="elseSource">The monad to run if the condition is false.</param>
        /// <param name="selector">The asynchronous static condition to evaluate.</param>
        /// <returns>An Either monad that conditionally runs one of two monads.</returns>
        public static IEitherMonad<TLeft, TRight> If<TLeft, TRight>(IEitherMonad<TLeft, TRight> thenSource, IEitherMonad<TLeft, TRight> elseSource, Task<Func<bool>> selector)
            => new IfStaticAsyncCore<TLeft, TRight>(thenSource, elseSource, selector);
    }
}
