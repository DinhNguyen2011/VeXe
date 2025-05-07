using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Asp.netWebDatVe.Controllers
{
    public class NguoiDungController : Controller
    {

        private readonly QLDatVeContext _context;

        public NguoiDungController(QLDatVeContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.nd = _context.NguoiDungs.Include(n => n.MaQuyenNavigation).ToList();
            return View();
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.pq = new SelectList(await _context.PhanQuyens.ToListAsync(), "MaQuyen", "TenQuyen");
            return View();
        }

  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NguoiDung model, IFormFile? HinhAnh)
        {
            if (ModelState.IsValid)
            {
            
                var emailExists = await _context.NguoiDungs.AnyAsync(nd => nd.Email == model.Email);
                if (emailExists)
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại. Vui lòng sử dụng email khác.");
                    ViewBag.pq = new SelectList(await _context.PhanQuyens.ToListAsync(), "MaQuyen", "TenQuyen");
                    return View(model);
                }

                if (HinhAnh != null && HinhAnh.Length > 0)
                {
                    var fileName = Path.GetFileName(HinhAnh.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await HinhAnh.CopyToAsync(stream);
                    }

                    model.HinhAnh = "~/images/" + fileName;
                }

                await _context.NguoiDungs.AddAsync(model);
                await _context.SaveChangesAsync(); 
                return RedirectToAction(nameof(Index));
            }

       
            ViewBag.pq = new SelectList(await _context.PhanQuyens.ToListAsync(), "MaQuyen", "TenQuyen");
            return View(model);
        }

   
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDung = await _context.NguoiDungs.FindAsync(id);
            if (nguoiDung == null)
            {
                return NotFound();
            }

        
            ViewBag.pq = new SelectList(await _context.PhanQuyens.ToListAsync(), "MaQuyen", "TenQuyen", nguoiDung.MaQuyen);
            return View(nguoiDung);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NguoiDung model, IFormFile? HinhAnh)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
              
                    var emailExists = await _context.NguoiDungs
                        .AnyAsync(nd => nd.Email == model.Email && nd.Id != model.Id);
                    if (emailExists)
                    {
                        ModelState.AddModelError("Email", "Email đã tồn tại. Vui lòng sử dụng email khác.");
                        ViewBag.pq = new SelectList(await _context.PhanQuyens.ToListAsync(), "MaQuyen", "TenQuyen", model.MaQuyen);
                        return View(model);
                    }

                    if (HinhAnh != null && HinhAnh.Length > 0)
                    {
                        var fileName = Path.GetFileName(HinhAnh.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await HinhAnh.CopyToAsync(stream);
                        }

                        model.HinhAnh = "~/images/" + fileName;
                    }

                  
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NguoiDungExists(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

        
            ViewBag.pq = new SelectList(await _context.PhanQuyens.ToListAsync(), "MaQuyen", "TenQuyen", model.MaQuyen);
            return View(model);
        }

        private bool NguoiDungExists(int id)
        {
            return _context.NguoiDungs.Any(e => e.Id == id);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDung = _context.NguoiDungs
                .Include(n => n.MaQuyenNavigation)
                .FirstOrDefault(m => m.Id == id);

            if (nguoiDung == null)
            {
                return NotFound();
            }

            return View(nguoiDung);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var nguoiDung = _context.NguoiDungs.Find(id);

            if (nguoiDung == null)
            {
                return NotFound();
            }

            _context.NguoiDungs.Remove(nguoiDung);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nguoiDung = _context.NguoiDungs
                .Include(n => n.MaQuyenNavigation)  
                .FirstOrDefault(m => m.Id == id);  

            if (nguoiDung == null)
            {
                return NotFound();
            }

            return View(nguoiDung); 
        }

    }
}
