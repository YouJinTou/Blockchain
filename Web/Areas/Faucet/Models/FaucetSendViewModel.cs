using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Faucet.Models
{
    public class FaucetSendViewModel
    {
        public decimal Balance { get; set; }

        [Required]
        public string To { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Invalid amount.")]
        public decimal Amount { get; set; }
    }
}
