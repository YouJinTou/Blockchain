using Microsoft.AspNetCore.Mvc;
using Models.Web.Users;
using Services.Nodes;
using System;

namespace WebNode.Controllers
{
    [Route("Users")]
    public class UsersController : BaseController
    {
        public UsersController(INodeService nodeService)
            : base(nodeService)
        {
        }

        [HttpPost("Register")]
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
