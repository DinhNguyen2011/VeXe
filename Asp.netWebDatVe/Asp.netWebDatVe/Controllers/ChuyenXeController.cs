using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Asp.netWebDatVe.Controllers
{
    [Authorize(Roles = "1")]
    public class ChuyenXeController : Controller
    {
        private readonly QLDatVeContext _context;

        public ChuyenXeController(QLDatVeContext context)
        {
            _context = context;
        }

        // GET: ChuyenXe
        public async Task<IActionResult> Index(bool? isCompleted)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            var query = _context.ChuyenXes
                .Include(c => c.BienSoXeNavigation)
                .Include(c => c.MaTuyenNavigation)
                .Include(t => t.MaTaiXeNavigation)
                .Include(x =>x.MaNhanVien1Navigation)
                .Include(k => k.MaNhanVienNavigation)
                .AsQueryable();

            if (isCompleted == true)
            {
                query = query.Where(c => c.ThoiDiemDenDuKien < DateTime.Now);
            }

            var chuyenXes = await query.ToListAsync();
            ViewBag.ct = chuyenXes;
            ViewBag.IsCompleted = isCompleted;

            return View();
        }

        // GET: ChuyenXe/Create
        public IActionResult Create()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
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

            ViewBag.TaiXes = _context.NhanViens
                .Where(n => n.VaiTro == "Tài xế")
                .Select(n => new SelectListItem
                {
                    Value = n.MaNhanVien.ToString(),
                    Text = n.HoTen
                }).ToList();

            ViewBag.NhanViens = _context.NhanViens
                .Where(n => n.VaiTro == "Nhân viên hỗ trợ")
                .Select(n => new SelectListItem
                {
                    Value = n.MaNhanVien.ToString(),
                    Text = n.HoTen
                }).ToList();

            ViewBag.NhanVien1s = _context.NhanViens
                .Where(n => n.VaiTro == "Nhân viên phụ xe")
                .Select(n => new SelectListItem
                {
                    Value = n.MaNhanVien.ToString(),
                    Text = n.HoTen
                }).ToList();

            return View();
        }

        // POST: ChuyenXe/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ChuyenXe chuyenXe)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
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
                TempData["Success"] = "Thêm chuyến xe thành công.";
                return RedirectToAction("Index");
            }

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

            ViewBag.TaiXes = _context.NhanViens
                .Where(n => n.VaiTro == "Tài xế")
                .Select(n => new SelectListItem
                {
                    Value = n.MaNhanVien.ToString(),
                    Text = n.HoTen
                }).ToList();

            ViewBag.NhanViens = _context.NhanViens
                .Where(n => n.VaiTro == "Nhân viên hỗ trợ")
                .Select(n => new SelectListItem
                {
                    Value = n.MaNhanVien.ToString(),
                    Text = n.HoTen
                }).ToList();

            ViewBag.NhanVien1s = _context.NhanViens
                .Where(n => n.VaiTro == "Nhân viên phụ xe")
                .Select(n => new SelectListItem
                {
                    Value = n.MaNhanVien.ToString(),
                    Text = n.HoTen
                }).ToList();

            return View(chuyenXe);
        }

        // GET: ChuyenXe/Edit/5
        public IActionResult Edit(int id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
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
                Text = x.Bienso,
                Selected = x.Bienso == chuyenXe.BienSoXe
            }).ToList();

            ViewBag.TaiXes = _context.NhanViens
                .Where(n => n.VaiTro == "Tài xế")
                .Select(n => new SelectListItem
                {
                    Value = n.MaNhanVien.ToString(),
                    Text = n.HoTen,
                    Selected = n.MaNhanVien == chuyenXe.MaTaiXe
                }).ToList();

            ViewBag.NhanViens = _context.NhanViens
                .Where(n => n.VaiTro == "Nhân viên hỗ trợ")
                .Select(n => new SelectListItem
                {
                    Value = n.MaNhanVien.ToString(),
                    Text = n.HoTen,
                    Selected = n.MaNhanVien == chuyenXe.MaNhanVien
                }).ToList();

            ViewBag.NhanVien1s = _context.NhanViens
                .Where(n => n.VaiTro == "Nhân viên phụ xe")
                .Select(n => new SelectListItem
                {
                    Value = n.MaNhanVien.ToString(),
                    Text = n.HoTen,
                    Selected = n.MaNhanVien == chuyenXe.MaNhanVien1
                }).ToList();

            return View(chuyenXe);
        }

        // POST: ChuyenXe/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ChuyenXe chuyenXe)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chuyenXe);
                    _context.SaveChanges();
                    TempData["Success"] = "Cập nhật chuyến xe thành công.";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ChuyenXes.Any(c => c.MaChuyen == chuyenXe.MaChuyen))
                    {
                        return NotFound();
                    }
                    throw;
                }
            }

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

            ViewBag.TaiXes = _context.NhanViens
                .Where(n => n.VaiTro == "Tài xế")
                .Select(n => new SelectListItem
                {
                    Value = n.MaNhanVien.ToString(),
                    Text = n.HoTen
                }).ToList();

            ViewBag.NhanViens = _context.NhanViens
                .Where(n => n.VaiTro == "Nhân viên hỗ trợ")
                .Select(n => new SelectListItem
                {
                    Value = n.MaNhanVien.ToString(),
                    Text = n.HoTen
                }).ToList();

            ViewBag.NhanVien1s = _context.NhanViens
                .Where(n => n.VaiTro == "Nhân viên phụ xe")
                .Select(n => new SelectListItem
                {
                    Value = n.MaNhanVien.ToString(),
                    Text = n.HoTen
                }).ToList();

            return View(chuyenXe);
        }

        // GET: ChuyenXe/Delete/5
        public IActionResult Delete(int id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            var chuyenXe = _context.ChuyenXes
                .Include(c => c.BienSoXeNavigation)
                .Include(c => c.MaTuyenNavigation)
                .Include(c => c.MaTaiXeNavigation)
                .Include(c => c.MaNhanVienNavigation)
                .Include(c => c.MaNhanVien1Navigation)
                .FirstOrDefault(cx => cx.MaChuyen == id);

            if (chuyenXe == null)
            {
                TempData["Error"] = "Chuyến xe không tồn tại.";
                return RedirectToAction("Index");
            }

            var veXeExists = _context.VeXes.Any(vx => vx.MaChuyen == id);
            ViewBag.CanDelete = !veXeExists;
            return View(chuyenXe);
        }

        // POST: ChuyenXe/Delete/5
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

        // GET: ChuyenXe/Details/5
        public IActionResult Details(int id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            var chuyenXe = _context.ChuyenXes
                .Include(c => c.BienSoXeNavigation)
                .Include(c => c.MaTuyenNavigation)
                .Include(c => c.MaTaiXeNavigation)
                .Include(c => c.MaNhanVienNavigation)
                .Include(c => c.MaNhanVien1Navigation)
                .FirstOrDefault(c => c.MaChuyen == id);

            if (chuyenXe == null)
            {
                return NotFound();
            }

            return View(chuyenXe);
        }

        // POST: ChuyenXe/ResetSeatsByBienso
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
            TempData["Success"] = $"Đã đặt lại trạng thái tất cả ghế của xe {bienso} thành trống.";
            return RedirectToAction(nameof(Index));
        }
    }
}