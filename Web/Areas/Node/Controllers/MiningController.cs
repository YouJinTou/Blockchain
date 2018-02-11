using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Nodes;
using System;

namespace Web.Areas.Node.Controllers
{
    [Route("[area]/[controller]")]
    public class MiningController : BaseController
    {
        public MiningController(INodeService nodeService)
            : base(nodeService)
        {
        }

        [HttpPost]
        [Route("Receive")]
        public IActionResult ReceiveBlock([FromBody]Block block)
        {
            try
            {
                this.NodeService.ReceiveBlock(block);

                return Ok("Successfully mined block.");
            }
            catch (ArgumentException aex)
            {
                return BadRequest(aex.Message);
            }
        }
    }
}
