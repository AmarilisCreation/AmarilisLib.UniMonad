using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Writer
    {
        private class SelectCore<TOutput, TValue, TResult> : IWriterMonad<TOutput, TResult>
        {
            private IWriterMonad<TOutput, TValue> _self;
            private Func<TValue, TResult> _selector;
            public SelectCore(IWriterMonad<TOutput, TValue> self, Func<TValue, TResult> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<WriterResult<TOutput, TResult>> IWriterMonad<TOutput, TResult>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return new WriterResult<TOutput, TResult>(_selector(selfResult.Value), selfResult.Output);
            }
        }
        /// <summary>
        /// Projects each value of the writer monad into a new form using the provided selector function.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TValue">The type of the value in the original monad.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="self">The original writer monad.</param>
        /// <param name="selector">A function to apply to each value of the original monad.</param>
        /// <returns>A writer monad with the result of applying the selector function.</returns>
        public static IWriterMonad<TOutput, TResult> Select<TOutput, TValue, TResult>(this IWriterMonad<TOutput, TValue> self, Func<TValue, TResult> selector)
            => new SelectCore<TOutput, TValue, TResult>(self, selector);
        private class SelectAsyncCore<TOutput, TValue, TResult> : IWriterMonad<TOutput, TResult>
        {
            private IWriterMonad<TOutput, TValue> _self;
            private Func<TValue, Task<TResult>> _selector;
            public SelectAsyncCore(IWriterMonad<TOutput, TValue> self, Func<TValue, Task<TResult>> selector)
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
                return new WriterResult<TOutput, TResult>(selectorResult, selfResult.Output);
            }
        }
        /// <summary>
        /// Projects each value of the writer monad into a new form using the provided asynchronous selector function.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TValue">The type of the value in the original monad.</typeparam>
        /// <typeparam name="TResult">The type of the resulting value.</typeparam>
        /// <param name="self">The original writer monad.</param>
        /// <param name="selector">A function to asynchronously apply to each value of the original monad.</param>
        /// <returns>A writer monad with the result of applying the selector function.</returns>
        public static IWriterMonad<TOutput, TResult> Select<TOutput, TValue, TResult>(this IWriterMonad<TOutput, TValue> self, Func<TValue, Task<TResult>> selector)
            => new SelectAsyncCore<TOutput, TValue, TResult>(self, selector);
    }
}
