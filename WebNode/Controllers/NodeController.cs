using Microsoft.AspNetCore.Mvc;
using Models.Web.Nodes;
using Services.Nodes;
using System;

namespace WebNode.Controllers
{
    [Route("Node")]
    public class NodeController : BaseController
    {
        public NodeController(INodeService nodeService)
            : base(nodeService)
        {
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            return new JsonResult(this.NodeService.GetNode());
        }

        [HttpPost("Peers/Add")]
        public IActionResult AddPeer([FromBody]AddPeerModel model)
        {
            try
            {
                this.NodeService.AddPeer(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok("Peer added.");
        }
    }
}
