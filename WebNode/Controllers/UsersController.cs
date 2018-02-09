using Microsoft.AspNetCore.Mvc;
using System;
using WebNode.ApiModels.Users;
using WebNode.Code;

namespace WebNode.Controllers
{
    public class UsersController : BaseController
    {
        public UsersController(INodeService nodeService)
            : base(nodeService)
        {
        }

        [HttpPost]
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

            return Ok();
        }
    }
}
