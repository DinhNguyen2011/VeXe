using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Asp.netWebDatVe.Controllers
{
    [Authorize(Roles ="1")]
    public class TuyenXeController : Controller
    {
        private readonly QLDatVeContext _context;

        public TuyenXeController(QLDatVeContext context)
        {
            _context = context;
        }

        // GET: TuyenXe
        public async Task<IActionResult> Index()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            var tuyenXes = await _context.TuyenXes
                .Include(t => t.MaBenXeDiNavigation)
                .Include(t => t.MaBenXeDenNavigation)
                .ToListAsync();
            return View(tuyenXes);
        }

        // GET: TuyenXe/Create
        public IActionResult Create()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            ViewBag.MaBenXeDi = new SelectList(_context.BenXes, "MaBenXe", "TenBenXe");
            ViewBag.MaBenXeDen = new SelectList(_context.BenXes, "MaBenXe", "TenBenXe");
            return View();
        }

        // POST: TuyenXe/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TuyenXe tuyenXe)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (tuyenXe.DiemDi == tuyenXe.DiemDen)
            {
                ModelState.AddModelError("", "Điểm đi và điểm đến không được trùng nhau.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(tuyenXe);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Thêm tuyến xe thành công.";
                return RedirectToAction(nameof(Index));
            }

            // Nếu có lỗi: giữ lại dropdown
            ViewBag.MaBenXeDi = new SelectList(_context.BenXes, "MaBenXe", "TenBenXe", tuyenXe.MaBenXeDi);
            ViewBag.MaBenXeDen = new SelectList(_context.BenXes, "MaBenXe", "TenBenXe", tuyenXe.MaBenXeDen);
            return View(tuyenXe);
        }


        // GET: TuyenXe/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (id == null)
            {
                return NotFound();
            }

            var tuyenXe = await _context.TuyenXes.FindAsync(id);
            if (tuyenXe == null)
            {
                return NotFound();
            }

            ViewBag.MaBenXeDi = new SelectList(_context.BenXes, "MaBenXe", "TenBenXe", tuyenXe.MaBenXeDi);
            ViewBag.MaBenXeDen = new SelectList(_context.BenXes, "MaBenXe", "TenBenXe", tuyenXe.MaBenXeDen);
            return View(tuyenXe);
        }

        // POST: TuyenXe/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TuyenXe tuyenXe)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (id != tuyenXe.MaTuyen)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tuyenXe);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Cập nhật tuyến xe thành công.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.TuyenXes.Any(e => e.MaTuyen == id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.MaBenXeDi = new SelectList(_context.BenXes, "MaBenXe", "TenBenXe", tuyenXe.MaBenXeDi);
            ViewBag.MaBenXeDen = new SelectList(_context.BenXes, "MaBenXe", "TenBenXe", tuyenXe.MaBenXeDen);
            return View(tuyenXe);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (id == null)
            {
                return NotFound();
            }

            var tuyenXe = await _context.TuyenXes
                .Include(t => t.MaBenXeDiNavigation)
                .Include(t => t.MaBenXeDenNavigation)
                .FirstOrDefaultAsync(m => m.MaTuyen == id);

            if (tuyenXe == null)
            {
                return NotFound();
            }

            return View(tuyenXe);
        }

        // POST: TuyenXe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            var tuyenXe = await _context.TuyenXes.FindAsync(id);
            if (tuyenXe == null)
            {
                return NotFound();
            }

            // Kiểm tra xem tuyến xe có chuyến xe liên quan không
            if (_context.ChuyenXes.Any(c => c.MaTuyen == id))
            {
                TempData["Error"] = "Không thể xóa tuyến xe vì có chuyến xe liên quan.";
                return RedirectToAction(nameof(Index));
            }

            _context.TuyenXes.Remove(tuyenXe);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Xóa tuyến xe thành công.";
            return RedirectToAction(nameof(Index));
        }
    }
}
