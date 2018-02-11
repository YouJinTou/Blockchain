using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Nodes;
using System.Collections.Generic;
using Web.Areas.Explorer.Models;

namespace Web.Areas.Explorer.Controllers
{
    [Area("Explorer")]
    [Route("[area]")]
    [Route("[area]/[controller]")]
    public class ExplorerController : Controller
    {
        private INodeService nodeService;

        public ExplorerController(INodeService nodeService)
        {
            this.nodeService = nodeService;
        }

        [HttpGet]
        [Route("")]
        [Route("[action]")]
        [Route("[area]/[action]")]
        public IActionResult Index()
        {
            var blocks = Mapper.Map<ICollection<Block>, ICollection<BlockViewModel>>(
                this.nodeService.GetChain());

            return View(blocks);
        }
    }
}
