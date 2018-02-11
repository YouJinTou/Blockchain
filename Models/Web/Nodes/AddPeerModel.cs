using System;
using System.ComponentModel.DataAnnotations;

namespace Models.Web.Nodes
{
    public class AddPeerModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public Uri NetworkAddress { get; set; }
    }
}
