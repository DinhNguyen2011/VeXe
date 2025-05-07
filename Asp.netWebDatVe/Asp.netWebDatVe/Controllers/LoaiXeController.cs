using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Asp.netWebDatVe.Controllers
{
    public class LoaiXeController : Controller
    {
        private readonly QLDatVeContext _context;

        public LoaiXeController(QLDatVeContext context)
        {
            _context = context;
        }

  
        public IActionResult Index()
        {
            var loaiXeList = _context.Loaixes.ToList();
            return View(loaiXeList);
        }

    
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Loaixe loaiXe)
        {
            if (ModelState.IsValid)
            {
                _context.Loaixes.Add(loaiXe);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(loaiXe);
        }

        public IActionResult Edit(int id)
        {
            var loaiXe = _context.Loaixes.Find(id);
            if (loaiXe == null)
            {
                return NotFound();
            }
            return View(loaiXe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Loaixe loaiXe)
        {
            if (id != loaiXe.IdLoai)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Update(loaiXe);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(loaiXe);
        }
        public IActionResult Delete(int id)
        {
            var loaiXe = _context.Loaixes.Find(id);
            if (loaiXe == null)
            {
                return NotFound();
            }
            return View(loaiXe);
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var loaiXe = _context.Loaixes.Find(id);
            if (loaiXe != null)
            {
                _context.Loaixes.Remove(loaiXe);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
