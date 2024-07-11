using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Reader
    {
        private class CreateCore<TEnvironment, TValue> : IReaderMonad<TEnvironment, TValue>
        {
            private Func<TEnvironment, TValue> _func;
            public CreateCore(Func<TEnvironment, TValue> func)
            {
                _func = func;
            }
            public Task<TValue> RunAsync(TEnvironment environment, CancellationToken cancellationToken)
            {
                return Task.Factory.StartNew(
                    () => _func(environment),
                    cancellationToken,
                    TaskCreationOptions.DenyChildAttach,
                    TaskScheduler.FromCurrentSynchronizationContext());
            }
        }
        /// <summary>
        /// Creates a Reader monad from a function that takes an environment and returns a value.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="func">The function that takes an environment and returns a value.</param>
        /// <returns>A Reader monad that executes the function.</returns>
        public static IReaderMonad<TEnvironment, TValue> Create<TEnvironment, TValue>(Func<TEnvironment, TValue> func)
            => new CreateCore<TEnvironment, TValue>(func);
        private class CreateAsyncCore<TEnvironment, TValue> : IReaderMonad<TEnvironment, TValue>
        {
            private Func<TEnvironment, Task<TValue>> _func;
            public CreateAsyncCore(Func<TEnvironment, Task<TValue>> func)
            {
                _func = func;
            }
            public Task<TValue> RunAsync(TEnvironment environment, CancellationToken cancellationToken)
            {
                var task = _func(environment);
                cancellationToken.Register(() => task.Dispose());
                return task;
            }
        }
        /// <summary>
        /// Creates an asynchronous reader monad using a provided function.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="func">The function to create the result.</param>
        /// <returns>An asynchronous reader monad.</returns>
        public static IReaderMonad<TEnvironment, TValue> Create<TEnvironment, TValue>(Func<TEnvironment, Task<TValue>> func)
            => new CreateAsyncCore<TEnvironment, TValue>(func);
    }
}
