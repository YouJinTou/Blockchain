using System;

namespace Web.Areas.Explorer.Models
{
    public class ExplorerViewModel
    {
        public int Id { get; set; }

        public int TransactionsCount { get; set; }

        public int Difficulty { get; set; }

        public string Hash { get; set; }

        public string PreviousHash { get; set; }

        public string MinedBy { get; set; }

        public ulong Nonce { get; set; }

        public DateTime MinedOn { get; set; }
    }
}
