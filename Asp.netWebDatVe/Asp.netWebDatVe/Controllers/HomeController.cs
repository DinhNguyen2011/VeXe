using System.Diagnostics;
using System.Text.RegularExpressions;
using Asp.netWebDatVe.Models;
using Asp.netWebDatVe.Models.Payment;
using Asp.netWebDatVe.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Security.Claims;

namespace Asp.netWebDatVe.Controllers
{
    public class HomeController : Controller
    {
        private readonly QLDatVeContext db;
        private readonly ILogger<HomeController> _logger;
        private readonly IVNPayService _vnpayService;
        private readonly IMoMoService _moMoService;
        private readonly IEmailService _emailService;

        public HomeController(QLDatVeContext context, ILogger<HomeController> logger, IVNPayService vnpayService, IMoMoService moMoService, IEmailService emailService)
        {
            db = context;
            _logger = logger;
            _vnpayService = vnpayService;
            _moMoService = moMoService;
            _emailService = emailService;
        }
        //ngăn truy cập tk admin 
        private IActionResult RestrictAdminAccess()
        {
         
            if (User.Identity.IsAuthenticated)
            {
                var maQuyen = User.FindFirst(ClaimTypes.Role)?.Value;
                if (maQuyen == "1" || maQuyen == "2")
                {
                    HttpContext.Session.Clear(); 
                  //  TempData["Error"] = "Tài khoản admin không được phép truy cập giao diện người dùng.";
                    return RedirectToAction("Login", "Account");
                }
            }
            return null;
        }

        // 
        public IActionResult Index(string diemDi = "", string diemDen = "", DateTime? ngayDi = null, bool isSubmitted = false)
        {
            var restrictResult = RestrictAdminAccess();
            if (restrictResult != null) return restrictResult;

            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            ViewData["Title"] = "Trang Chủ";
            // khai báo thời gian 
            var tgian = DateTime.Now;
            ViewBag.DanhSachDiemDi = db.TuyenXes.Select(t => t.DiemDi).Distinct().ToList();
            ViewBag.DanhSachDiemDen = db.TuyenXes.Select(t => t.DiemDen).Distinct().ToList();
            ViewBag.DiemDi = diemDi;
            ViewBag.DiemDen = diemDen;
            ViewBag.NgayDi = ngayDi;

            if (isSubmitted && (string.IsNullOrEmpty(diemDi) || string.IsNullOrEmpty(diemDen) || ngayDi == null))
            {
                ViewBag.Mes = "Vui lòng nhập đầy đủ thông tin chuyến đi";
                ViewBag.ChuyenXes = new List<ChuyenXe>();
                ViewBag.KhuyenMais = db.KhuyenMais
                    //.Where(k => k.NgayBatDau <= DateTime.Now && k.NgayKetThuc >= DateTime.Now)
                    .ToList();
                return View();
            }

            var tuyenXe = db.TuyenXes
                .Include(t => t.MaBenXeDiNavigation)
                .Include(t => t.MaBenXeDenNavigation)
                .FirstOrDefault(t => t.DiemDi == diemDi && t.DiemDen == diemDen);

            if (tuyenXe == null)
            {
                ViewBag.Message = "Tìm chuyến xe phù hợp để đi.";
                ViewBag.ChuyenXes = new List<ChuyenXe>();
                ViewBag.KhuyenMais = db.KhuyenMais
                   // .Where(k => k.NgayBatDau  DateTime.Now && k.NgayKetThuc >= DateTime.Now)
                    .ToList();
                return View();
            }

            var searchDate = ngayDi ?? DateTime.Now;

            var chuyenXes = db.ChuyenXes
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
 
            ViewBag.KhuyenMais = db.KhuyenMais
               // .Where(k => k.NgayBatDau <= DateTime.Now && k.NgayKetThuc >= DateTime.Now)
                .ToList();

            return View();
        }

        //khuyến mãi 
        [HttpGet]
        public async Task<IActionResult> ChiTietKhuyenMai(int? maKhuyenMai)
        {
            var restrictResult = RestrictAdminAccess();
            if (restrictResult != null) return restrictResult;
            if (maKhuyenMai == null)
            {

                TempData["Error"] = "Mã khuyến mãi không hợp lệ.";
                return RedirectToAction("Index");
            }

            var khuyenMai = await db.KhuyenMais
                .AsNoTracking()
                .FirstOrDefaultAsync(km => km.MaKhuyenMai == maKhuyenMai);

            if (khuyenMai == null)
            {

                TempData["Error"] = "Không tìm thấy chương trình khuyến mãi.";
                return RedirectToAction("Index");
            }

            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            ViewData["Title"] = $"Chi tiết Khuyến mãi: {khuyenMai.TenKhuyenMai}";

            return View(khuyenMai);
        }


        //chọn ghê -> view 
        public IActionResult ChonGhe(int maChuyen)
        {
            var restrictResult = RestrictAdminAccess();
            if (restrictResult != null) return restrictResult;

            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            ViewData["Title"] = "Chọn ghế";

            // Truy vấn ChuyenXe
            var chuyenXe = db.ChuyenXes
                .Include(cx => cx.BienSoXeNavigation)
                .ThenInclude(x => x.IdLoaiNavigation)
                .Include(cx => cx.MaTuyenNavigation)
                .ThenInclude(tx => tx.MaBenXeDiNavigation)
                .Include(cx => cx.MaTuyenNavigation)
                .ThenInclude(tx => tx.MaBenXeDenNavigation)
                .FirstOrDefault(cx => cx.MaChuyen == maChuyen);

            if (chuyenXe == null || chuyenXe.BienSoXeNavigation == null || chuyenXe.BienSoXeNavigation.IdLoaiNavigation == null || chuyenXe.MaTuyenNavigation == null)
            {
                TempData["Error"] = "Chuyến xe, loại xe hoặc tuyến xe không tồn tại.";
                return RedirectToAction("Index");
            }

            var loaixe = chuyenXe.BienSoXeNavigation.IdLoaiNavigation;
            var soGhe = loaixe.Soghe;

            // Lấy danh sách ghế và chuyển sang client-side
            var danhSachGhe = db.Vitrighes
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
                    // Tách số từ Tenvitri (bỏ "G") và chuyển thành số
                    string number = ghe.Tenvitri.Replace("G", "");
                    return int.TryParse(number, out int result) ? result : int.MaxValue; // Xử lý lỗi nếu Tenvitri không đúng định dạng
                })
                .ToList();

            // Kiểm tra số ghế thực tế
            var soGheThucTe = danhSachGhe.Count;
            if (soGhe != soGheThucTe)
            {
                soGhe = soGheThucTe;
                _logger.LogWarning($"Số ghế trong loaixe.Soghe ({loaixe.Soghe}) không khớp với danh sách ghế thực tế ({soGheThucTe}) cho chuyến xe {maChuyen}.");
            }

            ViewBag.MaChuyen = maChuyen;
            ViewBag.ChuyenXe = chuyenXe;
            ViewBag.SoGhe = soGhe;
            ViewBag.GheDaDat = danhSachGhe.Where(ghe => ghe.Trangthai == true).Select(ghe => ghe.IdVitri).ToList();
            ViewBag.DanhSachGhe = danhSachGhe;
            ViewBag.TuyenXe = chuyenXe.MaTuyenNavigation;

            return View("ChonGhe");
        }


        // dùng thông tin bảng tạm ( payment. BangJSonTamPDV)
        [HttpPost]
        public IActionResult DatVe(int maChuyen, string selectedSeats, string tenKhachHang, string soDienThoai, string email, string ghiChu, decimal totalPrice, string paymentMethod)
        {
            var restrictResult = RestrictAdminAccess();
            if (restrictResult != null) return restrictResult;

            if (string.IsNullOrWhiteSpace(tenKhachHang) || string.IsNullOrWhiteSpace(soDienThoai) || string.IsNullOrWhiteSpace(email))
            {
                TempData["Error"] = "Vui lòng điền đầy đủ thông tin.";
                return RedirectToAction("ChonGhe", new { maChuyen = maChuyen });
            }

            //if (!Regex.IsMatch(soDienThoai, @"^[0][0-9]{9}$"))
            //{
            //    TempData["Error"] = "Số điện thoại không hợp lệ.";
            //    return RedirectToAction("ChonGhe", new { maChuyen = maChuyen });
            //}

            //if (!Regex.IsMatch(soDienThoai, @"^0[35789][0-9]{8}$"))
            //{
            //    TempData["Error"] = "Số điện thoại phải có 10 chữ số và bắt đầu bằng 03, 05, 07, 08, hoặc 09.";
            //    return RedirectToAction("ChonGhe", new { maChuyen });
            //}
            if (string.IsNullOrWhiteSpace(paymentMethod) || !new[] { "VNPay", "MoMo" }.Contains(paymentMethod))
            {
                TempData["Error"] = "Vui lòng chọn phương thức thanh toán hợp lệ.";
                return RedirectToAction("ChonGhe", new { maChuyen = maChuyen });
            }
            //
            var seatIds = selectedSeats.Split(',').Select(int.Parse).ToList();
            var vitri = db.Vitrighes
                .Where(g => seatIds.Contains(g.IdVitri) && g.Trangthai != true)
                .ToList();

            if (vitri.Count != seatIds.Count)
            {
                TempData["Error"] = "Một hoặc nhiều ghế bạn chọn đã được đặt trước.";
                return RedirectToAction("ChonGhe", new { maChuyen = maChuyen });
            }

            //
            var chuyenXe = db.ChuyenXes
                .Include(cx => cx.BienSoXeNavigation)
                .FirstOrDefault(cx => cx.MaChuyen == maChuyen);

            if (chuyenXe == null || chuyenXe.BienSoXeNavigation == null)
            {
                TempData["Error"] = "Chuyến xe không tồn tại.";
                return RedirectToAction("ChonGhe", new { maChuyen = maChuyen });
            }
            //// Kiểm tra thời gian đặt vé
            //if (chuyenXe.ThoiDiemKhoiHanh <= DateTime.Now.AddMinutes(30))
            //{
            //    TempData["Error"] = "Không thể đặt vé khi thời gian khởi hành còn dưới 30 phút.";
            //    return RedirectToAction("ChonGhe", new { maChuyen });
            //}
            // Kiểm tra tổng tiền
            var giaVe = chuyenXe.GiaVe ?? 0;
            var expectedTotalPrice = vitri.Count * giaVe;
            if (totalPrice != expectedTotalPrice)
            {
                TempData["Error"] = "Tổng tiền không khớp. Vui lòng thử lại.";
                return RedirectToAction("ChonGhe", new { maChuyen = maChuyen });
            }

            //
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
                TenChuyenXe = chuyenXe.TenChuyenXe,
                PaymentMethod = paymentMethod
            };
            //var pendingBooking = new BangJSonTamPDV
            //{
            //    MaChuyen = maChuyen,
            //    SeatIds = seatIds,
            //    TenKhachHang = tenKhachHang.Trim(),
            //    SoDienThoai = soDienThoai.Trim(),
            //    Email = email.Trim(),
            //    GhiChu = ghiChu.Trim(),
            //    TotalPrice = totalPrice,
            //    NgayDat = DateTime.Now,
            //    TenChuyenXe = chuyenXe.TenChuyenXe,
            //    PaymentMethod = paymentMethod
            //};
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var serializedBooking = JsonSerializer.Serialize(pendingBooking, options);
                _logger.LogInformation("Serialized PendingBooking: {serializedBooking}", serializedBooking);
                HttpContext.Session.SetString("PendingBooking", serializedBooking);
                //phiên hết hạn 15p
                //HttpContext.Session.SetInt32("PendingBookingTimeout", (int)DateTime.Now.AddMinutes(15).Ticks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi serialize PendingBooking: {message}", ex.Message);
                TempData["Error"] = "Lỗi khi lưu thông tin đặt vé. Vui lòng thử lại.";
                return RedirectToAction("ChonGhe", new { maChuyen = maChuyen });
            }
            // tạo url thanh toán
            //Payment- PaymentInformationModel 
            var paymentInfo = new PaymentInformationModel
            {
                MaPhieu = (int)(DateTime.Now.Ticks % int.MaxValue),
                Amount = totalPrice,
                OrderDescription = $"Thanh toan ve xe cho {tenKhachHang}",
                Name = tenKhachHang,
                OrderType = "bus_booking"
            };

            try
            {
                if (paymentMethod == "MoMo")
                {
                    var paymentUrl = _moMoService.CreatePaymentUrl(paymentInfo, HttpContext);
                    return Redirect(paymentUrl);
                }
                else
                {
                    var paymentUrl = _vnpayService.CreatePaymentUrl(paymentInfo, HttpContext);
                    return Redirect(paymentUrl);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi tạo URL thanh toán {paymentMethod}: {ex.Message}");
                TempData["Error"] = $"Lỗi khi tạo URL thanh toán: {ex.Message}";
                return RedirectToAction("ChonGhe", new { maChuyen = maChuyen });
            }
        }


        //trả về kết quả  view
        //Xử lý callback từ VNPay
        [HttpGet]
        public async Task<IActionResult> PaymentCallback()
        {
            var restrictResult = RestrictAdminAccess();
            if (restrictResult != null) return restrictResult;

            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            //
            var response = _vnpayService.PaymentExecute(Request.Query);
            return await ProcessPaymentCallback(response, "VNPay");
        }

        //Xử lý callback từ Momo
        [HttpGet]
        public async Task<IActionResult> MoMoPaymentCallback()
        {
            var restrictResult = RestrictAdminAccess();
            if (restrictResult != null) return restrictResult;

            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            //
            var response = _moMoService.PaymentExecute(Request.Query);
            return await ProcessPaymentCallback(response, "MoMo");
        }

        //Xử lý callback thanh toán chung cho VNPay/MoMo
        private async Task<IActionResult> ProcessPaymentCallback(PaymentResponse response, string paymentMethod)
        {
            var pendingBookingJson = HttpContext.Session.GetString("PendingBooking");
            if (string.IsNullOrEmpty(pendingBookingJson))
            {
                _logger.LogWarning("PendingBookingJson is empty or null.");
                TempData["ThongBao"] = "Phiên đặt vé đã hết hạn hoặc không tồn tại.";
                return View("PaymentCallback", response);
            }
            //
            BangJSonTamPDV pendingBooking;
            try
            {
                _logger.LogInformation("PendingBookingJson: {pendingBookingJson}", pendingBookingJson);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                pendingBooking = JsonSerializer.Deserialize<BangJSonTamPDV>(pendingBookingJson, options);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi deserialize PendingBooking: {message}", ex.Message);
                TempData["ThongBao"] = "Lỗi khi đọc thông tin đặt vé. Vui lòng thử lại.";
                return View("PaymentCallback", response);
            }
            //Kiểm tra lại trạng thái ghế
            var vitri = db.Vitrighes
                .Where(g => pendingBooking.SeatIds.Contains(g.IdVitri) && g.Trangthai != true)
                .ToList();

            if (vitri.Count != pendingBooking.SeatIds.Count)
            {
                HttpContext.Session.Remove("PendingBooking");
                TempData["ThongBao"] = "Một hoặc nhiều ghế đã được đặt bởi người khác.";
                return View("PaymentCallback", response);
            }

            try
            {
                if (response.Success)
                {
                    foreach (var seat in vitri)
                    {
                        //Cập nhật trạng thái các ghế được chọn thành đã đặt
                        seat.Trangthai = true;
                    }
                    ////////
                    var phieuDatVe = new PhieuDatVe
                    {
                        Email = pendingBooking.Email,
                        NgayDat = pendingBooking.NgayDat,
                        TongTien = pendingBooking.TotalPrice,
                        TrangThai = "Đã thanh toán",
                        VnpTransactionId = paymentMethod == "VNPay" ? response.TransactionId : null,
                        MoMoTransactionId = paymentMethod == "MoMo" ? response.TransactionId : null
                    };
                    db.PhieuDatVes.Add(phieuDatVe);
                    await db.SaveChangesAsync();

                    ////
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
                    ////
                    var thanhToan = new ThanhToan
                    {
                        MaPhieu = phieuDatVe.MaPhieu,
                        PhuongThuc = paymentMethod,
                        SoTien = pendingBooking.TotalPrice,
                        NgayThanhToan = DateTime.Now,
                        MaGiaoDich = response.TransactionId,
                        TrangThai = "Thành công"
                    };
                    db.ThanhToans.Add(thanhToan);

                    await db.SaveChangesAsync();
                    //Gửi email xác nhận thanh toán:
                    try
                    {
                        var chuyenXe = await db.ChuyenXes
                            .Include(cx => cx.MaTuyenNavigation)
                            .FirstOrDefaultAsync(cx => cx.MaChuyen == pendingBooking.MaChuyen);

                        var seatNames = db.Vitrighes
                            .Where(g => pendingBooking.SeatIds.Contains(g.IdVitri))
                            .Select(g => g.Tenvitri)
                            .ToList();

                        string emailSubject = "Xác Nhận Thanh Toán Đặt Vé Xe Thành Công";
                        string emailBody = "<h2>Xác Nhận Thanh Toán Đặt Vé</h2>" +
                                           $"<p>Xin chào {pendingBooking.TenKhachHang},</p>" +
                                           "<p>Chúng tôi xin xác nhận rằng bạn đã thanh toán thành công cho vé xe qua Trang Website Xe Khách Khánh An. Dưới đây là thông tin chi tiết:</p>" +
                                           "<ul>" +
                                           $"<li><strong>Mã Phiếu:</strong> {phieuDatVe.MaPhieu}</li>" +
                                           $"<li><strong>Tên Chuyến Xe:</strong> {pendingBooking.TenChuyenXe}</li>" +
                                           $"<li><strong>Tuyến:</strong> {chuyenXe?.MaTuyenNavigation?.DiemDi} - {chuyenXe?.MaTuyenNavigation?.DiemDen}</li>" +
                                           $"<li><strong>Thời Gian Khởi Hành:</strong> {chuyenXe?.ThoiDiemKhoiHanh?.ToString("HH:mm dd/MM/yyyy")}</li>" +
                                           $"<li><strong>Ghế:</strong> {string.Join(", ", seatNames)}</li>" +
                                           $"<li><strong>1v/G:</strong> {chuyenXe.GiaVe?.ToString("N0")} VND</li>" +
                                           $"<li><strong>Tổng Tiền:</strong> {pendingBooking.TotalPrice.ToString("N0")} VND</li>" +
                                           $"<li><strong>Mã Giao Dịch:</strong> {response.TransactionId}</li>" +
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

                    TempData["ThongBao"] = $"Thanh toán thành công qua {paymentMethod}! Vé của bạn đã được xác nhận.";
                }
                
                else
                {
                    TempData["ThongBao"] = $"Thanh toán {paymentMethod} thất bại. Mã lỗi: {response.MoMoResponseCode ?? response.VnPayResponseCode}";
                }

                await db.SaveChangesAsync();
                HttpContext.Session.Remove("PendingBooking");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi xử lý thanh toán {paymentMethod} cho email {pendingBooking.Email}");
                TempData["ThongBao"] = $"Đã xảy ra lỗi khi xử lý thanh toán {paymentMethod}. Vui lòng liên hệ hỗ trợ.";

                foreach (var seatId in pendingBooking.SeatIds)
                {
                    var ghe = db.Vitrighes.FirstOrDefault(g => g.IdVitri == seatId);
                    if (ghe != null && ghe.Trangthai == true)
                    {
                        ghe.Trangthai = false;
                    }
                }
                await db.SaveChangesAsync();
                HttpContext.Session.Remove("PendingBooking");
            }

            return View("PaymentCallback", response);
        }


        //Xử lý thông báo 
        [HttpPost]
        public async Task<IActionResult> MoMoNotify()
        {
            var response = _moMoService.PaymentExecute(Request.Query);
            if (!response.Success)
            {
                _logger.LogWarning("MoMo Notify: Payment failed or invalid signature.");
                return Ok();
            }

            var pendingBookingJson = HttpContext.Session.GetString("PendingBooking");
            if (string.IsNullOrEmpty(pendingBookingJson))
            {
                _logger.LogWarning("MoMo Notify: Pending booking not found.");
                return Ok();
            }

            BangJSonTamPDV pendingBooking;
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                pendingBooking = JsonSerializer.Deserialize<BangJSonTamPDV>(pendingBookingJson, options);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi deserialize PendingBooking trong MoMoNotify: {message}", ex.Message);
                return Ok();
            }

            var vitri = db.Vitrighes
                .Where(g => pendingBooking.SeatIds.Contains(g.IdVitri) && g.Trangthai != true)
                .ToList();

            if (vitri.Count != pendingBooking.SeatIds.Count)
            {
                _logger.LogWarning("MoMo Notify: One or more seats are already booked.");
                return Ok();
            }

            try
            {
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
                    MoMoTransactionId = response.TransactionId
                };
                db.PhieuDatVes.Add(phieuDatVe);
                await db.SaveChangesAsync();

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

                var thanhToan = new ThanhToan
                {
                    MaPhieu = phieuDatVe.MaPhieu,
                    PhuongThuc = "MoMo",
                    SoTien = pendingBooking.TotalPrice,
                    NgayThanhToan = DateTime.Now,
                    MaGiaoDich = response.TransactionId,
                    TrangThai = "Thành công"
                };
                db.ThanhToans.Add(thanhToan);

                await db.SaveChangesAsync();
                HttpContext.Session.Remove("PendingBooking");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "MoMo Notify: Error processing payment.");
            }

            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        
        //view
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}