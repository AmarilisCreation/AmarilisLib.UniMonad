using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class State
    {
        private class PutCore<TState> : IStateMonad<TState, Unit>
        {
            private TState _state;
            public PutCore(TState state)
            {
                _state = state;
            }
            Task<StateResult<TState, Unit>> IStateMonad<TState, Unit>.RunAsync(TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(new StateResult<TState, Unit>(_state, Unit.Default));
            }
        }
        /// <summary>
        /// Creates a state monad that sets the state to the specified value and returns a unit value.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="state">The state to set.</param>
        /// <returns>A state monad that sets the state and returns a unit value.</returns>
        public static IStateMonad<TState, Unit> Put<TState>(TState state)
            => new PutCore<TState>(state);
    }
}
