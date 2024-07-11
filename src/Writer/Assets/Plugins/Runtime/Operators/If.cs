using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Writer
    {
        private class IfCore<TOutput, TValue> : IWriterMonad<TOutput, TValue>
        {
            private IWriterMonad<TOutput, TValue> _self;
            private IWriterMonad<TOutput, TValue> _elseSource;
            private Func<WriterResult<TOutput, TValue>, bool> _selector;
            public IfCore(IWriterMonad<TOutput, TValue> self, IWriterMonad<TOutput, TValue> elseSource, Func<WriterResult<TOutput, TValue>, bool> selector)
            {
                _self = self;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<WriterResult<TOutput, TValue>> IWriterMonad<TOutput, TValue>.RunAsync(CancellationToken cancellationToken)
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
        /// Conditionally executes one of two writer monads based on a selector function applied to the result of the first monad.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The initial writer monad.</param>
        /// <param name="elseSource">The writer monad to execute if the selector returns false.</param>
        /// <param name="selector">A function to determine which writer monad to execute based on the result of the first monad.</param>
        /// <returns>A writer monad that conditionally executes one of two writer monads.</returns>
        public static IWriterMonad<TOutput, TValue> If<TOutput, TValue>(this IWriterMonad<TOutput, TValue> self, IWriterMonad<TOutput, TValue> elseSource, Func<WriterResult<TOutput, TValue>, bool> selector)
            => new IfCore<TOutput, TValue>(self, elseSource, selector);

        private class IfStaticCore<TOutput, TValue> : IWriterMonad<TOutput, TValue>
        {
            private IWriterMonad<TOutput, TValue> _thenSource;
            private IWriterMonad<TOutput, TValue> _elseSource;
            private Func<bool> _selector;
            public IfStaticCore(IWriterMonad<TOutput, TValue> thenSource, IWriterMonad<TOutput, TValue> elseSource, Func<bool> selector)
            {
                _thenSource = thenSource;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<WriterResult<TOutput, TValue>> IWriterMonad<TOutput, TValue>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if(_selector())
                {
                    var thenResult =  await _thenSource.RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return thenResult;
                }
                var elseresult = await _elseSource.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return elseresult;
            }
        }
        /// <summary>
        /// Conditionally executes one of two writer monads based on a static selector function.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="thenSource">The writer monad to execute if the selector returns true.</param>
        /// <param name="elseSource">The writer monad to execute if the selector returns false.</param>
        /// <param name="selector">A static function to determine which writer monad to execute.</param>
        /// <returns>A writer monad that conditionally executes one of two writer monads.</returns>
        public static IWriterMonad<TOutput, TValue> If<TOutput, TValue>(IWriterMonad<TOutput, TValue> thenSource, IWriterMonad<TOutput, TValue> elseSource, Func<bool> selector)
            => new IfStaticCore<TOutput, TValue>(thenSource, elseSource, selector);

        private class IfAsyncCore<TOutput, TValue> : IWriterMonad<TOutput, TValue>
        {
            private IWriterMonad<TOutput, TValue> _self;
            private IWriterMonad<TOutput, TValue> _elseSource;
            private Func<WriterResult<TOutput, TValue>, Task<bool>> _selector;
            public IfAsyncCore(IWriterMonad<TOutput, TValue> self, IWriterMonad<TOutput, TValue> elseSource, Func<WriterResult<TOutput, TValue>, Task<bool>> selector)
            {
                _self = self;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<WriterResult<TOutput, TValue>> IWriterMonad<TOutput, TValue>.RunAsync(CancellationToken cancellationToken)
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
        /// Conditionally executes one of two writer monads based on an asynchronous selector function applied to the result of the first monad.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The initial writer monad.</param>
        /// <param name="elseSource">The writer monad to execute if the selector returns false.</param>
        /// <param name="selector">An asynchronous function to determine which writer monad to execute based on the result of the first monad.</param>
        /// <returns>A writer monad that conditionally executes one of two writer monads.</returns>
        public static IWriterMonad<TOutput, TValue> If<TOutput, TValue>(this IWriterMonad<TOutput, TValue> self, IWriterMonad<TOutput, TValue> elseSource, Func<WriterResult<TOutput, TValue>, Task<bool>> selector)
            => new IfAsyncCore<TOutput, TValue>(self, elseSource, selector);

        private class IfStaticAsyncCore<TOutput, TValue> : IWriterMonad<TOutput, TValue>
        {
            private IWriterMonad<TOutput, TValue> _thenSource;
            private IWriterMonad<TOutput, TValue> _elseSource;
            private Task<Func<bool>> _selector;
            public IfStaticAsyncCore(IWriterMonad<TOutput, TValue> thenSource, IWriterMonad<TOutput, TValue> elseSource, Task<Func<bool>> selector)
            {
                _thenSource = thenSource;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<WriterResult<TOutput, TValue>> IWriterMonad<TOutput, TValue>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector;
                cancellationToken.ThrowIfCancellationRequested();
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
        /// Conditionally executes one of two writer monads based on an asynchronous selector function.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="thenSource">The writer monad to execute if the selector returns true.</param>
        /// <param name="elseSource">The writer monad to execute if the selector returns false.</param>
        /// <param name="selector">An asynchronous function to determine which writer monad to execute.</param>
        /// <returns>A writer monad that conditionally executes one of two writer monads.</returns>
        public static IWriterMonad<TOutput, TValue> If<TOutput, TValue>(IWriterMonad<TOutput, TValue> thenSource, IWriterMonad<TOutput, TValue> elseSource, Task<Func<bool>> selector)
            => new IfStaticAsyncCore<TOutput, TValue>(thenSource, elseSource, selector);
    }
}
