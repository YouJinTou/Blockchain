using System.ComponentModel.DataAnnotations;

namespace Models.Web.Users
{
    public class RegisterUserModel
    {
        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Invalid amount. Must be non-negative.")]
        public decimal Balance { get; set; }
    }
}
