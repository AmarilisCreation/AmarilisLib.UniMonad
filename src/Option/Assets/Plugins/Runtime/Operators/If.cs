using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Option
    {
        private struct IfElseCore<T> : IOptionMonad<T>
        {
            private IOptionMonad<T> _self;
            private IOptionMonad<T> _elseSource;
            private Func<OptionResult<T>, bool> _selector;
            public IfElseCore(IOptionMonad<T> self, IOptionMonad<T> elseSource, Func<OptionResult<T>, bool> selector)
            {
                _self = self;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<OptionResult<T>> IOptionMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(_selector(result)) return result;
                return await _elseSource.RunAsync(cancellationToken);
            }
        }
        /// <summary>
        /// Conditionally returns the result of one Option monad or another based on a selector function.
        /// </summary>
        /// <typeparam name="T">The type of the value in the Option monad.</typeparam>
        /// <param name="self">The initial Option monad to evaluate.</param>
        /// <param name="elseSource">The Option monad to return if the selector function returns false.</param>
        /// <param name="selector">A function that determines which Option monad to return based on the result of the initial Option monad.</param>
        /// <returns>An Option monad based on the selector function's evaluation.</returns>
        public static IOptionMonad<T> If<T>(this IOptionMonad<T> self, IOptionMonad<T> elseSource, Func<OptionResult<T>, bool> selector)
            => new IfElseCore<T>(self, elseSource, selector);

        private struct IfCore<T> : IOptionMonad<T>
        {
            private IOptionMonad<T> _self;
            private Func<OptionResult<T>, bool> _selector;
            public IfCore(IOptionMonad<T> self, Func<OptionResult<T>, bool> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<OptionResult<T>> IOptionMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(_selector(result)) return result;
                return await None<T>().RunAsync(cancellationToken);
            }
        }
        /// <summary>
        /// Conditionally returns the result of an Option monad or None based on a selector function.
        /// </summary>
        /// <typeparam name="T">The type of the value in the Option monad.</typeparam>
        /// <param name="self">The Option monad to evaluate.</param>
        /// <param name="selector">A function that determines whether to return the Option monad or None based on its result.</param>
        /// <returns>An Option monad or None based on the selector function's evaluation.</returns>
        public static IOptionMonad<T> If<T>(this IOptionMonad<T> self, Func<OptionResult<T>, bool> selector)
            => new IfCore<T>(self, selector);

        private struct IfElseStaticCore<T> : IOptionMonad<T>
        {
            private IOptionMonad<T> _thenSource;
            private IOptionMonad<T> _elseSource;
            private Func<bool> _selector;
            public IfElseStaticCore(IOptionMonad<T> thenSource, IOptionMonad<T> elseSource, Func<bool> selector)
            {
                _thenSource = thenSource;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<OptionResult<T>> IOptionMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if(_selector())
                {
                    var thenResult = await _thenSource.RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return thenResult;
                }
                var elseResult = await _elseSource.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return elseResult;
            }
        }
        /// <summary>
        /// Conditionally returns one of two Option monads based on a static selector function.
        /// </summary>
        /// <typeparam name="T">The type of the value in the Option monad.</typeparam>
        /// <param name="thenSource">The Option monad to return if the selector function returns true.</param>
        /// <param name="elseSource">The Option monad to return if the selector function returns false.</param>
        /// <param name="selector">A function that determines which Option monad to return.</param>
        /// <returns>An Option monad based on the static selector function's evaluation.</returns>
        public static IOptionMonad<T> If<T>(IOptionMonad<T> thenSource, IOptionMonad<T> elseSource, Func<bool> selector)
            => new IfElseStaticCore<T>(thenSource, elseSource, selector);

        private struct IfStaticCore<T> : IOptionMonad<T>
        {
            private IOptionMonad<T> _thenSource;
            private Func<bool> _selector;
            public IfStaticCore(IOptionMonad<T> thenSource, Func<bool> selector)
            {
                _thenSource = thenSource;
                _selector = selector;
            }
            async Task<OptionResult<T>> IOptionMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if(_selector())
                {
                    var thenResult = await _thenSource.RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return thenResult;
                }
                var noneResult = await None<T>().RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return noneResult;
            }
        }
        /// <summary>
        /// Conditionally returns an Option monad or None based on a static selector function.
        /// </summary>
        /// <typeparam name="T">The type of the value in the Option monad.</typeparam>
        /// <param name="thenSource">The Option monad to return if the selector function returns true.</param>
        /// <param name="selector">A function that determines whether to return the Option monad or None.</param>
        /// <returns>An Option monad or None based on the static selector function's evaluation.</returns>
        public static IOptionMonad<T> If<T>(IOptionMonad<T> thenSource, Func<bool> selector)
            => new IfStaticCore<T>(thenSource, selector);

        private struct IfElseAsyncCore<T> : IOptionMonad<T>
        {
            private IOptionMonad<T> _self;
            private IOptionMonad<T> _elseSource;
            private Func<OptionResult<T>, Task<bool>> _selector;
            public IfElseAsyncCore(IOptionMonad<T> self, IOptionMonad<T> elseSource, Func<OptionResult<T>, Task<bool>> selector)
            {
                _self = self;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<OptionResult<T>> IOptionMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector(result);
                if(selectorResult) return result;
                var elseResult = await _elseSource.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return elseResult;
            }
        }
        /// <summary>
        /// Conditionally returns the result of one Option monad or another based on an asynchronous selector function.
        /// </summary>
        /// <typeparam name="T">The type of the value in the Option monad.</typeparam>
        /// <param name="self">The initial Option monad to evaluate.</param>
        /// <param name="elseSource">The Option monad to return if the selector function returns false.</param>
        /// <param name="selector">An asynchronous function that determines which Option monad to return based on the result of the initial Option monad.</param>
        /// <returns>An Option monad based on the asynchronous selector function's evaluation.</returns>
        public static IOptionMonad<T> If<T>(this IOptionMonad<T> self, IOptionMonad<T> elseSource, Func<OptionResult<T>, Task<bool>> selector)
            => new IfElseAsyncCore<T>(self, elseSource, selector);

        private struct IfAsyncCore<T> : IOptionMonad<T>
        {
            private IOptionMonad<T> _self;
            private Func<OptionResult<T>, Task<bool>> _selector;
            public IfAsyncCore(IOptionMonad<T> self, Func<OptionResult<T>, Task<bool>> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<OptionResult<T>> IOptionMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector(result);
                cancellationToken.ThrowIfCancellationRequested();
                if(selectorResult) return result;
                var noneResult = await None<T>().RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return noneResult;
            }
        }
        /// <summary>
        /// Conditionally returns the result of an Option monad or None based on an asynchronous selector function.
        /// </summary>
        /// <typeparam name="T">The type of the value in the Option monad.</typeparam>
        /// <param name="self">The Option monad to evaluate.</param>
        /// <param name="selector">An asynchronous function that determines whether to return the Option monad or None based on its result.</param>
        /// <returns>An Option monad or None based on the asynchronous selector function's evaluation.</returns>
        public static IOptionMonad<T> If<T>(this IOptionMonad<T> self, Func<OptionResult<T>, Task<bool>> selector)
            => new IfAsyncCore<T>(self, selector);

        private struct IfElseStaticAsyncCore<T> : IOptionMonad<T>
        {
            private IOptionMonad<T> _thenSource;
            private IOptionMonad<T> _elseSource;
            private Task<Func<bool>> _selector;
            public IfElseStaticAsyncCore(IOptionMonad<T> thenSource, IOptionMonad<T> elseSource, Task<Func<bool>> selector)
            {
                _thenSource = thenSource;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<OptionResult<T>> IOptionMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector;
                cancellationToken.ThrowIfCancellationRequested();
                if(selectorResult())
                {
                    var thenResult = await _thenSource.RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return thenResult;
                }
                var elseResult = await _elseSource.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return elseResult;
            }
        }
        /// <summary>
        /// Conditionally returns one of two Option monads based on an asynchronous static selector function.
        /// </summary>
        /// <typeparam name="T">The type of the value in the Option monad.</typeparam>
        /// <param name="thenSource">The Option monad to return if the selector function returns true.</param>
        /// <param name="elseSource">The Option monad to return if the selector function returns false.</param>
        /// <param name="selector">An asynchronous function that determines which Option monad to return.</param>
        /// <returns>An Option monad based on the asynchronous static selector function's evaluation.</returns>
        public static IOptionMonad<T> If<T>(IOptionMonad<T> thenSource, IOptionMonad<T> elseSource, Task<Func<bool>> selector)
            => new IfElseStaticAsyncCore<T>(thenSource, elseSource, selector);

        private struct IfStaticAsyncCore<T> : IOptionMonad<T>
        {
            private IOptionMonad<T> _thenSource;
            private Task<Func<bool>> _selector;
            public IfStaticAsyncCore(IOptionMonad<T> thenSource, Task<Func<bool>> selector)
            {
                _thenSource = thenSource;
                _selector = selector;
            }
            async Task<OptionResult<T>> IOptionMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector;
                cancellationToken.ThrowIfCancellationRequested();
                if(selectorResult()) {
                    var thenResult = await _thenSource.RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return thenResult;

                }
                var elseResult = await None<T>().RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return elseResult;
            }
        }
        /// <summary>
        /// Conditionally returns an Option monad or None based on an asynchronous static selector function.
        /// </summary>
        /// <typeparam name="T">The type of the value in the Option monad.</typeparam>
        /// <param name="thenSource">The Option monad to return if the selector function returns true.</param>
        /// <param name="selector">An asynchronous function that determines whether to return the Option monad or None.</param>
        /// <returns>An Option monad or None based on the asynchronous static selector function's evaluation.</returns>
        public static IOptionMonad<T> If<T>(IOptionMonad<T> thenSource, Task<Func<bool>> selector)
            => new IfStaticAsyncCore<T>(thenSource, selector);
    }
}
