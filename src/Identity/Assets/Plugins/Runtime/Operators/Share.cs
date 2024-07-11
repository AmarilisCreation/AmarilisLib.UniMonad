using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Identity
    {
        private class ShareCore<T> : IIdentityMonad<T>
        {
            private IIdentityMonad<T> _self;
            private T _cache;
            private bool _hasCache;
            public ShareCore(IIdentityMonad<T> self)
            {
                _self = self;
                _cache = default;
                _hasCache = false;
            }
            async Task<T> IIdentityMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if(_hasCache) return _cache;
                _hasCache = true;
                _cache = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return _cache;
            }
        }
        /// <summary>
        /// Creates an Identity monad that caches the result of the specified monad.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="self">The source monad.</param>
        /// <returns>An Identity monad that caches the result of the specified monad.</returns>
        public static IIdentityMonad<T> Share<T>(this IIdentityMonad<T> self)
            => new ShareCore<T>(self);
    }
}
