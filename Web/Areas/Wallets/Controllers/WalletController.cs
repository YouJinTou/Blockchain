using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Models.Web.Wallets;
using Services.Wallets;
using System;
using Web.Areas.Wallets.Models;

namespace Web.Controllers
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
        public IActionResult Create(CreateWalletViewModel model)
        {
            return View(model ?? new CreateWalletViewModel());
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult CreateWallet([FromForm]CreateWalletViewModel model)
        {
            var credentials = new WalletCredentials();

            try
            {
                credentials = this.walletService.CreateWallet(model.Password);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return RedirectToAction("Create", credentials);
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
        public IActionResult Send()
        {
            return View();
        }
    }
}
