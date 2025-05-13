using Asp.netWebDatVe.Models;
using Asp.netWebDatVe.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Asp.netWebDatVe.Controllers
{
 
    public class LienHeVoiChungToiController : Controller
    {
        private readonly QLDatVeContext db;
        private readonly ILogger<LienHeVoiChungToiController> _logger;
        private readonly IEmailService _emailService;

        public LienHeVoiChungToiController(QLDatVeContext context, ILogger<LienHeVoiChungToiController> logger, IEmailService emailService)
        {
            db = context;
            _logger = logger;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LienHe lienHe)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;

            if (ModelState.IsValid)
            {
                try
                {
                    lienHe.NgayGui = DateTime.Now;
                    db.LienHes.Add(lienHe);
                    db.SaveChanges();

                    // Gửi email xác nhận
                    try
                    {
                        string emailSubject = "Xác Nhận Gửi Liên Hệ Thành Công";
                        string emailBody = "<h2>Xác Nhận Liên Hệ</h2>" +
                                           $"<p>Xin chào {lienHe.HoVaTen},</p>" +
                                           "<p>Chúng tôi đã nhận được liên hệ của bạn. Cảm ơn bạn đã phản hồi:</p>" +
                                           "<ul>" +
                                           $"<li><strong>Họ và Tên:</strong> {lienHe.HoVaTen}</li>" +
                                           $"<li><strong>Email:</strong> {lienHe.Email}</li>" +
                                           $"<li><strong>Số Điện Thoại:</strong> {lienHe.Sdt ?? "Không cung cấp"}</li>" +
                                           $"<li><strong>Nội Dung:</strong> {lienHe.NoiDung}</li>" +
                                           $"<li><strong>Ngày Gửi:</strong> {lienHe.NgayGui?.ToString("HH:mm dd/MM/yyyy")}</li>" +
                                           "</ul>" +
                                           "<p>Chúng tôi sẽ phản hồi bạn trong thời gian sớm nhất. Cảm ơn bạn đã liên hệ với chúng tôi!</p>" +
                                           "<p>Trân trọng,<br>Hệ Thống Đặt Vé Xe Khánh An</p>";

                        await _emailService.SendEmailAsync(lienHe.Email, emailSubject, emailBody);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Lỗi khi gửi email xác nhận liên hệ cho {lienHe.Email}");
                        ViewBag.Message = "Gửi liên hệ thành công, nhưng không thể gửi email xác nhận.";
                        ModelState.Clear();
                        return View();
                    }

                    ViewBag.Message = "Gửi liên hệ thành công! Cảm ơn bạn đã liên hệ.";
                    ModelState.Clear();
                    return View();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi lưu liên hệ từ {Email}", lienHe.Email);
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi gửi liên hệ. Vui lòng thử lại sau.");
                }
            }

            return View(lienHe);
        }
        [Authorize(Roles = "1,2")]
        public IActionResult DanhSach()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;

            var danhSachLienHe = db.LienHes
                .OrderByDescending(x => x.NgayGui)
                .ToList();

            return View(danhSachLienHe);
        }
        [Authorize(Roles = "1,2")]
        [HttpPost]
        public IActionResult Xoa(int id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;

            var lienHe = db.LienHes.Find(id);
            if (lienHe != null)
            {
                try
                {
                    db.LienHes.Remove(lienHe);
                    db.SaveChanges();
                    TempData["Message"] = "Xóa liên hệ thành công!";
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi xóa liên hệ với ID {Id}", id);
                    TempData["Error"] = "Đã xảy ra lỗi khi xóa liên hệ.";
                }
            }
            else
            {
                TempData["Error"] = "Liên hệ không tồn tại.";
            }

            return RedirectToAction("DanhSach");
        }
    }
}