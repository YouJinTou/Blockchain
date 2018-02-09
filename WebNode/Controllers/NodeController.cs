using Microsoft.AspNetCore.Mvc;
using System;
using WebNode.ApiModels.Nodes;
using WebNode.Code;

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
