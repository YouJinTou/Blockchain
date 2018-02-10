namespace Web.Areas.Wallets.Models
{
    public class TransactionViewModel
    {
        public string Hash { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public decimal Amount { get; set; }
    }
}
