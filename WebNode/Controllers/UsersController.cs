using Microsoft.AspNetCore.Mvc;
using System;
using WebNode.ApiModels.Users;
using WebNode.Code;

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
