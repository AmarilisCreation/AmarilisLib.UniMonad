using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Writer
    {
        private class SelectManyCore<TOutput, TValue, TResult> : IWriterMonad<TOutput, TResult>
        {
            private IWriterMonad<TOutput, TValue> _self;
            private Func<TValue, IWriterMonad<TOutput, TResult>> _selector;
            public SelectManyCore(IWriterMonad<TOutput, TValue> self, Func<TValue, IWriterMonad<TOutput, TResult>> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<WriterResult<TOutput, TResult>> IWriterMonad<TOutput, TResult>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _selector(selfResult.Value).RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return result;
            }
        }
        /// <summary>
        /// Projects each value of the writer monad into a new monad and flattens the result.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TValue">The type of the value in the original monad.</typeparam>
        /// <typeparam name="TResult">The type of the value in the resulting monad.</typeparam>
        /// <param name="self">The original writer monad.</param>
        /// <param name="selector">A function to apply to each value of the original monad.</param>
        /// <returns>A writer monad with the result of applying the selector and flattening the result.</returns>
        public static IWriterMonad<TOutput, TResult> SelectMany<TOutput, TValue, TResult>(this IWriterMonad<TOutput, TValue> self, Func<TValue, IWriterMonad<TOutput, TResult>> selector)
            => new SelectManyCore<TOutput, TValue, TResult>(self, selector);

        private class SelectManyCore<TOutput, TFirst, TSecond, TResult> : IWriterMonad<TOutput, TResult>
        {
            private IWriterMonad<TOutput, TFirst> _self;
            private Func<TFirst, IWriterMonad<TOutput, TSecond>> _selector;
            private Func<TFirst, TSecond, TResult> _projector;
            public SelectManyCore(IWriterMonad<TOutput, TFirst> self, Func<TFirst, IWriterMonad<TOutput, TSecond>> selector, Func<TFirst, TSecond, TResult> projector)
            {
                _self = self;
                _selector = selector;
                _projector = projector;
            }
            async Task<WriterResult<TOutput, TResult>> IWriterMonad<TOutput, TResult>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var firstResult = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var secondResult = await _selector(firstResult.Value).RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return new WriterResult<TOutput, TResult>(_projector(firstResult.Value, secondResult.Value), firstResult.Output.Concat(secondResult.Output));
            }
        }
        /// <summary>
        /// Projects each value of the writer monad into a new monad and flattens the result.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TValue">The type of the value in the original monad.</typeparam>
        /// <typeparam name="TResult">The type of the value in the resulting monad.</typeparam>
        /// <param name="self">The original writer monad.</param>
        /// <param name="selector">A function to apply to each value of the original monad.</param>
        /// <returns>A writer monad with the result of applying the selector and flattening the result.</returns>
        public static IWriterMonad<TOutput, TResult> SelectMany<TOutput, TFirst, TSecond, TResult>(this IWriterMonad<TOutput, TFirst> self, Func<TFirst, IWriterMonad<TOutput, TSecond>> selector, Func<TFirst, TSecond, TResult> projector)
            => new SelectManyCore<TOutput, TFirst, TSecond, TResult>(self, selector, projector);

        private class SelectManyAsyncCore<TOutput, TValue, TResult> : IWriterMonad<TOutput, TResult>
        {
            private IWriterMonad<TOutput, TValue> _self;
            private Func<TValue, Task<IWriterMonad<TOutput, TResult>>> _selector;
            public SelectManyAsyncCore(IWriterMonad<TOutput, TValue> self, Func<TValue, Task<IWriterMonad<TOutput, TResult>>> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<WriterResult<TOutput, TResult>> IWriterMonad<TOutput, TResult>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector(selfResult.Value);
                cancellationToken.ThrowIfCancellationRequested();
                var result = await (selectorResult).RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return result;
            }
        }
        /// <summary>
        /// Projects each value of the writer monad into a new monad asynchronously and flattens the result.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TValue">The type of the value in the original monad.</typeparam>
        /// <typeparam name="TResult">The type of the value in the resulting monad.</typeparam>
        /// <param name="self">The original writer monad.</param>
        /// <param name="selector">A function to asynchronously apply to each value of the original monad.</param>
        /// <returns>A writer monad with the result of applying the selector and flattening the result.</returns>
        public static IWriterMonad<TOutput, TResult> SelectMany<TOutput, TValue, TResult>(this IWriterMonad<TOutput, TValue> self, Func<TValue, Task<IWriterMonad<TOutput, TResult>>> selector)
            => new SelectManyAsyncCore<TOutput, TValue, TResult>(self, selector);

        private class SelectManyAsyncCore<TOutput, TFirst, TSecond, TResult> : IWriterMonad<TOutput, TResult>
        {
            private IWriterMonad<TOutput, TFirst> _self;
            private Func<TFirst, IWriterMonad<TOutput, TSecond>> _selector;
            private Func<TFirst, TSecond, Task<TResult>> _projector;
            public SelectManyAsyncCore(IWriterMonad<TOutput, TFirst> self, Func<TFirst, IWriterMonad<TOutput, TSecond>> selector, Func<TFirst, TSecond, Task<TResult>> projector)
            {
                _self = self;
                _selector = selector;
                _projector = projector;
            }
            async Task<WriterResult<TOutput, TResult>> IWriterMonad<TOutput, TResult>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var firstResult = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var secondResult = await _selector(firstResult.Value).RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var projectorResult = await _projector(firstResult.Value, secondResult.Value);
                cancellationToken.ThrowIfCancellationRequested();
                return new WriterResult<TOutput, TResult>(projectorResult, firstResult.Output.Concat(secondResult.Output));
            }
        }
        /// <summary>
        /// Projects each value of the writer monad into a new monad asynchronously, flattens the result, and applies a result selector function.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TFirst">The type of the first value.</typeparam>
        /// <typeparam name="TSecond">The type of the second value.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="self">The original writer monad.</param>
        /// <param name="selector">A function to apply to each value of the original monad.</param>
        /// <param name="projector">A function to combine the values asynchronously.</param>
        /// <returns>A writer monad with the result of applying the selector and projector functions.</returns>
        public static IWriterMonad<TOutput, TResult> SelectMany<TOutput, TFirst, TSecond, TResult>(this IWriterMonad<TOutput, TFirst> self, Func<TFirst, IWriterMonad<TOutput, TSecond>> selector, Func<TFirst, TSecond, Task<TResult>> projector)
            => new SelectManyAsyncCore<TOutput, TFirst, TSecond, TResult>(self, selector, projector);
    }
}
