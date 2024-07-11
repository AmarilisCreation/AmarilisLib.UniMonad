using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Option
    {
        private struct WhereCore<T> : IOptionMonad<T>
        {
            private IOptionMonad<T> _self;
            private Func<T, bool> _selector;
            public WhereCore(IOptionMonad<T> self, Func<T, bool> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<OptionResult<T>> IOptionMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(!result.IsNone && _selector(result.Value))
                {
                    var justResult = await Return(result.Value).RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return justResult;
                }
                var noneResult = await None<T>().RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return noneResult;
            }
        }
        /// <summary>
        /// Filters the Option monad using a specified predicate function.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="self">The Option monad to filter.</param>
        /// <param name="selector">A function to test each value for a condition.</param>
        /// <returns>An Option monad that contains the value that satisfies the condition.</returns>
        public static IOptionMonad<T> Where<T>(this IOptionMonad<T> self, Func<T, bool> selector)
            => new WhereCore<T>(self, selector);

        private struct WhereAsyncCore<T> : IOptionMonad<T>
        {
            private IOptionMonad<T> _self;
            private Func<T, Task<bool>> _selector;
            public WhereAsyncCore(IOptionMonad<T> self, Func<T, Task<bool>> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<OptionResult<T>> IOptionMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector(result.Value);
                cancellationToken.ThrowIfCancellationRequested();
                if(!result.IsNone && selectorResult)
                {
                    var justResult = await Return(result.Value).RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return justResult;
                }
                var noneResult = await None<T>().RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return noneResult;
            }
        }
        /// <summary>
        /// Filters the Option monad using a specified asynchronous predicate function.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="self">The Option monad to filter.</param>
        /// <param name="selector">A function to asynchronously test each value for a condition.</param>
        /// <returns>An Option monad that contains the value that satisfies the condition.</returns>
        public static IOptionMonad<T> Where<T>(this IOptionMonad<T> self, Func<T, Task<bool>> selector)
            => new WhereAsyncCore<T>(self, selector);
    }
}
