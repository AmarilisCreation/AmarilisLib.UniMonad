using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace AmarilisLib.Monad
{
    [Serializable]
    public abstract class DrawableSerializableEitherBase { }

    /// <summary>
    /// Represents a serializable Either monad property that can be drawn in Unity's inspector.
    /// </summary>
    /// <typeparam name="TLeft">The type of the Left value.</typeparam>
    /// <typeparam name="TRight">The type of the Right value.</typeparam>
    [Serializable]
    public class SerializableEitherProperty<TLeft, TRight> : DrawableSerializableEitherBase, IEitherMonad<TLeft, TRight>
    {
        /// <summary>
        /// The Left value of the Either monad.
        /// </summary>
        [SerializeField]
        private TLeft _left;

        /// <summary>
        /// The Right value of the Either monad.
        /// </summary>
        [SerializeField]
        private TRight _right;

        /// <summary>
        /// Indicates whether the Either monad represents a Right value.
        /// </summary>
        [SerializeField]
        private bool _isRight = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableEitherProperty{TLeft, TRight}"/> class.
        /// </summary>
        public SerializableEitherProperty()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableEitherProperty{TLeft, TRight}"/> class with the specified values.
        /// </summary>
        /// <param name="left">The Left value.</param>
        /// <param name="right">The Right value.</param>
        /// <param name="isRight">A value indicating whether the Either monad represents a Right value.</param>
        public SerializableEitherProperty(TLeft left, TRight right, bool isRight)
        {
            _left = left;
            _right = right;
            _isRight = isRight;
        }

        /// <summary>
        /// Runs the monad asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A token to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains an <see cref="EitherResult{TLeft, TRight}"/>.</returns>
        Task<EitherResult<TLeft, TRight>> IEitherMonad<TLeft, TRight>.RunAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if(_isRight)
            {
                return Either.ReturnRight<TLeft, TRight>(_right).RunAsync(cancellationToken);
            }
            return Either.ReturnLeft<TLeft, TRight>(_left).RunAsync(cancellationToken);
        }
    }
}
