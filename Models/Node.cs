using Models.Validation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models
{
    public class Node
    {
        private readonly object locker = new object();

        private string name;
        private Uri networkAddress;
        private ICollection<Node> peers;
        private LinkedList<Block> blockchain;
        private ICollection<Transaction> pendingTransactions;
        private IDictionary<Address, uint> miningJobs;
        private IDictionary<Address, decimal> balances;
        private IBlockchainValidator chainValidator;

        public Node(string name, Uri networkAddress, IBlockchainValidator blockchainValidator)
        {
            this.name = name;
            this.networkAddress = networkAddress;
            this.peers = new List<Node>();
            this.blockchain = new LinkedList<Block>();
            this.pendingTransactions = new List<Transaction>();
            this.miningJobs = new Dictionary<Address, uint>();
            this.balances = new Dictionary<Address, decimal>();
            this.chainValidator = blockchainValidator;

            Task.Run(() => TryUpdateChain());
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

            if (this.chainValidator.ShouldUpdateChain(this, peer))
            {
                this.UpdateChain(peer);
            }
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
            if (!this.chainValidator.BlockIsValid(this.blockchain.Last?.Value, block))
            {
                return;
            }

            this.blockchain.AddLast(block);

            this.BroadcastBlock(block);
        }

        private void TryUpdateChain()
        {
            while (true)
            {
                foreach (var peer in this.peers)
                {
                    if (this.chainValidator.ShouldUpdateChain(this, peer))
                    {
                        this.UpdateChain(peer);
                    }
                }

                Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        private void UpdateChain(Node peer)
        {
            lock (this.locker)
            {
                this.blockchain = peer.Blockchain;
            }
        }
    }
}
