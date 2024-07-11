using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Either
    {
        private struct ShareCore<TLeft, TRight> : IEitherMonad<TLeft, TRight>
        {
            private IEitherMonad<TLeft, TRight> _self;
            private EitherResult<TLeft, TRight> _cache;
            private bool _hasCache;
            public ShareCore(IEitherMonad<TLeft, TRight> self)
            {
                _self = self;
                _cache = default;
                _hasCache = false;
            }
            async Task<EitherResult<TLeft, TRight>> IEitherMonad<TLeft, TRight>.RunAsync(CancellationToken cancellationToken)
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
        /// Creates an Either monad that caches the result of the specified monad.
        /// </summary>
        /// <typeparam name="TLeft">The type of the Left value.</typeparam>
        /// <typeparam name="TRight">The type of the Right value.</typeparam>
        /// <param name="self">The Either monad to share.</param>
        /// <returns>An Either monad that caches the result of the specified monad.</returns>
        public static IEitherMonad<TLeft, TRight> Share<TLeft, TRight>(this IEitherMonad<TLeft, TRight> self)
            => new ShareCore<TLeft, TRight>(self);
    }
}
