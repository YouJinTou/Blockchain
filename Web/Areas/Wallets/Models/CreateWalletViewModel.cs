using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class CreateWalletViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Private Key")]
        public string PrivateKey { get; set; }

        public string Address { get; set; }
    }
}
