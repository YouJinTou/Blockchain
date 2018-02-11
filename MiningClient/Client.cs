﻿using Models;
using Models.Mining;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace MiningClient
{
    public class Client
    {
        private static readonly string Endpoint = "http://localhost:60000/";
        private static readonly string NodeEndpoint = $"{Endpoint}node/node/get";
        private static readonly string SendEndpoint = $"{Endpoint}node/mining/receive";
        private static readonly HttpClient client = new HttpClient();

        public static void Main()
        {
            var miner = InstantiateMiner();

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
            var responseString = client.GetStringAsync(NodeEndpoint).Result;
            var node = JsonConvert.DeserializeObject<Node>(responseString);

            return node;
        }

        public static void SendBlock(Block block)
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(block), Encoding.UTF8, "application/json");
            var response = client.PostAsync(new Uri(SendEndpoint), content).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;

            Console.WriteLine(responseString);
        }
    }
}
