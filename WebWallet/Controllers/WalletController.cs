using Microsoft.AspNetCore.Mvc;
using Services.Wallets;
using System;
using WebWallet.Models;

namespace WebWallet.Controllers
{
    public class WalletController : Controller
    {
        private IWalletService walletService;

        public WalletController(IWalletService walletService)
        {
            this.walletService = walletService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
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
    }
}
