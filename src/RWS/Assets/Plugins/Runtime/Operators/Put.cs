using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class RWS
    {
        private class PutCore<TEnvironment, TOutput, TState> : IRWSMonad<TEnvironment, TOutput, TState, Unit>
        {
            private TState _state;
            public PutCore(TState state)
            {
                _state = state;
            }
            Task<RWSResult<TOutput, TState, Unit>> IRWSMonad<TEnvironment, TOutput, TState, Unit>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(new RWSResult<TOutput, TState, Unit>(Unit.Default, Array.Empty<TOutput>(), _state));
            }
        }
        /// <summary>
        /// Creates an RWS monad that sets the state to the specified value and returns a Unit result.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="state">The state to set.</param>
        /// <returns>An RWS monad that sets the state to the specified value and returns a Unit result.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, Unit> Put<TEnvironment, TOutput, TState>(TState state)
            => new PutCore<TEnvironment, TOutput, TState>(state);
    }
}
