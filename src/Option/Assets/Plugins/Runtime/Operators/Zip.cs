using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Option
    {
        private class ZipCore<TFirst, TSecond, TResult> : IOptionMonad<TResult>
        {
            private IOptionMonad<TFirst> _self;
            private IOptionMonad<TSecond> _second;
            private Func<TFirst, TSecond, TResult> _resultSelector;
            public ZipCore(IOptionMonad<TFirst> self, IOptionMonad<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
            {
                _self = self;
                _second = second;
                _resultSelector = resultSelector;
            }

            async Task<OptionResult<TResult>> IOptionMonad<TResult>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var secondResult = await _second.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(!selfResult.IsNone && !secondResult.IsNone) {
                    var justResult = await Return(_resultSelector(selfResult.Value, secondResult.Value)).RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return justResult;
                }
                var noneResult = await None<TResult>().RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return noneResult;
            }
        }
        /// <summary>
        /// Combines the results of two Option monads using a specified result selector function.
        /// </summary>
        /// <typeparam name="TFirst">The type of the first value.</typeparam>
        /// <typeparam name="TSecond">The type of the second value.</typeparam>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <param name="self">The first Option monad.</param>
        /// <param name="second">The second Option monad.</param>
        /// <param name="resultSelector">A function to select the result value from the first and second values.</param>
        /// <returns>An Option monad that represents the combined result.</returns>
        public static IOptionMonad<TResult> Zip<TFirst, TSecond, TResult>(this IOptionMonad<TFirst> self, IOptionMonad<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
            => new ZipCore<TFirst, TSecond, TResult>(self, second, resultSelector);

        private class ZipAsyncCore<TFirst, TSecond, TResult> : IOptionMonad<TResult>
        {
            private IOptionMonad<TFirst> _self;
            private IOptionMonad<TSecond> _second;
            private Func<TFirst, TSecond, Task<TResult>> _resultSelector;
            public ZipAsyncCore(IOptionMonad<TFirst> self, IOptionMonad<TSecond> second, Func<TFirst, TSecond, Task<TResult>> resultSelector)
            {
                _self = self;
                _second = second;
                _resultSelector = resultSelector;
            }

            async Task<OptionResult<TResult>> IOptionMonad<TResult>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var secondResult = await _second.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(!selfResult.IsNone && !secondResult.IsNone) {
                    var justResult = await Return(await _resultSelector(selfResult.Value, secondResult.Value)).RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return justResult;
                }
                var noneResult = await None<TResult>().RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return noneResult;
            }
        }
        /// <summary>
        /// Combines the results of two Option monads using a specified asynchronous result selector function.
        /// </summary>
        /// <typeparam name="TFirst">The type of the first value.</typeparam>
        /// <typeparam name="TSecond">The type of the second value.</typeparam>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <param name="self">The first Option monad.</param>
        /// <param name="second">The second Option monad.</param>
        /// <param name="resultSelector">A function to asynchronously select the result value from the first and second values.</param>
        public static IOptionMonad<TResult> Zip<TFirst, TSecond, TResult>(this IOptionMonad<TFirst> self, IOptionMonad<TSecond> second, Func<TFirst, TSecond, Task<TResult>> resultSelector)
            => new ZipAsyncCore<TFirst, TSecond, TResult>(self, second, resultSelector);
    }
}
