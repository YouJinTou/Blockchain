using Microsoft.AspNetCore.Mvc;
using Services.Nodes;

namespace Web.Controllers
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
