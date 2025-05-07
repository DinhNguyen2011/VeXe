using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.netWebDatVe.Controllers
{
    public class LichTrinhController : Controller
    {
        private QLDatVeContext db = new QLDatVeContext();


        public IActionResult Index()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;

            var tuyenXes = db.TuyenXes.Include(t => t.MaBenXeNavigation).Include(t => t.MaBenXeDenNavigation).ToList();

            return View(tuyenXes);
        }
        public IActionResult test()
        {

            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;

            var tuyenXes = db.TuyenXes.Include(t => t.MaBenXeNavigation).Include(t => t.MaBenXeDenNavigation).ToList();

            return View(tuyenXes);
        }
    }
}