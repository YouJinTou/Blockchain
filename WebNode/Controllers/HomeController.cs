using Microsoft.AspNetCore.Mvc;
using Services.Nodes;

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
