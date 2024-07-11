using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Option
    {
        private class CatchCore<T> : IOptionMonad<T>
        {
            private IOptionMonad<T> _self;
            private Func<IOptionMonad<T>> _selector;
            public CatchCore(IOptionMonad<T> self, Func<IOptionMonad<T>> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<OptionResult<T>> IOptionMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(selfResult.IsJust) return selfResult;
                var catchResult = await _selector().RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return catchResult;
            }
        }
        /// <summary>
        /// Catches a None result and returns an alternative Option monad.
        /// </summary>
        /// <typeparam name="T">The type of the value in the Option monad.</typeparam>
        /// <param name="self">The Option monad to evaluate.</param>
        /// <param name="selector">A function that returns an alternative Option monad if the initial Option monad is None.</param>
        /// <returns>An Option monad that is either the original result or an alternative if the original was None.</returns>
        public static IOptionMonad<T> Catch<T>(this IOptionMonad<T> self, Func<IOptionMonad<T>> selector)
            => new CatchCore<T>(self, selector);

        private class CatchAsyncCore<T> : IOptionMonad<T>
        {
            private IOptionMonad<T> _self;
            private Func<OptionResult<T>, Task<OptionResult<T>>> _selector;
            public CatchAsyncCore(IOptionMonad<T> self, Func<OptionResult<T>, Task<OptionResult<T>>> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<OptionResult<T>> IOptionMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(result.IsJust) return result;
                var selectorResult = await _selector(result);
                cancellationToken.ThrowIfCancellationRequested();
                return selectorResult;
            }
        }
        /// <summary>
        /// Catches a None result and returns an alternative Option monad based on an asynchronous function.
        /// </summary>
        /// <typeparam name="T">The type of the value in the Option monad.</typeparam>
        /// <param name="self">The Option monad to evaluate.</param>
        /// <param name="selector">An asynchronous function that returns an alternative Option monad if the initial Option monad is None.</param>
        /// <returns>An Option monad that is either the original result or an alternative if the original was None.</returns>
        public static IOptionMonad<T> Catch<T>(this IOptionMonad<T> self, Func<OptionResult<T>, Task<OptionResult<T>>> selector)
            => new CatchAsyncCore<T>(self, selector);
    }
}
