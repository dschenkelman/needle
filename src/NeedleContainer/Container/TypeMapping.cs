namespace Needle.Container
{
    using System;

    internal class TypeMapping
    {
        public TypeMapping(string id, Type fromType, Type toType, RegistrationLifetime lifetime)
        {
            this.Id = id;
            this.FromType = fromType;
            this.ToType = toType;
            this.Lifetime = lifetime;
        }

        internal Type FromType { get; private set; }

        internal Type ToType { get; set; }

        internal string Id { get; set; }

        internal RegistrationLifetime Lifetime { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) 
            {
                return false; 
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != typeof(TypeMapping)) 
            {
                return false;
            }

            return this.Equals((TypeMapping)obj);
        }

        public bool Equals(TypeMapping other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return object.Equals(other.FromType, this.FromType) && object.Equals(other.ToType, this.ToType) && object.Equals(other.Id, this.Id) && object.Equals(other.Lifetime, this.Lifetime);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = this.FromType != null ? this.FromType.GetHashCode() : 0;
                result = (result * 397) ^ (this.ToType != null ? this.ToType.GetHashCode() : 0);
                result = (result * 397) ^ (this.Id != null ? this.Id.GetHashCode() : 0);
                result = (result * 397) ^ this.Lifetime.GetHashCode();
                return result;
            }
        }
    }
}