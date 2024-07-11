using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Reader
    {
        private class AskReturnNoArgumentCore<TEnvironment> : IReaderMonad<TEnvironment, TEnvironment>
        {
            Task<TEnvironment> IReaderMonad<TEnvironment, TEnvironment>.RunAsync(TEnvironment environment, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(environment);
            }
        }
        /// <summary>
        /// Returns the environment.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <returns>A Reader monad that returns the environment.</returns>
        public static IReaderMonad<TEnvironment, TEnvironment> Ask<TEnvironment>()
            => new AskReturnNoArgumentCore<TEnvironment>();

        private class AskReturnEnvironmentSelectCore<TEnvironment> : IReaderMonad<TEnvironment, TEnvironment>
        {
            private Func<TEnvironment, TEnvironment> _selector;
            public AskReturnEnvironmentSelectCore(Func<TEnvironment, TEnvironment> selector)
            {
                _selector = selector;
            }
            Task<TEnvironment> IReaderMonad<TEnvironment, TEnvironment>.RunAsync(TEnvironment environment, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(_selector(environment));
            }
        }
        /// <summary>
        /// Returns the environment after applying a selector function.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <param name="selector">A function to apply to the environment.</param>
        /// <returns>A Reader monad that returns the selected environment.</returns>
        public static IReaderMonad<TEnvironment, TEnvironment> Ask<TEnvironment>(Func<TEnvironment, TEnvironment> selector)
            => new AskReturnEnvironmentSelectCore<TEnvironment>(selector);

        private class AskCore<TEnvironment, TValue> : IReaderMonad<TEnvironment, TEnvironment>
        {
            public AskCore(IReaderMonad<TEnvironment, TValue> self)
            {

            }
            Task<TEnvironment> IReaderMonad<TEnvironment, TEnvironment>.RunAsync(TEnvironment environment, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(environment);
            }
        }
        /// <summary>
        /// Returns the environment.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The Reader monad.</param>
        /// <returns>A Reader monad that returns the environment.</returns>
        public static IReaderMonad<TEnvironment, TEnvironment> Ask<TEnvironment, TValue>(this IReaderMonad<TEnvironment, TValue> self)
            => new AskCore<TEnvironment, TValue>(self);

        private class AskSelectCore<TEnvironment, TValue> : IReaderMonad<TEnvironment, TEnvironment>
        {
            private Func<TEnvironment, TEnvironment> _selector;
            public AskSelectCore(IReaderMonad<TEnvironment, TValue> self, Func<TEnvironment, TEnvironment> selector)
            {
                _selector = selector;
            }
            Task<TEnvironment> IReaderMonad<TEnvironment, TEnvironment>.RunAsync(TEnvironment environment, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(_selector(environment));
            }
        }
        /// <summary>
        /// Returns the environment after applying a selector function.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The Reader monad.</param>
        /// <param name="selector">A function to apply to the environment.</param>
        /// <returns>A Reader monad that returns the selected environment.</returns>
        public static IReaderMonad<TEnvironment, TEnvironment> Ask<TEnvironment, TValue>(this IReaderMonad<TEnvironment, TValue> self, Func<TEnvironment, TEnvironment> selector)
            => new AskSelectCore<TEnvironment, TValue>(self, selector);

        private class AskReturnEnvironmentSelectAsyncCore<TEnvironment> : IReaderMonad<TEnvironment, TEnvironment>
        {
            private Func<TEnvironment, Task<TEnvironment>> _selector;
            public AskReturnEnvironmentSelectAsyncCore(Func<TEnvironment, Task<TEnvironment>> selector)
            {
                _selector = selector;
            }
            async Task<TEnvironment> IReaderMonad<TEnvironment, TEnvironment>.RunAsync(TEnvironment environment, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector(environment);
                cancellationToken.ThrowIfCancellationRequested();
                return selectorResult;
            }
        }
        /// <summary>
        /// Returns the environment after applying an asynchronous selector function.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <param name="selector">An asynchronous function to apply to the environment.</param>
        /// <returns>A Reader monad that returns the selected environment.</returns>
        public static IReaderMonad<TEnvironment, TEnvironment> Ask<TEnvironment>(Func<TEnvironment, Task<TEnvironment>> selector)
            => new AskReturnEnvironmentSelectAsyncCore<TEnvironment>(selector);

        private class AskSelectAsyncCore<TEnvironment, TValue> : IReaderMonad<TEnvironment, TEnvironment>
        {
            private Func<TEnvironment, Task<TEnvironment>> _selector;
            public AskSelectAsyncCore(IReaderMonad<TEnvironment, TValue> self, Func<TEnvironment, Task<TEnvironment>> selector)
            {
                _selector = selector;
            }
            async Task<TEnvironment> IReaderMonad<TEnvironment, TEnvironment>.RunAsync(TEnvironment environment, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector(environment);
                cancellationToken.ThrowIfCancellationRequested();
                return selectorResult;
            }
        }
        /// <summary>
        /// Returns the environment after applying an asynchronous selector function.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The Reader monad.</param>
        /// <param name="selector">An asynchronous function to apply to the environment.</param>
        /// <returns>A Reader monad that returns the selected environment.</returns>
        public static IReaderMonad<TEnvironment, TEnvironment> Ask<TEnvironment, TValue>(this IReaderMonad<TEnvironment, TValue> self, Func<TEnvironment, Task<TEnvironment>> selector)
            => new AskSelectAsyncCore<TEnvironment, TValue>(self, selector);
    }
}
