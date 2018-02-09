using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace MiningClient
{
    public class Client
    {
        private static readonly HttpClient client = new HttpClient();

        public Client()
        {
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

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

            var miner = new Miner(Console.ReadLine());

            return miner;
        }

        public static Node GetNodeInfo()
        {
            var responseString = client.GetStringAsync("http://localhost:54532/node").Result;
            var node = JsonConvert.DeserializeObject<Node>(responseString);

            return node;
        }

        public static void SendBlock(Block block)
        {
            var blockAsDictionary = new Dictionary<string, string>
            {
                { "id", block.Id.ToString() },
                { "transactions", JsonConvert.SerializeObject(block.Transactions) },
                { "difficulty", block.Difficulty.ToString() },
                { "previousHash", block.PreviousHash },
                { "minedBy", block.MinedBy },
                { "nonce", block.Nonce.ToString() },
                { "minedOn", block.MinedOn.Ticks.ToString() },
                { "hash", block.Hash }
            };
            var content = new StringContent(
                JsonConvert.SerializeObject(blockAsDictionary), Encoding.UTF8, "application/json");
            var response = client.PostAsync(new Uri("http://localhost:54532/mining/receive"), content).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;

            Console.WriteLine(responseString);
        }
    }
}
