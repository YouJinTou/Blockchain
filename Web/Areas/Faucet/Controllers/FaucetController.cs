using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models.Web.Faucet;
using Models.Web.Wallets;
using Services.Faucet;
using System;
using Web.Areas.Faucet.Models;

namespace Web.Areas.Faucet.Controllers
{
    [Area("Faucet")]
    [Route("[area]")]
    [Route("[area]/[controller]")]
    public class FaucetController : Controller
    {
        private IFaucetService faucetService;

        public FaucetController(IFaucetService faucetService)
        {
            this.faucetService = faucetService;
        }

        [Route("")]
        [Route("[action]")]
        [Route("[area]/[action]")]
        [HttpGet]
        public IActionResult Index(string message)
        {
            ViewBag.Message = message;

            return View(new FaucetSendViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("[action]")]
        public IActionResult Send([FromForm]FaucetSendViewModel model)
        {
            try
            {
                this.faucetService.SendFunds(
                    Mapper.Map<FaucetSendViewModel, FaucetSendModel>(model));

                return View("Create", model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Create", new { ex.Message });
            }
        }
    }
}
