using Microsoft.AspNetCore.Mvc;
using WebNode.Code;

namespace WebNode.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(INodeService nodeService)
            : base(nodeService)
        {
        }

        public IActionResult Index()
        {
            return Ok();
        }
    }
}
