namespace Models.Hashing
{
    public interface IHasher
    {
        string GetHash(string seed);
    }
}
