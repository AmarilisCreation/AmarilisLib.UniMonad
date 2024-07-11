using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Identity
    {
        private class IfCore<T> : IIdentityMonad<T>
        {
            private IIdentityMonad<T> _self;
            private IIdentityMonad<T> _elseSource;
            private Func<T, bool> _selector;
            public IfCore(IIdentityMonad<T> self, IIdentityMonad<T> elseSource, Func<T, bool> selector)
            {
                _self = self;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<T> IIdentityMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(_selector(selfResult)) return selfResult;
                return await _elseSource.RunAsync(cancellationToken);
            }
        }
        /// <summary>
        /// Creates an Identity monad that conditionally selects the result based on the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="self">The source monad.</param>
        /// <param name="elseSource">The monad to run if the predicate is false.</param>
        /// <param name="selector">A function to test each element for a condition.</param>
        /// <returns>An Identity monad that conditionally selects the result based on the specified predicate.</returns>
        public static IIdentityMonad<T> If<T>(this IIdentityMonad<T> self, IIdentityMonad<T> elseSource, Func<T, bool> selector)
            => new IfCore<T>(self, elseSource, selector);

        private class IfStaticCore<T> : IIdentityMonad<T>
        {
            private IIdentityMonad<T> _thenSource;
            private IIdentityMonad<T> _elseSource;
            private Func<bool> _selector;
            public IfStaticCore(IIdentityMonad<T> thenSource, IIdentityMonad<T> elseSource, Func<bool> selector)
            {
                _thenSource = thenSource;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<T> IIdentityMonad<T>.RunAsync(CancellationToken cancellationToken)
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
        /// Creates an Identity monad that conditionally selects the result based on the specified predicate.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="thenSource">The monad to run if the predicate is true.</param>
        /// <param name="elseSource">The monad to run if the predicate is false.</param>
        /// <param name="selector">A function to test a condition.</param>
        /// <returns>An Identity monad that conditionally selects the result based on the specified predicate.</returns>
        public static IIdentityMonad<T> If<T>(IIdentityMonad<T> thenSource, IIdentityMonad<T> elseSource, Func<bool> selector)
            => new IfStaticCore<T>(thenSource, elseSource, selector);
        private class IfAsyncCore<T> : IIdentityMonad<T>
        {
            private IIdentityMonad<T> _self;
            private IIdentityMonad<T> _elseSource;
            private Func<T, Task<bool>> _selector;
            public IfAsyncCore(IIdentityMonad<T> self, IIdentityMonad<T> elseSource, Func<T, Task<bool>> selector)
            {
                _self = self;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<T> IIdentityMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector(selfResult);
                cancellationToken.ThrowIfCancellationRequested();
                if(selectorResult) return selfResult;
                var elseResult = await _elseSource.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return elseResult;
            }
        }
        /// <summary>
        /// Creates an Identity monad that conditionally selects the result based on the specified asynchronous predicate.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="self">The source monad.</param>
        /// <param name="elseSource">The monad to run if the predicate is false.</param>
        /// <param name="selector">An asynchronous function to test each element for a condition.</param>
        /// <returns>An Identity monad that conditionally selects the result based on the specified asynchronous predicate.</returns>
        public static IIdentityMonad<T> If<T>(this IIdentityMonad<T> self, IIdentityMonad<T> elseSource, Func<T, Task<bool>> selector)
            => new IfAsyncCore<T>(self, elseSource, selector);

        private class IfStaticAsyncCore<T> : IIdentityMonad<T>
        {
            private IIdentityMonad<T> _thenSource;
            private IIdentityMonad<T> _elseSource;
            private Task<Func<bool>> _selector;
            public IfStaticAsyncCore(IIdentityMonad<T> thenSource, IIdentityMonad<T> elseSource, Task<Func<bool>> selector)
            {
                _thenSource = thenSource;
                _elseSource = elseSource;
                _selector = selector;
            }
            async Task<T> IIdentityMonad<T>.RunAsync(CancellationToken cancellationToken)
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
        /// Creates an Identity monad that conditionally selects the result based on the specified asynchronous predicate.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="thenSource">The monad to run if the predicate is true.</param>
        /// <param name="elseSource">The monad to run if the predicate is false.</param>
        /// <param name="selector">An asynchronous function to test a condition.</param>
        /// <returns>An Identity monad that conditionally selects the result based on the specified asynchronous predicate.</returns>
        public static IIdentityMonad<T> If<T>(IIdentityMonad<T> thenSource, IIdentityMonad<T> elseSource, Task<Func<bool>> selector)
            => new IfStaticAsyncCore<T>(thenSource, elseSource, selector);
    }
}
