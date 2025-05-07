using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Asp.netWebDatVe.Controllers
{
    public class ChuyenXeController : Controller
    {
        private readonly QLDatVeContext _context;

        public ChuyenXeController(QLDatVeContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.ct = _context.ChuyenXes.Include(c => c.BienSoXeNavigation).Include(c => c.MaTuyenNavigation);
            return View();
        }
        public IActionResult Create()
        {
            ViewBag.TuyenXes = _context.TuyenXes.Select(t => new SelectListItem
            {
                Value = t.MaTuyen.ToString(),
                Text = t.DiemDi + " - " + t.DiemDen
            }).ToList();

            ViewBag.Xes = _context.Xes.Select(x => new SelectListItem
            {
                Value = x.Bienso,
                Text = x.Bienso
            }).ToList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ChuyenXe chuyenXe)
        {
            if (ModelState.IsValid)
            {
                // Nếu không nhập giá vé, lấy giá hiện hành từ tuyến xe
                if (chuyenXe.GiaVe == null)
                {
                    var tuyenXe = _context.TuyenXes.Find(chuyenXe.MaTuyen);
                    chuyenXe.GiaVe = tuyenXe?.GiaHienHanh;
                }

                _context.ChuyenXes.Add(chuyenXe);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TuyenXes = _context.TuyenXes.ToList();
            ViewBag.Xes = _context.Xes.ToList();
            return View(chuyenXe);
        }

    
public IActionResult Edit(int id)
        {
            var chuyenXe = _context.ChuyenXes
                .FirstOrDefault(c => c.MaChuyen == id);

            if (chuyenXe == null)
            {
                return NotFound();
            }

            ViewBag.TuyenXes = _context.TuyenXes.Select(t => new SelectListItem
            {
                Value = t.MaTuyen.ToString(),
                Text = t.DiemDi + " - " + t.DiemDen,
                Selected = t.MaTuyen == chuyenXe.MaTuyen
            }).ToList();

            ViewBag.Xes = _context.Xes.Select(x => new SelectListItem
            {
                Value = x.Bienso,
                Text = x.Tenxe,
                Selected = x.Bienso == chuyenXe.BienSoXe
            }).ToList();

            return View(chuyenXe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ChuyenXe chuyenXe)
        {
            if (ModelState.IsValid)
            {
                _context.Update(chuyenXe);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TuyenXes = _context.TuyenXes.ToList();
            ViewBag.Xes = _context.Xes.ToList();
            return View(chuyenXe);
        }
        public IActionResult Delete(int id)
        {
           
            var chuyenXe = _context.ChuyenXes.FirstOrDefault(cx => cx.MaChuyen == id);
            var veXeExists = _context.VeXes.Any(vx => vx.MaChuyen == id);

            if (chuyenXe == null)
            {
                TempData["Error"] = "Chuyến xe không tồn tại.";
                return RedirectToAction("Index");
            }

           
            ViewBag.CanDelete = !veXeExists;

            return View(chuyenXe);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var chuyenXe = _context.ChuyenXes.FirstOrDefault(cx => cx.MaChuyen == id);
            if (chuyenXe == null)
            {
                TempData["Error"] = "Chuyến xe không tồn tại.";
                return RedirectToAction("Index");
            }

            if (_context.VeXes.Any(vx => vx.MaChuyen == id))
            {
                TempData["Error"] = "Không thể xóa chuyến xe vì có vé liên quan.";
                return RedirectToAction("Index");
            }

            _context.ChuyenXes.Remove(chuyenXe);
            _context.SaveChanges();

            TempData["Success"] = "Xóa chuyến xe thành công.";
            return RedirectToAction("Index");
        }


        public IActionResult Details(int id)
        {
            var chuyenXe = _context.ChuyenXes
                .Include(c => c.BienSoXeNavigation)
                .Include(c => c.MaTuyenNavigation)
                .FirstOrDefault(c => c.MaChuyen == id);

            if (chuyenXe == null)
            {
                return NotFound();
            }

            return View(chuyenXe);
        }
    }
}