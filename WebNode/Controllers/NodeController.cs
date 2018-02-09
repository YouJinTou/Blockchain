using Microsoft.AspNetCore.Mvc;
using WebNode.Code;

namespace WebNode.Controllers
{
    public class NodeController : BaseController
    {
        public NodeController(INodeService nodeService)
            : base(nodeService)
        {
        }

        [HttpGet("Node")]
        public IActionResult Get()
        {
            return new JsonResult(this.NodeService.GetNode());
        }
    }
}
