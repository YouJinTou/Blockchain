using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Wallets.Models
{
    public class CreateWalletViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Private Key")]
        public string PrivateKey { get; set; }

        [Display(Name = "Public Key")]
        public string PublicKey { get; set; }

        public string Address { get; set; }
    }
}
