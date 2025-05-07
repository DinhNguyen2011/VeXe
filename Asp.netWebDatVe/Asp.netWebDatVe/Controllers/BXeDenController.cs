using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Mvc;

namespace Asp.netWebDatVe.Controllers
{
    public class BXeDenController : Controller
    {
        private readonly QLDatVeContext _context;
        public BXeDenController(QLDatVeContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.bxd = _context.BenXeDens.ToList();
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(BenXeDen bx)
        {
            if (ModelState.IsValid)
            {
                if (_context.BenXeDens.Find(bx.MaBenXeDen) != null)
                {
                    ModelState.AddModelError("", "Mã trùng,không thêm được!!");
                    return View(bx);
                }
                else
                {
                    _context.BenXeDens.Add(bx);
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

            int dem = _context.TuyenXes.Where(k => k.MaBenXeDen == id).Count();
            ViewBag.f = dem;


            var BeXeDen = _context.BenXeDens.Find(id);
            if (BeXeDen == null)
            {
                return NotFound();
            }

            return View(BeXeDen);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
          
            var BeXeDen = _context.BenXeDens.Find(id);
            if (BeXeDen != null)
            {

                _context.BenXeDens.Remove(BeXeDen);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {

            var BeXeDen = _context.BenXeDens.FirstOrDefault(b => b.MaBenXeDen == id);


            if (BeXeDen == null)
            {
                return NotFound();
            }


            return View(BeXeDen);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {

            var BeXeDen = _context.BenXeDens.FirstOrDefault(b => b.MaBenXeDen == id);
            if (BeXeDen == null)
            {
                return NotFound();
            }
            return View(BeXeDen);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BenXeDen model)
        {
            if (ModelState.IsValid)
            {
                var ktra = _context.BenXeDens.Find(model.MaBenXeDen);
                if (ktra != null)
                {
                    ktra.TenBenXeDen = model.TenBenXeDen;
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
