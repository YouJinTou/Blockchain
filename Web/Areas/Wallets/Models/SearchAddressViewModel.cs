using System.ComponentModel.DataAnnotations;

namespace Web.Areas.Wallets.Models
{
    public class SearchAddressViewModel
    {
        [Required]
        public string Address { get; set; }

        public AddressHistoryViewModel AddressHistory { get; set; }
    }
}
