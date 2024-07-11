using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Reader
    {
        private class DoCore<TEnvironment, TValue> : IReaderMonad<TEnvironment, TValue>
        {
            private IReaderMonad<TEnvironment, TValue> _self;
            private Action<TValue> _action;
            public DoCore(IReaderMonad<TEnvironment, TValue> self, Action<TValue> action)
            {
                _self = self;
                _action = action;
            }
            async Task<TValue> IReaderMonad<TEnvironment, TValue>.RunAsync(TEnvironment environment, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(environment, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                _action(result);
                return result;
            }
        }
        /// <summary>
        /// Executes an action on the result of the Reader monad.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The Reader monad.</param>
        /// <param name="action">The action to execute on the result.</param>
        /// <returns>A Reader monad that executes the action on the result.</returns>
        public static IReaderMonad<TEnvironment, TValue> Do<TEnvironment, TValue>(this IReaderMonad<TEnvironment, TValue> self, Action<TValue> action)
            => new DoCore<TEnvironment, TValue>(self, action);

        private class DoAsyncCore<TEnvironment, TValue> : IReaderMonad<TEnvironment, TValue>
        {
            private IReaderMonad<TEnvironment, TValue> _self;
            private Func<TValue, Task> _action;
            public DoAsyncCore(IReaderMonad<TEnvironment, TValue> self, Func<TValue, Task> action)
            {
                _self = self;
                _action = action;
            }
            async Task<TValue> IReaderMonad<TEnvironment, TValue>.RunAsync(TEnvironment environment, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(environment, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                await _action(result);
                cancellationToken.ThrowIfCancellationRequested();
                return result;
            }
        }
        /// <summary>
        /// Executes an asynchronous action on the result of the Reader monad.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The Reader monad.</param>
        /// <param name="action">The asynchronous action to execute on the result.</param>
        /// <returns>A Reader monad that executes the asynchronous action on the result.</returns>
        public static IReaderMonad<TEnvironment, TValue> Do<TEnvironment, TValue>(this IReaderMonad<TEnvironment, TValue> self, Func<TValue, Task> action)
            => new DoAsyncCore<TEnvironment, TValue>(self, action);
    }
}
