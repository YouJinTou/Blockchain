using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Faucet.Models
{
    public class FaucetSendViewModel
    {
        public string Address { get; set; }

        public decimal Balance { get; set; }

        [Required]
        public string To { get; set; }
    }
}
