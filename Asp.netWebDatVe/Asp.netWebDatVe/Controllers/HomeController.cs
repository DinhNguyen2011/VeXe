using System.Diagnostics;
using System.Text.RegularExpressions;
using Asp.netWebDatVe.Models;
using Asp.netWebDatVe.Models.Payment;
using Asp.netWebDatVe.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Asp.netWebDatVe.Controllers
{
    public class HomeController : Controller
    {
        private readonly QLDatVeContext db;
        private readonly ILogger<HomeController> _logger;
        private readonly IVNPayService _vnpayService;

        public HomeController(QLDatVeContext context, ILogger<HomeController> logger, IVNPayService vnpayService)
        {
            db = context;
            _logger = logger;
            _vnpayService = vnpayService;
        }

        public IActionResult Index(string diemDi = "", string diemDen = "", DateTime? ngayDi = null)
        {

            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            ViewData["Title"] = "Trang Chủ";

            var tgian = DateTime.Now;
            ViewBag.DanhSachDiemDi = db.TuyenXes.Select(t => t.DiemDi).Distinct().ToList();
            ViewBag.DanhSachDiemDen = db.TuyenXes.Select(t => t.DiemDen).Distinct().ToList();
            ViewBag.DiemDi = diemDi;
            ViewBag.DiemDen = diemDen;
            ViewBag.NgayDi = ngayDi;

            if (string.IsNullOrEmpty(diemDi) || string.IsNullOrEmpty(diemDen) || ngayDi == null)
            {
                ViewBag.ChuyenXes = new List<ChuyenXe>();
                return View();
            }

            var tuyenXe = db.TuyenXes
                .Include(t => t.MaBenXeNavigation)
                .Include(t => t.MaBenXeDenNavigation)
                .FirstOrDefault(t => t.DiemDi == diemDi && t.DiemDen == diemDen);

            if (tuyenXe == null)
            {
                ViewBag.Message = "Không tìm thấy tuyến xe phù hợp.";
                ViewBag.ChuyenXes = new List<ChuyenXe>();
                return View();
            }

            var chuyenXes = db.ChuyenXes
                .Include(cx => cx.BienSoXeNavigation)
                .ThenInclude(x => x.IdLoaiNavigation)
                .Include(cx => cx.VeXes)
                .Where(cx => cx.MaTuyen == tuyenXe.MaTuyen &&
                             cx.ThoiDiemKhoiHanh.HasValue &&
                             cx.ThoiDiemKhoiHanh.Value.Date == ngayDi.Value.Date && cx.ThoiDiemKhoiHanh >= tgian)
                .ToList();


            if (chuyenXes.Count == 0)
            {
                ViewBag.Message = "Không có chuyến xe vào ngày đã chọn.";
            }

            ViewBag.TuyenXe = tuyenXe;
            ViewBag.ChuyenXes = chuyenXes;

            return View();
        }
        public IActionResult ChonGhe(int maChuyen)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            ViewData["Title"] = "Chọn ghế";


            var chuyenXe = db.ChuyenXes
                .Include(cx => cx.BienSoXeNavigation)
                .ThenInclude(x => x.IdLoaiNavigation)
                .FirstOrDefault(cx => cx.MaChuyen == maChuyen);

            if (chuyenXe == null || chuyenXe.BienSoXeNavigation?.IdLoaiNavigation == null)
            {
                TempData["Error"] = "Chuyến xe hoặc loại xe không tồn tại.";
                return RedirectToAction("Index");
            }


            var loaixe = chuyenXe.BienSoXeNavigation.IdLoaiNavigation;
            var soGhe = loaixe.Soghe;


            var danhSachGhe = db.Vitrighes
                .Where(ghe => ghe.Bienso == chuyenXe.BienSoXe)
                .Select(ghe => new
                {
                    ghe.IdVitri,
                    ghe.Tenvitri,
                    ghe.Trangthai
                })
                .ToList();


            ViewBag.MaChuyen = maChuyen;
            ViewBag.ChuyenXe = chuyenXe;
            ViewBag.SoGhe = soGhe;
            ViewBag.GheDaDat = danhSachGhe.Where(ghe => ghe.Trangthai == true).Select(ghe => ghe.IdVitri).ToList();
            ViewBag.DanhSachGhe = danhSachGhe;

            return View("ChonGhe");
        }


        [HttpPost]
        public IActionResult DatVe(int maChuyen, string selectedSeats, string tenKhachHang, string soDienThoai, string email, string ghiChu, decimal totalPrice)
        {
            // Validate dữ liệu đầu vào
            if (string.IsNullOrWhiteSpace(tenKhachHang) || string.IsNullOrWhiteSpace(soDienThoai) || string.IsNullOrWhiteSpace(email))
            {
                TempData["Error"] = "Vui lòng điền đầy đủ thông tin.";
                return RedirectToAction("ChonGhe", new { maChuyen = maChuyen });
            }

            if (!Regex.IsMatch(soDienThoai, @"^[0-9]{10}$"))
            {
                TempData["Error"] = "Số điện thoại không hợp lệ.";
                return RedirectToAction("ChonGhe", new { maChuyen = maChuyen });
            }

            var seatIds = selectedSeats.Split(',').Select(int.Parse).ToList();

            // Kiểm tra trạng thái ghế
            var vitri = db.Vitrighes
                .Where(g => seatIds.Contains(g.IdVitri) && g.Trangthai != true)
                .ToList();

            if (vitri.Count != seatIds.Count)
            {
                TempData["Error"] = "Một hoặc nhiều ghế bạn chọn đã được đặt trước.";
                return RedirectToAction("ChonGhe", new { maChuyen = maChuyen });
            }

            // Kiểm tra tổng tiền
            var chuyenXe = db.ChuyenXes.FirstOrDefault(cx => cx.MaChuyen == maChuyen);
            var giaVe = chuyenXe?.GiaVe ?? 0;
            var expectedTotalPrice = vitri.Count * giaVe;
            if (totalPrice != expectedTotalPrice)
            {
                TempData["Error"] = "Tổng tiền không khớp. Vui lòng thử lại.";
                return RedirectToAction("ChonGhe", new { maChuyen = maChuyen });
            }

            // Đánh dấu ghế tạm thời
            foreach (var seat in vitri)
            {
                seat.Trangthai = true;
            }
            db.SaveChanges();

            // Lưu thông tin vào Session
            var pendingBooking = new BangJSonTamPDV
            {
                MaChuyen = maChuyen,
                SeatIds = seatIds,
                TenKhachHang = tenKhachHang,
                SoDienThoai = soDienThoai,
                Email = email,
                GhiChu = ghiChu,
                TotalPrice = totalPrice,
                NgayDat = DateTime.Now,
                TenChuyenXe = chuyenXe?.TenChuyenXe
            };

            HttpContext.Session.SetString("PendingBooking", JsonConvert.SerializeObject(pendingBooking));

            // Tạo thông tin thanh toán
            var paymentInfo = new PaymentInformationModel
            {
                MaPhieu = (int)(DateTime.Now.Ticks % int.MaxValue), // Tạm dùng timestamp làm mã phiếu
                Amount = totalPrice,
                OrderDescription = $"Thanh toan ve xe cho {tenKhachHang}",
                Name = tenKhachHang,
                OrderType = "bus_booking"
            };

            // Tạo URL thanh toán VNPay
            string vnpayUrl = _vnpayService.CreatePaymentUrl(paymentInfo, HttpContext);
            return Redirect(vnpayUrl);
        }

        [HttpGet]
        public IActionResult PaymentCallback()
        {
            var paymentResponse = _vnpayService.PaymentExecute(Request.Query);

            // Kiểm tra tính hợp lệ của dữ liệu
            if (!paymentResponse.Success && paymentResponse.VnPayResponseCode == "97")
            {
                TempData["ThongBao"] = "Dữ liệu trả về từ VNPay không hợp lệ.";
                return View("PaymentCallback", paymentResponse);
            }

            // Lấy thông tin từ Session
            var pendingBookingJson = HttpContext.Session.GetString("PendingBooking");
            if (string.IsNullOrEmpty(pendingBookingJson))
            {
                TempData["ThongBao"] = "Phiên đặt vé đã hết hạn hoặc không tồn tại.";
                return View("PaymentCallback", paymentResponse);
            }

            var pendingBooking = JsonConvert.DeserializeObject<BangJSonTamPDV>(pendingBookingJson);

            // Kiểm tra lại trạng thái ghế
            var vitri = db.Vitrighes
                .Where(g => pendingBooking.SeatIds.Contains(g.IdVitri) && g.Trangthai == true)
                .ToList();

            if (vitri.Count != pendingBooking.SeatIds.Count)
            {
                // Giải phóng ghế nếu còn
                foreach (var seatId in pendingBooking.SeatIds)
                {
                    var ghe = db.Vitrighes.FirstOrDefault(g => g.IdVitri == seatId);
                    if (ghe != null)
                        ghe.Trangthai = false;
                }
                db.SaveChanges();

                HttpContext.Session.Remove("PendingBooking");
                TempData["ThongBao"] = "Một hoặc nhiều ghế đã được đặt bởi người khác.";
                return View("PaymentCallback", paymentResponse);
            }

            try
            {
                if (paymentResponse.Success)
                {
                    // Tạo PhieuDatVe
                    var phieuDatVe = new PhieuDatVe
                    {
                        Email = pendingBooking.Email,
                        NgayDat = pendingBooking.NgayDat,
                        TongTien = pendingBooking.TotalPrice,
                        TrangThai = "Đã thanh toán",
                        VnpTransactionId = paymentResponse.TransactionId
                    };
                    db.PhieuDatVes.Add(phieuDatVe);
                    db.SaveChanges();

                    // Tạo VeXe
                    foreach (var seatId in pendingBooking.SeatIds)
                    {
                        var veXe = new VeXe
                        {
                            MaPhieu = phieuDatVe.MaPhieu,
                            MaChuyen = pendingBooking.MaChuyen,
                            IdVitri = seatId,
                            TenKh = pendingBooking.TenKhachHang,
                            Email = pendingBooking.Email,
                            GhiChu = pendingBooking.GhiChu,
                            TenVe = pendingBooking.TenChuyenXe,
                            TrangThai = "Đã thanh toán",
                            NgayDat = pendingBooking.NgayDat
                        };
                        db.VeXes.Add(veXe);
                    }

                    TempData["ThongBao"] = "Thanh toán thành công! Vé của bạn đã được xác nhận.";
                }
                else
                {
                    // Giải phóng ghế
                    foreach (var seatId in pendingBooking.SeatIds)
                    {
                        var ghe = db.Vitrighes.FirstOrDefault(g => g.IdVitri == seatId);
                        if (ghe != null)
                            ghe.Trangthai = false;
                    }

                    TempData["ThongBao"] = "Thanh toán thất bại. Vui lòng thử lại.";
                }

                db.SaveChanges();
                HttpContext.Session.Remove("PendingBooking"); // Xóa Session
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi xử lý thanh toán cho email {pendingBooking.Email}");
                TempData["ThongBao"] = "Đã xảy ra lỗi khi xử lý thanh toán. Vui lòng liên hệ hỗ trợ.";

                // Giải phóng ghế nếu có lỗi
                foreach (var seatId in pendingBooking.SeatIds)
                {
                    var ghe = db.Vitrighes.FirstOrDefault(g => g.IdVitri == seatId);
                    if (ghe != null)
                        ghe.Trangthai = false;
                }
                db.SaveChanges();
                HttpContext.Session.Remove("PendingBooking");
            }

            return View("PaymentCallback", paymentResponse);
        }

        public IActionResult XemCacPhieuDatVe()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            ViewData["Title"] = "Thông tin vé";

            string? userEmail = HttpContext.Session.GetString("UserInfo");

            if (userEmail == null)
            {
                TempData["Error"] = "Vui lòng đăng nhập để xem thông tin đặt vé.";
                return RedirectToAction("Login", "Account");
            }
            var user = JsonConvert.DeserializeObject<NguoiDung>(userEmail);
            string email = user?.Email ?? "";


            var bookings = db.PhieuDatVes
                .Where(p => p.Email == email)
                .OrderByDescending(p => p.NgayDat)
                .ToList();

            return View(bookings);
        }
        [HttpPost]
        public IActionResult HuyPhieuDatVe(int maPhieu)
        {
            var phieu = db.PhieuDatVes
                .Include(p => p.VeXes)
                .ThenInclude(v => v.IdVitriNavigation)
                .FirstOrDefault(p => p.MaPhieu == maPhieu);

            if (phieu == null)
            {
                TempData["Error"] = "Không tìm thấy phiếu đặt vé.";
                return RedirectToAction("XemCacPhieuDatVe");
            }

            // Đổi trạng thái ghế về false (chưa đặt)
            foreach (var ve in phieu.VeXes)
            {
                if (ve.IdVitriNavigation != null)
                {
                    ve.IdVitriNavigation.Trangthai = false;
                }
            }

        
            db.VeXes.RemoveRange(phieu.VeXes);

           
            db.PhieuDatVes.Remove(phieu);

            db.SaveChanges();

            TempData["Message"] = "Hủy vé thành công.";
            return RedirectToAction("XemCacPhieuDatVe");
        }

        public IActionResult Details(int id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            ViewData["Title"] = "Chi tiết phiếu đặt vé";

            // Kiểm tra Session
            string? userInfo = HttpContext.Session.GetString("UserInfo");

            if (userInfo == null)
            {
                TempData["Error"] = "Vui lòng đăng nhập để xem chi tiết phiếu đặt vé.";
                return RedirectToAction("Login", "Account");
            }


            var booking = db.PhieuDatVes
                .Include(p => p.VeXes)
                .FirstOrDefault(p => p.MaPhieu == id);

            if (booking == null)
            {
                TempData["Error"] = "Phiếu đặt vé không tồn tại.";
                return RedirectToAction("ChiTietVe");
            }

            return View(booking);
        }

    }
}
