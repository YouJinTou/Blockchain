namespace Models.Mining
{
    public interface IConsensusStrategy
    {
        Block GetBlock(ChainStats chainStats);
    }
}
