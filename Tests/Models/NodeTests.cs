using Models;
using Models.Hashing;
using Models.Mining;
using Models.Validation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests.Models
{
    [TestFixture]
    public class NodeTests
    {

        private Node node1;
        private Node node2;
        private Block genesisBlock;
        private Block block1;

        [SetUp]
        public void SetupTest()
        {
            var chainValidator = new BlockchainValidator(new Sha256Hasher());
            var txValidator = new TransactionValidator(new Sha256Hasher(), new MessageSignerVerifier());
            var consensusAlgorithm = new ProofOfWork();
            uint difficulty = 2;
            var minerAddress = "miner-address";
            this.node1 = new Node("Node 1", new Uri("http://127.0.1.1"), chainValidator, txValidator);
            this.node2 = new Node("Node 2", new Uri("http://127.0.1.2"), chainValidator, txValidator);
            this.genesisBlock = new Block(
                0, new List<Transaction>(), difficulty, string.Empty, minerAddress, 0);
            this.block1 = consensusAlgorithm.GetBlock(new ChainStats
            {
                Difficulty = difficulty,
                LastBlockHash = this.genesisBlock.Hash,
                LastBlockId = this.genesisBlock.Id,
                MinerAddress = minerAddress,
                PendingTransactions = new List<Transaction>()
            });

            this.node1.RegisterAddress(minerAddress, 0.0m);
            this.node2.RegisterAddress(minerAddress, 0.0m);
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
        public void ReceiveBlock_BroadcastBlock_PeerBlocksUpdated()
        {
            this.node1.AddPeer(this.node2);

            this.node1.ReceiveBlock(this.genesisBlock);
            this.node1.ReceiveBlock(this.block1);

            Assert.That(this.node2.Blockchain.Count == 2);
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
    }
}
