using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class IO
    {
        private class IfCore<T> : IIOMonad<T>
        {
            private IIOMonad<T> _self;
            private IIOMonad<T> _elseSource;
            private Func<T, bool> _selector;
            public IfCore(IIOMonad<T> self, IIOMonad<T> elseSource, Func<T, bool> selector)
            {
                _self = self;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<T> IIOMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(_selector(result)) return result;
                return await _elseSource.RunAsync(cancellationToken);
            }
        }
        /// <summary>
        /// Creates a conditional monad that runs the source monad and applies a condition to its result.
        /// If the condition is true, the result is returned, otherwise the else monad is run and its result is returned.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the monad.</typeparam>
        /// <param name="self">The source monad.</param>
        /// <param name="elseSource">The monad to run if the condition is false.</param>
        /// <param name="selector">The condition to apply to the result of the source monad.</param>
        /// <returns>A conditional monad.</returns>
        public static IIOMonad<T> If<T>(this IIOMonad<T> self, IIOMonad<T> elseSource, Func<T, bool> selector)
            => new IfCore<T>(self, elseSource, selector);

        private class IfStaticCore<T> : IIOMonad<T>
        {
            private IIOMonad<T> _thenSource;
            private IIOMonad<T> _elseSource;
            private Func<bool> _selector;
            public IfStaticCore(IIOMonad<T> thenSource, IIOMonad<T> elseSource, Func<bool> selector)
            {
                _thenSource = thenSource;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<T> IIOMonad<T>.RunAsync(CancellationToken cancellationToken)
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
        /// Creates a conditional monad that runs either the thenSource monad or the elseSource monad based on a static condition.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the monad.</typeparam>
        /// <param name="thenSource">The monad to run if the condition is true.</param>
        /// <param name="elseSource">The monad to run if the condition is false.</param>
        /// <param name="selector">The static condition to determine which monad to run.</param>
        /// <returns>A conditional monad.</returns>
        public static IIOMonad<T> If<T>(IIOMonad<T> thenSource, IIOMonad<T> elseSource, Func<bool> selector)
            => new IfStaticCore<T>(thenSource, elseSource, selector);

        private class IfAsyncCore<T> : IIOMonad<T>
        {
            private IIOMonad<T> _self;
            private IIOMonad<T> _elseSource;
            private Func<T, Task<bool>> _selector;
            public IfAsyncCore(IIOMonad<T> self, IIOMonad<T> elseSource, Func<T, Task<bool>> selector)
            {
                _self = self;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<T> IIOMonad<T>.RunAsync(CancellationToken cancellationToken)
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
        /// Creates a conditional monad that runs the source monad and applies an asynchronous condition to its result.
        /// If the condition is true, the result is returned, otherwise the else monad is run and its result is returned.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the monad.</typeparam>
        /// <param name="self">The source monad.</param>
        /// <param name="elseSource">The monad to run if the condition is false.</param>
        /// <param name="selector">The asynchronous condition to apply to the result of the source monad.</param>
        /// <returns>A conditional monad.</returns>
        public static IIOMonad<T> If<T>(this IIOMonad<T> self, IIOMonad<T> elseSource, Func<T, Task<bool>> selector)
            => new IfAsyncCore<T>(self, elseSource, selector);

        private class IfStaticAsyncCore<T> : IIOMonad<T>
        {
            private IIOMonad<T> _thenSource;
            private IIOMonad<T> _elseSource;
            private Task<Func<bool>> _selector;
            public IfStaticAsyncCore(IIOMonad<T> thenSource, IIOMonad<T> elseSource, Task<Func<bool>> selector)
            {
                _thenSource = thenSource;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<T> IIOMonad<T>.RunAsync(CancellationToken cancellationToken)
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
        }
        /// <summary>
        /// Creates a conditional monad that runs either the thenSource monad or the elseSource monad based on an asynchronous condition.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the monad.</typeparam>
        /// <param name="thenSource">The monad to run if the condition is true.</param>
        /// <param name="elseSource">The monad to run if the condition is false.</param>
        /// <param name="selector">The asynchronous condition to determine which monad to run.</param>
        /// <returns>A conditional monad.</returns>
        public static IIOMonad<T> If<T>(IIOMonad<T> thenSource, IIOMonad<T> elseSource, Task<Func<bool>> selector)
            => new IfStaticAsyncCore<T>(thenSource, elseSource, selector);
    }
}
