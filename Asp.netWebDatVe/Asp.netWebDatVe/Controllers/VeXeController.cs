using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Asp.netWebDatVe.Controllers
{
    public class VeXeController : Controller
    {
        private readonly QLDatVeContext _context;

        public VeXeController(QLDatVeContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var veXeList = _context.VeXes
                .Include(v => v.MaChuyenNavigation)
                .Include(v => v.IdVitriNavigation)
                .Include(v => v.MaPhieuNavigation)
                .ToList();
            return View(veXeList);
        }


        public IActionResult Create()
        {

            ViewBag.ChuyenXeList = new SelectList(_context.ChuyenXes, "MaChuyen", "TenChuyenXe");

            ViewBag.BienSoList = new SelectList(_context.Xes, "Bienso", "Bienso");

         
            ViewBag.ViTriList = new SelectList(Enumerable.Empty<object>(), "IdVitri", "Tenvitri");

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VeXe veXe, string selectedBienSo)
        {
            if (ModelState.IsValid)
            {
                _context.VeXes.Add(veXe);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ChuyenXeList = new SelectList(_context.ChuyenXes, "MaChuyen", "TenChuyenXe");
            ViewBag.BienSoList = new SelectList(_context.Xes, "Bienso", "Bienso");

           
            var viTriList = _context.Vitrighes.Where(v => v.Bienso == selectedBienSo && v.Trangthai == false)
                                              .ToList();
            ViewBag.ViTriList = new SelectList(viTriList, "IdVitri", "Tenvitri");

            return View(veXe);
        }
       
        [HttpGet]
        public IActionResult GetViTriGheByBienSo(string bienSo)
        {
            var viTriList = _context.Vitrighes
                                    .Where(v => v.Bienso == bienSo && v.Trangthai == false)
                                    .Select(v => new
                                    {
                                        idVitri = v.IdVitri,
                                        tenvitri = v.Tenvitri
                                    })
                                    .ToList();
            return Json(viTriList);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.VeXes == null)
            {
                return NotFound();
            }

            var veXe = await _context.VeXes
                .Include(v => v.IdVitriNavigation)
                .Include(v => v.MaChuyenNavigation)
                .Include(v => v.MaPhieuNavigation)
                .FirstOrDefaultAsync(m => m.MaVe == id);
            if (veXe == null)
            {
                return NotFound();
            }

            return View(veXe);
        }

        // POST: VeXes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.VeXes == null)
            {
                return Problem("Entity set 'QLDatVeContext.VeXes'  is null.");
            }
            var veXe = await _context.VeXes.FindAsync(id);
            if (veXe != null)
            {
                _context.VeXes.Remove(veXe);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Details(int id)
        {
          
            var veXe = _context.VeXes
                .Include(v => v.MaChuyenNavigation)
                .Include(v => v.IdVitriNavigation) 
                .FirstOrDefault(v => v.MaVe == id);  

            if (veXe == null)
            {
                return NotFound(); 
            }

            return View(veXe); 
        }
        public IActionResult ThongKeDoanhThu(DateTime? fromDate, DateTime? toDate)
        {
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");

            var query = _context.VeXes
                .Where(v => v.NgayDat.HasValue && v.MaChuyenNavigation != null && v.MaChuyenNavigation.GiaVe.HasValue);

            if (fromDate.HasValue)
            {
                query = query.Where(v => v.NgayDat.Value.Date >= fromDate.Value.Date);
            }

            if (toDate.HasValue)
            {
                query = query.Where(v => v.NgayDat.Value.Date <= toDate.Value.Date);
            }

            var thongKe = query
                .GroupBy(v => v.NgayDat.Value.Date)
                .Select(g => new DoanhThuViewModel
                {
                    Ngay = g.Key,
                    DoanhThu = g.Sum(v => v.MaChuyenNavigation.GiaVe ?? 0)
                })
                .OrderBy(x => x.Ngay)
                .ToList();

            return View(thongKe);
        }



    }
}
