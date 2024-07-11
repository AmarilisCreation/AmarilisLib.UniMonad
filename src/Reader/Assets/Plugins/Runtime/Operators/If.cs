using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Reader
    {
        private class IfCore<TEnvironment, TValue> : IReaderMonad<TEnvironment, TValue>
        {
            private IReaderMonad<TEnvironment, TValue> _self;
            private IReaderMonad<TEnvironment, TValue> _elseSource;
            private Func<TValue, bool> _selector;
            public IfCore(IReaderMonad<TEnvironment, TValue> self, IReaderMonad<TEnvironment, TValue> elseSource, Func<TValue, bool> selector)
            {
                _self = self;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<TValue> IReaderMonad<TEnvironment, TValue>.RunAsync(TEnvironment environment, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(environment, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(_selector(result))
                {
                    return result;
                }
                return await _elseSource.RunAsync(environment, cancellationToken);
            }
        }
        /// <summary>
        /// Executes one of two Reader monads based on a condition evaluated on the result of the first monad.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The primary Reader monad.</param>
        /// <param name="elseSource">The Reader monad to execute if the condition is false.</param>
        /// <param name="selector">A function to evaluate the condition on the result of the primary Reader monad.</param>
        /// <returns>A Reader monad that executes one of the two Reader monads based on the condition.</returns>
        public static IReaderMonad<TEnvironment, TValue> If<TEnvironment, TValue>(this IReaderMonad<TEnvironment, TValue> self, IReaderMonad<TEnvironment, TValue> elseSource, Func<TValue, bool> selector)
            => new IfCore<TEnvironment, TValue>(self, elseSource, selector);

        private class IfStaticCore<TEnvironment, TValue> : IReaderMonad<TEnvironment, TValue>
        {
            private IReaderMonad<TEnvironment, TValue> _thenSource;
            private IReaderMonad<TEnvironment, TValue> _elseSource;
            private Func<bool> _selector;
            public IfStaticCore(IReaderMonad<TEnvironment, TValue> thenSource, IReaderMonad<TEnvironment, TValue> elseSource, Func<bool> selector)
            {
                _thenSource = thenSource;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<TValue> IReaderMonad<TEnvironment, TValue>.RunAsync(TEnvironment environment, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if(_selector())
                {
                    var thenResult = await _thenSource.RunAsync(environment, cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return thenResult;
                }
                var elseResult = await _elseSource.RunAsync(environment, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return elseResult;
            }
        }
        /// <summary>
        /// Executes one of two static Reader monads based on a condition.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="thenSource">The Reader monad to execute if the condition is true.</param>
        /// <param name="elseSource">The Reader monad to execute if the condition is false.</param>
        /// <param name="selector">A function to evaluate the condition.</param>
        /// <returns>A Reader monad that executes one of the two static Reader monads based on the condition.</returns>
        public static IReaderMonad<TEnvironment, TValue> If<TEnvironment, TValue>(IReaderMonad<TEnvironment, TValue> thenSource, IReaderMonad<TEnvironment, TValue> elseSource, Func<bool> selector)
            => new IfStaticCore<TEnvironment, TValue>(thenSource, elseSource, selector);

        private class IfAsyncCore<TEnvironment, TValue> : IReaderMonad<TEnvironment, TValue>
        {
            private IReaderMonad<TEnvironment, TValue> _self;
            private IReaderMonad<TEnvironment, TValue> _elseSource;
            private Func<TValue, Task<bool>> _selector;
            public IfAsyncCore(IReaderMonad<TEnvironment, TValue> self, IReaderMonad<TEnvironment, TValue> elseSource, Func<TValue, Task<bool>> selector)
            {
                _self = self;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<TValue> IReaderMonad<TEnvironment, TValue>.RunAsync(TEnvironment environment, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(environment, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector(result);
                cancellationToken.ThrowIfCancellationRequested();
                if(selectorResult) return result;
                var elseResult = await _elseSource.RunAsync(environment, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return result;
            }
        }
        /// <summary>
        /// Executes one of two Reader monads based on an asynchronous condition evaluated on the result of the first monad.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The primary Reader monad.</param>
        /// <param name="elseSource">The Reader monad to execute if the condition is false.</param>
        /// <param name="selector">An asynchronous function to evaluate the condition on the result of the primary Reader monad.</param>
        /// <returns>A Reader monad that executes one of the two Reader monads based on the asynchronous condition.</returns>
        public static IReaderMonad<TEnvironment, TValue> If<TEnvironment, TValue>(this IReaderMonad<TEnvironment, TValue> self, IReaderMonad<TEnvironment, TValue> elseSource, Func<TValue, Task<bool>> selector)
            => new IfAsyncCore<TEnvironment, TValue>(self, elseSource, selector);

        private class IfStaticAsyncCore<TEnvironment, TValue> : IReaderMonad<TEnvironment, TValue>
        {
            private IReaderMonad<TEnvironment, TValue> _thenSource;
            private IReaderMonad<TEnvironment, TValue> _elseSource;
            private Task<Func<bool>> _selector;
            public IfStaticAsyncCore(IReaderMonad<TEnvironment, TValue> thenSource, IReaderMonad<TEnvironment, TValue> elseSource, Task<Func<bool>> selector)
            {
                _thenSource = thenSource;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<TValue> IReaderMonad<TEnvironment, TValue>.RunAsync(TEnvironment environment, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector;
                cancellationToken.ThrowIfCancellationRequested();
                if(selectorResult()) {
                    var thenResult = await _thenSource.RunAsync(environment, cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return thenResult;
                }
                var elseResult = await _elseSource.RunAsync(environment, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return elseResult;
            }
        }
        /// <summary>
        /// Executes one of two static Reader monads based on an asynchronous condition.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="thenSource">The Reader monad to execute if the condition is true.</param>
        /// <param name="elseSource">The Reader monad to execute if the condition is false.</param>
        /// <param name="selector">An asynchronous function to evaluate the condition.</param>
        /// <returns>A Reader monad that executes one of the two static Reader monads based on the asynchronous condition.</returns>
        public static IReaderMonad<TEnvironment, TValue> If<TEnvironment, TValue>(IReaderMonad<TEnvironment, TValue> thenSource, IReaderMonad<TEnvironment, TValue> elseSource, Task<Func<bool>> selector)
            => new IfStaticAsyncCore<TEnvironment, TValue>(thenSource, elseSource, selector);
    }
}
