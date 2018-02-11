using Microsoft.AspNetCore.Mvc;
using Models.Web.Nodes;
using Services.Nodes;
using System;

namespace Web.Areas.Node.Controllers
{
    [Route("[area]/[controller]")]
    public class NodeController : BaseController
    {
        public NodeController(INodeService nodeService)
            : base(nodeService)
        {
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Get()
        {
            return Json(this.NodeService.GetNode());
        }

        [HttpPost]
        [Route("Peers/Add")]
        public IActionResult AddPeer([FromBody]AddPeerModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ArgumentException("Invalid data.");
                }

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
