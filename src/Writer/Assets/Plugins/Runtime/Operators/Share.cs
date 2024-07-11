using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Writer
    {
        private class ShareCore<TOutput, TValue> : IWriterMonad<TOutput, TValue>
        {
            private IWriterMonad<TOutput, TValue> _self;
            private WriterResult<TOutput, TValue> _cache;
            private bool _hasCache;
            public ShareCore(IWriterMonad<TOutput, TValue> self)
            {
                _self = self;
                _cache = default;
                _hasCache = false;
            }
            async Task<WriterResult<TOutput, TValue>> IWriterMonad<TOutput, TValue>.RunAsync(CancellationToken cancellationToken)
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
        /// Creates a writer monad that caches the result of the computation, returning the cached result on subsequent executions.
        /// </summary>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The original writer monad.</param>
        /// <returns>A writer monad that caches the result of the computation.</returns>
        public static IWriterMonad<TOutput, TValue> Share<TOutput, TValue>(this IWriterMonad<TOutput, TValue> self)
            => new ShareCore<TOutput, TValue>(self);
    }
}
