namespace Needle.Container
{
    using System;
    using System.Collections.Generic;
    using Needle.Container.Fluency;

    public interface INeedleContainer : IHideObjectMethods
    {
        /// <summary>Gets an object from the container.</summary>
        /// <typeparam name="T">The type of the object to get from the container.</typeparam>
        /// <returns>Returns an instance of the requested object based on the previous container configuration.</returns>
        T Get<T>();
       
        /// <summary>Gets an object from the container registered with the specified id.</summary>
        /// <typeparam name="T">The type of the object to get from the container.</typeparam>
        /// <param name="id">The id with which the object to obtain was registered.</param>
        /// <returns>Returns an instance of the requested object based on the previous container configuration.</returns>
        T Get<T>(string id);
       
        /// <summary>Gets an object from the container.</summary>
        /// <param name="type">The type of the object to get from the container.</param>
        /// <returns>Returns an instance of the requested object based on the previous container configuration.</returns>
        object Get(Type type);
        
        /// <summary>Gets an object from the container.</summary>
        /// <param name="type">The type of the object to get from the container.</param>
        /// <param name="id">The id with which the object to obtain was registered.</param>
        /// <returns>Returns an instance of the requested object based on the previous container configuration.</returns>
        object Get(Type type, string id);
        
        /// <summary>Gets all registered instances in the container for a given type.</summary>
        /// <typeparam name="T">The type of the objects to get from the container.</typeparam>
        /// <returns>Returns all instances of the requested object based on the previous container configuration.</returns>
        IEnumerable<T> GetAll<T>();

        /// <summary>Gets all registered instances in the container for a given type.</summary>
        /// <param name="type">The type of the objects to get from the container.</param>
        /// <returns>Returns all instances of the requested object based on the previous container configuration.</returns>
        IEnumerable<object> GetAll(Type type);

        /// <summary>Receives a type for which a mapping needs to be established.</summary>
        /// <param name="typeFrom">The type to be mapped from.</param>
        /// <returns>An object that can be used to map the type to another type.</returns>
        IMappable<object> Map(Type typeFrom);
        
        /// <summary>Receives a type for which a mapping needs to be established.</summary>
        /// <typeparam name="TFrom">The type to be mapped from.</typeparam>
        /// <returns>An object that can be used to map the type to another type.</returns>
        IMappable<TFrom> Map<TFrom>() where TFrom : class;

        /// <summary>
        /// Stores an object in the container so it can be obtained in the future.
        /// </summary>
        /// <typeparam name="T">The type to register the instance. This type will be used to get the object.</typeparam>
        /// <param name="instance">The instance of the object.</param>
        /// <returns>Returns an instance that can be used to commit the storage in the container or name the stored instance.</returns>
        ICommittableIdentifiable Store<T>(T instance);

        /// <summary>
        /// Stores an object in the container so it can be obtained in the future.
        /// </summary>
        /// <param name="type">The type to register the instance. This type will be used to get the object.</param>
        /// <param name="instance">The instance of the object.</param>
        /// <returns>Returns an instance that can be used to commit the storage in the container or name the stored instance.</returns>
        ICommittableIdentifiable Store(Type type, object instance);
    }
}
