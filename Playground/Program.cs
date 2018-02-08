using Models;
using System;

namespace Playground
{
    class Program
    {
        static void Main()
        {
            var from = new Address("123");
            var to = new Address("456");
            var amount = 10.0m;
            var sig = "123";
            uint blockId = 0;
            var tx = new Transaction(from, to, amount, sig, blockId);

            Console.WriteLine(tx.Hash);
        }
    }
}
