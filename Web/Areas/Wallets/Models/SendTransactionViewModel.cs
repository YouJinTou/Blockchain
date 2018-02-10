using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Wallets.Models
{
    public class SendTransactionViewModel
    {
        [Required]
        public string From { get; set; }

        [Required]
        public string To { get; set; }

        [Required]
        [Range(0.01d, double.MaxValue, ErrorMessage = "Invalid amount.")]
        public string Amount { get; set; }

        [Required]
        [Display(Name = "Private Key")]
        public string PrivateKey { get; set; }
    }
}
