using Microsoft.AspNetCore.Mvc;
using Services.Nodes;

namespace Web.Controllers
{
    [Area("Node")]
    [Route("[area]")]
    public class BaseController : Controller
    {
        private INodeService nodeService;

        public BaseController(INodeService nodeService)
        {
            this.nodeService = nodeService;
        }

        protected INodeService NodeService => this.nodeService;

        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
