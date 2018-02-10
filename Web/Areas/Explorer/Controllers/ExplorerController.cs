using Microsoft.AspNetCore.Mvc;
using Services.Nodes;

namespace Web.Controllers
{
    [Area("Explorer")]
    [Route("[area]")]
    [Route("[area]/[controller]")]
    public class ExplorerController : Controller
    {
        private INodeService nodeService;

        public ExplorerController(INodeService nodeService)
        {
            this.nodeService = nodeService;
        }

        [HttpGet]
        [Route("")]
        [Route("[action]")]
        [Route("[area]/[action]")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
