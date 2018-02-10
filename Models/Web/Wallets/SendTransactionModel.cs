namespace Models.Web.Wallets
{
    public class SendTransactionModel
    {
        public string From { get; set; }

        public string To { get; set; }

        public decimal Amount { get; set; }

        public string PrivateKey { get; set; }
    }
}
