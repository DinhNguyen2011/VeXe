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
        private readonly IEmailService _emailService; 

        public HomeController(QLDatVeContext context, ILogger<HomeController> logger, IVNPayService vnpayService, IEmailService emailService)
        {
            db = context;
            _logger = logger;
            _vnpayService = vnpayService;
            _emailService = emailService; 
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
                .Include(t => t.MaBenXeDiNavigation)
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
                .Include(cx => cx.MaNhanVienNavigation) // Thêm để lấy thông tin nhân viên
                .Include(cx => cx.MaTaiXeNavigation)
                .Include(cx => cx.MaNhanVien1Navigation)
                .Include(cx => cx.VeXes)
                .Where(cx => cx.MaTuyen == tuyenXe.MaTuyen &&
                             cx.ThoiDiemKhoiHanh.HasValue &&
                             cx.ThoiDiemKhoiHanh.Value.Date == ngayDi.Value.Date &&
                             cx.ThoiDiemKhoiHanh >= tgian)
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

            // Truy vấn ChuyenXe, bao gồm cả thông tin tuyến xe và bến xe
            var chuyenXe = db.ChuyenXes
                .Include(cx => cx.BienSoXeNavigation)
                .ThenInclude(x => x.IdLoaiNavigation)
                .Include(cx => cx.MaTuyenNavigation)
                .ThenInclude(tx => tx.MaBenXeDiNavigation) // Bao gồm bến xe đi
                .Include(cx => cx.MaTuyenNavigation)
                .ThenInclude(tx => tx.MaBenXeDenNavigation) // Bao gồm bến xe đến
                .FirstOrDefault(cx => cx.MaChuyen == maChuyen);

            if (chuyenXe == null || chuyenXe.BienSoXeNavigation == null || chuyenXe.BienSoXeNavigation.IdLoaiNavigation == null || chuyenXe.MaTuyenNavigation == null)
            {
                TempData["Error"] = "Chuyến xe, loại xe hoặc tuyến xe không tồn tại.";
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

            // Gán thông tin tuyến xe vào ViewBag để sử dụng trong view
            ViewBag.TuyenXe = chuyenXe.MaTuyenNavigation;

            return View("ChonGhe");
        }

        [HttpPost]
        public IActionResult DatVe(int maChuyen, string selectedSeats, string tenKhachHang, string soDienThoai, string email, string ghiChu, decimal totalPrice)
        {
            if (string.IsNullOrWhiteSpace(tenKhachHang) || string.IsNullOrWhiteSpace(soDienThoai) || string.IsNullOrWhiteSpace(email))
            {
                TempData["Error"] = "Vui lòng điền đầy đủ thông tin.";
                return RedirectToAction("ChonGhe", new { maChuyen = maChuyen });
            }

            if (!Regex.IsMatch(soDienThoai, @"^[0][0-9]{9}$"))
            {
                TempData["Error"] = "Số điện thoại không hợp lệ.";
                return RedirectToAction("ChonGhe", new { maChuyen = maChuyen });
            }

            var seatIds = selectedSeats.Split(',').Select(int.Parse).ToList();

            var vitri = db.Vitrighes
                .Where(g => seatIds.Contains(g.IdVitri) && g.Trangthai != true)
                .ToList();

            if (vitri.Count != seatIds.Count)
            {
                TempData["Error"] = "Một hoặc nhiều ghế bạn chọn đã được đặt trước.";
                return RedirectToAction("ChonGhe", new { maChuyen = maChuyen });
            }

            var chuyenXe = db.ChuyenXes
                .Include(cx => cx.BienSoXeNavigation)
                .FirstOrDefault(cx => cx.MaChuyen == maChuyen);

            if (chuyenXe == null || chuyenXe.BienSoXeNavigation == null)
            {
                TempData["Error"] = "Chuyến xe không tồn tại.";
                return RedirectToAction("ChonGhe", new { maChuyen = maChuyen });
            }

            var giaVe = chuyenXe.GiaVe ?? 0;
            var expectedTotalPrice = vitri.Count * giaVe;
            if (totalPrice != expectedTotalPrice)
            {
                TempData["Error"] = "Tổng tiền không khớp. Vui lòng thử lại.";
                return RedirectToAction("ChonGhe", new { maChuyen = maChuyen });
            }

            // KHÔNG đánh dấu ghế ở đây nữa
            // foreach (var seat in vitri)
            // {
            //     seat.Trangthai = true;
            // }
            // db.SaveChanges();

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
                TenChuyenXe = chuyenXe.TenChuyenXe
            };

            HttpContext.Session.SetString("PendingBooking", JsonConvert.SerializeObject(pendingBooking));

            var paymentInfo = new PaymentInformationModel
            {
                MaPhieu = (int)(DateTime.Now.Ticks % int.MaxValue),
                Amount = totalPrice,
                OrderDescription = $"Thanh toan ve xe cho {tenKhachHang}",
                Name = tenKhachHang,
                OrderType = "bus_booking"
            };

            string vnpayUrl = _vnpayService.CreatePaymentUrl(paymentInfo, HttpContext);
            return Redirect(vnpayUrl);
        }

        [HttpGet]
        public async Task<IActionResult> PaymentCallback()
        {
            var paymentResponse = _vnpayService.PaymentExecute(Request.Query);

            if (!paymentResponse.Success && paymentResponse.VnPayResponseCode == "97")
            {
                TempData["ThongBao"] = "Dữ liệu trả về từ VNPay không hợp lệ.";
                return View("PaymentCallback", paymentResponse);
            }

            var pendingBookingJson = HttpContext.Session.GetString("PendingBooking");
            if (string.IsNullOrEmpty(pendingBookingJson))
            {
                TempData["ThongBao"] = "Phiên đặt vé đã hết hạn hoặc không tồn tại.";
                return View("PaymentCallback", paymentResponse);
            }

            var pendingBooking = JsonConvert.DeserializeObject<BangJSonTamPDV>(pendingBookingJson);

            var vitri = db.Vitrighes
                .Where(g => pendingBooking.SeatIds.Contains(g.IdVitri) && g.Trangthai != true)
                .ToList();

            if (vitri.Count != pendingBooking.SeatIds.Count)
            {
                HttpContext.Session.Remove("PendingBooking");
                TempData["ThongBao"] = "Một hoặc nhiều ghế đã được đặt bởi người khác.";
                return View("PaymentCallback", paymentResponse);
            }

            try
            {
                if (paymentResponse.Success)
                {
                    // Thanh toán thành công: Đánh dấu ghế là đã đặt
                    foreach (var seat in vitri)
                    {
                        seat.Trangthai = true;
                    }

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
                            NgayDat = pendingBooking.NgayDat,
                            Sđt = pendingBooking.SoDienThoai
                        };
                        db.VeXes.Add(veXe);
                    }

                    // Tạo bản ghi ThanhToan dựa trên PhieuDatVe
                    var thanhToan = new ThanhToan
                    {
                        MaPhieu = phieuDatVe.MaPhieu,
                        PhuongThuc = "VNPAY",
                        SoTien = pendingBooking.TotalPrice,
                        NgayThanhToan = DateTime.Now,
                        MaGiaoDich = paymentResponse.TransactionId,
                        TrangThai = "Thành công"
                    };
                    db.ThanhToans.Add(thanhToan);

                    db.SaveChanges();

                    // Gửi email xác nhận
                    try
                    {
                        var chuyenXe = db.ChuyenXes
                            .Include(cx => cx.MaTuyenNavigation)
                            .FirstOrDefault(cx => cx.MaChuyen == pendingBooking.MaChuyen);

                        var seatNames = db.Vitrighes
                            .Where(g => pendingBooking.SeatIds.Contains(g.IdVitri))
                            .Select(g => g.Tenvitri)
                            .ToList();

                        string emailSubject = "Xác Nhận Thanh Toán Đặt Vé Xe Thành Công";
                        string emailBody = "<h2>Xác Nhận Thanh Toán Đặt Vé</h2>" +
                                           $"<p>Xin chào {pendingBooking.TenKhachHang},</p>" +
                                           "<p>Chúng tôi xin xác nhận rằng bạn đã thanh toán thành công cho vé xe. Dưới đây là thông tin chi tiết:</p>" +
                                           "<ul>" +
                                           $"<li><strong>Mã Phiếu:</strong> {phieuDatVe.MaPhieu}</li>" +
                                           $"<li><strong>Tên Chuyến Xe:</strong> {pendingBooking.TenChuyenXe}</li>" +
                                           $"<li><strong>Tuyến:</strong> {chuyenXe?.MaTuyenNavigation?.DiemDi} - {chuyenXe?.MaTuyenNavigation?.DiemDen}</li>" +
                                           $"<li><strong>Thời Gian Khởi Hành:</strong> {chuyenXe?.ThoiDiemKhoiHanh?.ToString("HH:mm dd/MM/yyyy")}</li>" +
                                           $"<li><strong>Ghế:</strong> {string.Join(", ", seatNames)}</li>" +
                                           $"<li><strong>1v/G:</strong> {chuyenXe.GiaVe?.ToString("N0")} VND</li>" +
                                           $"<li><strong>Tổng Tiền:</strong> {pendingBooking.TotalPrice.ToString("N0")} VND</li>" +
                                           $"<li><strong>Mã Giao Dịch:</strong> {paymentResponse.TransactionId}</li>" +
                                           $"<li><strong>Ngày Thanh Toán:</strong> {DateTime.Now.ToString("HH:mm dd/MM/yyyy")}</li>" +
                                           "</ul>" +
                                           "<p>Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi. Nếu có thắc mắc, vui lòng liên hệ qua tổng đài <strong>1000 1234</strong>.</p>" +
                                           "<p>Trân trọng,<br>Xe khách Khánh An</p>";

                        await _emailService.SendEmailAsync(phieuDatVe.Email, emailSubject, emailBody);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Lỗi khi gửi email xác nhận cho {phieuDatVe.Email}");
                        TempData["ThongBao"] = "Thanh toán thành công, nhưng không thể gửi email xác nhận. Vui lòng kiểm tra thông tin vé trong hệ thống.";
                    }

                    TempData["ThongBao"] = "Thanh toán thành công! Vé của bạn đã được xác nhận.";
                }
                else
                {
                    TempData["ThongBao"] = "Thanh toán thất bại hoặc bị hủy. Ghế đã được trả lại trạng thái trống.";
                }

                db.SaveChanges();
                HttpContext.Session.Remove("PendingBooking");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi xử lý thanh toán cho email {pendingBooking.Email}");
                TempData["ThongBao"] = "Đã xảy ra lỗi khi xử lý thanh toán. Vui lòng liên hệ hỗ trợ.";

                foreach (var seatId in pendingBooking.SeatIds)
                {
                    var ghe = db.Vitrighes.FirstOrDefault(g => g.IdVitri == seatId);
                    if (ghe != null && ghe.Trangthai == true)
                    {
                        ghe.Trangthai = false;
                    }
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

            var bookings = db.PhieuDatVes
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
            var phieu = db.PhieuDatVes
                .Include(p => p.VeXes)
                .ThenInclude(v => v.IdVitriNavigation)
                .FirstOrDefault(p => p.MaPhieu == maPhieu);

            if (phieu == null)
            {
                TempData["Error"] = "Không tìm thấy phiếu đặt vé.";
                return RedirectToAction("XemCacPhieuDatVe");
            }

            // Kiểm tra trạng thái trước khi hủy
            if (phieu.TrangThai != "Đã thanh toán")
            {
                TempData["Error"] = "Chỉ có thể hủy vé đã thanh toán.";
                return RedirectToAction("XemCacPhieuDatVe");
            }

            // Đổi trạng thái ghế về false
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

            var booking = db.PhieuDatVes
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