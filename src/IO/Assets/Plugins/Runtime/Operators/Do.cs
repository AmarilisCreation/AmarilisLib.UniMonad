using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class IO
    {
        private class DoCore<T> : IIOMonad<T>
        {
            private IIOMonad<T> _self;
            private Action<T> _action;
            public DoCore(IIOMonad<T> self, Action<T> action)
            {
                _self = self;
                _action = action;
            }
            async Task<T> IIOMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                _action(result);
                return result;
            }
        }
        /// <summary>
        /// Creates a monad that performs an action on the result of the source monad.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the monad.</typeparam>
        /// <param name="self">The source monad.</param>
        /// <param name="action">The action to perform on the result of the source monad.</param>
        /// <returns>A monad that performs the specified action on the result of the source monad.</returns>
        public static IIOMonad<T> Do<T>(this IIOMonad<T> self, Action<T> action)
            => new DoCore<T>(self, action);

        private class DoAsyncCore<T> : IIOMonad<T>
        {
            private IIOMonad<T> _self;
            private Func<T, Task> _action;
            public DoAsyncCore(IIOMonad<T> self, Func<T, Task> action)
            {
                _self = self;
                _action = action;
            }
            async Task<T> IIOMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                await _action(result);
                cancellationToken.ThrowIfCancellationRequested();
                return result;
            }
        }
        /// <summary>
        /// Creates a monad that performs an asynchronous action on the result of the source monad.
        /// </summary>
        /// <typeparam name="T">The type of the value returned by the monad.</typeparam>
        /// <param name="self">The source monad.</param>
        /// <param name="action">The asynchronous action to perform on the result of the source monad.</param>
        /// <returns>A monad that performs the specified asynchronous action on the result of the source monad.</returns>
        public static IIOMonad<T> Do<T>(this IIOMonad<T> self, Func<T, Task> action)
            => new DoAsyncCore<T>(self, action);
    }
}
