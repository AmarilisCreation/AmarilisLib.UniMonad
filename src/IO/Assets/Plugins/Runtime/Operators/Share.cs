using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class IO
    {
        private class ShareCore<T> : IIOMonad<T>
        {
            private IIOMonad<T> _self;
            private T _cache;
            private bool _hasCache;
            public ShareCore(IIOMonad<T> self)
            {
                _self = self;
                _cache = default;
                _hasCache = false;
            }
            async Task<T> IIOMonad<T>.RunAsync(CancellationToken cancellationToken)
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
        /// Creates a monad that caches the result of the specified monad.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the monad.</typeparam>
        /// <param name="self">The monad whose result will be cached.</param>
        /// <returns>An IO monad that caches the result of the specified monad.</returns>
        public static IIOMonad<T> Share<T>(this IIOMonad<T> self)
            => new ShareCore<T>(self);
    }
}
