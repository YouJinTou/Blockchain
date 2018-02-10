using Microsoft.AspNetCore.Mvc;
using Services.Nodes;

namespace Web.Controllers
{
    [Area("Node")]
    public class BaseController : ControllerBase
    {
        private INodeService nodeService;

        public BaseController(INodeService nodeService)
        {
            this.nodeService = nodeService;
        }

        protected INodeService NodeService => this.nodeService;
    }
}
