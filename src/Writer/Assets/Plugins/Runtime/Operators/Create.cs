using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Writer
    {
        private class CreateCore<TOutput, TValue> : IWriterMonad<TOutput, TValue>
        {
            private Func<WriterResult<TOutput, TValue>> _func;
            public CreateCore(Func<WriterResult<TOutput, TValue>> func)
            {
                _func = func;
            }
            Task<WriterResult<TOutput, TValue>> IWriterMonad<TOutput, TValue>.RunAsync(CancellationToken cancellationToken)
            {
                return Task.Factory.StartNew(
                    _func,
                    cancellationToken,
                    TaskCreationOptions.DenyChildAttach,
                    TaskScheduler.FromCurrentSynchronizationContext());
            }
        }
        /// <summary>
        /// Creates a writer monad from the specified function.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="func">The function that defines the writer monad.</param>
        /// <returns>A writer monad created from the specified function.</returns>
        public static IWriterMonad<TOutput, TValue> Create<TOutput, TValue>(Func<WriterResult<TOutput, TValue>> func)
            => new CreateCore<TOutput, TValue>(func);
        private class CreateAsyncCore<TOutput, TValue> : IWriterMonad<TOutput, TValue>
        {
            private Func<Task<WriterResult<TOutput, TValue>>> _func;
            public CreateAsyncCore(Func<Task<WriterResult<TOutput, TValue>>> func)
            {
                _func = func;
            }
            Task<WriterResult<TOutput, TValue>> IWriterMonad<TOutput, TValue>.RunAsync(CancellationToken cancellationToken)
            {
                var task = _func();
                cancellationToken.Register(() => task.Dispose());
                return task;
            }
        }
        /// <summary>
        /// Creates an asynchronous writer monad using a provided function.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="func">The function to create the writer result.</param>
        /// <returns>An asynchronous writer monad.</returns>
        public static IWriterMonad<TOutput, TValue> Create<TOutput, TValue>(Func<Task<WriterResult<TOutput, TValue>>> func)
            => new CreateAsyncCore<TOutput, TValue>(func);
    }
}
