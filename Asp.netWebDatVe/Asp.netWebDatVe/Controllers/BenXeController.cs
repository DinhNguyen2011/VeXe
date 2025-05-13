using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.netWebDatVe.Controllers
{
    [Authorize(Roles = "1")] // Chỉ admin được truy cập
    public class BenXeController : Controller
    {
        private readonly QLDatVeContext _context;

        public BenXeController(QLDatVeContext context)
        {
            _context = context;
        }

        // GET: BenXe
        public async Task<IActionResult> Index()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            var benXes = await _context.BenXes.ToListAsync();
            return View(benXes);
        }

        // GET: BenXe/Create
        public IActionResult Create()

        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            return View();
        }

        // POST: BenXe/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BenXe benXe)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (ModelState.IsValid)
            {
                _context.Add(benXe);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Thêm bến xe thành công.";
                return RedirectToAction(nameof(Index));
            }
            return View(benXe);
        }

        // GET: BenXe/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (id == null)
            {
                return NotFound();
            }

            var benXe = await _context.BenXes.FindAsync(id);
            if (benXe == null)
            {
                return NotFound();
            }
            return View(benXe);
        }

        // POST: BenXe/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BenXe benXe)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (id != benXe.MaBenXe)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(benXe);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Cập nhật bến xe thành công.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.BenXes.Any(e => e.MaBenXe == id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(benXe);
        }

        // GET: BenXe/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (id == null)
            {
                return NotFound();
            }

            var benXe = await _context.BenXes
                .FirstOrDefaultAsync(m => m.MaBenXe == id);

            if (benXe == null)
            {
                return NotFound();
            }

            return View(benXe);
        }

        // POST: BenXe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            var benXe = await _context.BenXes.FindAsync(id);
            if (benXe == null)
            {
                return NotFound();
            }

            // Kiểm tra xem bến xe có tuyến xe liên quan không
            if (_context.TuyenXes.Any(t => t.MaBenXeDi == id || t.MaBenXeDen == id))
            {
                TempData["Error"] = "Không thể xóa bến xe vì có tuyến xe liên quan.";
                return RedirectToAction(nameof(Index));
            }

            _context.BenXes.Remove(benXe);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Xóa bến xe thành công.";
            return RedirectToAction(nameof(Index));
        }
    }
}