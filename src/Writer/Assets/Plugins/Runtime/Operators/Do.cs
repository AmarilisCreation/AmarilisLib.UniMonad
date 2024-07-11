using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Writer
    {
        private class DoCore<TOutput, TValue> : IWriterMonad<TOutput, TValue>
        {
            private IWriterMonad<TOutput, TValue> _self;
            private Action<WriterResult<TOutput, TValue>> _action;
            public DoCore(IWriterMonad<TOutput, TValue> self, Action<WriterResult<TOutput, TValue>> action)
            {
                _self = self;
                _action = action;
            }
            async Task<WriterResult<TOutput, TValue>> IWriterMonad<TOutput, TValue>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                _action(result);
                return result;
            }
        }
        /// <summary>
        /// Executes the specified action on the result produced by the writer monad.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The original writer monad.</param>
        /// <param name="action">The action to execute on the result.</param>
        /// <returns>A writer monad that executes the specified action on the result.</returns>
        public static IWriterMonad<TOutput, TValue> Do<TOutput, TValue>(this IWriterMonad<TOutput, TValue> self, Action<WriterResult<TOutput, TValue>> action)
            => new DoCore<TOutput, TValue>(self, action);

        private class DoAsyncCore<TOutput, TValue> : IWriterMonad<TOutput, TValue>
        {
            private IWriterMonad<TOutput, TValue> _self;
            private Func<WriterResult<TOutput, TValue>, Task> _action;
            public DoAsyncCore(IWriterMonad<TOutput, TValue> self, Func<WriterResult<TOutput, TValue>, Task> action)
            {
                _self = self;
                _action = action;
            }
            async Task<WriterResult<TOutput, TValue>> IWriterMonad<TOutput, TValue>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                await _action(result);
                cancellationToken.ThrowIfCancellationRequested();
                return result;
            }
        }
        /// <summary>
        /// Asynchronously executes the specified action on the result produced by the writer monad.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The original writer monad.</param>
        /// <param name="action">The asynchronous action to execute on the result.</param>
        /// <returns>A writer monad that asynchronously executes the specified action on the result.</returns>
        public static IWriterMonad<TOutput, TValue> Do<TOutput, TValue>(this IWriterMonad<TOutput, TValue> self, Func<WriterResult<TOutput, TValue>, Task> action)
            => new DoAsyncCore<TOutput, TValue>(self, action);
    }
}
