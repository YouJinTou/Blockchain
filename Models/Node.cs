using Models.Validation;
using System;
using System.Collections.Generic;

namespace Models
{
    public class Node
    {
        private string name;
        private Uri networkAddress;
        private ICollection<Node> peers;
        private LinkedList<Block> blockchain;
        private ICollection<Transaction> pendingTransactions;
        private IDictionary<Address, uint> miningJobs;
        private IDictionary<Address, decimal> balances;
        private IBlockValidator blockValidator;

        public Node(string name, Uri networkAddress, IBlockValidator blockValidator)
        {
            this.name = name;
            this.networkAddress = networkAddress;
            this.peers = new List<Node>();
            this.blockchain = new LinkedList<Block>();
            this.pendingTransactions = new List<Transaction>();
            this.miningJobs = new Dictionary<Address, uint>();
            this.balances = new Dictionary<Address, decimal>();
            this.blockValidator = blockValidator;
        }

        public string Name => this.name;

        public Uri NetworkAddress => this.networkAddress;

        public ICollection<Node> Peers => this.peers;

        public LinkedList<Block> Blockchain => this.blockchain;

        public ICollection<Transaction> PendingTransactions => this.pendingTransactions;

        public IDictionary<Address, uint> MiningJobs => this.miningJobs;

        public IDictionary<Address, decimal> Balances => this.balances;

        public void AddPeer(Node peer)
        {
            this.peers.Add(peer);
        }

        public void BroadcastBlock(Block block)
        {
            foreach (var peer in this.peers)
            {
                peer.ReceiveBlock(block);
            }
        }

        public void ReceiveBlock(Block block)
        {
            if (!this.BlockIsValid(block))
            {
                return;
            }
        }

        private bool BlockIsValid(Block block)
        {
            return this.blockValidator.BlockIsValid(
                this.pendingTransactions, this.blockchain.Last.Value, block);
        }
    }
}
