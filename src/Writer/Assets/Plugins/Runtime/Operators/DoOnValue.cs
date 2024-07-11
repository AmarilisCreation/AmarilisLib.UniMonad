using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Writer
    {
        private class DoOnValueCore<TOutput, TValue> : IWriterMonad<TOutput, TValue>
        {
            private IWriterMonad<TOutput, TValue> _self;
            private Action<TValue> _action;
            public DoOnValueCore(IWriterMonad<TOutput, TValue> self, Action<TValue> action)
            {
                _self = self;
                _action = action;
            }
            async Task<WriterResult<TOutput, TValue>> IWriterMonad<TOutput, TValue>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                _action(result.Value);
                return result;
            }
        }
        /// <summary>
        /// Executes the specified action on the value produced by the writer monad.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The original writer monad.</param>
        /// <param name="action">The action to execute on the value.</param>
        /// <returns>A writer monad that executes the specified action on the value.</returns>
        public static IWriterMonad<TOutput, TValue> DoOnValue<TOutput, TValue>(this IWriterMonad<TOutput, TValue> self, Action<TValue> action)
            => new DoOnValueCore<TOutput, TValue>(self, action);

        private class DoOnValueAsyncCore<TOutput, TValue> : IWriterMonad<TOutput, TValue>
        {
            private IWriterMonad<TOutput, TValue> _self;
            private Func<TValue, Task> _action;
            public DoOnValueAsyncCore(IWriterMonad<TOutput, TValue> self, Func<TValue, Task> action)
            {
                _self = self;
                _action = action;
            }
            async Task<WriterResult<TOutput, TValue>> IWriterMonad<TOutput, TValue>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                await _action(result.Value);
                cancellationToken.ThrowIfCancellationRequested();
                return result;
            }
        }
        /// <summary>
        /// Asynchronously executes the specified action on the value produced by the writer monad.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The original writer monad.</param>
        /// <param name="action">The asynchronous action to execute on the value.</param>
        /// <returns>A writer monad that asynchronously executes the specified action on the value.</returns>
        public static IWriterMonad<TOutput, TValue> DoOnValue<TOutput, TValue>(this IWriterMonad<TOutput, TValue> self, Func<TValue, Task> action)
            => new DoOnValueAsyncCore<TOutput, TValue>(self, action);
    }
}
