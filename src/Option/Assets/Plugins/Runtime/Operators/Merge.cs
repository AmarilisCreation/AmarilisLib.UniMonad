using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Option
    {
        private struct MergeCore<T> : IOptionMonad<T[]>
        {
            private IOptionMonad<T>[] _sources;
            public MergeCore(IOptionMonad<T>[] sources)
            {
                _sources = sources;
            }
            async Task<OptionResult<T[]>> IOptionMonad<T[]>.RunAsync(CancellationToken cancellationToken)
            {
                var resultArray = new T[_sources.Length];
                for(var i = 0; i < _sources.Length; i++)
                {
                    var source = await _sources[i].RunAsync(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    if(source.IsNone) {
                        var noneResult = await None<T[]>().RunAsync(cancellationToken);
                        cancellationToken.ThrowIfCancellationRequested();
                        return noneResult;
                    }
                    resultArray[i] = source.Value;
                }
                var justResult = await Return(resultArray).RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return justResult;
            }
        }
        /// <summary>
        /// Merges multiple Option monads into a single Option monad containing an array of values.
        /// If any of the Option monads contain None, the resulting monad will also be None.
        /// </summary>
        /// <typeparam name="T">The type of the values in the Option monads.</typeparam>
        /// <param name="self">The first Option monad to merge.</param>
        /// <param name="sources">Additional Option monads to merge.</param>
        /// <returns>An Option monad containing an array of values if all sources contain values; otherwise, an Option monad containing None.</returns>
        public static IOptionMonad<T[]> Merge<T>(this IOptionMonad<T> self, params IOptionMonad<T>[] sources)
            => Merge(new List<IOptionMonad<T>>() { self }.Concat(sources));
        /// <summary>
        /// Merges multiple Option monads into a single Option monad containing an array of values.
        /// If any of the Option monads contain None, the resulting monad will also be None.
        /// </summary>
        /// <typeparam name="T">The type of the values in the Option monads.</typeparam>
        /// <param name="sources">A collection of Option monads to merge.</param>
        /// <returns>An Option monad containing an array of values if all sources contain values; otherwise, an Option monad containing None.</returns>
        public static IOptionMonad<T[]> Merge<T>(IEnumerable<IOptionMonad<T>> sources)
            => Merge(sources.ToArray());
        /// <summary>
        /// Merges multiple Option monads into a single Option monad containing an array of values.
        /// If any of the Option monads contain None, the resulting monad will also be None.
        /// </summary>
        /// <typeparam name="T">The type of the values in the Option monads.</typeparam>
        /// <param name="sources">An array of Option monads to merge.</param>
        /// <returns>An Option monad containing an array of values if all sources contain values; otherwise, an Option monad containing None.</returns>
        public static IOptionMonad<T[]> Merge<T>(params IOptionMonad<T>[] sources)
            => new MergeCore<T>(sources);
    }
}
