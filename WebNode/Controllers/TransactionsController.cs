using Microsoft.AspNetCore.Mvc;
using System;
using WebNode.ApiModels.Users;
using WebNode.Code;

namespace WebNode.Controllers
{
    public class TransactionsController : BaseController
    {
        public TransactionsController(INodeService nodeService)
            : base(nodeService)
        {
        }

        [HttpPost]
        public IActionResult New([FromBody]TransactionModel model)
        {
            try
            {
                this.NodeService.ReceiveTransaction(model);
            }
            catch (ArgumentException aex)
            {
                return BadRequest(aex.Message);
            }

            return Ok();
        }
    }
}
