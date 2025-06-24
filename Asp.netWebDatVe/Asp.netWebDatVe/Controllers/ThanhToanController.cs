using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Asp.netWebDatVe.Controllers
{
    [Authorize(Roles = "1")]
    public class ThanhToanController : Controller
    {
        private readonly QLDatVeContext _context;

        public ThanhToanController(QLDatVeContext context)
        {
            _context = context;
        }

        // GET: ThanhToan
        public async Task<IActionResult> Index()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            var danhSach = await _context.ThanhToans
                .Include(t => t.MaPhieuNavigation)
                .ToListAsync();
            return View(danhSach);
        }

        // GET: ThanhToan/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (id == null)
            {
                return NotFound();
            }

            var thanhToan = await _context.ThanhToans
                .Include(t => t.MaPhieuNavigation)
                .FirstOrDefaultAsync(m => m.MaThanhToan == id);

            if (thanhToan == null)
            {
                return NotFound();
            }

            return View(thanhToan);
        }

        // POST: ThanhToan/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var thanhToan = await _context.ThanhToans.FindAsync(id);
            if (thanhToan != null)
            {
                _context.ThanhToans.Remove(thanhToan);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}