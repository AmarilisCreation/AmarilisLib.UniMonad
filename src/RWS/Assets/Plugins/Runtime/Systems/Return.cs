using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class RWS
    {
        private class ReturnCore<TEnvironment, TOutput, TState, TValue> : IRWSMonad<TEnvironment, TOutput, TState, TValue>
        {
            private TValue _value;
            public ReturnCore(TValue value)
            {
                _value = value;
            }
            Task<RWSResult<TOutput, TState, TValue>> IRWSMonad<TEnvironment, TOutput, TState, TValue>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(new RWSResult<TOutput, TState, TValue>(_value, Array.Empty<TOutput>(), state));
            }
        }
        /// <summary>
        /// Creates an RWS monad that returns the specified value without modifying the environment or state.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="value">The value to be returned by the monad.</param>
        /// <returns>An RWS monad that returns the specified value.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TValue> Return<TEnvironment, TOutput, TState, TValue>(TValue value)
            => new ReturnCore<TEnvironment, TOutput, TState, TValue>(value);
    }
}
