using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Option
    {
        private struct SelectCore<T, TResult> : IOptionMonad<TResult>
        {
            private IOptionMonad<T> _self;
            private Func<T, TResult> _selector;
            public SelectCore(IOptionMonad<T> self, Func<T, TResult> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<OptionResult<TResult>> IOptionMonad<TResult>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(selfResult.IsJust) {
                    var justResult = await Return(_selector(selfResult.Value)).RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return justResult;
                }
                var noneResult = await None<TResult>().RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return noneResult;
            }
        }
        /// <summary>
        /// Projects each element of an Option monad to a new form.
        /// </summary>
        /// <typeparam name="T">The type of the value in the original Option monad.</typeparam>
        /// <typeparam name="TResult">The type of the value in the resulting Option monad.</typeparam>
        /// <param name="self">The Option monad to project.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An Option monad whose elements are the result of invoking the transform function on each element of the source.</returns>
        public static IOptionMonad<TResult> Select<T, TResult>(this IOptionMonad<T> self, Func<T, TResult> selector)
            => new SelectCore<T, TResult>(self, selector);

        private struct SelectAsyncCore<T, TResult> : IOptionMonad<TResult>
        {
            private IOptionMonad<T> _self;
            private Func<T, Task<TResult>> _selector;
            public SelectAsyncCore(IOptionMonad<T> self, Func<T, Task<TResult>> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<OptionResult<TResult>> IOptionMonad<TResult>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(selfResult.IsJust) {
                    var selectorResult = await _selector(selfResult.Value);
                    cancellationToken.ThrowIfCancellationRequested();
                    var justResult = await Return(selectorResult).RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return justResult;
                }
                var noneResult = await None<TResult>().RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return noneResult;
            }
        }
        /// <summary>
        /// Projects each element of an Option monad to a new form asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the value in the original Option monad.</typeparam>
        /// <typeparam name="TResult">The type of the value in the resulting Option monad.</typeparam>
        /// <param name="self">The Option monad to project.</param>
        /// <param name="selector">An asynchronous transform function to apply to each element.</param>
        /// <returns>An Option monad whose elements are the result of invoking the asynchronous transform function on each element of the source.</returns>
        public static IOptionMonad<TResult> Select<T, TResult>(this IOptionMonad<T> self, Func<T, Task<TResult>> selector)
            => new SelectAsyncCore<T, TResult>(self, selector);
    }
}
