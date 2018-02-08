using System;
using System.Collections.Generic;

namespace Models
{
    public class Block
    {
        private uint id;
        private IEnumerable<Transaction> transactions;
        private uint difficulty;
        private string previousHash;
        private Address minedBy;
        private ulong nonce;
        private DateTime minedOn;
        private string hash;

        public Block(
            uint id, 
            IEnumerable<Transaction> transactions,
            uint difficulty,
            string hash,
            string previousHash,
            Address minedBy,
            ulong nonce)
        {
            this.id = id;
            this.transactions = transactions;
            this.difficulty = difficulty;
            this.hash = hash;
            this.previousHash = previousHash;
            this.minedBy = minedBy;
            this.nonce = nonce;
            this.minedOn = DateTime.Now;
        }

        public uint Id => this.id;

        public IEnumerable<Transaction> Transactions => this.transactions;

        public uint Difficulty => this.difficulty;

        public string Hash => this.hash;

        public string PreviousHash => this.previousHash;

        public Address MinedBy => this.minedBy;

        public ulong Nonce => this.nonce;

        public DateTime MinedOn => this.minedOn;
    }
}
