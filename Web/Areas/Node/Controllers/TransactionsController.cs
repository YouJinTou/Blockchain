﻿using Microsoft.AspNetCore.Mvc;
using Services.Nodes;
using System;

namespace Web.Areas.Node.Controllers
{
    [Route("[area]/[controller]")]
    public class TransactionsController : BaseController
    {
        public TransactionsController(INodeService nodeService)
            : base(nodeService)
        {
        }

        [HttpGet]
        [Route("{hash}")]
        public IActionResult Get(string hash)
        {
            try
            {
                if (string.IsNullOrEmpty(hash))
                {
                    throw new ArgumentException("Transaction is required.");
                }

                return new JsonResult(this.NodeService.GetTransaction(hash));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
