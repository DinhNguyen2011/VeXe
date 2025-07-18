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

            return View(chuyenXes);

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
                // Kiểm tra thời gian đến dự kiến phải lớn hơn thời gian hiện tại
                if (chuyenXe.ThoiDiemDenDuKien.HasValue && chuyenXe.ThoiDiemDenDuKien <= DateTime.Now)
                {
                    ModelState.AddModelError("ThoiDiemDenDuKien", "Thời gian đến dự kiến phải lớn hơn thời gian hiện tại.");
                }
                // Kiểm tra thời gian đến phải lớn hơn thời gian khởi hành
                else if (chuyenXe.ThoiDiemKhoiHanh.HasValue && chuyenXe.ThoiDiemDenDuKien.HasValue && chuyenXe.ThoiDiemDenDuKien <= chuyenXe.ThoiDiemKhoiHanh)
                {
                    ModelState.AddModelError("ThoiDiemDenDuKien", "Thời gian đến dự kiến phải lớn hơn thời gian khởi hành.");
                }
                else
                {
                    // Kiểm tra xung đột thời gian với các chuyến xe hiện có của cùng xe
                    var conflictingTripsByXe = _context.ChuyenXes
                        .Where(cx => cx.BienSoXe == chuyenXe.BienSoXe
                                  && cx.ThoiDiemKhoiHanh.HasValue
                                  && cx.ThoiDiemDenDuKien.HasValue
                                  && cx.ThoiDiemKhoiHanh <= chuyenXe.ThoiDiemDenDuKien
                                  && chuyenXe.ThoiDiemKhoiHanh <= cx.ThoiDiemDenDuKien)
                        .ToList();

                    if (conflictingTripsByXe.Any())
                    {
                        ModelState.AddModelError("BienSoXe", "Xe này đã được sử dụng trong một chuyến xe khác trong khoảng thời gian bạn chọn. Vui lòng chọn xe hoặc thời gian khác.");
                    }

                    // Kiểm tra xung đột thời gian với các nhân viên
                    var conflictingTripsByNhanVien = _context.ChuyenXes
                        .Where(cx => (cx.MaNhanVien == chuyenXe.MaNhanVien || cx.MaTaiXe == chuyenXe.MaTaiXe || cx.MaNhanVien1 == chuyenXe.MaNhanVien1)
                                  && cx.ThoiDiemKhoiHanh.HasValue
                                  && cx.ThoiDiemDenDuKien.HasValue
                                  && cx.ThoiDiemKhoiHanh <= chuyenXe.ThoiDiemDenDuKien
                                  && chuyenXe.ThoiDiemKhoiHanh <= cx.ThoiDiemDenDuKien)
                        .ToList();

                    if (conflictingTripsByNhanVien.Any())
                    {
                        ModelState.AddModelError("", "Một hoặc nhiều nhân viên đã được gán cho chuyến xe khác trong khoảng thời gian này. Vui lòng chọn nhân viên hoặc thời gian khác.");
                    }

                    if (!conflictingTripsByXe.Any() && !conflictingTripsByNhanVien.Any())
                    {
                        // Nếu không nhập giá vé, lấy giá hiện hành từ tuyến xe
                        if (chuyenXe.GiaVe == null)
                        {
                            var tuyenXe = _context.TuyenXes.Find(chuyenXe.MaTuyen);
                            if (tuyenXe == null)
                            {
                                ModelState.AddModelError("MaTuyen", "Tuyến xe không tồn tại.");
                            }
                            else
                            {
                                chuyenXe.GiaVe = tuyenXe.GiaHienHanh;
                            }
                        }

                        if (ModelState.IsValid)
                        {
                            _context.ChuyenXes.Add(chuyenXe);
                            _context.SaveChanges();
                            TempData["Success"] = "Thêm chuyến xe thành công.";
                            return RedirectToAction("Index");
                        }
                    }
                }
            }

            // Nếu có lỗi, nạp lại dữ liệu cho các dropdown
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

        // GET: ChuyenXe/Details/5 -- chi tiết
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

    }
}