using Models.Validation;
using Secp256k1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Models
{
    public class Node : IEquatable<Node>
    {
        private readonly object locker = new object();

        private string name;
        private Uri networkAddress;
        private ICollection<Node> peers;
        private LinkedList<Block> blockchain;
        private ICollection<Transaction> pendingTransactions;
        private IDictionary<string, uint> miningJobs;
        private IDictionary<string, decimal> balances;
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
            this.peers = new HashSet<Node>();
            this.blockchain = new LinkedList<Block>();
            this.pendingTransactions = new List<Transaction>();
            this.miningJobs = new Dictionary<string, uint>();
            this.balances = new Dictionary<string, decimal>();
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

        public IDictionary<string, uint> MiningJobs => this.miningJobs;

        public IDictionary<string, decimal> Balances => this.balances;

        public Block LastBlock => this.blockchain.Last?.Value;

        public uint Difficulty => this.difficulty;

        public void AddPeer(Node peer)
        {
            if (this.peers.Contains(peer))
            {
                throw new ArgumentException(
                    $"Peer with address {peer.networkAddress} already exists.");
            }

            this.peers.Add(peer);

            if (this.chainValidator.ShouldUpdateChain(this, peer))
            {
                this.UpdateChain(peer);
            }
        }

        public bool ReceiveBlock(Block block)
        {
            if (!this.chainValidator.BlockIsValid(this.blockchain.Last?.Value, block))
            {
                return false;
            }

            this.blockchain.AddLast(block);

            this.AdjustBalances(block);

            this.BroadcastBlock(block);

            return true;
        }

        public void ReceiveTransaction(Transaction transaction, SignedMessage signature)
        {
            this.transactionValidator.ValidateTransaction(transaction, this.balances, signature);

            this.pendingTransactions.Add(transaction);
        }

        public void RegisterAddress(string address, decimal balance)
        {
            if (balance < 0.0m)
            {
                throw new ArgumentException("Invalid amount. Must be non-negative.");
            }

            if (this.balances.ContainsKey(address))
            {
                throw new ArgumentException($"Address {address} already exists.");
            }

            this.balances.Add(address, balance);
        }

        public bool Equals(Node other)
        {
            if (other == null)
            {
                return false;
            }

            return this.networkAddress == other.networkAddress;
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Node);
        }

        public override int GetHashCode()
        {
            return this.networkAddress.GetHashCode();
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
            var transactionsToRemove = new List<Transaction>();

            foreach (var transaction in block.Transactions)
            {
                var matchingTransaction = 
                    this.pendingTransactions.FirstOrDefault(t => t.Hash == transaction.Hash);

                if (matchingTransaction == null)
                {
                    continue;
                }

                this.balances[matchingTransaction.From] -= matchingTransaction.Amount;
                this.balances[matchingTransaction.To] += matchingTransaction.Amount;

                transactionsToRemove.Add(matchingTransaction);
            }

            foreach (var transaction in transactionsToRemove)
            {
                this.pendingTransactions.Remove(transaction);
            }
        }

        private void BroadcastBlock(Block block)
        {
            foreach (var peer in this.peers)
            {
                peer.ReceiveBlock(block);
            }
        }
    }
}
