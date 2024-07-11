using System;
using System.Threading.Tasks;
using System.Threading;

namespace AmarilisLib.Monad
{
    public static partial class Try
    {
        private struct ShareCore<T> : ITryMonad<T>
        {
            private ITryMonad<T> _self;
            private TryResult<T> _cache;
            private bool _hasCache;
            public ShareCore(ITryMonad<T> self)
            {
                _self = self;
                _cache = default;
                _hasCache = false;
            }
            async Task<TryResult<T>> ITryMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    if(_hasCache) return _cache;
                    _hasCache = true;
                    _cache = await _self.RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    return _cache;
                }
                catch(Exception exception)
                {
                    _cache = TryResult<T>.Failure(exception);
                    return _cache;
                }
            }
        }
        /// <summary>
        /// Creates a try monad that caches its result to share among multiple consumers.
        /// </summary>
        /// <typeparam name="T">The type of the result.</typeparam>
        /// <param name="self">The try monad to share.</param>
        /// <returns>A try monad that caches its result to share among multiple consumers.</returns>
        public static ITryMonad<T> Share<T>(this ITryMonad<T> self)
            => new ShareCore<T>(self);
    }
}