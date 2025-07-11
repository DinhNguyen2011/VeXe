using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.netWebDatVe.Controllers
{
    [Authorize(Roles = "1")]
    public class KhuyenMaiController : Controller
    {
        private readonly QLDatVeContext _context;

        public KhuyenMaiController(QLDatVeContext context)
        {
            _context = context;
        }

        // GET: KhuyenMai
        public IActionResult Index()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            ViewBag.km = _context.KhuyenMais.ToList();
            return View();
        }

        // GET: KhuyenMai/Create
        public IActionResult Create()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            return View();
        }

        // POST: KhuyenMai/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KhuyenMai model, IFormFile? HinhAnh)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;

            if (ModelState.IsValid)
            {
                // Check for duplicate TenKhuyenMai
                var tenKhuyenMaiExists = await _context.KhuyenMais.AnyAsync(km => km.TenKhuyenMai == model.TenKhuyenMai);
                if (tenKhuyenMaiExists)
                {
                    ModelState.AddModelError("TenKhuyenMai", "Tên khuyến mãi đã tồn tại. Vui lòng sử dụng tên khác.");
                    return View(model);
                }

                // Handle image upload
                if (HinhAnh != null && HinhAnh.Length > 0)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(HinhAnh.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("HinhAnh", "Chỉ chấp nhận các định dạng ảnh: .jpg, .jpeg, .png, .gif.");
                        return View(model);
                    }

                    var fileName = Guid.NewGuid().ToString() + extension; // Unique filename to avoid conflicts
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await HinhAnh.CopyToAsync(stream);
                    }

                    model.HinhAnh = "~/images/" + fileName;
                }

                await _context.KhuyenMais.AddAsync(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: KhuyenMai/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (id == null)
            {
                return NotFound();
            }

            var khuyenMai = await _context.KhuyenMais.FindAsync(id);
            if (khuyenMai == null)
            {
                return NotFound();
            }

            return View(khuyenMai);
        }

        // POST: KhuyenMai/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, KhuyenMai model, IFormFile? HinhAnh)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (id != model.MaKhuyenMai)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Check for duplicate TenKhuyenMai
                    var tenKhuyenMaiExists = await _context.KhuyenMais
                        .AnyAsync(km => km.TenKhuyenMai == model.TenKhuyenMai && km.MaKhuyenMai != model.MaKhuyenMai);
                    if (tenKhuyenMaiExists)
                    {
                        ModelState.AddModelError("TenKhuyenMai", "Tên khuyến mãi đã tồn tại. Vui lòng sử dụng tên khác.");
                        return View(model);
                    }

                    // Handle image upload
                    if (HinhAnh != null && HinhAnh.Length > 0)
                    {
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        var extension = Path.GetExtension(HinhAnh.FileName).ToLowerInvariant();
                        if (!allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("HinhAnh", "Chỉ chấp nhận các định dạng ảnh: .jpg, .jpeg, .png, .gif.");
                            return View(model);
                        }

                        // Delete old image if exists
                        if (!string.IsNullOrEmpty(model.HinhAnh))
                        {
                            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", model.HinhAnh.TrimStart('~', '/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        var fileName = Guid.NewGuid().ToString() + extension;
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
                    if (!KhuyenMaiExists(model.MaKhuyenMai))
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

            return View(model);
        }

        // GET: KhuyenMai/Delete/5
        public IActionResult Delete(int? id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (id == null)
            {
                return NotFound();
            }

            var khuyenMai = _context.KhuyenMais.FirstOrDefault(m => m.MaKhuyenMai == id);
            if (khuyenMai == null)
            {
                return NotFound();
            }

            return View(khuyenMai);
        }

        // POST: KhuyenMai/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            var khuyenMai = _context.KhuyenMais.Find(id);

            if (khuyenMai == null)
            {
                return NotFound();
            }

            // Delete image file if exists
            if (!string.IsNullOrEmpty(khuyenMai.HinhAnh))
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", khuyenMai.HinhAnh.TrimStart('~', '/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.KhuyenMais.Remove(khuyenMai);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: KhuyenMai/Details/5
        public IActionResult Details(int? id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (id == null)
            {
                return NotFound();
            }

            var khuyenMai = _context.KhuyenMais.FirstOrDefault(m => m.MaKhuyenMai == id);
            if (khuyenMai == null)
            {
                return NotFound();
            }

            return View(khuyenMai);
        }

        private bool KhuyenMaiExists(int id)
        {
            return _context.KhuyenMais.Any(e => e.MaKhuyenMai == id);
        }
    }
}