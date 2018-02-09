using Microsoft.AspNetCore.Mvc;

namespace WebWallet.Controllers
{
    public class WalletController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
