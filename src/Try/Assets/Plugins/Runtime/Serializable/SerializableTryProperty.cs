using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace AmarilisLib.Monad
{
    /// <summary>
    /// Represents an exception that can be serialized for try monad operations.
    /// </summary>
    public class SerializableTryException : Exception
    {
        public SerializableTryException()
            : base()
        {
        }

        public SerializableTryException(string message)
            : base(message)
        {
        }
    }
    [Serializable]
    public abstract class DrawableSerializableTryBase { }

    /// <summary>
    /// Represents a serializable property for try monad operations.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    [Serializable]
    public class SerializableTryProperty<T> : DrawableSerializableTryBase, ITryMonad<T>
    {
        [SerializeField]
        T _succeededValue;
        [SerializeField]
        string _faultedMessage = "";
        [SerializeField]
        bool _isSucceeded = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableTryProperty{T}"/> class.
        /// </summary>
        public SerializableTryProperty()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableTryProperty{T}"/> class with specified values.
        /// </summary>
        /// <param name="succeededValue">The value to return if the operation succeeds.</param>
        /// <param name="faultedMessage">The message for the exception if the operation fails.</param>
        /// <param name="isSucceeded">A value indicating whether the operation succeeded.</param>
        public SerializableTryProperty(T succeededValue, string faultedMessage, bool isSucceeded)
        {
            _succeededValue = succeededValue;
            _faultedMessage = faultedMessage;
            _isSucceeded = isSucceeded;
        }

        /// <summary>
        /// Runs the try monad operation asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to observe while waiting for the task to complete.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the try result.</returns>
        Task<TryResult<T>> ITryMonad<T>.RunAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if(_isSucceeded)
            {
                return Task.FromResult(TryResult<T>.Success(_succeededValue));
            }
            else
            {
                return Task.FromResult(TryResult<T>.Failure(new SerializableTryException(_faultedMessage)));
            }
        }
    }
}
