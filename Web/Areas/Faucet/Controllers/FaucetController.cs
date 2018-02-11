using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models.Web.Faucet;
using Models.Web.Settings;
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
        private IOptions<FaucetSettings> faucetSettings;

        public FaucetController(
            IFaucetService faucetService, IOptions<FaucetSettings> faucetSettings)
        {
            this.faucetService = faucetService;
            this.faucetSettings = faucetSettings;
        }

        [Route("")]
        [Route("[action]")]
        [Route("[area]/[action]")]
        [HttpGet]
        public IActionResult Index(string message)
        {
            var model = new FaucetSendViewModel
            {
                Address = this.faucetSettings.Value.Address,
                Balance = this.faucetService.GetBalance()
            };
            ViewBag.Message = message;

            return View(model);
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

                return RedirectToAction("Index", new { message = "Transaction successful." });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", new { ex.Message });
            }
        }
    }
}
