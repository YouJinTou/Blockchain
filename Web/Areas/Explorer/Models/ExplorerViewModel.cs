using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Explorer.Models
{
    public class ExplorerViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Transactions Count")]
        public int TransactionsCount { get; set; }

        public int Difficulty { get; set; }

        public string Hash { get; set; }

        [Display(Name = "Last Hash")]
        public string PreviousHash { get; set; }

        [Display(Name = "Miner")]
        public string MinedBy { get; set; }

        public ulong Nonce { get; set; }

        [Display(Name = "Timestamp")]
        public DateTime MinedOn { get; set; }
    }
}
