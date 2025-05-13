using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asp.netWebDatVe.Controllers
{
    [Authorize] 
    public class HomeAdminController : Controller
    {
        public IActionResult Index()
        {
            ViewData["UserName"] = User.Identity.Name;
            return View();
        }
    }
}