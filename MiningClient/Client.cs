﻿using Models;
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
            var responseString = client.GetStringAsync("http://localhost:60000/node").Result;
            var node = JsonConvert.DeserializeObject<Node>(responseString);

            return node;
        }

        public static void SendBlock(Block block)
        {
            var content = new StringContent(JsonConvert.SerializeObject(block), Encoding.UTF8, "application/json");
            var response = client.PostAsync(new Uri("http://localhost:60000/mining/receive"), content).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;

            Console.WriteLine(responseString);
        }
    }
}
