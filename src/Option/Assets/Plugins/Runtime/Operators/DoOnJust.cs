using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Option
    {
        private struct DoOnJustCore<T> : IOptionMonad<T>
        {
            private IOptionMonad<T> _self;
            private Action<T> _action;
            public DoOnJustCore(IOptionMonad<T> self, Action<T> action)
            {
                _self = self;
                _action = action;
            }
            async Task<OptionResult<T>> IOptionMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(result.IsJust) _action(result.Value);
                return result;
            }
        }
        /// <summary>
        /// Executes an action if the Option monad contains a value (Just).
        /// </summary>
        /// <typeparam name="T">The type of the value in the Option monad.</typeparam>
        /// <param name="self">The Option monad to evaluate.</param>
        /// <param name="action">The action to execute if the Option monad contains a value.</param>
        /// <returns>The original Option monad after potentially executing the action.</returns>
        public static IOptionMonad<T> DoOnJust<T>(this IOptionMonad<T> self, Action<T> action)
            => new DoOnJustCore<T>(self, action);

        private struct DoOnJustAsyncCore<T> : IOptionMonad<T>
        {
            private IOptionMonad<T> _self;
            private Func<T, Task> _action;
            public DoOnJustAsyncCore(IOptionMonad<T> self, Func<T, Task> action)
            {
                _self = self;
                _action = action;
            }
            async Task<OptionResult<T>> IOptionMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(result.IsJust) {
                    await _action(result.Value);
                    cancellationToken.ThrowIfCancellationRequested();
                }
                return result;
            }
        }
        /// <summary>
        /// Executes an asynchronous action if the Option monad contains a value (Just).
        /// </summary>
        /// <typeparam name="T">The type of the value in the Option monad.</typeparam>
        /// <param name="self">The Option monad to evaluate.</param>
        /// <param name="action">The asynchronous action to execute if the Option monad contains a value.</param>
        /// <returns>The original Option monad after potentially executing the action.</returns>
        public static IOptionMonad<T> DoOnJust<T>(this IOptionMonad<T> self, Func<T, Task> action)
            => new DoOnJustAsyncCore<T>(self, action);
    }
}
