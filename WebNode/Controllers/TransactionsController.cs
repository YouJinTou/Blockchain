﻿using Microsoft.AspNetCore.Mvc;
using Models.Web.Users;
using Services.Nodes;
using System;

namespace Web.Controllers
{
    public class TransactionsController : BaseController
    {
        public TransactionsController(INodeService nodeService)
            : base(nodeService)
        {
        }

        [HttpGet("{hash}")]
        public IActionResult Get(string hash)
        {
            try
            {
                return new JsonResult(this.NodeService.GetTransaction(hash));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
