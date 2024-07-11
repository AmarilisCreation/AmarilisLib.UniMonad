using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class State
    {
        private class GetReturnNoArgumentCore<TState> : IStateMonad<TState, TState>
        {
            public GetReturnNoArgumentCore()
            {

            }
            Task<StateResult<TState, TState>> IStateMonad<TState, TState>.RunAsync(TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(new StateResult<TState, TState>(state, state));
            }
        }
        /// <summary>
        /// Returns the current state.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <returns>A state monad that returns the current state.</returns>
        public static IStateMonad<TState, TState> Get<TState>()
            => new GetReturnNoArgumentCore<TState>();

        private class GetReturnSelectCore<TState> : IStateMonad<TState, TState>
        {
            private Func<TState, TState> _selector;
            public GetReturnSelectCore(Func<TState, TState> selector)
            {
                _selector = selector;
            }
            Task<StateResult<TState, TState>> IStateMonad<TState, TState>.RunAsync(TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(new StateResult<TState, TState>(state, _selector(state)));
            }
        }
        /// <summary>
        /// Returns the current state after applying the provided selector function.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="selector">A function to apply to the current state.</param>
        /// <returns>A state monad that returns the state after applying the selector function.</returns>
        public static IStateMonad<TState, TState> Get<TState>(Func<TState, TState> selector)
            => new GetReturnSelectCore<TState>(selector);

        private class GetReturnSelectAsyncCore<TState> : IStateMonad<TState, TState>
        {
            private Func<TState, Task<TState>> _selector;
            public GetReturnSelectAsyncCore(Func<TState, Task<TState>> selector)
            {
                _selector = selector;
            }
            async Task<StateResult<TState, TState>> IStateMonad<TState, TState>.RunAsync(TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return new StateResult<TState, TState>(state, await _selector(state));
            }
        }
        /// <summary>
        /// Asynchronously returns the current state after applying the provided selector function.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="selector">A function to asynchronously apply to the current state.</param>
        /// <returns>A state monad that asynchronously returns the state after applying the selector function.</returns>
        public static IStateMonad<TState, TState> Get<TState>(Func<TState, Task<TState>> selector)
            => new GetReturnSelectAsyncCore<TState>(selector);
    }
}
