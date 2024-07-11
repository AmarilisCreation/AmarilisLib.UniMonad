using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class RWS
    {
        private class PutGetCore<TEnvironment, TOutput, TState> : IRWSMonad<TEnvironment, TOutput, TState, TState>
        {
            private TState _state;
            public PutGetCore(TState state)
            {
                _state = state;
            }
            Task<RWSResult<TOutput, TState, TState>> IRWSMonad<TEnvironment, TOutput, TState, TState>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(new RWSResult<TOutput, TState, TState>(_state, Array.Empty<TOutput>(), _state));
            }
        }
        /// <summary>
        /// Creates an RWS monad that sets the state to the specified value and returns the new state.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="state">The state to set.</param>
        /// <returns>An RWS monad that sets the state to the specified value and returns the new state.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TState> PutGet<TEnvironment, TOutput, TState>(TState state)
            => new PutGetCore<TEnvironment, TOutput, TState>(state);
    }
}
