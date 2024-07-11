using System;
using System.Threading.Tasks;
using System.Threading;

namespace AmarilisLib.Monad
{
    public static partial class Try
    {
        private struct IfCore<T> : ITryMonad<T>
        {
            private ITryMonad<T> _self;
            private ITryMonad<T> _elseSource;
            private Func<TryResult<T>, bool> _selector;
            public IfCore(ITryMonad<T> self, ITryMonad<T> elseSource, Func<TryResult<T>, bool> selector)
            {
                _self = self;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<TryResult<T>> ITryMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var result = await _self.RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    if(_selector(result)) return result;
                    else return await _elseSource.RunAsync(cancellationToken);
                }
                catch(Exception exception)
                {
                    return TryResult<T>.Failure(exception);
                }
            }
        }
        /// <summary>
        /// Conditionally runs one of two try monads based on a selector function.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="self">The try monad to run if the selector returns true.</param>
        /// <param name="elseSource">The try monad to run if the selector returns false.</param>
        /// <param name="selector">The function to determine which monad to run.</param>
        /// <returns>A try monad that runs the appropriate monad based on the selector function.</returns>
        public static ITryMonad<T> If<T>(this ITryMonad<T> self, ITryMonad<T> elseSource, Func<TryResult<T>, bool> selector)
            => new IfCore<T>(self, elseSource, selector);

        private struct IfStaticCore<T> : ITryMonad<T>
        {
            private ITryMonad<T> _thenSource;
            private ITryMonad<T> _elseSource;
            private Func<bool> _selector;
            public IfStaticCore(ITryMonad<T> thenSource, ITryMonad<T> elseSource, Func<bool> selector)
            {
                _thenSource = thenSource;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<TryResult<T>> ITryMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    if(_selector())
                    {
                        var thenResult = await _thenSource.RunAsync(cancellationToken);
                        cancellationToken.ThrowIfCancellationRequested();
                        return thenResult;
                    }
                    return await _elseSource.RunAsync(cancellationToken);
                }
                catch(Exception exception)
                {
                    return TryResult<T>.Failure(exception);
                }
            }
        }
        /// <summary>
        /// Conditionally runs one of two try monads based on a static selector function.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="thenSource">The try monad to run if the selector returns true.</param>
        /// <param name="elseSource">The try monad to run if the selector returns false.</param>
        /// <param name="selector">The static function to determine which monad to run.</param>
        /// <returns>A try monad that runs the appropriate monad based on the static selector function.</returns>
        public static ITryMonad<T> If<T>(ITryMonad<T> thenSource, ITryMonad<T> elseSource, Func<bool> selector)
            => new IfStaticCore<T>(thenSource, elseSource, selector);
        private struct IfAsyncCore<T> : ITryMonad<T>
        {
            private ITryMonad<T> _self;
            private ITryMonad<T> _elseSource;
            private Func<TryResult<T>, Task<bool>> _selector;
            public IfAsyncCore(ITryMonad<T> self, ITryMonad<T> elseSource, Func<TryResult<T>, Task<bool>> selector)
            {
                _self = self;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<TryResult<T>> ITryMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                try
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
                catch(Exception exception)
                {
                    return TryResult<T>.Failure(exception);
                }
            }
        }
        /// <summary>
        /// Conditionally runs one of two try monads based on an asynchronous selector function.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="self">The try monad to run if the selector returns true.</param>
        /// <param name="elseSource">The try monad to run if the selector returns false.</param>
        /// <param name="selector">The asynchronous function to determine which monad to run.</param>
        /// <returns>A try monad that runs the appropriate monad based on the asynchronous selector function.</returns>
        public static ITryMonad<T> If<T>(this ITryMonad<T> self, ITryMonad<T> elseSource, Func<TryResult<T>, Task<bool>> selector)
            => new IfAsyncCore<T>(self, elseSource, selector);

        private struct IfStaticAsyncCore<T> : ITryMonad<T>
        {
            private ITryMonad<T> _thenSource;
            private ITryMonad<T> _elseSource;
            private Task<Func<bool>> _selector;
            public IfStaticAsyncCore(ITryMonad<T> thenSource, ITryMonad<T> elseSource, Task<Func<bool>> selector)
            {
                _thenSource = thenSource;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<TryResult<T>> ITryMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var selectorResult = await _selector;
                    cancellationToken.ThrowIfCancellationRequested();
                    if(selectorResult())
                    {
                        var thenResult = await _thenSource.RunAsync(cancellationToken);
                        cancellationToken.ThrowIfCancellationRequested();
                        return thenResult;
                    }
                    var elseResult = await _elseSource.RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return elseResult;
                }
                catch(Exception exception)
                {
                    return TryResult<T>.Failure(exception);
                }
            }
        }
        /// <summary>
        /// Conditionally runs one of two try monads based on an asynchronous static selector function.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="thenSource">The try monad to run if the selector returns true.</param>
        /// <param name="elseSource">The try monad to run if the selector returns false.</param>
        /// <param name="selector">The asynchronous static function to determine which monad to run.</param>
        /// <returns>A try monad that runs the appropriate monad based on the asynchronous static selector function.</returns>
        public static ITryMonad<T> If<T>(ITryMonad<T> thenSource, ITryMonad<T> elseSource, Task<Func<bool>> selector)
            => new IfStaticAsyncCore<T>(thenSource, elseSource, selector);
    }
}