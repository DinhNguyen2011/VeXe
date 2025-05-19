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
            db = context;
        }

        public IActionResult Index(string searchTerm = "", DateTime? ngayDi = null)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            ViewData["Title"] = "Lịch Trình";

            var tuyenXesQuery = db.TuyenXes
                .Include(t => t.MaBenXeDiNavigation)
                .Include(t => t.MaBenXeDenNavigation)
                .AsQueryable();

            // Lọc theo searchTerm
            if (!string.IsNullOrEmpty(searchTerm))
            {
                tuyenXesQuery = tuyenXesQuery.Where(t => t.DiemDi.Contains(searchTerm) || t.DiemDen.Contains(searchTerm));
            }

            // Lọc theo ngayDi (tùy chọn)
            if (ngayDi.HasValue)
            {
                tuyenXesQuery = tuyenXesQuery
                    .Where(t => t.ChuyenXes.Any(cx => cx.ThoiDiemKhoiHanh.HasValue && cx.ThoiDiemKhoiHanh.Value.Date == ngayDi.Value.Date));
            }

            var tuyenXes = tuyenXesQuery.ToList();

            ViewBag.SearchTerm = searchTerm;
            ViewBag.NgayDi = ngayDi?.ToString("yyyy-MM-dd") ?? DateTime.Now.ToString("yyyy-MM-dd");

            return View(tuyenXes);
        }

    
    }
}