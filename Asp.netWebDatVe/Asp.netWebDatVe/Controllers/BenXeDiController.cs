using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.netWebDatVe.Controllers
{
    //[Authorize]
    public class BenXeDiController : Controller
    {
        private readonly QLDatVeContext _context;
        public BenXeDiController(QLDatVeContext context)
        {
            _context = context;
        }
     
        public IActionResult Index()
        {
            ViewBag.bxd = _context.BenXes.ToList();
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(BenXe bx)
        {
            if (ModelState.IsValid)
            {
                if (_context.BenXes.Find(bx.MaBenXe) != null)
                {
                    ModelState.AddModelError("", "Mã trùng,không thêm được!!");
                    return View(bx);
                }
                else
                {
                    _context.BenXes.Add(bx);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View(bx);
            }
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {

            int dem = _context.TuyenXes.Where(k => k.MaBenXe == id).Count();
            ViewBag.f = dem;


            var benXe = _context.BenXes.Find(id);
            if (benXe == null)
            {
                return NotFound();
            }

            return View(benXe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Tìm đối tượng bến xe
            var benXe = _context.BenXes.Find(id);
            if (benXe != null)
            {

                _context.BenXes.Remove(benXe);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {

            var benXe = _context.BenXes.FirstOrDefault(b => b.MaBenXe == id);


            if (benXe == null)
            {
                return NotFound();
            }


            return View(benXe);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {

            var benXe = _context.BenXes.FirstOrDefault(b => b.MaBenXe == id);
            if (benXe == null)
            {
                return NotFound();
            }
            return View(benXe);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BenXe model)
        {
            if (ModelState.IsValid)
            {
                var ktra = _context.BenXes.Find(model.MaBenXe);
                if (ktra != null)
                {
                    ktra.TenBenXe = model.TenBenXe;
                    ktra.DiaChi = model.DiaChi;
                    ktra.Sdt = model.Sdt;

                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    return BadRequest("Bến xe không tồn tại.");
                }
            }

            return View(model);  
        }
    }
}
