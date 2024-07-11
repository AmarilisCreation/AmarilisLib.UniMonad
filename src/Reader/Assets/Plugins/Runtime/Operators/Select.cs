using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Reader
    {
        private class SelectCore<TEnvironment, TValue, TResult> : IReaderMonad<TEnvironment, TResult>
        {
            private IReaderMonad<TEnvironment, TValue> _self;
            private Func<TValue, TResult> _selector;
            public SelectCore(IReaderMonad<TEnvironment, TValue> self, Func<TValue, TResult> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<TResult> IReaderMonad<TEnvironment, TResult>.RunAsync(TEnvironment environment, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(environment, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return _selector(selfResult);
            }
        }
        /// <summary>
        /// Projects each value of the Reader monad into a new form using a selector function.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="self">The Reader monad to project.</param>
        /// <param name="selector">A projection function to apply to each value.</param>
        /// <returns>A Reader monad whose values are the result of invoking the projection function on each value of the original Reader monad.</returns>
        public static IReaderMonad<TEnvironment, TResult> Select<TEnvironment, TValue, TResult>(this IReaderMonad<TEnvironment, TValue> self, Func<TValue, TResult> selector)
            => new SelectCore<TEnvironment, TValue, TResult>(self, selector);
        private class SelectAsyncCore<TEnvironment, TValue, TResult> : IReaderMonad<TEnvironment, TResult>
        {
            private IReaderMonad<TEnvironment, TValue> _self;
            private Func<TValue, Task<TResult>> _selector;
            public SelectAsyncCore(IReaderMonad<TEnvironment, TValue> self, Func<TValue, Task<TResult>> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<TResult> IReaderMonad<TEnvironment, TResult>.RunAsync(TEnvironment environment, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(environment, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector(selfResult);
                cancellationToken.ThrowIfCancellationRequested();
                return selectorResult;
            }
        }
        /// <summary>
        /// Projects each value of the Reader monad into a new form using an asynchronous selector function.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="self">The Reader monad to project.</param>
        /// <param name="selector">An asynchronous projection function to apply to each value.</param>
        /// <returns>A Reader monad whose values are the result of invoking the asynchronous projection function on each value of the original Reader monad.</returns>
        public static IReaderMonad<TEnvironment, TResult> Select<TEnvironment, TValue, TResult>(this IReaderMonad<TEnvironment, TValue> self, Func<TValue, Task<TResult>> selector)
            => new SelectAsyncCore<TEnvironment, TValue, TResult>(self, selector);
    }
}
