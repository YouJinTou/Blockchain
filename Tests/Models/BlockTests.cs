using Models;
using Models.Hashing;
using Models.Validation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.Models
{
    [TestFixture]
    public class BlockTests
    {
        private const uint Difficulty = 2;

        private Node node1;
        private Node node2;
        private Address minerAddress;
        private Block genesisBlock;
        private Block block1;

        [SetUp]
        public void SetupTest()
        {
            var validator = new BlockchainValidator(new Sha256Hasher());
            this.node1 = new Node("Node 1", new Uri("http://127.0.1.1"), validator);
            this.node2 = new Node("Node 2", new Uri("http://127.0.1.2"), validator);
            this.minerAddress = new Address("miner-address");
            this.genesisBlock = new Block(
                0, new List<Transaction>(), Difficulty, string.Empty, minerAddress, 0);
            this.block1 = this.GetValidBlock(this.genesisBlock.Hash, this.genesisBlock.Id);
        }

        [TestCase]
        public void ReceiveBlock_ValidGenesis_ChainLength1()
        {
            this.node1.ReceiveBlock(this.genesisBlock);

            Assert.AreEqual(1, this.node1.Blockchain.Count);
        }

        [TestCase]
        public void ReceiveBlock_FirstBlock_ChainLength2()
        {
            this.node1.ReceiveBlock(this.genesisBlock);
            this.node1.ReceiveBlock(this.block1);

            Assert.AreEqual(2, this.node1.Blockchain.Count);
        }

        [TestCase]
        public void AddPeer_InvalidChain_DoNothing()
        {
            this.node1.AddPeer(this.node2);

            Assert.That(this.node1.Blockchain.Count == 0);
        }

        [TestCase]
        public void AddPeer_PeerHasLongerChain_ReplaceChain()
        {
            this.node1.ReceiveBlock(this.genesisBlock);

            this.node2.AddPeer(this.node1);

            Assert.That(this.node2.Blockchain.Count == 1);
        }

        [TestCase]
        public async Task TryUpdateChain_PeerAddedBlockAfterHandshake_UpdateChainAfterPoll()
        {
            this.node1.AddPeer(this.node2);

            this.node2.ReceiveBlock(genesisBlock);

            await Task.Delay(TimeSpan.FromSeconds(1.1));

            Assert.That(this.node1.Blockchain.Count == 1);
        }

        private Block GetValidBlock(string prevBlockHash, uint prevId)
        {
            uint nonce = 0;

            while (true)
            {
                var block = new Block(1, new List<Transaction>(), Difficulty, prevBlockHash, this.minerAddress, nonce);
                var actualLeadingString = block.Hash.Substring(0, (int)block.Difficulty);
                var expectedLeadingString = new string('0', (int)block.Difficulty);

                if (actualLeadingString.Equals(expectedLeadingString))
                {
                    return block;
                }

                nonce++;
            }
        }
    }
}
