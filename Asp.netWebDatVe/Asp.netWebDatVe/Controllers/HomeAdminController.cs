using Microsoft.AspNetCore.Mvc;

namespace Asp.netWebDatVe.Controllers
{
    public class HomeAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
