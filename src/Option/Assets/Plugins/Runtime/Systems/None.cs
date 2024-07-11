using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Option
    {
        private struct NoneCore<T> : IOptionMonad<T>
        {
            public Task<OptionResult<T>> RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(OptionResult<T>.None());
            }
        }
        /// <summary>
        /// Creates an Option monad that represents a None result.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <returns>An Option monad that represents a None result.</returns>
        public static IOptionMonad<T> None<T>() => new NoneCore<T>();
    }
}
