using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Asp.netWebDatVe.Controllers
{
    [Authorize(Roles = "1")]
    public class PhieuDatVeController : Controller
    {
        private readonly QLDatVeContext _context;

        public PhieuDatVeController(QLDatVeContext context)
        {
            _context = context;
        }

        // GET: PhieuDatVe
        public async Task<IActionResult> Index(string trangThai)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            var query = _context.PhieuDatVes
                .Include(p => p.MaKhuyenMaiNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(trangThai))
            {
                query = query.Where(p => p.TrangThai == trangThai);
            }

            var phieuDatVes = await query.ToListAsync();

            // Tạo SelectList cho dropdown TrangThai
            ViewBag.TrangThaiList = new SelectList(
                new[] { "Đã thanh toán", "Chưa thanh toán"}.Select(s => new { Value = s, Text = s }),
                "Value",
                "Text",
                trangThai
            );

            return View(phieuDatVes);
        }

        // GET: PhieuDatVe/Create
        public IActionResult Create()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            ViewBag.MaKhuyenMai = new SelectList(_context.KhuyenMais, "MaKhuyenMai", "TenKhuyenMai");
            ViewBag.TrangThai = new SelectList(new[] { "Đã thanh toán", "Chưa thanh toán"});
            return View();
        }

        // POST: PhieuDatVe/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PhieuDatVe phieuDatVe)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (ModelState.IsValid)
            {
                phieuDatVe.NgayDat = phieuDatVe.NgayDat ?? DateTime.Now;
                _context.Add(phieuDatVe);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Thêm phiếu đặt vé thành công.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.MaKhuyenMai = new SelectList(_context.KhuyenMais, "MaKhuyenMai", "TenKhuyenMai", phieuDatVe.MaKhuyenMai);
            ViewBag.TrangThai = new SelectList(new[] { "Đã thanh toán", "Chưa thanh toán"}, phieuDatVe.TrangThai);
            return View(phieuDatVe);
        }

        // GET: PhieuDatVe/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (id == null)
            {
                return NotFound();
            }

            var phieuDatVe = await _context.PhieuDatVes.FindAsync(id);
            if (phieuDatVe == null)
            {
                return NotFound();
            }

            ViewBag.MaKhuyenMai = new SelectList(_context.KhuyenMais, "MaKhuyenMai", "TenKhuyenMai", phieuDatVe.MaKhuyenMai);
            ViewBag.TrangThai = new SelectList(new[] { "Đã thanh toán", "Chưa thanh toán" }, phieuDatVe.TrangThai);
            return View(phieuDatVe);
        }

        // POST: PhieuDatVe/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PhieuDatVe phieuDatVe)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (id != phieuDatVe.MaPhieu)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(phieuDatVe);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Cập nhật phiếu đặt vé thành công.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.PhieuDatVes.Any(e => e.MaPhieu == id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.MaKhuyenMai = new SelectList(_context.KhuyenMais, "MaKhuyenMai", "TenKhuyenMai", phieuDatVe.MaKhuyenMai);
            ViewBag.TrangThai = new SelectList(new[] { "Đã thanh toán", "Chưa thanh toán" }, phieuDatVe.TrangThai);
            return View(phieuDatVe);
        }

        // GET: PhieuDatVe/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (id == null)
            {
                return NotFound();
            }

            var phieuDatVe = await _context.PhieuDatVes
                .Include(p => p.MaKhuyenMaiNavigation)
                .FirstOrDefaultAsync(m => m.MaPhieu == id);

            if (phieuDatVe == null)
            {
                return NotFound();
            }

            ViewBag.VeXeCount = await _context.VeXes.CountAsync(v => v.MaPhieu == id);
            ViewBag.ThanhToanExists = await _context.ThanhToans.AnyAsync(t => t.MaPhieu == id);
            return View(phieuDatVe);
        }

        // POST: PhieuDatVe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            var phieuDatVe = await _context.PhieuDatVes
                .Include(p => p.VeXes)
                .FirstOrDefaultAsync(p => p.MaPhieu == id);

            if (phieuDatVe == null)
            {
                TempData["Error"] = "Phiếu đặt vé không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            // Kiểm tra ThanhToan
            if (_context.ThanhToans.Any(t => t.MaPhieu == id && t.TrangThai == "Đã thanh toán"))
            {
                TempData["Error"] = "Không thể xóa phiếu đã thanh toán.";
                return RedirectToAction(nameof(Index));
            }

            // Đặt lại trạng thái ghế cho các VeXe liên quan
            var veXes = phieuDatVe.VeXes;
            foreach (var veXe in veXes)
            {
                var vitriGhe = await _context.Vitrighes.FindAsync(veXe.IdVitri);
                if (vitriGhe != null)
                {
                    vitriGhe.Trangthai = false;
                }
            }

            _context.PhieuDatVes.Remove(phieuDatVe);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Xóa phiếu đặt vé thành công.";
            return RedirectToAction(nameof(Index));
        }

        // POST: PhieuDatVe/ResetSeatsByPhieuDatVe
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetSeatsByPhieuDatVe(int maPhieu)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            var phieuDatVe = await _context.PhieuDatVes
                .Include(p => p.VeXes)
                .FirstOrDefaultAsync(p => p.MaPhieu == maPhieu);

            if (phieuDatVe == null)
            {
                TempData["Error"] = "Phiếu đặt vé không tồn tại.";
                return RedirectToAction(nameof(Index));
            }

            if (phieuDatVe.TrangThai != "Hủy")
            {
                TempData["Error"] = "Chỉ có thể đặt lại ghế cho phiếu đã hủy.";
                return RedirectToAction(nameof(Index));
            }

            var veXes = phieuDatVe.VeXes;
            if (!veXes.Any())
            {
                TempData["Error"] = "Phiếu này không có vé xe liên quan.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var veXe in veXes)
            {
                var vitriGhe = await _context.Vitrighes.FindAsync(veXe.IdVitri);
                if (vitriGhe != null)
                {
                    vitriGhe.Trangthai = false;
                }
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = $"Đã đặt lại trạng thái ghế cho phiếu {maPhieu}.";
            return RedirectToAction(nameof(Index));
        }
    }
}