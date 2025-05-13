using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.netWebDatVe.Controllers
{
    public class LichTrinhController : Controller
    {
        private readonly QLDatVeContext db;

        public LichTrinhController(QLDatVeContext context)
        {
            db = context; // Sử dụng dependency injection thay vì khởi tạo mới
        }

        public IActionResult Index(string searchTerm = "", DateTime? ngayDi = null)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;

            var tuyenXesQuery = db.TuyenXes
                .Include(t => t.MaBenXeDiNavigation)
                .Include(t => t.MaBenXeDenNavigation)
                .AsQueryable();

            // Lọc theo searchTerm (tìm trong DiemDi hoặc DiemDen)
            if (!string.IsNullOrEmpty(searchTerm))
            {
                tuyenXesQuery = tuyenXesQuery.Where(t => t.DiemDi.Contains(searchTerm) || t.DiemDen.Contains(searchTerm));
            }

         

            var tuyenXes = tuyenXesQuery.ToList();

            ViewBag.SearchTerm = searchTerm;
            ViewBag.NgayDi = ngayDi;

            return View(tuyenXes);
        }

        public IActionResult Test()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;

            var tuyenXes = db.TuyenXes
                .Include(t => t.MaBenXeDiNavigation)
                .Include(t => t.MaBenXeDenNavigation)
                .ToList();

            return View("Index", tuyenXes); // Sử dụng cùng view Index
        }
    }
}