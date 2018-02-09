using Models;
using System.Collections.Generic;

namespace WebNode.Code
{
    public class GenesisBlockGenerator : IBlockGenerator
    {
        public Block GenerateBlock()
        {
            return new Block(0, new List<Transaction>(), 0, "NONE", "NONE", 0);
        }
    }
}
