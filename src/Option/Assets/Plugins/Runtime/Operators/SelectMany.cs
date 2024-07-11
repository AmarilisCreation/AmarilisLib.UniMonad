using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Option
    {
        private struct SelectManyCore<T, TResult> : IOptionMonad<TResult>
        {
            private IOptionMonad<T> _self;
            private Func<T, IOptionMonad<TResult>> _selector;
            public SelectManyCore(IOptionMonad<T> self, Func<T, IOptionMonad<TResult>> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<OptionResult<TResult>> IOptionMonad<TResult>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(result.IsJust) {
                    var justResult = await _selector(result.Value).RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return justResult;
                }
                var noneResult = await None<TResult>().RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return noneResult;
            }
        }
        /// <summary>
        /// Projects each element of an Option monad to another Option monad and flattens the resulting monads into a single monad.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <param name="self">The Option monad to project.</param>
        /// <param name="selector">A function to project the value of the original Option monad into another Option monad.</param>
        /// <returns>An Option monad that represents the flattened result of the projection.</returns>
        public static IOptionMonad<TResult> SelectMany<T, TResult>(this IOptionMonad<T> self, Func<T, IOptionMonad<TResult>> selector)
            => new SelectManyCore<T, TResult>(self, selector);

        private struct SelectManyCore<TFirst, TSecond, TResult> : IOptionMonad<TResult>
        {
            private IOptionMonad<TFirst> _self;
            private Func<TFirst, IOptionMonad<TSecond>> _selector;
            private Func<TFirst, TSecond, TResult> _projector;
            public SelectManyCore(IOptionMonad<TFirst> self, Func<TFirst, IOptionMonad<TSecond>> selector, Func<TFirst, TSecond, TResult> projector)
            {
                _self = self;
                _selector = selector;
                _projector = projector;
            }
            async Task<OptionResult<TResult>> IOptionMonad<TResult>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(!selfResult.IsNone)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var secondResult = await _selector(selfResult.Value).RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    if(secondResult.IsJust) {
                        var justResult = await Return(_projector(selfResult.Value, secondResult.Value)).RunAsync(cancellationToken);
                        cancellationToken.ThrowIfCancellationRequested();
                        return justResult;
                    }
                }
                var noneResult = await None<TResult>().RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return noneResult;
            }
        }
        /// <summary>
        /// Projects each element of an Option monad to another Option monad, flattens the resulting monads into a single monad, and applies a result selector function to the values.
        /// </summary>
        /// <typeparam name="TFirst">The type of the first value.</typeparam>
        /// <typeparam name="TSecond">The type of the second value.</typeparam>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <param name="self">The Option monad to project.</param>
        /// <param name="selector">A function to project the value of the original Option monad into another Option monad.</param>
        /// <param name="projector">A function to apply to the values of the original and projected Option monads.</param>
        /// <returns>An Option monad that represents the result of the projection and application of the result selector function.</returns>
        public static IOptionMonad<TResult> SelectMany<TFirst, TSecond, TResult>(this IOptionMonad<TFirst> self, Func<TFirst, IOptionMonad<TSecond>> selector, Func<TFirst, TSecond, TResult> projector)
            => new SelectManyCore<TFirst, TSecond, TResult>(self, selector, projector);

        private struct SelectManyAsyncCore<T, TResult> : IOptionMonad<TResult>
        {
            private IOptionMonad<T> _self;
            private Func<T, Task<IOptionMonad<TResult>>> _selector;
            public SelectManyAsyncCore(IOptionMonad<T> self, Func<T, Task<IOptionMonad<TResult>>> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<OptionResult<TResult>> IOptionMonad<TResult>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(result.IsJust) {
                    var justResult = await (await _selector(result.Value)).RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return justResult;
                }
                var noneResult = await None<TResult>().RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return noneResult;
            }
        }
        /// <summary>
        /// Projects each element of an Option monad to another Option monad asynchronously and flattens the resulting monads into a single monad.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <param name="self">The Option monad to project.</param>
        /// <param name="selector">A function to asynchronously project the value of the original Option monad into another Option monad.</param>
        /// <returns>An Option monad that represents the flattened result of the projection.</returns>
        public static IOptionMonad<TResult> SelectMany<T, TResult>(this IOptionMonad<T> self, Func<T, Task<IOptionMonad<TResult>>> selector)
            => new SelectManyAsyncCore<T, TResult>(self, selector);

        private struct SelectManyAsyncCore<TFirst, TSecond, TResult> : IOptionMonad<TResult>
        {
            private IOptionMonad<TFirst> _self;
            private Func<TFirst, IOptionMonad<TSecond>> _selector;
            private Func<TFirst, TSecond, Task<TResult>> _projector;
            public SelectManyAsyncCore(IOptionMonad<TFirst> self, Func<TFirst, IOptionMonad<TSecond>> selector, Func<TFirst, TSecond, Task<TResult>> projector)
            {
                _self = self;
                _selector = selector;
                _projector = projector;
            }
            async Task<OptionResult<TResult>> IOptionMonad<TResult>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(!selfResult.IsNone)
                {
                    var secondResult = await _selector(selfResult.Value).RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    if(secondResult.IsJust) {
                        var projectorResult = await _projector(selfResult.Value, secondResult.Value);
                        cancellationToken.ThrowIfCancellationRequested();
                        var justResult = await Return(projectorResult).RunAsync(cancellationToken);
                        cancellationToken.ThrowIfCancellationRequested();
                        return justResult;
                    }
                }
                var noneResult = await None<TResult>().RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return noneResult;
            }
        }
        /// <summary>
        /// Projects each element of an Option monad to another Option monad asynchronously, flattens the resulting monads into a single monad, and applies a result selector function to the values.
        /// </summary>
        /// <typeparam name="TFirst">The type of the first value.</typeparam>
        /// <typeparam name="TSecond">The type of the second value.</typeparam>
        /// <typeparam name="TResult">The type of the result value.</typeparam>
        /// <param name="self">The Option monad to project.</param>
        /// <param name="selector">A function to project the value of the original Option monad into another Option monad.</param>
        /// <param name="projector">A function to asynchronously apply to the values of the original and projected Option monads.</param>
        /// <returns>An Option monad that represents the result of the projection and application of the result selector function.</returns>
        public static IOptionMonad<TResult> SelectMany<TFirst, TSecond, TResult>(this IOptionMonad<TFirst> self, Func<TFirst, IOptionMonad<TSecond>> selector, Func<TFirst, TSecond, Task<TResult>> projector)
            => new SelectManyAsyncCore<TFirst, TSecond, TResult>(self, selector, projector);
    }
}
