using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class State
    {
        private class DoOnStateCore<TState, TValue> : IStateMonad<TState, TValue>
        {
            private IStateMonad<TState, TValue> _self;
            private Action<TState> _action;
            public DoOnStateCore(IStateMonad<TState, TValue> self, Action<TState> action)
            {
                _self = self;
                _action = action;
            }
            async Task<StateResult<TState, TValue>> IStateMonad<TState, TValue>.RunAsync(TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                _action(result.State);
                return result;
            }
        }
        /// <summary>
        /// Executes the specified action on the state after the state monad computation.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The original state monad.</param>
        /// <param name="action">The action to execute on the state.</param>
        /// <returns>A state monad that executes the specified action on the state.</returns>
        public static IStateMonad<TState, TValue> DoOnState<TState, TValue>(this IStateMonad<TState, TValue> self, Action<TState> action)
            => new DoOnStateCore<TState, TValue>(self, action);

        private class DoOnStateAsyncCore<TState, TValue> : IStateMonad<TState, TValue>
        {
            private IStateMonad<TState, TValue> _self;
            private Func<TState, Task> _action;
            public DoOnStateAsyncCore(IStateMonad<TState, TValue> self, Func<TState, Task> action)
            {
                _self = self;
                _action = action;
            }
            async Task<StateResult<TState, TValue>> IStateMonad<TState, TValue>.RunAsync(TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                await _action(result.State);
                return result;
            }
        }
        /// <summary>
        /// Asynchronously executes the specified action on the state after the state monad computation.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The original state monad.</param>
        /// <param name="action">The asynchronous action to execute on the state.</param>
        /// <returns>A state monad that asynchronously executes the specified action on the state.</returns>
        public static IStateMonad<TState, TValue> DoOnState<TState, TValue>(this IStateMonad<TState, TValue> self, Func<TState, Task> action)
            => new DoOnStateAsyncCore<TState, TValue>(self, action);
    }
}
