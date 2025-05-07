using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Mvc;

namespace Asp.netWebDatVe.Controllers
{
    public class TraCuuPhieuDatVeController : Controller
    {
        private QLDatVeContext db = new QLDatVeContext();

        
        public IActionResult Index()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            return View();
        }

       
        [HttpPost]
        public IActionResult Index(string maVe, string email)
        {

            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (string.IsNullOrWhiteSpace(maVe) || string.IsNullOrWhiteSpace(email))
            {
                ViewBag.Message = "Vui lòng nhập đầy đủ thông tin!";
                return View();
            }


            var phieuDatVe = db.PhieuDatVes
                               .FirstOrDefault(p => p.MaPhieu.ToString() == maVe && p.Email == email);

            if (phieuDatVe == null)
            {
                ViewBag.Message = "Không tìm thấy thông tin đặt vé. Vui lòng kiểm tra lại!";
                return View();
            }

            ViewBag.PhieuDatVe = phieuDatVe;
            return View();
        }
    }
}
