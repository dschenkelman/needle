﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18034
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Needle.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Needle.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot commit more than once using the same instance..
        /// </summary>
        internal static string CannotCallCommitMoreThanOnce {
            get {
                return ResourceManager.GetString("CannotCallCommitMoreThanOnce", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;The mapping element must speficify a from type. Add a &apos;from&apos; attribute to specifiy the from type for the mapping.&quot;.
        /// </summary>
        internal static string FromTypeMissingFromConfigurationMapping {
            get {
                return ResourceManager.GetString("FromTypeMissingFromConfigurationMapping", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;Type to map must be in the inheritance hierarchy of type from mapping.&quot;.
        /// </summary>
        internal static string IncorrectTypeMappingTypes {
            get {
                return ResourceManager.GetString("IncorrectTypeMappingTypes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;There are many injection constructors marked in type {0}. Mark only one constructor in that class.&quot;.
        /// </summary>
        internal static string MultipleInjectionConstructors {
            get {
                return ResourceManager.GetString("MultipleInjectionConstructors", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;There is no registration with the Id &apos;{0}&apos; for the type {1} in the container.&quot;.
        /// </summary>
        internal static string RegistrationWithIdNotFound {
            get {
                return ResourceManager.GetString("RegistrationWithIdNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;The mapping element must speficify a to type. Add a &apos;to&apos; attribute to specifiy the to type for the mapping.&quot;.
        /// </summary>
        internal static string ToTypeMissingFromConfigurationMapping {
            get {
                return ResourceManager.GetString("ToTypeMissingFromConfigurationMapping", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;No injection constructor defined in type {0}.&quot;.
        /// </summary>
        internal static string UndefinedInjectionConstructor {
            get {
                return ResourceManager.GetString("UndefinedInjectionConstructor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &quot;The type {0} is cannot be instantiated directly and does not have a registered mapping.&quot;.
        /// </summary>
        internal static string UnregisteredMappingForType {
            get {
                return ResourceManager.GetString("UnregisteredMappingForType", resourceCulture);
            }
        }
    }
}