using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class RWS
    {
        private class TellCore<TEnvironment, TOutput, TState> : IRWSMonad<TEnvironment, TOutput, TState, Unit>
        {
            private TOutput _output;
            public TellCore(TOutput output)
            {
                _output = output;
            }
            Task<RWSResult<TOutput, TState, Unit>> IRWSMonad<TEnvironment, TOutput, TState, Unit>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(new RWSResult<TOutput, TState, Unit>(Unit.Default, new[] { _output }, state));
            }
        }
        /// <summary>
        /// Creates an RWS monad that produces the specified output without modifying the state.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <param name="output">The output to produce.</param>
        /// <returns>An RWS monad that produces the specified output.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, Unit> Tell<TEnvironment, TOutput, TState>(TOutput output)
            => new TellCore<TEnvironment, TOutput, TState>(output);
        /// <summary>
        /// Creates an RWS monad that produces the specified value and output without modifying the state.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="value">The value to produce.</param>
        /// <param name="output">The output to produce.</param>
        /// <returns>An RWS monad that produces the specified value and output.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TValue> Tell<TEnvironment, TOutput, TState, TValue>(TValue value, TOutput output)
            => Create<TEnvironment, TOutput, TState, TValue>((environment, state) => new RWSResult<TOutput, TState, TValue>(value, new[] { output }, state));
        /// <summary>
        /// Creates an RWS monad that produces the specified value and outputs without modifying the state.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="value">The value to produce.</param>
        /// <param name="outputs">The outputs to produce.</param>
        /// <returns>An RWS monad that produces the specified value and outputs.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TValue> Tell<TEnvironment, TOutput, TState, TValue>(TValue value, IEnumerable<TOutput> outputs)
            => Create<TEnvironment, TOutput, TState, TValue>((environment, state) => new RWSResult<TOutput, TState, TValue>(value, outputs, state));
    }
}
