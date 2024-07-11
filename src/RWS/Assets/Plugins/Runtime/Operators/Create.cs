using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class RWS
    {
        private class CreateCore<TEnvironment, TOutput, TState, TValue> : IRWSMonad<TEnvironment, TOutput, TState, TValue>
        {
            private Func<TEnvironment, TState, RWSResult<TOutput, TState, TValue>> _func;
            public CreateCore(Func<TEnvironment, TState, RWSResult<TOutput, TState, TValue>> func)
            {
                _func = func;
            }
            Task<RWSResult<TOutput, TState, TValue>> IRWSMonad<TEnvironment, TOutput, TState, TValue>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                return Task.Factory.StartNew(
                    () => _func(environment, state),
                    cancellationToken,
                    TaskCreationOptions.DenyChildAttach,
                    TaskScheduler.FromCurrentSynchronizationContext());
            }
        }
        /// <summary>
        /// Creates an RWS monad from the specified function.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="func">The function to create the RWS monad from.</param>
        /// <returns>An RWS monad created from the specified function.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TValue> Create<TEnvironment, TOutput, TState, TValue>(Func<TEnvironment, TState, RWSResult<TOutput, TState, TValue>> func)
            => new CreateCore<TEnvironment, TOutput, TState, TValue>(func);
        private class CreateAsyncCore<TEnvironment, TOutput, TState, TValue> : IRWSMonad<TEnvironment, TOutput, TState, TValue>
        {
            private Func<TEnvironment, TState, Task<RWSResult<TOutput, TState, TValue>>> _func;
            public CreateAsyncCore(Func<TEnvironment, TState, Task<RWSResult<TOutput, TState, TValue>>> func)
            {
                _func = func;
            }
            Task<RWSResult<TOutput, TState, TValue>> IRWSMonad<TEnvironment, TOutput, TState, TValue>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                var task = _func(environment, state);
                cancellationToken.Register(() => task.Dispose());
                return task;
            }
        }
        /// <summary>
        /// Creates an asynchronous read-write state monad using a provided function.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="func">The function to create the RWS result.</param>
        /// <returns>An asynchronous read-write state monad.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TValue> Create<TEnvironment, TOutput, TState, TValue>(Func<TEnvironment, TState, Task<RWSResult<TOutput, TState, TValue>>> func)
            => new CreateAsyncCore<TEnvironment, TOutput, TState, TValue>(func);
    }
}
