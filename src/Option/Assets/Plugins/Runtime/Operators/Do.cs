using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class Option
    {
        private struct DoCore<T> : IOptionMonad<T>
        {
            private IOptionMonad<T> _self;
            private Action<OptionResult<T>> _action;
            public DoCore(IOptionMonad<T> self, Action<OptionResult<T>> action)
            {
                _self = self;
                _action = action;
            }
            async Task<OptionResult<T>> IOptionMonad<T>.RunAsync(CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                _action(result);
                return result;
            }
        }
        /// <summary>
        /// Executes an action on the Option monad's result, regardless of its value.
        /// </summary>
        /// <typeparam name="T">The type of the value in the Option monad.</typeparam>
        /// <param name="self">The Option monad to evaluate.</param>
        /// <param name="action">The action to execute on the Option monad's result.</param>
        /// <returns>The original Option monad after executing the action.</returns>
        public static IOptionMonad<T> Do<T>(this IOptionMonad<T> self, Action<OptionResult<T>> action)
            => new DoCore<T>(self, action);

        private struct DoAsyncCore<T> : IOptionMonad<T>
        {
            private IOptionMonad<T> _self;
            private Func<OptionResult<T>, Task> _action;
            public DoAsyncCore(IOptionMonad<T> self, Func<OptionResult<T>, Task> action)
            {
                _self = self;
                _action = action;
            }
            async Task<OptionResult<T>> IOptionMonad<T>.RunAsync(CancellationToken cancellationToken)
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
        /// Executes an asynchronous action on the Option monad's result, regardless of its value.
        /// </summary>
        /// <typeparam name="T">The type of the value in the Option monad.</typeparam>
        /// <param name="self">The Option monad to evaluate.</param>
        /// <param name="action">The asynchronous action to execute on the Option monad's result.</param>
        /// <returns>The original Option monad after executing the action.</returns>
        public static IOptionMonad<T> Do<T>(this IOptionMonad<T> self, Func<OptionResult<T>, Task> action)
            => new DoAsyncCore<T>(self, action);
    }
}
