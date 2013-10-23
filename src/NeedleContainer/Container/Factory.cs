using System;

namespace Needle.Container
{
    /// <summary>
    /// Encapsulates a method that acts as a factory for elements of type <paramref name="T"/>. This is required because in <see cref="System.Func{TResult}"/> is not contravariant in PCL projects.
    /// </summary>
    /// 
    /// <returns>
    /// The object to be created.
    /// </returns>
    /// <typeparam name="T">The type of the object to be created.</typeparam>
    public delegate T Factory<out T>();
}
