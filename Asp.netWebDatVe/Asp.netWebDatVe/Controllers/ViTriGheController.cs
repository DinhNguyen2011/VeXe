using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.netWebDatVe.Controllers
{
    [Authorize(Roles = "1")]
    public class VitrigheController : Controller
    {
        private readonly QLDatVeContext _context;

        public VitrigheController(QLDatVeContext context)
        {
            _context = context;
        }

        // GET: Vitrighe
        public async Task<IActionResult> Index(string bienso)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            var query = _context.Vitrighes
                .Include(v => v.BiensoNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(bienso))
            {
                query = query.Where(v => v.Bienso == bienso);
            }

            var vitriGhes = await query.ToListAsync();

            ViewBag.BiensoList = new SelectList(
                await _context.Xes.Select(x => new { x.Bienso }).ToListAsync(),
                "Bienso",
                "Bienso",
                bienso
            );

            return View(vitriGhes);
        }

        // GET: Vitrighe/Create
        public IActionResult Create()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            ViewBag.Bienso = new SelectList(_context.Xes, "Bienso", "Bienso");
            return View(new Vitrighe { Trangthai = false }); // Giá trị mặc định
        }

        // POST: Vitrighe/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Bienso,Tenvitri,Trangthai")] Vitrighe vitrighe)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;

            // Kiểm tra Bienso tồn tại
            if (string.IsNullOrEmpty(vitrighe.Bienso) || !_context.Xes.Any(x => x.Bienso == vitrighe.Bienso))
            {
                ModelState.AddModelError("Bienso", "Vui lòng chọn biển số xe hợp lệ.");
            }

            // Kiểm tra Tenvitri trùng lặp
            if (!string.IsNullOrEmpty(vitrighe.Tenvitri) &&
                _context.Vitrighes.Any(v => v.Bienso == vitrighe.Bienso && v.Tenvitri == vitrighe.Tenvitri))
            {
                ModelState.AddModelError("Tenvitri", "Tên vị trí đã tồn tại cho xe này.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(vitrighe);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Thêm vị trí ghế thành công.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", $"Lỗi khi thêm: {ex.InnerException?.Message ?? ex.Message}");
                }
            }

            // Log lỗi ModelState
            foreach (var error in ModelState)
            {
                foreach (var err in error.Value.Errors)
                {
                    Console.WriteLine($"Create Error - {error.Key}: {err.ErrorMessage}");
                }
            }

            ViewBag.Bienso = new SelectList(_context.Xes, "Bienso", "Bienso", vitrighe.Bienso);
            return View(vitrighe);
        }

        // GET: Vitrighe/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (id == null)
            {
                return NotFound();
            }

            var vitrighe = await _context.Vitrighes.FindAsync(id);
            if (vitrighe == null)
            {
                return NotFound();
            }

            ViewBag.Bienso = new SelectList(_context.Xes, "Bienso", "Bienso", vitrighe.Bienso);
            return View(vitrighe);
        }

        // POST: Vitrighe/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdVitri,Bienso,Tenvitri,Trangthai")] Vitrighe vitrighe)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (id != vitrighe.IdVitri)
            {
                return NotFound();
            }

            // Kiểm tra Bienso tồn tại
            if (string.IsNullOrEmpty(vitrighe.Bienso) || !_context.Xes.Any(x => x.Bienso == vitrighe.Bienso))
            {
                ModelState.AddModelError("Bienso", "Vui lòng chọn biển số xe hợp lệ.");
            }

            // Kiểm tra Tenvitri trùng lặp (loại trừ bản ghi hiện tại)
            if (!string.IsNullOrEmpty(vitrighe.Tenvitri) &&
                _context.Vitrighes.Any(v => v.Bienso == vitrighe.Bienso && v.Tenvitri == vitrighe.Tenvitri && v.IdVitri != id))
            {
                ModelState.AddModelError("Tenvitri", "Tên vị trí đã tồn tại cho xe này.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vitrighe);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Cập nhật vị trí ghế thành công.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Vitrighes.Any(e => e.IdVitri == id))
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

            // Log lỗi ModelState
            foreach (var error in ModelState)
            {
                foreach (var err in error.Value.Errors)
                {
                    Console.WriteLine($"Edit Error - {error.Key}: {err.ErrorMessage}");
                }
            }

            ViewBag.Bienso = new SelectList(_context.Xes, "Bienso", "Bienso", vitrighe.Bienso);
            return View(vitrighe);
        }

        // GET: Vitrighe/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (id == null)
            {
                return NotFound();
            }

            var vitrighe = await _context.Vitrighes
                .Include(v => v.BiensoNavigation)
                .FirstOrDefaultAsync(m => m.IdVitri == id);

            if (vitrighe == null)
            {
                return NotFound();
            }

            return View(vitrighe);
        }

        // POST: Vitrighe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            var vitrighe = await _context.Vitrighes.FindAsync(id);
            if (vitrighe == null)
            {
                return NotFound();
            }

            if (_context.VeXes.Any(v => v.IdVitri == id))
            {
                TempData["Error"] = "Không thể xóa vị trí ghế vì có vé xe liên quan.";
                return RedirectToAction(nameof(Index));
            }

            _context.Vitrighes.Remove(vitrighe);
            await _context.SaveChangesAsync();
            TempData["Message"] = "Xóa vị trí ghế thành công.";
            return RedirectToAction(nameof(Index));
        }

        // POST: Vitrighe/ResetSeatsByBienso
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetSeatsByBienso(string bienso)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (string.IsNullOrEmpty(bienso))
            {
                TempData["Error"] = "Biển số xe không hợp lệ.";
                return RedirectToAction(nameof(Index));
            }

            var seats = await _context.Vitrighes
                .Where(v => v.Bienso == bienso)
                .ToListAsync();

            if (!seats.Any())
            {
                TempData["Error"] = "Không tìm thấy vị trí ghế cho xe này.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var seat in seats)
            {
                seat.Trangthai = false;
            }

            await _context.SaveChangesAsync();
            TempData["Message"] = $"Đã đặt lại trạng thái tất cả ghế của xe {bienso} thành trống.";
            return RedirectToAction(nameof(Index));
        }
    }
}