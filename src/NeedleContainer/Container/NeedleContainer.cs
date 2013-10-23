namespace Needle.Container
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Needle.Builder;
    using Needle.Container.Fluency;
    using Needle.Exceptions;
    using Needle.Helpers;
    using Needle.Properties;
    
    /// <summary>
    /// NeedleContainer implementation. Helps decoupling components by injecting them with their required dependencies.
    /// </summary>
    public partial class NeedleContainer
    {
        private static readonly string DefaultRegistrationId = string.Empty;

        private readonly IBuilder builder;
        private readonly IDictionary<Type, List<TypeMapping>> typeMappings;
        private readonly IDictionary<Type, List<InstanceRegistration>> instanceMappings;

        /// <summary>
        /// Initializes a new instance of the <see cref="NeedleContainer"/> class.
        /// </summary>
        public NeedleContainer()
        {
            this.builder = new NeedleBuilder();
            this.typeMappings = new Dictionary<Type, List<TypeMapping>>();
            this.instanceMappings = new Dictionary<Type, List<InstanceRegistration>>();
            this.Store<INeedleContainer>(this).Commit();
        }

        /// <summary>
        /// Gets an object from the container.
        /// </summary>
        /// <param name="type">The type of the object to get from the container.</param>
        /// <returns>
        /// Returns an instance of the requested object based on the previous container configuration.
        /// </returns>
        public object Get(Type type)
        {
            return this.Get(type, DefaultRegistrationId);
        }

        /// <summary>
        /// Gets an object from the container.
        /// </summary>
        /// <param name="type">The type of the object to get from the container.</param>
        /// <param name="id">The id with which the object to obtain was registered.</param>
        /// <returns>
        /// Returns an instance of the requested object based on the previous container configuration.
        /// </returns>
        public object Get(Type type, string id)
        {
            TypeMapping mapping = this.GetTypeMapping(type, id);

            return this.GetInstanceFromType(mapping);
        }

        /// <summary>
        /// Gets all registered instances in the container for a given type.
        /// </summary>
        /// <param name="type">The type of the objects to get from the container.</param>
        /// <returns>
        /// Returns all instances of the requested object based on the previous container configuration.
        /// </returns>
        public IEnumerable<object> GetAll(Type type)
        {
            IEnumerable<TypeMapping> mappings = this.GetMappingsForType(type);
            
            if (mappings == null)
            {
                // there are no mappings, assume the type can be instantiated
                if (this.instanceMappings.ContainsKey(type))
                {
                    return this.instanceMappings[type].Select(m => m.Instance);
                }

                throw new CreationException(string.Format(Resources.UnregisteredMappingForType, type.FullName));
            }

            return this.GetAllInternal(mappings);
        }

        /// <summary>Receives a type for which a mapping needs to be established.</summary>
        /// <param name="typeFrom">The type to be mapped from.</param>
        /// <returns>An object that can be used to map the type to another type.</returns>
        public IMappable<object> Map(Type typeFrom)
        {
            return this.GenericMapping<object>(typeFrom);
        }

        /// <summary>
        /// Stores an object in the container so it can be obtained in the future.
        /// </summary>
        /// <param name="type">The type to register the instance. This type will be used to get the object.</param>
        /// <param name="instance">The instance of the object.</param>
        public ICommittableIdentifiable Store(Type type, object instance)
        {
            Guard.ThrowIfNullArgument(instance, "instance");

            Type mappedType = type;

            if (!NeedleContainer.CanBeDirectlyInstantiated(type))
            {
                mappedType = instance.GetType();
            }

            var mapping = new TypeMapping(DefaultRegistrationId, type, mappedType, RegistrationLifetime.Singleton);
            var registration = new InstanceRegistration(DefaultRegistrationId, instance);

            return new StorageRegistrationProxy(mapping, registration, this.StoreInternal);
        }

        #region Private Members

        /// <summary>
        /// Determines whether the type provided can be instantiated.
        /// </summary>
        /// <param name="t">The type to be verified for instantiation.</param>
        /// <returns>
        /// <c>true</c> if the type can be instantiated; otherwise, <c>false</c>.
        /// </returns>
        private static bool CanBeDirectlyInstantiated(Type t)
        {
            return !((t.IsInterface || t.IsAbstract) && !t.IsGenericType);
        }

        private void MapInternal(TypeMapping mapping, InstanceRegistration registration, Factory<object> factory)
        {
            this.CreateTypeMappingsForTypeIfNecessary(mapping.FromType);

            this.UpdateTypeMappingsForType(mapping.FromType, mapping);

            if (mapping.Lifetime == RegistrationLifetime.Singleton && !this.instanceMappings.ContainsKey(mapping.ToType))
            {
                this.instanceMappings[mapping.ToType] = new List<InstanceRegistration>();
            }

            if (factory != null)
            {
                this.builder.AddFactoryMethod(mapping, factory);
            }
        }
        
        private void StoreInternal(TypeMapping mapping, InstanceRegistration registration)
        {
            this.CreateTypeMappingsForTypeIfNecessary(mapping.FromType);

            this.UpdateTypeMappingsForType(mapping.FromType, mapping);

            this.CreateInstanceMappingsForTypeIfNecessary(mapping.ToType);

            this.StoreNamedInstanceInternal(mapping.ToType, registration);
        }

        /// <summary>
        /// Updates the type mappings for the particular type with the TypeMapping provided. 
        /// If there is already a mapping with the same id then it is overwritten. Otherwise, the new mappings is added.
        /// </summary>
        /// <param name="fromType">From.</param>
        /// <param name="mapping">The mapping.</param>
        private void UpdateTypeMappingsForType(Type fromType, TypeMapping mapping)
        {
            var mappingForType = this.typeMappings[fromType];

            var existingMapping = mappingForType.SingleOrDefault(m => m.Id == mapping.Id);

            if (existingMapping != null)
            {
                // there is already a mapping for the type with the same name
                this.typeMappings[fromType].Remove(existingMapping);
            }
         
            this.typeMappings[fromType].Add(mapping);
        }

        /// <summary>
        /// Gets an instance of the provided type with the specified type mapping.
        /// </summary>
        /// <param name="mapping">The type mapping to obtain the instance from.</param>
        /// <returns></returns>
        private object GetInstanceFromType(TypeMapping mapping)
        {
            if (mapping.Lifetime == RegistrationLifetime.Singleton)
            {
                return this.GetRegisteredSingleton(mapping, mapping.Id);
            }

            return this.builder.Build(mapping, this);
        }

        /// <summary>
        /// Determines whether there is an instance of the specified type stored with the provided id.
        /// </summary>
        /// <param name="type">The type of the instance.</param>
        /// <param name="id">The id of the registration.</param>
        /// <returns><c>true</c> if there is an instance of the specified type stored with the provided id; otherwise, <c>false</c>.
        /// </returns>
        private bool IsInstanceMappingWithIdStored(Type type, string id)
        {
            return this.instanceMappings.ContainsKey(type) && this.instanceMappings[type].Any(im => im.Id == id);
        }

        /// <summary>
        /// Gets the registered singleton instance.
        /// </summary>
        /// <param name="typeMapping">Type of the object to obtain.</param>
        /// <param name="id">The id of the instance to obtain.</param>
        /// <returns></returns>
        private object GetRegisteredSingleton(TypeMapping typeMapping, string id)
        {
            if (this.instanceMappings[typeMapping.ToType].Count == 0 || !this.IsInstanceMappingWithIdStored(typeMapping.ToType, id))
            {
                object newInstance = this.builder.Build(typeMapping, this);
                this.instanceMappings[typeMapping.ToType].Add(new InstanceRegistration(id, newInstance));
            }

            InstanceRegistration registration = this.GetRegistrationForTypeWithId(typeMapping.ToType, id);
            if (registration == null)
            {
                throw new CreationException();
            }

            return registration.Instance;
        }

        private InstanceRegistration GetRegistrationForTypeWithId(Type mappedType, string id)
        {
            List<InstanceRegistration> registrations = this.instanceMappings[mappedType];
            InstanceRegistration registration = registrations.FirstOrDefault(r => id.Equals(r.Id));
            return registration;
        }

        /// <summary>
        /// Gets the type mapped for the provided type.
        /// </summary>
        /// <param name="type">The type from which to find a mapped type.</param>
        /// <param name="id">The id under which the mapping is registered</param>
        /// <returns>The <see cref="TypeMapping"/> instance that represents the mapping.</returns>
        private TypeMapping GetTypeMapping(Type type, string id)
        {
            if (this.typeMappings.ContainsKey(type))
            {
                var mappings = this.typeMappings[type];
                var mapping = mappings.FirstOrDefault(m => m.Id == id);

                if (mapping != null)
                {
                    return mapping;
                }       
            }
            
            if (!CanBeDirectlyInstantiated(type))
            {
                // not instantiatable and mapping could not be found => throw
                throw new CreationException(string.Format(CultureInfo.CurrentCulture, Resources.UnregisteredMappingForType, type));
            }

            if (!string.IsNullOrEmpty(id) && !this.IsInstanceMappingWithIdStored(type, id))
            {
                // can be instantiated, but there is no mapping for the id in the singletons. 
                // there is no id for Id if all that is required is the type
                throw new RegistrationNotFoundException(string.Format(CultureInfo.CurrentCulture, Resources.RegistrationWithIdNotFound, id, type));
            }
            
            // no mapping was found but the type can be instantiated. create mock mapping object to reflect what the situation is.
            return new TypeMapping(id, type, type, RegistrationLifetime.Transient);
        }

        /// <summary>
        /// Stores the a named instance.
        /// </summary>
        /// <param name="mappedType">The mapped type for the provided instance.</param>
        /// <param name="registration">The <see cref="InstanceRegistration"/> to store.</param>
        private void StoreNamedInstanceInternal(Type mappedType, InstanceRegistration registration)
        {
            List<InstanceRegistration> instanceRegistrations = this.instanceMappings[mappedType];
            var existingRegistration = instanceRegistrations.SingleOrDefault(r => registration.Id.Equals(r.Id));

            if (existingRegistration != null)
            {
                // there is already an instance registered with the same id
                instanceRegistrations.Remove(existingRegistration);
            }
            
            this.instanceMappings[mappedType].Add(registration);
        }

        /// <summary>
        /// Gets all registered objects of the specified type.
        /// </summary>
        /// <param name="mappings">All type mappings that are stored for a particular type.</param>
        /// <returns></returns>
        private IEnumerable<object> GetAllInternal(IEnumerable<TypeMapping> mappings)
        {
            var objects = new List<object>();

            foreach (var mapping in mappings)
            {
                if (!this.instanceMappings.ContainsKey(mapping.ToType))
                {
                    throw new CreationException(string.Format(Resources.UnregisteredMappingForType, mapping.ToType.FullName));
                }

                object instance;

                TypeMapping mapping1 = mapping;
                if (this.instanceMappings[mapping.ToType].Any(r => r.Id == mapping1.Id))
                {
                    // there is an instance mapping => use it
                    instance = this.instanceMappings[mapping.ToType].First(r => r.Id == mapping1.Id).Instance;
                }
                else 
                {
                    // there is no instance mapping, the mapping represents  a first time singleton or transient.
                    instance = this.Get(mapping.FromType, mapping.Id);
                }

                objects.Add(instance);
            }
            
            return objects.AsEnumerable();
        }

        /// <summary>
        /// Instantiates the type mappings list for the type if necessary.
        /// </summary>
        /// <param name="type">The type for which the list must be created.</param>
        private void CreateTypeMappingsForTypeIfNecessary(Type type)
        {
            if (!this.typeMappings.ContainsKey(type))
            {
                this.typeMappings[type] = new List<TypeMapping>();
            }
        }

        /// <summary>
        /// Instantiates the instance mappings list for the type if necessary.
        /// </summary>
        /// <param name="type">The type for which the list must be created.</param>
        private void CreateInstanceMappingsForTypeIfNecessary(Type type)
        {
            if (!this.instanceMappings.ContainsKey(type))
            {
                this.instanceMappings[type] = new List<InstanceRegistration>();
            }
        }

        /// <summary>
        /// Gets the mapped types for the type provided as parameter.
        /// </summary>
        /// <param name="type">The type to obtain the mapped types from.</param>
        /// <returns></returns>
        private IEnumerable<TypeMapping> GetMappingsForType(Type type)
        {
            if (!this.typeMappings.ContainsKey(type))
            {
                return null;
            }

            return this.typeMappings[type];
        }

        private Mappable<T> GenericMapping<T>(Type typeFrom) where T : class
        {
            var mapping = new TypeMapping(DefaultRegistrationId, typeFrom, null, RegistrationLifetime.Transient);
            return new Mappable<T>(mapping, null, this.MapInternal);
        }
        #endregion
    }
}
