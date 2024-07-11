using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class IO
    {
        private class CreateCore<T> : IIOMonad<T>
        {
            private Func<T> _func;
            public CreateCore(Func<T> func)
            {
                _func = func;
            }
            Task<T> IIOMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                return Task.Factory.StartNew(
                    _func,
                    cancellationToken,
                    TaskCreationOptions.DenyChildAttach,
                    TaskScheduler.FromCurrentSynchronizationContext());
            }
        }
        /// <summary>
        /// Creates a monad that produces a value by invoking the specified function.
        /// </summary>
        /// <typeparam name="T">The type of the value produced by the function.</typeparam>
        /// <param name="func">The function to invoke to produce the value.</param>
        /// <returns>A monad that produces the value returned by the specified function.</returns>
        public static IIOMonad<T> Create<T>(Func<T> func)
            => new CreateCore<T>(func);
        private class CreateAsyncCore<T> : IIOMonad<T>
        {
            private Func<Task<T>> _func;
            public CreateAsyncCore(Func<Task<T>> func)
            {
                _func = func;
            }
            Task<T> IIOMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                var task = _func();
                cancellationToken.Register(() => task.Dispose());
                return task;
            }
        }
        /// <summary>
        /// Creates an asynchronous IO monad using a provided function.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="func">The function to create the result.</param>
        /// <returns>An asynchronous IO monad.</returns>
        public static IIOMonad<T> Create<T>(Func<Task<T>> func)
            => new CreateAsyncCore<T>(func);
    }
}
