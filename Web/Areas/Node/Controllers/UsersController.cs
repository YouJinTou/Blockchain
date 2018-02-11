using Microsoft.AspNetCore.Mvc;
using Models.Web.Users;
using Services.Nodes;
using System;

namespace Web.Areas.Node.Controllers
{
    [Route("[area]/[controller]")]
    public class UsersController : BaseController
    {
        public UsersController(INodeService nodeService)
            : base(nodeService)
        {
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Register([FromBody]RegisterUserModel model)
        {
            try
            {
                this.NodeService.RegisterAddress(model);
            }
            catch (ArgumentException aex)
            {
                return BadRequest(aex.Message);
            }

            return Ok("Address registered.");
        }
    }
}
