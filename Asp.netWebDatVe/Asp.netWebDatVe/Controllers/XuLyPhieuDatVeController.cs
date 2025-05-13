using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Asp.netWebDatVe.Controllers
{
    [Authorize]
    public class XuLyPhieuDatVeController : Controller
    {
        private readonly QLDatVeContext _db;

        public XuLyPhieuDatVeController(QLDatVeContext db)
        {
            _db = db;
        }

        public IActionResult XemCacPhieuDatVe()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            ViewData["Title"] = "Thông tin vé";

            string? userInfo = HttpContext.Session.GetString("UserInfo");

            if (string.IsNullOrEmpty(userInfo))
            {
                TempData["Error"] = "Vui lòng đăng nhập để xem thông tin đặt vé.";
                return RedirectToAction("Login", "Account");
            }

            var user = JsonConvert.DeserializeObject<NguoiDung>(userInfo);
            if (user == null || string.IsNullOrEmpty(user.Email))
            {
                TempData["Error"] = "Thông tin người dùng không hợp lệ.";
                return RedirectToAction("Login", "Account");
            }

            var bookings = _db.PhieuDatVes
                .Include(p => p.VeXes)
                .ThenInclude(v => v.MaChuyenNavigation)
                .Include(p => p.MaKhuyenMaiNavigation)
                .Where(p => p.Email == user.Email)
                .OrderByDescending(p => p.NgayDat)
                .ToList();

            return View(bookings);
        }

        [HttpPost]
        public IActionResult HuyPhieuDatVe(int maPhieu)
        {
            var phieu = _db.PhieuDatVes
                .Include(p => p.VeXes)
                .ThenInclude(v => v.IdVitriNavigation)
                .Include(p => p.VeXes)
                .ThenInclude(v => v.MaChuyenNavigation)
                .FirstOrDefault(p => p.MaPhieu == maPhieu);

            if (phieu == null)
            {
                TempData["Error"] = "Không tìm thấy phiếu đặt vé.";
                return RedirectToAction("XemCacPhieuDatVe");
            }

            // Kiểm tra trạng thái phiếu
            if (phieu.TrangThai != "Đã thanh toán")
            {
                TempData["Error"] = "Chỉ có thể hủy vé đã thanh toán.";
                return RedirectToAction("XemCacPhieuDatVe");
            }

            // Lấy thời gian hiện tại
            var now = DateTime.Now;

            // Lấy thời gian khởi hành sớm nhất từ các vé xe
            DateTime? earliestNgayDi = phieu.VeXes
                .Select(v => v.MaChuyenNavigation?.ThoiDiemKhoiHanh)
                .Where(d => d.HasValue)
                .Min();

            if (!earliestNgayDi.HasValue)
            {
                TempData["Error"] = "Không tìm thấy thông tin chuyến xe.";
                return RedirectToAction("XemCacPhieuDatVe");
            }

            // Tính khoảng thời gian từ hiện tại đến thời gian khởi hành (giờ)
            var hoursUntilDeparture = (earliestNgayDi.Value - now).TotalHours;

            // Tính số tiền hoàn theo quy định
            decimal refundAmount = 0;
            if (hoursUntilDeparture >= 24)
            {
                refundAmount = phieu.TongTien * 0.9m ?? 0;
            }
            else if (hoursUntilDeparture >= 12)
            {
                refundAmount = phieu.TongTien * 0.5m ?? 0;
            }
            // Sau 12h: refundAmount = 0 (không hoàn tiền)

            // Cập nhật trạng thái phiếu đặt vé
            phieu.TrangThai = $"Đã hủy + số tiền hoàn: {refundAmount:N0}";

            // Cập nhật trạng thái vé xe và ghế
            foreach (var ve in phieu.VeXes)
            {
                ve.TrangThai = "Đã hủy";
                if (ve.IdVitriNavigation != null)
                {
                    ve.IdVitriNavigation.Trangthai = false;
                }
            }

            // Lưu thay đổi
            _db.SaveChanges();

            // Thông báo
            if (refundAmount > 0)
            {
                TempData["Message"] = $"Hủy vé thành công. Số tiền hoàn: {refundAmount:N0} VNĐ. Vui lòng liên hệ tổng đài 1000 1457 để xác nhận thông tin hoàn tiền.";
            }
            else
            {
                TempData["Message"] = "Hủy vé thành công. Không được hoàn tiền do hủy quá gần giờ khởi hành.";
            }

            return RedirectToAction("XemCacPhieuDatVe");
        }

        public IActionResult Details(int id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            ViewData["Title"] = "Chi tiết phiếu đặt vé";

            string? userInfo = HttpContext.Session.GetString("UserInfo");

            if (string.IsNullOrEmpty(userInfo))
            {
                TempData["Error"] = "Vui lòng đăng nhập để xem chi tiết phiếu đặt vé.";
                return RedirectToAction("Login", "Account");
            }

            var user = JsonConvert.DeserializeObject<NguoiDung>(userInfo);
            if (user == null || string.IsNullOrEmpty(user.Email))
            {
                TempData["Error"] = "Thông tin người dùng không hợp lệ.";
                return RedirectToAction("Login", "Account");
            }

            var booking = _db.PhieuDatVes
                .Include(p => p.VeXes)
                .ThenInclude(v => v.IdVitriNavigation)
                .ThenInclude(vg => vg.BiensoNavigation)
                .Include(p => p.MaKhuyenMaiNavigation)
                .FirstOrDefault(p => p.MaPhieu == id && p.Email == user.Email);

            if (booking == null)
            {
                TempData["Error"] = "Phiếu đặt vé không tồn tại hoặc không thuộc về bạn.";
                return RedirectToAction("XemCacPhieuDatVe");
            }

            return View(booking);
        }
    }
}