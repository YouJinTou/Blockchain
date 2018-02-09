using Microsoft.AspNetCore.Mvc;
using WebNode.Code;

namespace WebNode.Controllers
{
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
