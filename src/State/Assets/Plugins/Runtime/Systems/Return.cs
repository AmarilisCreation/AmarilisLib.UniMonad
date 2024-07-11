using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class State
    {
        private class ReturnCore<TState, TValue> : IStateMonad<TState, TValue>
        {
            private TValue _value;
            public ReturnCore(TValue value)
            {
                _value = value;
            }
            Task<StateResult<TState, TValue>> IStateMonad<TState, TValue>.RunAsync(TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.FromResult(new StateResult<TState, TValue>(state, _value));
            }
        }
        /// <summary>
        /// Creates a state monad that returns a specified value without modifying the state.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="value">The value to return.</param>
        /// <returns>A state monad that returns the specified value.</returns>
        public static IStateMonad<TState, TValue> Return<TState, TValue>(TValue value)
            => new ReturnCore<TState, TValue>(value);
    }
}
