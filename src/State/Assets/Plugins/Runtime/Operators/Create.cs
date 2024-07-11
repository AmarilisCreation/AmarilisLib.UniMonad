using System;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class State
    {
        private class CreateCore<TState, TValue> : IStateMonad<TState, TValue>
        {
            private Func<TState, StateResult<TState, TValue>> _func;
            public CreateCore(Func<TState, StateResult<TState, TValue>> func)
            {
                _func = func;
            }
            Task<StateResult<TState, TValue>> IStateMonad<TState, TValue>.RunAsync(TState state, CancellationToken cancellationToken)
            {
                return Task.Factory.StartNew(
                    () => _func(state),
                    cancellationToken,
                    TaskCreationOptions.DenyChildAttach,
                    TaskScheduler.FromCurrentSynchronizationContext());
            }
        }
        /// <summary>
        /// Creates a state monad from the specified function.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="func">The function that defines the state monad.</param>
        /// <returns>A state monad created from the specified function.</returns>
        public static IStateMonad<TState, TValue> Create<TState, TValue>(Func<TState, StateResult<TState, TValue>> func)
            => new CreateCore<TState, TValue>(func);
        private class CreateAsyncCore<TState, TValue> : IStateMonad<TState, TValue>
        {
            private Func<TState, Task<StateResult<TState, TValue>>> _func;
            public CreateAsyncCore(Func<TState, Task<StateResult<TState, TValue>>> func)
            {
                _func = func;
            }
            Task<StateResult<TState, TValue>> IStateMonad<TState, TValue>.RunAsync(TState state, CancellationToken cancellationToken)
            {
                var task = _func(state);
                cancellationToken.Register(() => task.Dispose());
                return task;
            }
        }
        /// <summary>
        /// Creates an asynchronous state monad using a provided function.
        /// </summary>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="func">The function to create the state result.</param>
        /// <returns>An asynchronous state monad.</returns>
        public static IStateMonad<TState, TValue> Create<TState, TValue>(Func<TState, Task<StateResult<TState, TValue>>> func)
            => new CreateAsyncCore<TState, TValue>(func);
    }
}
