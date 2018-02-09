using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Register([FromBody]RegisterUserModel model)
        {
            this.NodeService.RegisterAddress(model);

            return Ok();
        }
    }
}
