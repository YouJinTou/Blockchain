using Models;
using Models.Hashing;
using Models.Validation;
using System;

namespace MiningClient
{
    public class Client
    {
        public static void Main()
        {
            var miner = InstantiateMiner();

            miner.MineBlock();
        }

        private static Miner InstantiateMiner()
        {
            Console.Write("Address: ");

            var address = new Address(Console.ReadLine());

            Console.Write("Node URL: ");

            var hasher = new Sha256Hasher();
            var chainValidator = new BlockchainValidator(hasher);
            var txValidator = new TransactionValidator(hasher);
            var node = new Node("Node", new Uri(Console.ReadLine()), chainValidator, txValidator);
            var miner = new Miner(address, node, hasher);

            return miner;
        }
    }
}
