using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Option
    {
        private struct DoOnNoneCore<T> : IOptionMonad<T>
        {
            private IOptionMonad<T> _self;
            private Action _action;
            public DoOnNoneCore(IOptionMonad<T> self, Action action)
            {
                _self = self;
                _action = action;
            }
            async Task<OptionResult<T>> IOptionMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(result.IsNone) _action();
                return result;
            }
        }
        /// <summary>
        /// Executes an action if the Option monad contains None.
        /// </summary>
        /// <typeparam name="T">The type of the value in the Option monad.</typeparam>
        /// <param name="self">The Option monad to evaluate.</param>
        /// <param name="action">The action to execute if the Option monad contains None.</param>
        /// <returns>The original Option monad after potentially executing the action.</returns>
        public static IOptionMonad<T> DoOnNone<T>(this IOptionMonad<T> self, Action action)
            => new DoOnNoneCore<T>(self, action);

        private struct DoOnNoneAsyncCore<T> : IOptionMonad<T>
        {
            private IOptionMonad<T> _self;
            private Func<Task> _action;
            public DoOnNoneAsyncCore(IOptionMonad<T> self, Func<Task> action)
            {
                _self = self;
                _action = action;
            }
            async Task<OptionResult<T>> IOptionMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                if(result.IsNone) {
                    await _action();
                    cancellationToken.ThrowIfCancellationRequested();
                }
                return result;
            }
        }
        /// <summary>
        /// Executes an asynchronous action if the Option monad contains None.
        /// </summary>
        /// <typeparam name="T">The type of the value in the Option monad.</typeparam>
        /// <param name="self">The Option monad to evaluate.</param>
        /// <param name="action">The asynchronous action to execute if the Option monad contains None.</param>
        /// <returns>The original Option monad after potentially executing the action.</returns>
        public static IOptionMonad<T> DoOnNone<T>(this IOptionMonad<T> self, Func<Task> action)
            => new DoOnNoneAsyncCore<T>(self, action);
    }
}
