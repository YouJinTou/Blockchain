using System;

namespace Models
{
    public class Address : IEquatable<Address>
    {
        private string id;

        public Address(string id)
        {
            this.id = id;
        }

        public string Id => id;

        public bool Equals(Address other)
        {
            return this.id == other.id;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Address);
        }

        public override int GetHashCode()
        {
            return this.id.GetHashCode();
        }

        public override string ToString()
        {
            return this.id;
        }
    }
}
