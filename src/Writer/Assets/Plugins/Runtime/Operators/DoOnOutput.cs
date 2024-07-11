using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Writer
    {
        private class DoOnOutputCore<TOutput, TValue> : IWriterMonad<TOutput, TValue>
        {
            private IWriterMonad<TOutput, TValue> _self;
            private Action<IEnumerable<TOutput>> _action;
            public DoOnOutputCore(IWriterMonad<TOutput, TValue> self, Action<IEnumerable<TOutput>> action)
            {
                _self = self;
                _action = action;
            }
            async Task<WriterResult<TOutput, TValue>> IWriterMonad<TOutput, TValue>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                _action(result.Output);
                return result;
            }
        }
        /// <summary>
        /// Executes the specified action on the output produced by the writer monad.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The original writer monad.</param>
        /// <param name="action">The action to execute on the output.</param>
        /// <returns>A writer monad that executes the specified action on the output.</returns>
        public static IWriterMonad<TOutput, TValue> DoOnOutput<TOutput, TValue>(this IWriterMonad<TOutput, TValue> self, Action<IEnumerable<TOutput>> action)
            => new DoOnOutputCore<TOutput, TValue>(self, action);

        private class DoOnOutputAsyncCore<TOutput, TValue> : IWriterMonad<TOutput, TValue>
        {
            private IWriterMonad<TOutput, TValue> _self;
            private Func<IEnumerable<TOutput>, Task> _action;
            public DoOnOutputAsyncCore(IWriterMonad<TOutput, TValue> self, Func<IEnumerable<TOutput>, Task> action)
            {
                _self = self;
                _action = action;
            }
            async Task<WriterResult<TOutput, TValue>> IWriterMonad<TOutput, TValue>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                await _action(result.Output);
                cancellationToken.ThrowIfCancellationRequested();
                return result;
            }
        }
        /// <summary>
        /// Asynchronously executes the specified action on the output produced by the writer monad.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The original writer monad.</param>
        /// <param name="action">The asynchronous action to execute on the output.</param>
        /// <returns>A writer monad that asynchronously executes the specified action on the output.</returns>
        public static IWriterMonad<TOutput, TValue> DoOnOutput<TOutput, TValue>(this IWriterMonad<TOutput, TValue> self, Func<IEnumerable<TOutput>, Task> action)
            => new DoOnOutputAsyncCore<TOutput, TValue>(self, action);
    }
}
