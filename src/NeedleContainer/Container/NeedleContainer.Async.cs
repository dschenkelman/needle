namespace Needle.Container
{
    using System;
    using System.Threading.Tasks;

    using Needle.Exceptions;

    public partial class NeedleContainer
    {
        /// <summary>
        /// Gets an object from the container asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the object to get from the container.</typeparam>
        /// <param name="getCallback">Callback method to be executed once the object is obtained. Receives the object and an GetAsyncResult instance.</param>
        /// <seealso cref="GetAsyncResult"/>
        public Task<T> GetAsync<T>()
        {
            return Task.Run(() => this.Get<T>());
        }

        /// <summary>
        /// Gets an object from the container asynchronously.
        /// </summary>
        /// <param name="type">The type of the object to get from the container.</param>
        /// <param name="getCallback">Callback method to be executed once the object is obtained. Receives the object and an GetAsyncResult instance.</param>
        /// <seealso cref="GetAsyncResult"/>
        public Task<object> GetAsync(Type type)
        {
            return Task.Run(() => this.Get(type));
        }
    }
}
