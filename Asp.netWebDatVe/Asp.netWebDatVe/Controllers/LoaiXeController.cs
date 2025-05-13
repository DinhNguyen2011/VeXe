using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Asp.netWebDatVe.Controllers
{
    [Authorize(Roles = "1")]
    public class LoaiXeController : Controller
    {
        private readonly QLDatVeContext _context;

        public LoaiXeController(QLDatVeContext context)
        {
            _context = context;
        }

  
        public IActionResult Index()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            var loaiXeList = _context.Loaixes.ToList();
            return View(loaiXeList);
        }

    
        public IActionResult Create()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Loaixe loaiXe)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
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
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
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
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
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
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
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
