using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Asp.netWebDatVe.Controllers
{
    [Authorize(Roles = "1")]
    public class NhanVienController : Controller
    {
        private readonly QLDatVeContext _context;

        public NhanVienController(QLDatVeContext context)
        {
            _context = context;
        }

        // GET: NhanVien
        public async Task<IActionResult> Index(string vaiTro)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            var query = _context.NhanViens.AsQueryable();

            if (!string.IsNullOrEmpty(vaiTro))
            {
                query = query.Where(n => n.VaiTro == vaiTro);
            }

            var nhanViens = await query.ToListAsync();

            // Tạo SelectList cho dropdown VaiTro
            ViewBag.VaiTroList = new SelectList(
                new[] { "Tài xế", "Lơ xe", "Tài xế phụ" }.Select(s => new { Value = s, Text = s }),
                "Value",
                "Text",
                vaiTro
            );

            return View(nhanViens);
        }

        // GET: NhanVien/Create
        public IActionResult Create()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            ViewBag.VaiTro = new SelectList(new[] { "Tài xế", "Lơ xe", "Tài xế phụ" });
            return View();
        }

        // POST: NhanVien/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NhanVien nhanVien)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (ModelState.IsValid)
            {
                _context.Add(nhanVien);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Thêm nhân viên thành công.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.VaiTro = new SelectList(new[] { "Tài xế", "Lơ xe", "Tài xế phụ" }, nhanVien.VaiTro);
            return View(nhanVien);
        }

        // GET: NhanVien/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien == null)
            {
                return NotFound();
            }

            ViewBag.VaiTro = new SelectList(new[] { "Tài xế", "Lơ xe", "Tài xế phụ" }, nhanVien.VaiTro);
            return View(nhanVien);
        }

        // POST: NhanVien/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NhanVien nhanVien)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (id != nhanVien.MaNhanVien)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(nhanVien);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật nhân viên thành công.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.NhanViens.Any(e => e.MaNhanVien == id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.VaiTro = new SelectList(new[] { "Tài xế", "Lơ xe", "Tài xế phụ" }, nhanVien.VaiTro);
            return View(nhanVien);
        }

        // GET: NhanVien/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (id == null)
            {
                return NotFound();
            }

            var nhanVien = await _context.NhanViens
                .FirstOrDefaultAsync(m => m.MaNhanVien == id);

            if (nhanVien == null)
            {
                return NotFound();
            }

            ViewBag.ChuyenXeCount = await _context.ChuyenXes
                .CountAsync(c => c.MaTaiXe == id || c.MaNhanVien == id || c.MaNhanVien1 == id);

            return View(nhanVien);
        }

        // POST: NhanVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien == null)
            {
                TempData["Error"] = "Nhân viên không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            // Kiểm tra ràng buộc với ChuyenXe
            if (_context.ChuyenXes.Any(c => c.MaTaiXe == id || c.MaNhanVien == id || c.MaNhanVien1 == id))
            {
                TempData["Error"] = "Không thể xóa nhân viên vì có chuyến xe liên quan.";
                return RedirectToAction(nameof(Index));
            }

            _context.NhanViens.Remove(nhanVien);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Xóa nhân viên thành công.";
            return RedirectToAction(nameof(Index));
        }
    }
}