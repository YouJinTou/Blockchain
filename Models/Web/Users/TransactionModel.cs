namespace Models.Web.Users
{
    public class TransactionModel
    {
        public string From { get; set; }

        public string To { get; set; }

        public decimal Amount { get; set; }

        public string Signature { get; set; }
    }
}
