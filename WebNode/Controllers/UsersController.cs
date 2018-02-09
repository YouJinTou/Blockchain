using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public IActionResult Register(RegisterUserModel model)
        {
            return null;
        }
    }
}
