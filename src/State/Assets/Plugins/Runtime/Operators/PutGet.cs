using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class State
    {
        private class PutGetCore<TState> : IStateMonad<TState, TState>
        {
            private TState _state;
            public PutGetCore(TState state)
            {
                _state = state;
            }
            Task<StateResult<TState, TState>> IStateMonad<TState, TState>.RunAsync(TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(new StateResult<TState, TState>(_state, _state));
            }
        }
        /// <summary>
        /// Creates a state monad that sets the state to the specified value and returns the new state.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="state">The state to set.</param>
        /// <returns>A state monad that sets the state and returns the new state.</returns>
        public static IStateMonad<TState, TState> PutGet<TState>(TState state)
            => new PutGetCore<TState>(state);
    }
}
