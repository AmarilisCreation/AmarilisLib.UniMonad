using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Option
    {
        private class CreateCore<T> : IOptionMonad<T>
        {
            private Func<OptionResult<T>> _func;
            public CreateCore(Func<OptionResult<T>> func)
            {
                _func = func;
            }
            Task<OptionResult<T>> IOptionMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                return Task.Factory.StartNew(
                    _func,
                    cancellationToken,
                    TaskCreationOptions.DenyChildAttach,
                    TaskScheduler.FromCurrentSynchronizationContext());
            }
        }
        /// <summary>
        /// Creates a new Option monad from a function that returns an OptionResult.
        /// </summary>
        /// <typeparam name="T">The type of the value in the Option monad.</typeparam>
        /// <param name="func">The function that returns an OptionResult.</param>
        /// <returns>A new Option monad.</returns>
        public static IOptionMonad<T> Create<T>(Func<OptionResult<T>> func)
            => new CreateCore<T>(func);
        private class CreateAsyncCore<T> : IOptionMonad<T>
        {
            private Func<Task<OptionResult<T>>> _func;
            public CreateAsyncCore(Func<Task<OptionResult<T>>> func)
            {
                _func = func;
            }
            Task<OptionResult<T>> IOptionMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                var task = _func();
                cancellationToken.Register(() => task.Dispose());
                return task;
            }
        }
        /// <summary>
        /// Creates an asynchronous option monad using a provided function.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="func">The function to create the option result.</param>
        /// <returns>An asynchronous option monad.</returns>
        public static IOptionMonad<T> Create<T>(Func<Task<OptionResult<T>>> func)
            => new CreateAsyncCore<T>(func);
    }
}
