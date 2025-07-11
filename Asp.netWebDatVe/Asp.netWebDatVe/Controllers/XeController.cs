using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Asp.netWebDatVe.Controllers
{
    [Authorize(Roles = "1")]
    public class XeController : Controller
    {
        private readonly QLDatVeContext db = new QLDatVeContext();

        public IActionResult Index()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            ViewBag.xe = db.Xes.Include(x => x.IdLoaiNavigation).ToList();
            return View();
        }

        public IActionResult Create()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            ViewBag.LoaiXe = db.Loaixes.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([FromForm] XeModel xe)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;

            if (!ModelState.IsValid)
            {
                ViewBag.LoaiXe = db.Loaixes.ToList();
                return View(xe);
            }

            var existingXe = db.Xes.Find(xe.Bienso);
            if (existingXe != null)
            {
                ModelState.AddModelError("Bienso", "Biển số xe này đã tồn tại!");
                ViewBag.LoaiXe = db.Loaixes.ToList();
                return View(xe);
            }

            Xe newXe = new Xe
            {
                Bienso = xe.Bienso,
                IdLoai = xe.IdLoai,
                Tenxe = xe.Tenxe
            };

            if (xe.HinhAnh != null && xe.HinhAnh.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(xe.HinhAnh.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("HinhAnh", "Chỉ chấp nhận các định dạng: .jpg, .jpeg, .png, .gif");
                    ViewBag.LoaiXe = db.Loaixes.ToList();
                    return View(xe);
                }

                var fileName = xe.Bienso + "_" + xe.HinhAnh.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    xe.HinhAnh.CopyTo(stream);
                }

                newXe.HinhAnh = "~/images/" + fileName;
            }
            else
            {
                newXe.HinhAnh = "~/images/default.png";
            }

            db.Xes.Add(newXe);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Edit(string id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            var xe = db.Xes.Find(id);
            if (xe == null)
            {
                return NotFound();
            }

            var xeModel = new XeModel
            {
                Bienso = xe.Bienso,
                IdLoai = xe.IdLoai,
                Tenxe = xe.Tenxe,
                HinhAnhUrl = xe.HinhAnh // Lưu đường dẫn ảnh
            };

            ViewBag.LoaiXe = db.Loaixes.ToList();
            return View(xeModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromForm] XeModel xeModel)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;

            if (!ModelState.IsValid)
            {
                ViewBag.LoaiXe = db.Loaixes.ToList();
                return View(xeModel);
            }

            var xe = db.Xes.Find(xeModel.Bienso);
            if (xe == null)
            {
                return NotFound();
            }

            xe.Tenxe = xeModel.Tenxe;
            xe.IdLoai = xeModel.IdLoai;

            if (xeModel.HinhAnh != null && xeModel.HinhAnh.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(xeModel.HinhAnh.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("HinhAnh", "Chỉ chấp nhận các định dạng: .jpg, .jpeg, .png, .gif");
                    ViewBag.LoaiXe = db.Loaixes.ToList();
                    return View(xeModel);
                }

                // Xóa ảnh cũ nếu có
                if (!string.IsNullOrEmpty(xe.HinhAnh) && xe.HinhAnh != "~/images/default.png")
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", xe.HinhAnh.TrimStart('~'));
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Lưu ảnh mới
                var newFileName = xeModel.Bienso + "_" + xeModel.HinhAnh.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", newFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    xeModel.HinhAnh.CopyTo(stream);
                }

                xe.HinhAnh = "~/images/" + newFileName;
            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(string id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var xe = db.Xes
                .Include(x => x.IdLoaiNavigation)
                .FirstOrDefault(x => x.Bienso == id);

            if (xe == null)
            {
                return NotFound();
            }

            return View(xe);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (id == null || db.Xes == null)
            {
                return NotFound();
            }

            var xe = await db.Xes
                .Include(x => x.ChuyenXes)
                .Include(x => x.IdLoaiNavigation)   
                .FirstOrDefaultAsync(m => m.Bienso == id);

            if (xe == null)
            {
                return NotFound();
            }

            var isInChuyenXes = xe.ChuyenXes.Any(c => c.BienSoXe == id);
            ViewBag.CanDelete = !isInChuyenXes;

            return View(xe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (db.Xes == null)
            {
                return Problem("Entity set 'QLDatVeContext.Xes' is null.");
            }

            var xe = await db.Xes.FindAsync(id);
            if (xe != null)
            {
                if (!string.IsNullOrEmpty(xe.HinhAnh) && xe.HinhAnh != "~/images/default.png")
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", xe.HinhAnh.TrimStart('~'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                db.Xes.Remove(xe);
            }

            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}