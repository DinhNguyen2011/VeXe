using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace Asp.netWebDatVe.Controllers
{
    public class DatVeChoKHController : Controller
    {
        private readonly QLDatVeContext _db;
        private readonly ILogger<DatVeChoKHController> _logger;

        public DatVeChoKHController(QLDatVeContext db, ILogger<DatVeChoKHController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // GET: DatVeChoKH/Index
        public IActionResult Index(string diemDi = "", string diemDen = "", DateTime? ngayDi = null, bool isSubmitted = false)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            ViewData["Title"] = "Trang Chủ";

            var tgian = DateTime.Now;
            ViewBag.DanhSachDiemDi = _db.TuyenXes.Select(t => t.DiemDi).Distinct().ToList();
            ViewBag.DanhSachDiemDen = _db.TuyenXes.Select(t => t.DiemDen).Distinct().ToList();
            ViewBag.DiemDi = diemDi;
            ViewBag.DiemDen = diemDen;
            ViewBag.NgayDi = ngayDi;

            if (isSubmitted && (string.IsNullOrEmpty(diemDi) || string.IsNullOrEmpty(diemDen) || ngayDi == null))
            {
                ViewBag.Mes = "Vui lòng nhập đầy đủ thông tin chuyến đi";
                ViewBag.ChuyenXes = new List<ChuyenXe>();
                ViewBag.KhuyenMais = _db.KhuyenMais
                    .Where(k => k.NgayBatDau <= DateTime.Now && k.NgayKetThuc >= DateTime.Now)
                    .ToList();
                return View();
            }

            var tuyenXe = _db.TuyenXes
                .Include(t => t.MaBenXeDiNavigation)
                .Include(t => t.MaBenXeDenNavigation)
                .FirstOrDefault(t => t.DiemDi == diemDi && t.DiemDen == diemDen);

            if (tuyenXe == null)
            {
                ViewBag.Message = "Không tìm thấy tuyến xe phù hợp.";
                ViewBag.ChuyenXes = new List<ChuyenXe>();
                ViewBag.KhuyenMais = _db.KhuyenMais
                    .Where(k => k.NgayBatDau <= DateTime.Now && k.NgayKetThuc >= DateTime.Now)
                    .ToList();
                return View();
            }

            var searchDate = ngayDi ?? DateTime.Now;

            var chuyenXes = _db.ChuyenXes
                .Include(cx => cx.BienSoXeNavigation)
                .ThenInclude(x => x.IdLoaiNavigation)
                .Include(cx => cx.MaNhanVienNavigation)
                .Include(cx => cx.MaTaiXeNavigation)
                .Include(cx => cx.MaNhanVien1Navigation)
                .Include(cx => cx.VeXes)
                .Where(cx => cx.MaTuyen == tuyenXe.MaTuyen &&
                             cx.ThoiDiemKhoiHanh.HasValue &&
                             cx.ThoiDiemKhoiHanh.Value.Date == searchDate.Date &&
                             cx.ThoiDiemKhoiHanh >= tgian)
                .ToList();

            if (chuyenXes.Count == 0)
            {
                ViewBag.Message = "Không có chuyến xe vào ngày đã chọn.";
            }

            ViewBag.TuyenXe = tuyenXe;
            ViewBag.ChuyenXes = chuyenXes;
            ViewBag.KhuyenMais = _db.KhuyenMais
                .Where(k => k.NgayBatDau <= DateTime.Now && k.NgayKetThuc >= DateTime.Now)
                .ToList();

            return View();
        }

        // GET: DatVeChoKH/ChonGheHK
        public IActionResult ChonGheHK(int maChuyen)
        {
            _logger.LogInformation($"ChonGheHK called with maChuyen={maChuyen}");
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            ViewData["Title"] = "Chọn ghế";

            var chuyenXe = _db.ChuyenXes
                .Include(cx => cx.BienSoXeNavigation)
                .ThenInclude(x => x.IdLoaiNavigation)
                .Include(cx => cx.MaTuyenNavigation)
                .ThenInclude(tx => tx.MaBenXeDiNavigation)
                .Include(cx => cx.MaTuyenNavigation)
                .ThenInclude(tx => tx.MaBenXeDenNavigation)
                .FirstOrDefault(cx => cx.MaChuyen == maChuyen);

            if (chuyenXe == null || chuyenXe.BienSoXeNavigation == null || chuyenXe.BienSoXeNavigation.IdLoaiNavigation == null || chuyenXe.MaTuyenNavigation == null)
            {
                _logger.LogError($"Chuyến xe không tồn tại hoặc thông tin không đầy đủ: maChuyen={maChuyen}");
                ViewData["Error"] = "Chuyến xe không tồn tại hoặc thông tin không đầy đủ.";
                return View("Error");
            }

            var loaixe = chuyenXe.BienSoXeNavigation.IdLoaiNavigation;
            var soGhe = loaixe.Soghe;

            var danhSachGhe = _db.Vitrighes
                .Where(ghe => ghe.Bienso == chuyenXe.BienSoXe)
                .Select(ghe => new
                {
                    ghe.IdVitri,
                    ghe.Tenvitri,
                    ghe.Trangthai
                })
                .ToList()
                .OrderBy(ghe =>
                {
                    string number = ghe.Tenvitri.Replace("G", "");
                    return int.TryParse(number, out int result) ? result : int.MaxValue;
                })
                .ToList();

            var soGheThucTe = danhSachGhe.Count;
            if (soGhe != soGheThucTe)
            {
                soGhe = soGheThucTe;
                _logger.LogWarning($"Số ghế trong loaixe.Soghe ({loaixe.Soghe}) không khớp với danh sách ghế thực tế ({soGheThucTe}) cho chuyến xe {maChuyen}.");
            }

            var selectedSeats = HttpContext.Session.GetString("SelectedSeats")?.Split(',').ToList() ?? new List<string>();

            // Lấy thông tin khuyến mãi
            var khuyenMai = _db.KhuyenMais
                .FirstOrDefault(k => k.NgayBatDau <= DateTime.Now && k.NgayKetThuc >= DateTime.Now);

            ViewBag.ChuyenXe = chuyenXe;
            ViewBag.DanhSachGhe = danhSachGhe;
            ViewBag.SoGhe = soGhe;
            ViewBag.GheDaDat = danhSachGhe.Where(ghe => ghe.Trangthai == true).Select(ghe => ghe.IdVitri).ToList();
            ViewBag.SelectedSeats = selectedSeats;
            ViewBag.TuyenXe = chuyenXe.MaTuyenNavigation;
            ViewBag.MaChuyen = maChuyen;
            ViewBag.KhuyenMai = khuyenMai; // Thêm thông tin khuyến mãi vào ViewBag

            return View();
        }

        // POST: DatVeChoKH/DatVe
        [HttpPost]
        public IActionResult DatVe(int maChuyen, string selectedSeats, string tenKhachHang, string soDienThoai, string email, string ghiChu, decimal totalPrice, string phuongThuc)
        {
            _logger.LogInformation($"DatVe called: maChuyen={maChuyen}, selectedSeats={selectedSeats}, tenKhachHang={tenKhachHang}, soDienThoai={soDienThoai}, email={email}, phuongThuc={phuongThuc}, totalPrice={totalPrice}");

            if (string.IsNullOrWhiteSpace(tenKhachHang) || string.IsNullOrWhiteSpace(soDienThoai) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phuongThuc))
            {
                TempData["Error"] = "Vui lòng điền đầy đủ thông tin, bao gồm phương thức thanh toán.";
                return RedirectToAction("ChonGheHK", new { maChuyen });
            }

            if (!Regex.IsMatch(soDienThoai, @"^[0][0-9]{9}$"))
            {
                TempData["Error"] = "Số điện thoại không hợp lệ.";
                return RedirectToAction("ChonGheHK", new { maChuyen });
            }

            if (string.IsNullOrWhiteSpace(selectedSeats))
            {
                TempData["Error"] = "Vui lòng chọn ít nhất một ghế.";
                return RedirectToAction("ChonGheHK", new { maChuyen });
            }

            var seatIds = selectedSeats.Split(',').Select(int.Parse).ToList();

            var vitri = _db.Vitrighes
                .Where(g => seatIds.Contains(g.IdVitri) && g.Trangthai != true)
                .ToList();

            _logger.LogInformation($"Available seats: {vitri.Count}, Requested seats: {seatIds.Count}");
            if (vitri.Count != seatIds.Count)
            {
                TempData["Error"] = "Một hoặc nhiều ghế bạn chọn đã được đặt trước.";
                return RedirectToAction("ChonGheHK", new { maChuyen });
            }

            var chuyenXe = _db.ChuyenXes
                .Include(cx => cx.BienSoXeNavigation)
                .FirstOrDefault(cx => cx.MaChuyen == maChuyen);

            if (chuyenXe == null || chuyenXe.BienSoXeNavigation == null)
            {
                _logger.LogError($"Chuyến xe không tồn tại: maChuyen={maChuyen}");
                TempData["Error"] = "Chuyến xe không tồn tại.";
                return RedirectToAction("ChonGheHK", new { maChuyen });
            }

            var giaVe = chuyenXe.GiaVe ?? 0;
            var expectedTotalPrice = vitri.Count * giaVe;
            _logger.LogInformation($"Client totalPrice: {totalPrice}, Expected totalPrice: {expectedTotalPrice}");
            if (totalPrice != expectedTotalPrice)
            {
                TempData["Error"] = $"Tổng tiền không khớp. Client: {totalPrice}, Server: {expectedTotalPrice}";
                return RedirectToAction("ChonGheHK", new { maChuyen });
            }

            // Tạo PhieuDatVe
            var phieuDatVe = new PhieuDatVe
            {
                Email = email,
                NgayDat = DateTime.Now,
                TongTien = totalPrice,
                TrangThai = phuongThuc == "Lỗi" ? "Chưa thanh toán" : "Đã thanh toán"
            };

            // Áp dụng khuyến mãi
            var khuyenMai = _db.KhuyenMais
                .FirstOrDefault(k => k.NgayBatDau <= DateTime.Now && k.NgayKetThuc >= DateTime.Now);
            if (khuyenMai != null)
            {
                phieuDatVe.MaKhuyenMai = khuyenMai.MaKhuyenMai;
                phieuDatVe.TongTien = totalPrice * (1 - (khuyenMai.PhanTramGiam / 100));
                _logger.LogInformation($"Áp dụng khuyến mãi {khuyenMai.TenKhuyenMai}: Giảm {khuyenMai.PhanTramGiam}%, Tổng tiền sau giảm: {phieuDatVe.TongTien}");
            }

            _db.PhieuDatVes.Add(phieuDatVe);
            _db.SaveChanges();

            // Tạo VeXe và cập nhật trạng thái ghế
            foreach (var seatId in seatIds)
            {
                var veXe = new VeXe
                {
                    MaPhieu = phieuDatVe.MaPhieu,
                    MaChuyen = maChuyen,
                    IdVitri = seatId,
                    TenKh = tenKhachHang,
                    Email = email,
                    GhiChu = ghiChu,
                    TenVe = chuyenXe.TenChuyenXe,
                    TrangThai = phuongThuc == "Lỗi" ? "Chưa thanh toán" : "Đã thanh toán",
                    NgayDat = DateTime.Now,
                    Sđt = soDienThoai
                };
                _db.VeXes.Add(veXe);

                var ghe = _db.Vitrighes.Find(seatId);
                if (ghe != null)
                {
                    ghe.Trangthai = true;
                }
            }

            // Tạo ThanhToan
            var thanhToan = new ThanhToan
            {
                MaPhieu = phieuDatVe.MaPhieu,
                PhuongThuc = phuongThuc,
                SoTien = phieuDatVe.TongTien ?? totalPrice,
                NgayThanhToan = DateTime.Now,
                TrangThai = phuongThuc == "Lỗi" ? "Chưa thanh toán" : "Thành công",
                MaGiaoDich = phuongThuc == "VNPAY" ? null : "COUNTER_" + phieuDatVe.MaPhieu
            };
            _db.ThanhToans.Add(thanhToan);

            _db.SaveChanges();

            // Lưu ghế đã chọn vào session
            HttpContext.Session.SetString("SelectedSeats", selectedSeats);

            // Chuyển hướng đến InVe
            return RedirectToAction("InVe", new { maPhieu = phieuDatVe.MaPhieu });
        }

        // GET: DatVeChoKH/InVe
        public IActionResult InVe(int maPhieu)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            var phieuDatVe = _db.PhieuDatVes
                .Include(p => p.VeXes)
                .ThenInclude(v => v.IdVitriNavigation)
                .Include(p => p.MaKhuyenMaiNavigation)
                .Include(p => p.VeXes)
                .ThenInclude(v => v.MaChuyenNavigation)
                .ThenInclude(cx => cx.MaTuyenNavigation)
                .ThenInclude(tx => tx.MaBenXeDiNavigation)
                .Include(p => p.VeXes)
                .ThenInclude(v => v.MaChuyenNavigation)
                .ThenInclude(cx => cx.MaTuyenNavigation)
                .ThenInclude(tx => tx.MaBenXeDenNavigation)
                .FirstOrDefault(p => p.MaPhieu == maPhieu);

            if (phieuDatVe == null)
            {
                _logger.LogError($"Phiếu đặt vé không tồn tại: maPhieu={maPhieu}");
                ViewData["Error"] = "Phiếu đặt vé không tồn tại.";
                return View("Error");
            }

            return View(phieuDatVe);
        }
    }
}