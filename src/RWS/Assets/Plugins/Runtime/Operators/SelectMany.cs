using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AmarilisLib.Monad
{
    public static partial class RWS
    {
        private class SelectManyCore<TEnvironment, TOutput, TState, TValue, TResult> : IRWSMonad<TEnvironment, TOutput, TState, TResult>
        {
            private IRWSMonad<TEnvironment, TOutput, TState, TValue> _self;
            private Func<TValue, IRWSMonad<TEnvironment, TOutput, TState, TResult>> _selector;
            public SelectManyCore(IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Func<TValue, IRWSMonad<TEnvironment, TOutput, TState, TResult>> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<RWSResult<TOutput, TState, TResult>> IRWSMonad<TEnvironment, TOutput, TState, TResult>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(environment, state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return await _selector(selfResult.Value).RunAsync(environment, selfResult.State ?? state, cancellationToken);
            }
        }
        /// <summary>
        /// Projects each element of the RWS monad into a new form using a specified selector function.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value in the source RWS monad.</typeparam>
        /// <typeparam name="TResult">The type of the value in the resulting RWS monad.</typeparam>
        /// <param name="self">The source RWS monad.</param>
        /// <param name="selector">A function to project each value into a new form.</param>
        /// <returns>An RWS monad containing the projected values.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TResult> SelectMany<TEnvironment, TOutput, TState, TValue, TResult>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Func<TValue, IRWSMonad<TEnvironment, TOutput, TState, TResult>> selector)
            => new SelectManyCore<TEnvironment, TOutput, TState, TValue, TResult>(self, selector);

        private class SelectManyCore<TEnvironment, TOutput, TState, TFirst, TSecond, TResult> : IRWSMonad<TEnvironment, TOutput, TState, TResult>
        {
            private IRWSMonad<TEnvironment, TOutput, TState, TFirst> _self;
            private Func<TFirst, IRWSMonad<TEnvironment, TOutput, TState, TSecond>> _selector;
            private Func<TFirst, TSecond, TResult> _projector;
            public SelectManyCore(IRWSMonad<TEnvironment, TOutput, TState, TFirst> self, Func<TFirst, IRWSMonad<TEnvironment, TOutput, TState, TSecond>> selector, Func<TFirst, TSecond, TResult> projector)
            {
                _self = self;
                _selector = selector;
                _projector = projector;
            }
            async Task<RWSResult<TOutput, TState, TResult>> IRWSMonad<TEnvironment, TOutput, TState, TResult>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(environment, state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var secondResult = await _selector(selfResult.Value).RunAsync(environment, selfResult.State ?? state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return new RWSResult<TOutput, TState, TResult>(_projector(selfResult.Value, secondResult.Value), selfResult.Output.Concat(secondResult.Output), secondResult.State ?? selfResult.State ?? state);
            }
        }
        /// <summary>
        /// Projects each element of the RWS monad into a new form using a specified selector and result selector function.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TFirst">The type of the value in the first RWS monad.</typeparam>
        /// <typeparam name="TSecond">The type of the value in the second RWS monad.</typeparam>
        /// <typeparam name="TResult">The type of the value in the resulting RWS monad.</typeparam>
        /// <param name="self">The source RWS monad.</param>
        /// <param name="selector">A function to project each value into an intermediate form.</param>
        /// <param name="projector">A function to create a result value from two intermediate values.</param>
        /// <returns>An RWS monad containing the projected values.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TResult> SelectMany<TEnvironment, TOutput, TState, TFirst, TSecond, TResult>(this IRWSMonad<TEnvironment, TOutput, TState, TFirst> self, Func<TFirst, IRWSMonad<TEnvironment, TOutput, TState, TSecond>> selector, Func<TFirst, TSecond, TResult> projector)
            => new SelectManyCore<TEnvironment, TOutput, TState, TFirst, TSecond, TResult>(self, selector, projector);

        private class SelectManyAsyncCore<TEnvironment, TOutput, TState, TValue, TResult> : IRWSMonad<TEnvironment, TOutput, TState, TResult>
        {
            private IRWSMonad<TEnvironment, TOutput, TState, TValue> _self;
            private Func<TValue, Task<IRWSMonad<TEnvironment, TOutput, TState, TResult>>> _selector;
            public SelectManyAsyncCore(IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Func<TValue, Task<IRWSMonad<TEnvironment, TOutput, TState, TResult>>> selector)
            {
                _self = self;
                _selector = selector;
            }
            async Task<RWSResult<TOutput, TState, TResult>> IRWSMonad<TEnvironment, TOutput, TState, TResult>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(environment, state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var selectorResult = await _selector(selfResult.Value);
                cancellationToken.ThrowIfCancellationRequested();
                var result = await (selectorResult).RunAsync(environment, selfResult.State ?? state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                return result;
            }
        }
        /// <summary>
        /// Projects each element of the RWS monad into a new form using a specified asynchronous selector function.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TValue">The type of the value in the source RWS monad.</typeparam>
        /// <typeparam name="TResult">The type of the value in the resulting RWS monad.</typeparam>
        /// <param name="self">The source RWS monad.</param>
        /// <param name="selector">An asynchronous function to project each value into a new form.</param>
        /// <returns>An RWS monad containing the projected values.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TResult> SelectMany<TEnvironment, TOutput, TState, TValue, TResult>(this IRWSMonad<TEnvironment, TOutput, TState, TValue> self, Func<TValue, Task<IRWSMonad<TEnvironment, TOutput, TState, TResult>>> selector)
            => new SelectManyAsyncCore<TEnvironment, TOutput, TState, TValue, TResult>(self, selector);

        private class SelectManyAsyncCore<TEnvironment, TOutput, TState, TFirst, TSecond, TResult> : IRWSMonad<TEnvironment, TOutput, TState, TResult>
        {
            private IRWSMonad<TEnvironment, TOutput, TState, TFirst> _self;
            private Func<TFirst, IRWSMonad<TEnvironment, TOutput, TState, TSecond>> _selector;
            private Func<TFirst, TSecond, Task<TResult>> _projector;
            public SelectManyAsyncCore(IRWSMonad<TEnvironment, TOutput, TState, TFirst> self, Func<TFirst, IRWSMonad<TEnvironment, TOutput, TState, TSecond>> selector, Func<TFirst, TSecond, Task<TResult>> projector)
            {
                _self = self;
                _selector = selector;
                _projector = projector;
            }
            async Task<RWSResult<TOutput, TState, TResult>> IRWSMonad<TEnvironment, TOutput, TState, TResult>.RunAsync(TEnvironment environment, TState state, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var selfResult = await _self.RunAsync(environment, state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var secondResult = await _selector(selfResult.Value).RunAsync(environment, selfResult.State ?? state, cancellationToken);
                cancellationToken.ThrowIfCancellationRequested();
                var result = await _projector(selfResult.Value, secondResult.Value);
                cancellationToken.ThrowIfCancellationRequested();
                return new RWSResult<TOutput, TState, TResult>(result, selfResult.Output.Concat(secondResult.Output), secondResult.State ?? selfResult.State ?? state);
            }
        }
        /// <summary>
        /// Projects each element of the RWS monad into a new form using a specified selector and asynchronous result selector function.
        /// </summary>
        /// <typeparam name="TEnvironment">The type of the environment.</typeparam>
        /// <typeparam name="TOutput">The type of the output.</typeparam>
        /// <typeparam name="TState">The type of the state.</typeparam>
        /// <typeparam name="TFirst">The type of the value in the first RWS monad.</typeparam>
        /// <typeparam name="TSecond">The type of the value in the second RWS monad.</typeparam>
        /// <typeparam name="TResult">The type of the value in the resulting RWS monad.</typeparam>
        /// <param name="self">The source RWS monad.</param>
        /// <param name="selector">A function to project each value into an intermediate form.</param>
        /// <param name="projector">An asynchronous function to create a result value from two intermediate values.</param>
        /// <returns>An RWS monad containing the projected values.</returns>
        public static IRWSMonad<TEnvironment, TOutput, TState, TResult> SelectMany<TEnvironment, TOutput, TState, TFirst, TSecond, TResult>(this IRWSMonad<TEnvironment, TOutput, TState, TFirst> self, Func<TFirst, IRWSMonad<TEnvironment, TOutput, TState, TSecond>> selector, Func<TFirst, TSecond, Task<TResult>> projector)
            => new SelectManyAsyncCore<TEnvironment, TOutput, TState, TFirst, TSecond, TResult>(self, selector, projector);
    }
}
