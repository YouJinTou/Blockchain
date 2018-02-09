using Models.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private ITransactionValidator transactionValidator;
        private uint difficulty;

        public Node(
            string name, 
            Uri networkAddress, 
            IBlockchainValidator blockchainValidator, 
            ITransactionValidator transactionValidator)
        {
            this.name = name;
            this.networkAddress = networkAddress;
            this.peers = new List<Node>();
            this.blockchain = new LinkedList<Block>();
            this.pendingTransactions = new List<Transaction>();
            this.miningJobs = new Dictionary<Address, uint>();
            this.balances = new Dictionary<Address, decimal>();
            this.chainValidator = blockchainValidator;
            this.transactionValidator = transactionValidator;
            this.difficulty = 2;

            Task.Run(() => TryUpdateChain());
        }

        public string Name => this.name;

        public Uri NetworkAddress => this.networkAddress;

        public ICollection<Node> Peers => this.peers;

        public LinkedList<Block> Blockchain => this.blockchain;

        public ICollection<Transaction> PendingTransactions => this.pendingTransactions;

        public IDictionary<Address, uint> MiningJobs => this.miningJobs;

        public IDictionary<Address, decimal> Balances => this.balances;

        public Block LastBlock => this.blockchain.Last?.Value;

        public uint Difficulty => this.difficulty;

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

            this.AdjustBalances(block);

            this.BroadcastBlock(block);
        }

        public void ReceiveTransaction(Transaction transaction)
        {
            this.transactionValidator.ValidateTransaction(transaction, this.balances);

            this.pendingTransactions.Add(transaction);
        }

        public void RegisterAddress(Address address, decimal balance)
        {
            if (balance < 0.0m)
            {
                throw new ArgumentException("Invalid amount. Must be non-negative.");
            }

            if (this.balances.ContainsKey(address))
            {
                throw new ArgumentException($"Address {address.Id} already exists.");
            }

            this.balances.Add(address, balance);
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

        private void AdjustBalances(Block block)
        {
            foreach (var transaction in block.Transactions)
            {
                var matchingTransaction = 
                    this.pendingTransactions.First(t => t.Hash == transaction.Hash);
                this.balances[matchingTransaction.From] -= matchingTransaction.Amount;
                this.balances[matchingTransaction.To] += matchingTransaction.Amount;

                this.pendingTransactions.Remove(matchingTransaction);
            }
        }
    }
}
