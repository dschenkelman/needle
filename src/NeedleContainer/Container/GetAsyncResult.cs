namespace Needle.Container
{
    using Needle.Exceptions;

    public class GetAsyncResult
    {
        public GetAsyncResult(bool wasSuccessful, CreationException exception)
        {
            this.WasSuccessful = wasSuccessful;
            this.Exception = exception;
        }

        /// <summary>
        /// Gets any exception that ocurred when getting the object asynchronously.
        /// </summary>
        /// <value>The exception.</value>
        public CreationException Exception { get; private set; }

        /// <summary>
        /// Gets a value indicating whether was successful.
        /// </summary>
        /// <value><c>true</c> if the asynchronous operation was successeful; otherwise, <c>false</c>.</value>
        public bool WasSuccessful { get; private set; }
    }
}
