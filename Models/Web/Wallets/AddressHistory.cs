using System.Collections.Generic;

namespace Models.Web.Wallets
{
    public class AddressHistory
    {
        public string Address { get; set; }

        public decimal Balance { get; set; }

        public ICollection<Transaction> InTransactions { get; set; }

        public ICollection<Transaction> OutTransactions { get; set; }
    }
}
