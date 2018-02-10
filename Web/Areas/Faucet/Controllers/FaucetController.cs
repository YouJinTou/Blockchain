using Microsoft.AspNetCore.Mvc;

namespace Web.Areas.Faucet.Controllers
{
    [Area("Faucet")]
    [Route("[area]")]
    [Route("[area]/[controller]")]
    public class FaucetController : Controller
    {
        [Route("")]
        [Route("[action]")]
        [Route("[area]/[action]")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
