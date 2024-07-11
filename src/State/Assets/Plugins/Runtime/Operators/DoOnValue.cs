using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class State
    {
        private class DoOnValueCore<TState, TValue> : IStateMonad<TState, TValue>
        {
            private IStateMonad<TState, TValue> _self;
            private Action<TValue> _action;
            public DoOnValueCore(IStateMonad<TState, TValue> self, Action<TValue> action)
            {
                _self = self;
                _action = action;
            }
            async Task<StateResult<TState, TValue>> IStateMonad<TState, TValue>.RunAsync(TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                _action(result.Value);
                return result;
            }
        }
        /// <summary>
        /// Executes the specified action on the value produced by the state monad.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The original state monad.</param>
        /// <param name="action">The action to execute on the value.</param>
        /// <returns>A state monad that executes the specified action on the value.</returns>
        public static IStateMonad<TState, TValue> DoOnValue<TState, TValue>(this IStateMonad<TState, TValue> self, Action<TValue> action)
            => new DoOnValueCore<TState, TValue>(self, action);

        private class DoOnValueAsyncCore<TState, TValue> : IStateMonad<TState, TValue>
        {
            private IStateMonad<TState, TValue> _self;
            private Func<TValue, Task> _action;
            public DoOnValueAsyncCore(IStateMonad<TState, TValue> self, Func<TValue, Task> action)
            {
                _self = self;
                _action = action;
            }
            async Task<StateResult<TState, TValue>> IStateMonad<TState, TValue>.RunAsync(TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                await _action(result.Value);
                return result;
            }
        }
        /// <summary>
        /// Asynchronously executes the specified action on the value produced by the state monad.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The original state monad.</param>
        /// <param name="action">The asynchronous action to execute on the value.</param>
        /// <returns>A state monad that asynchronously executes the specified action on the value.</returns>
        public static IStateMonad<TState, TValue> DoOnValue<TState, TValue>(this IStateMonad<TState, TValue> self, Func<TValue, Task> action)
            => new DoOnValueAsyncCore<TState, TValue>(self, action);
    }
}
