using Models;
using Models.Mining;
using Services.Web;
using System;

namespace MiningClient
{
    public class Client
    {
        private static readonly string Endpoint = "http://localhost:60000/";
        private static readonly string NodeEndpoint = $"{Endpoint}node/node/get";
        private static readonly string SendEndpoint = $"{Endpoint}node/mining/receive";

        private static IWebService webService;

        public static void Main()
        {
            var miner = InstantiateMiner();
            webService = new WebService();

            while (true)
            {
                var node = GetNodeInfo();
                var block = miner.MineBlock(node);

                SendBlock(block);
            }
        }

        private static Miner InstantiateMiner()
        {
            Console.Write("Address: ");

            var miner = new Miner(Console.ReadLine(), new ProofOfWork());

            return miner;
        }

        public static Node GetNodeInfo()
        {
            return webService.GetDeserialized<Node>(NodeEndpoint);
        }

        public static void SendBlock(Block block)
        {
            var responseString = webService.SendSerialized(SendEndpoint, block);

            Console.WriteLine(responseString);
        }
    }
}
