using System.Collections.Generic;

namespace Web.Areas.Wallets.Models
{
    public class AddressHistoryViewModel
    {
        public string Address { get; set; }

        public decimal Balance { get; set; }

        public ICollection<TransactionViewModel> InTransactions { get; set; }

        public ICollection<TransactionViewModel> OutTransactions { get; set; }
    }
}
