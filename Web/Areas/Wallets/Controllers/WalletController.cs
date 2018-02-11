using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models.Web.Wallets;
using Services.Wallets;
using System;
using Web.Areas.Wallets.Models;

namespace Web.Areas.Wallets.Controllers
{
    [Area("Wallets")]
    [Route("[area]")]
    [Route("[area]/[controller]")]
    public class WalletController : Controller
    {
        private IWalletService walletService;

        public WalletController(IWalletService walletService)
        {
            this.walletService = walletService;
        }

        [HttpGet]
        [Route("")]
        [Route("[action]")]
        [Route("[area]/[action]")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Create(string message)
        {
            ViewBag.Message = message;

            return View(new CreateWalletViewModel());
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Route("[action]")]
        public IActionResult CreateWallet([FromForm]CreateWalletViewModel bindingModel)
        {
            try
            {
                var model = Mapper.Map<WalletCredentials, CreateWalletViewModel>(
                    this.walletService.CreateWallet(bindingModel.Password));

                return View("Create", model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Create", new { ex.Message });
            }
        }

        [HttpGet]
        [Route("Search")]
        public IActionResult Address()
        {
            var model = new SearchAddressViewModel
            {
                AddressHistory = new AddressHistoryViewModel()
            };

            return View(model);
        }

        [HttpPost]
        [Route("{address}")]
        [ValidateAntiForgeryToken]
        public IActionResult Address(string address)
        {
            var model = Mapper.Map<AddressHistory, SearchAddressViewModel>(
                this.walletService.GetAddressHistory(address));

            return View(model);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Send(string message = null)
        {
            ViewBag.Message = message;

            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult SendTransaction([FromForm]SendTransactionViewModel model)
        {
            try
            {
                this.walletService.SendTransaction(Mapper.Map
                    <SendTransactionViewModel, SendTransactionModel>(model));
            }
            catch (Exception ex)
            {
                return RedirectToAction("Send", new { message = ex.Message });
            }

            return RedirectToAction("Send", new { message = "Successfully sent transaction." });
        }
    }
}
