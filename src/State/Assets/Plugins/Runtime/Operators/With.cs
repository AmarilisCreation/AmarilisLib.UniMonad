using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class State
    {
        private class WithCore<TState, TValue> : IStateMonad<TState, TValue>
        {
            private IStateMonad<TState, TValue> _self;
            private Func<TState, TState> _selector;
            public WithCore(IStateMonad<TState, TValue> self, Func<TState, TState> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<StateResult<TState, TValue>> IStateMonad<TState, TValue>.RunAsync(TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return new StateResult<TState, TValue>(_selector(selfResult.State), selfResult.Value);
            }
        }
        /// <summary>
        /// Modifies the state using a specified selector function after running the state monad.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The original state monad.</param>
        /// <param name="selector">The function to modify the state.</param>
        /// <returns>A state monad that applies the selector function to the state.</returns>
        public static IStateMonad<TState, TValue> With<TState, TValue>(this IStateMonad<TState, TValue> self, Func<TState, TState> selector)
            => new WithCore<TState, TValue>(self, selector);

        private class WithAsyncCore<TState, TValue> : IStateMonad<TState, TValue>
        {
            private IStateMonad<TState, TValue> _self;
            private Func<TState, Task<TState>> _selector;
            public WithAsyncCore(IStateMonad<TState, TValue> self, Func<TState, Task<TState>> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<StateResult<TState, TValue>> IStateMonad<TState, TValue>.RunAsync(TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector(selfResult.State);
                return new StateResult<TState, TValue>(selectorResult, selfResult.Value);
            }
        }
        /// <summary>
        /// Modifies the state using a specified asynchronous selector function after running the state monad.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="self">The original state monad.</param>
        /// <param name="selector">The asynchronous function to modify the state.</param>
        /// <returns>A state monad that applies the asynchronous selector function to the state.</returns>
        public static IStateMonad<TState, TValue> With<TState, TValue>(this IStateMonad<TState, TValue> self, Func<TState, Task<TState>> selector)
            => new WithAsyncCore<TState, TValue>(self, selector);
    }
}
