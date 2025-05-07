using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Asp.netWebDatVe.Controllers
{
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
            var phanQuyenList = _context.PhanQuyens.ToList();
            return View(phanQuyenList);
        }

        // Trang thêm mới phân quyền
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PhanQuyen phanQuyen)
        {
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

        // Trang xóa phân quyền
        public IActionResult Delete(int id)
        {
            var phanQuyen = _context.PhanQuyens.Find(id);
            if (phanQuyen == null)
            {
                return NotFound();
            }
            return View(phanQuyen);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
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
