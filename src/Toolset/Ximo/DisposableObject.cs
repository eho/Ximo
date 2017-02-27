using System;

namespace Ximo
{
    /// <summary>
    ///     Defines an object base with necessary disposable implementation.
    /// </summary>
    public abstract class DisposableObject : IDisposable
    {
        /// <summary>
        ///     Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value><c>true</c> if this instance is disposed; otherwise, <c>false</c>.</value>
        public bool IsDisposed { get; private set; }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
        protected void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    Disposing();
                }
            }
            finally
            {
                IsDisposed = true;
            }
        }

        /// <summary>
        ///     Overridden in implementing objects to perform actual clean-up.
        /// </summary>
        protected virtual void Disposing()
        {
        }
    }
}