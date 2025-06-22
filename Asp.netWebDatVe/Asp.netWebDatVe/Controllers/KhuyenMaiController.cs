using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.netWebDatVe.Controllers
{
    [Authorize(Roles = "1")]
    public class KhuyenMaiController : Controller
    {
        private readonly QLDatVeContext _context;

        public KhuyenMaiController(QLDatVeContext context)
        {
            _context = context;
        }

        // GET: KhuyenMai
        public async Task<IActionResult> Index()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            return View(await _context.KhuyenMais.ToListAsync());
        }

        // GET: KhuyenMai/Create
        public IActionResult Create()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            return View(new KhuyenMai
            {
                NgayBatDau = DateTime.Now,
                NgayKetThuc = DateTime.Now.AddDays(7)
            });
        }
        // POST: KhuyenMai/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenKhuyenMai,MoTa,PhanTramGiam,NgayBatDau,NgayKetThuc")] KhuyenMai khuyenMai)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;

            if (khuyenMai.NgayBatDau >= khuyenMai.NgayKetThuc)
            {
                ModelState.AddModelError("NgayKetThuc", "Ngày kết thúc phải sau ngày bắt đầu.");
            }

            if (khuyenMai.PhanTramGiam <= 0 || khuyenMai.PhanTramGiam > 100)
            {
                ModelState.AddModelError("PhanTramGiam", "Phần trăm giảm phải từ 1 đến 100.");
            }

            if (_context.KhuyenMais.Any(k => k.TenKhuyenMai == khuyenMai.TenKhuyenMai))
            {
                ModelState.AddModelError("TenKhuyenMai", "Tên khuyến mãi đã tồn tại.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(khuyenMai);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Thêm khuyến mãi thành công.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", $"Lỗi khi thêm: {ex.InnerException?.Message ?? ex.Message}");
                }
            }

            return View(khuyenMai);
        }

        // GET: KhuyenMai/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (id == null)
            {
                return NotFound();
            }

            var khuyenMai = await _context.KhuyenMais.FindAsync(id);
            if (khuyenMai == null)
            {
                return NotFound();
            }
            return View(khuyenMai);
        }

        // POST: KhuyenMai/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaKhuyenMai,TenKhuyenMai,MoTa,PhanTramGiam,NgayBatDau,NgayKetThuc")] KhuyenMai khuyenMai)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (id != khuyenMai.MaKhuyenMai)
            {
                return NotFound();
            }

            if (khuyenMai.NgayBatDau >= khuyenMai.NgayKetThuc)
            {
                ModelState.AddModelError("NgayKetThuc", "Ngày kết thúc phải sau ngày bắt đầu.");
            }

            if (khuyenMai.PhanTramGiam <= 0 || khuyenMai.PhanTramGiam > 100)
            {
                ModelState.AddModelError("PhanTramGiam", "Phần trăm giảm phải từ 1 đến 100.");
            }

            if (_context.KhuyenMais.Any(k => k.TenKhuyenMai == khuyenMai.TenKhuyenMai && k.MaKhuyenMai != id))
            {
                ModelState.AddModelError("TenKhuyenMai", "Tên khuyến mãi đã tồn tại.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(khuyenMai);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Cập nhật khuyến mãi thành công.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.KhuyenMais.Any(e => e.MaKhuyenMai == id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", $"Lỗi khi cập nhật: {ex.InnerException?.Message ?? ex.Message}");
                }
            }
            return View(khuyenMai);
        }

        // GET: KhuyenMai/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (id == null)
            {
                return NotFound();
            }

            var khuyenMai = await _context.KhuyenMais.FirstOrDefaultAsync(m => m.MaKhuyenMai == id);
            if (khuyenMai == null)
            {
                return NotFound();
            }

            return View(khuyenMai);
        }

        // POST: KhuyenMai/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            var khuyenMai = await _context.KhuyenMais.FindAsync(id);
            if (khuyenMai == null)
            {
                return NotFound();
            }

            if (_context.PhieuDatVes.Any(p => p.MaKhuyenMai == id))
            {
                TempData["Error"] = "Không thể xóa khuyến mãi vì có phiếu đặt vé liên quan.";
                return RedirectToAction(nameof(Index));
            }

            _context.KhuyenMais.Remove(khuyenMai);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Xóa khuyến mãi thành công.";
            return RedirectToAction(nameof(Index));
        }
    }
}