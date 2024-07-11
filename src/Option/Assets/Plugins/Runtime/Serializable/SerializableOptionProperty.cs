using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace AmarilisLib.Monad
{
    [Serializable]
    public abstract class DrawableSerializableOptionBase { }

    /// <summary>
    /// Serializable class for an option property that can be drawn in the Unity inspector.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    [Serializable]
    public class SerializableOptionProperty<T> : DrawableSerializableOptionBase, IOptionMonad<T>
    {
        /// <summary>
        /// The value of the option property.
        /// </summary>
        [SerializeField]
        private T _value;

        /// <summary>
        /// Indicates whether the option property is Just.
        /// </summary>
        [SerializeField]
        private bool _isJust = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableOptionProperty{T}"/> class.
        /// </summary>
        public SerializableOptionProperty()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableOptionProperty{T}"/> class with the specified value and state.
        /// </summary>
        /// <param name="value">The value of the option property.</param>
        /// <param name="isJust">Indicates whether the option property is Just.</param>
        public SerializableOptionProperty(T value, bool isJust)
        {
            _value = value;
            _isJust = isJust;
        }

        /// <summary>
        /// Runs the option monad and produces an option result asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation, containing the option result.</returns>
        Task<OptionResult<T>> IOptionMonad<T>.RunAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if(_isJust)
            {
                return Option.Return(_value).RunAsync(cancellationToken);
            }
            else
            {
                return Option.None<T>().RunAsync(cancellationToken);
            }
        }
    }
}
