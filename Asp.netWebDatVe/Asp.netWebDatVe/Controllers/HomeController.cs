using Asp.netWebDatVe.Models;
using Asp.netWebDatVe.Models.Payment;
using Asp.netWebDatVe.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

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

        // Ngăn truy cập tài khoản admin
        private IActionResult RestrictAdminAccess()
        {
            if (User.Identity.IsAuthenticated)
            {
                var maQuyen = User.FindFirst(ClaimTypes.Role)?.Value;
                if (maQuyen == "1" || maQuyen == "2")
                {
                    HttpContext.Session.Clear();
                    return RedirectToAction("Login", "Account");
                }
            }
            return null;
        }

        // GET: Home/Index
        public IActionResult Index(string diemDi = "", string diemDen = "", DateTime? ngayDi = null, bool isSubmitted = false)
        {
            var restrictResult = RestrictAdminAccess();
            if (restrictResult != null) return restrictResult;

            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            ViewData["Title"] = "Trang Chủ";

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
                ViewBag.KhuyenMais = db.KhuyenMais.ToList();
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
                ViewBag.KhuyenMais = db.KhuyenMais.ToList();
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
            ViewBag.KhuyenMais = db.KhuyenMais.ToList();

            return View();
        }

        // GET: Home/ChiTietKhuyenMai
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

        // GET: Home/ChonGhe
        public IActionResult ChonGhe(int maChuyen)
        {
            var restrictResult = RestrictAdminAccess();
            if (restrictResult != null) return restrictResult;

            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            ViewData["Title"] = "Chọn ghế";

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
                _logger.LogError($"ChuyenXe not found for maChuyen = {maChuyen}");
                TempData["Error"] = "Chuyến xe, loại xe hoặc tuyến xe không tồn tại.";
                return RedirectToAction("Index");
            }

            var loaixe = chuyenXe.BienSoXeNavigation.IdLoaiNavigation;
            var soGhe = loaixe.Soghe;

            // Lấy danh sách ghế từ Vitrighe, bao gồm Trangthai
            var danhSachGhe = db.Vitrighes
                .Where(ghe => ghe.Bienso == chuyenXe.BienSoXe)
                .Select(ghe => new { ghe.IdVitri, ghe.Tenvitri, ghe.Trangthai })
                .ToList()
                .OrderBy(ghe =>
                {
                    string number = ghe.Tenvitri.Replace("G", "");
                    return int.TryParse(number, out int result) ? result : int.MaxValue;
                })
                .ToList();

            // Lấy danh sách ghế đã đặt từ VeXe (dùng trong DatVe)
            var gheDaDat = db.VeXes
       .Where(vx => vx.MaChuyen == maChuyen && vx.TrangThai == "Đã thanh toán" && vx.IdVitri.HasValue)
       .Select(vx => vx.IdVitri.Value)
       .ToList();
            _logger.LogInformation($"gheDaDat for maChuyen {maChuyen}: {string.Join(", ", gheDaDat)}");

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
            ViewBag.GheDaDat = gheDaDat;
            ViewBag.DanhSachGhe = danhSachGhe;
            ViewBag.TuyenXe = chuyenXe.MaTuyenNavigation;

            return View("ChonGhe");
        }

        // POST: Home/DatVe
        [HttpPost]
        public IActionResult DatVe(int maChuyen, string selectedSeats, string tenKhachHang, string soDienThoai, string email, string ghiChu, decimal totalPrice, string paymentMethod)
        {
            var restrictResult = RestrictAdminAccess();
            if (restrictResult != null) return restrictResult;

            if (string.IsNullOrWhiteSpace(tenKhachHang) || string.IsNullOrWhiteSpace(soDienThoai) || string.IsNullOrWhiteSpace(email))
            {
                TempData["Error"] = "Vui lòng điền đầy đủ thông tin.";
                return RedirectToAction("ChonGhe", new { maChuyen });
            }

            if (!Regex.IsMatch(soDienThoai, @"^0[35789][0-9]{8}$"))
            {
                TempData["Error"] = "Số điện thoại phải có 10 chữ số và bắt đầu bằng 03, 05, 07, 08, hoặc 09.";
                return RedirectToAction("ChonGhe", new { maChuyen });
            }

            if (string.IsNullOrWhiteSpace(paymentMethod) || !new[] { "VNPay", "MoMo" }.Contains(paymentMethod))
            {
                TempData["Error"] = "Vui lòng chọn phương thức thanh toán hợp lệ.";
                return RedirectToAction("ChonGhe", new { maChuyen });
            }

            if (string.IsNullOrWhiteSpace(selectedSeats))
            {
                TempData["Error"] = "Vui lòng chọn ít nhất một ghế.";
                return RedirectToAction("ChonGhe", new { maChuyen });
            }

            var seatIds = selectedSeats.Split(',').Select(int.Parse).ToList();
            var vitri = db.Vitrighes
                .Where(g => seatIds.Contains(g.IdVitri) && g.Bienso == db.ChuyenXes.FirstOrDefault(cx => cx.MaChuyen == maChuyen).BienSoXe)
                .ToList();

            if (vitri.Count != seatIds.Count)
            {
                TempData["Error"] = "Một hoặc nhiều ghế không hợp lệ hoặc không thuộc chuyến xe này.";
                return RedirectToAction("ChonGhe", new { maChuyen });
            }

            //// Kiểm tra trạng thái ghế trong Vitrighe
            //var gheDaDat = db.Vitrighes
            //    .Where(g => seatIds.Contains(g.IdVitri) && g.Trangthai == true)
            //    .Select(g => g.Tenvitri)
            //    .ToList();

            //if (gheDaDat.Any())
            //{
            //    TempData["Error"] = $"Các ghế {string.Join(", ", gheDaDat)} đã được đặt trước.";
            //    return RedirectToAction("ChonGhe", new { maChuyen });
            //}

            var chuyenXe = db.ChuyenXes
                .Include(cx => cx.BienSoXeNavigation)
                .FirstOrDefault(cx => cx.MaChuyen == maChuyen);

            if (chuyenXe == null || chuyenXe.BienSoXeNavigation == null)
            {
                TempData["Error"] = "Chuyến xe không tồn tại.";
                return RedirectToAction("ChonGhe", new { maChuyen });
            }

            var giaVe = chuyenXe.GiaVe ?? 0;
            var expectedTotalPrice = seatIds.Count * giaVe;
            if (totalPrice != expectedTotalPrice)
            {
                TempData["Error"] = "Tổng tiền không khớp. Vui lòng thử lại.";
                return RedirectToAction("ChonGhe", new { maChuyen });
            }

            var pendingBooking = new BangJSonTamPDV
            {
                MaChuyen = maChuyen,
                SeatIds = seatIds,
                TenKhachHang = tenKhachHang.Trim(),
                SoDienThoai = soDienThoai.Trim(),
                Email = email.Trim(),
                GhiChu = ghiChu?.Trim(),
                TotalPrice = totalPrice,
                NgayDat = DateTime.Now,
                TenChuyenXe = chuyenXe.TenChuyenXe,
                PaymentMethod = paymentMethod
            };

            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var serializedBooking = JsonSerializer.Serialize(pendingBooking, options);
                _logger.LogInformation("Serialized PendingBooking: {serializedBooking}", serializedBooking);
                HttpContext.Session.SetString("PendingBooking", serializedBooking);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi serialize PendingBooking: {message}", ex.Message);
                TempData["Error"] = "Lỗi khi lưu thông tin đặt vé. Vui lòng thử lại.";
                return RedirectToAction("ChonGhe", new { maChuyen });
            }

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
                return RedirectToAction("ChonGhe", new { maChuyen });
            }
        }

        // GET: Home/PaymentCallback
        [HttpGet]
        public async Task<IActionResult> PaymentCallback()
        {
            var restrictResult = RestrictAdminAccess();
            if (restrictResult != null) return restrictResult;

            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;

            var response = _vnpayService.PaymentExecute(Request.Query);
            return await ProcessPaymentCallback(response, "VNPay");
        }

        // GET: Home/MoMoPaymentCallback
        [HttpGet]
        public async Task<IActionResult> MoMoPaymentCallback()
        {
            var restrictResult = RestrictAdminAccess();
            if (restrictResult != null) return restrictResult;

            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;

            var response = _moMoService.PaymentExecute(Request.Query);
            return await ProcessPaymentCallback(response, "MoMo");
        }

        // Xử lý callback thanh toán chung cho VNPay/MoMo
        private async Task<IActionResult> ProcessPaymentCallback(PaymentResponse response, string paymentMethod)
        {
            var pendingBookingJson = HttpContext.Session.GetString("PendingBooking");
            if (string.IsNullOrEmpty(pendingBookingJson))
            {
                _logger.LogWarning("PendingBookingJson is empty or null.");
                TempData["ThongBao"] = "Phiên đặt vé đã hết hạn hoặc không tồn tại.";
                return View("PaymentCallback", response);
            }

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

            //// Kiểm tra lại trạng thái ghế từ Vitrighe
            //var gheDaDat = db.Vitrighes
            //    .Where(g => pendingBooking.SeatIds.Contains(g.IdVitri) && g.Trangthai == true)
            //    .Select(g => g.Tenvitri)
            //    .ToList();

            //if (gheDaDat.Any())
            //{
            //    HttpContext.Session.Remove("PendingBooking");
            //    TempData["ThongBao"] = $"Các ghế {string.Join(", ", gheDaDat)} đã được đặt bởi người khác.";
            //    return View("PaymentCallback", response);
            //}

            try
            {
                if (response.Success)
                {
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

                        // Cập nhật Vitrighe.Trangthai = true
                        var vitriGhe = db.Vitrighes.FirstOrDefault(g => g.IdVitri == seatId);
                        if (vitriGhe != null)
                        {
                            vitriGhe.Trangthai = true;
                        }
                    }

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
                                            string emailBody = @"
                      <div style='font-family: Arial, sans-serif; max-width: 700px; margin: auto; border: 1px solid #ccc; padding: 20px;'>
                        <h3 style='color: red; border-bottom: 1px solid #eee; padding-bottom: 10px;'>
                            📞 Thông tin liên hệ với nhà xe Khánh An
                        </h3>
                      
                        📧 Email: <a href='mailto:nhuocan1403@gmail.com'>nhuocan1403@gmail.com</a><br>
                        📱 SĐT: +84908569027</p>

                        <hr style='margin: 20px 0;'>
                         <h3>🚌 Cảm ơn bạn đã đặt vé xe của chúng tôi!!!</h3>
                        <h3>🚌 Thông tin chuyến xe:</h3>
                        <p><strong>" + chuyenXe?.MaTuyenNavigation?.DiemDi + " → " + chuyenXe?.MaTuyenNavigation?.DiemDen + @"</strong> &nbsp;&nbsp;<br>
                        💰 Giá vé: " + chuyenXe?.GiaVe?.ToString("N0") + @" VND &nbsp;&nbsp;<br>
                        👥 Số lượng vé: " + pendingBooking.SeatIds.Count + @"</p><br>

                        <p>⏰ Giờ: " + chuyenXe?.ThoiDiemKhoiHanh?.ToString("HH:mm") + @"<br>
                        📅 Ngày khởi hành: " + chuyenXe?.ThoiDiemKhoiHanh?.ToString("dd/MM/yyyy") + @"<br>
                        💺 Số ghế: " + string.Join(", ", seatNames) + @"<br>
                        💳 Tổng tiền vé: " + pendingBooking.TotalPrice.ToString("N0") + @" VND<br>
                        🚍 Biển số xe: " + chuyenXe.BienSoXe + @"</p><br>
                        👥 Ghi chú: " + chuyenXe.GhiChu + @"</p>

                        <hr style='margin: 20px 0;'>

                        <table style='width: 100%; font-size: 14px;'>
                            <tr>
                                <td style='text-align: right;'>Thành tiền:</td>
                                <td style='text-align: right; width: 150px;'>" + pendingBooking.TotalPrice.ToString("N0") + @" VND</td>
                            </tr>
                           
                            <tr>
                                <td style='text-align: right; font-weight: bold;'>Tổng cộng:</td>
                                <td style='text-align: right; color: red; font-weight: bold;'>" + pendingBooking.TotalPrice.ToString("N0") + @" VND</td>
                            </tr>
                        </table>

                        <p style='margin-top: 20px; font-style: italic;'>🔥 (Miễn phí nước uống, khăn lạnh, Wi-Fi, tivi)</p>
                    </div>";

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
                await db.SaveChangesAsync();
                HttpContext.Session.Remove("PendingBooking");
            }

            return View("PaymentCallback", response);
        }

        // POST: Home/MoMoNotify
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

            // Kiểm tra trạng thái ghế từ Vitrighe
            var gheDaDat = db.Vitrighes
                .Where(g => pendingBooking.SeatIds.Contains(g.IdVitri) && g.Trangthai == true)
                .Select(g => g.Tenvitri)
                .ToList();

            if (gheDaDat.Any())
            {
                _logger.LogWarning("MoMo Notify: One or more seats are already booked.");
                return Ok();
            }

            try
            {
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

                    // Cập nhật Vitrighe.Trangthai = true
                    var vitriGhe = db.Vitrighes.FirstOrDefault(g => g.IdVitri == seatId);
                    if (vitriGhe != null)
                    {
                        vitriGhe.Trangthai = true;
                    }
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
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}