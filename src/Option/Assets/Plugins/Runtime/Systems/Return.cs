using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Option
    {
        private struct ReturnCore<T> : IOptionMonad<T>
        {
            private T _value;
            public ReturnCore(T value)
            {
                _value = value;
            }
            Task<OptionResult<T>> IOptionMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(OptionResult<T>.Just(_value));
            }
        }
        /// <summary>
        /// Creates an Option monad that returns a specified value.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value to be returned by the Option monad.</param>
        /// <returns>An Option monad that returns the specified value.</returns>
        public static IOptionMonad<T> Return<T>(T value) => new ReturnCore<T>(value);

        private struct ReturnAsyncCore<T> : IOptionMonad<T>
        {
            private Func<Task<T>> _value;
            public ReturnAsyncCore(Func<Task<T>> value)
            {
                _value = value;
            }
            async Task<OptionResult<T>> IOptionMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _value();
                cancellationToken.ThrowIfCancellationRequested();
                return OptionResult<T>.Just(result);
            }
    }
        /// <summary>
        /// Creates an Option monad that returns a value from a specified asynchronous function.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">A function that asynchronously returns the value to be returned by the Option monad.</param>
        /// <returns>An Option monad that returns a value from the specified asynchronous function.</returns>
        public static IOptionMonad<T> Return<T>(Func<Task<T>> value) => new ReturnAsyncCore<T>(value);
    }
}
