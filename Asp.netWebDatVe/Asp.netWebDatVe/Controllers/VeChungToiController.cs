using Microsoft.AspNetCore.Mvc;

namespace Asp.netWebDatVe.Controllers
{
    public class VeChungToiController : Controller
    {
        public IActionResult Index()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            return View();
        }
    }
}
