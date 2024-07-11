using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Option
    {
        private struct ShareCore<T> : IOptionMonad<T>
        {
            private IOptionMonad<T> _self;
            private OptionResult<T> _cache;
            private bool _hasCache;
            public ShareCore(IOptionMonad<T> self)
            {
                _self = self;
                _cache = default;
                _hasCache = false;
            }
            async Task<OptionResult<T>> IOptionMonad<T>.RunAsync(CancellationToken cancellationToken)
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
        /// Shares the result of the Option monad, caching the result to avoid redundant computations.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="self">The Option monad to share.</param>
        /// <returns>An Option monad that shares the result, caching it after the first computation.</returns>
        public static IOptionMonad<T> Share<T>(this IOptionMonad<T> self)
            => new ShareCore<T>(self);
    }
}
