using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using WebNode.Code;

namespace WebNode.Controllers
{
    public class MiningController : BaseController
    {
        public MiningController(INodeService nodeService)
            : base(nodeService)
        {
        }

        [HttpPost("Receive")]
        public IActionResult ReceiveBlock([FromBody]Block block)
        {
            try
            {
                this.NodeService.ReceiveBlock(block);
            }
            catch (ArgumentException aex)
            {
                return BadRequest(aex.Message);
            }

            return Ok();
        }
    }
}
