using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Asp.netWebDatVe.Controllers
{
    [Authorize(Roles = "1")]
    public class PhanQuyenController : Controller
    {
        private readonly QLDatVeContext _context;

        public PhanQuyenController(QLDatVeContext context)
        {
            _context = context;
        }

        // Hiển thị danh sách phân quyền
        public IActionResult Index()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            var phanQuyenList = _context.PhanQuyens.ToList();
            return View(phanQuyenList);
        }

        // Trang thêm mới phân quyền
        public IActionResult Create()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PhanQuyen phanQuyen)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (ModelState.IsValid)
            {
                _context.PhanQuyens.Add(phanQuyen);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(phanQuyen);
        }

        // Trang chỉnh sửa phân quyền
        public IActionResult Edit(int id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            var phanQuyen = _context.PhanQuyens.Find(id);
            if (phanQuyen == null)
            {
                return NotFound();
            }
            return View(phanQuyen);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, PhanQuyen phanQuyen)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (id != phanQuyen.MaQuyen)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Update(phanQuyen);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(phanQuyen);
        }

        // GET: Trang xác nhận xóa phân quyền
        public IActionResult Delete(int id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;

            var phanQuyen = _context.PhanQuyens.Find(id);
            if (phanQuyen == null)
            {
                return NotFound();
            }

            // Kiểm tra xem quyền này có đang được sử dụng bởi người dùng nào không
            bool quyenDangDuocSuDung = _context.NguoiDungs.Any(nd => nd.MaQuyen == id);
            if (quyenDangDuocSuDung)
            {
                TempData["Error"] = "Không thể xóa phân quyền này vì đang được sử dụng bởi người dùng.";
                return RedirectToAction(nameof(Index));
            }

            return View(phanQuyen);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;

            var phanQuyen = _context.PhanQuyens.Find(id);
            if (phanQuyen != null)
            {
                _context.PhanQuyens.Remove(phanQuyen);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
