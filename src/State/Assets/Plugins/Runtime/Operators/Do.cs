using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class State
    {
        private class DoCore<TState, TValue> : IStateMonad<TState, TValue>
        {
            private IStateMonad<TState, TValue> _self;
            private Action<StateResult<TState, TValue>> _action;
            public DoCore(IStateMonad<TState, TValue> self, Action<StateResult<TState, TValue>> action)
            {
                _self = self;
                _action = action;
            }
            async Task<StateResult<TState, TValue>> IStateMonad<TState, TValue>.RunAsync(TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                _action(result);
                return result;
            }
        }
        /// <summary>
        /// Executes the specified action on the state result after the state monad computation.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The original state monad.</param>
        /// <param name="action">The action to execute on the state result.</param>
        /// <returns>A state monad that executes the specified action on the state result.</returns>
        public static IStateMonad<TState, TValue> Do<TState, TValue>(this IStateMonad<TState, TValue> self, Action<StateResult<TState, TValue>> action)
            => new DoCore<TState, TValue>(self, action);

        private class DoAsyncCore<TState, TValue> : IStateMonad<TState, TValue>
        {
            private IStateMonad<TState, TValue> _self;
            private Func<StateResult<TState, TValue>, Task> _action;
            public DoAsyncCore(IStateMonad<TState, TValue> self, Func<StateResult<TState, TValue>, Task> action)
            {
                _self = self;
                _action = action;
            }
            async Task<StateResult<TState, TValue>> IStateMonad<TState, TValue>.RunAsync(TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _self.RunAsync(state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                await _action(result);
                return result;
            }
        }
        /// <summary>
        /// Asynchronously executes the specified action on the state result after the state monad computation.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The original state monad.</param>
        /// <param name="action">The asynchronous action to execute on the state result.</param>
        /// <returns>A state monad that asynchronously executes the specified action on the state result.</returns>
        public static IStateMonad<TState, TValue> Do<TState, TValue>(this IStateMonad<TState, TValue> self, Func<StateResult<TState, TValue>, Task> action)
            => new DoAsyncCore<TState, TValue>(self, action);
    }
}
