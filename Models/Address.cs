namespace Models
{
    public class Address
    {
        private string id;

        public Address(string id)
        {
            this.id = id;
        }

        public string Id => id;

        public override string ToString()
        {
            return this.id;
        }
    }
}
