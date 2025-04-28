using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Asp.netWebDatVe.Controllers
{
    public class AccountController : Controller
    {
        private readonly QLDatVeContext _context;

        public AccountController(QLDatVeContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(NguoiDung model)
        {
            var user = _context.NguoiDungs
                .FirstOrDefault(u => u.Email == model.Email && u.MatKhau == model.MatKhau);

            if (user != null)
            {
                HttpContext.Session.SetString("UserInfo", JsonConvert.SerializeObject(user));
                HttpContext.Session.SetString("UserName", user.HoTen ?? user.Email);
                HttpContext.Session.SetInt32("UserId", user.Id);

                TempData["Success"] = "Đăng nhập thành công!";

                if (user.MaQuyen == 1)
                {
                    return RedirectToAction("Index", "HomeAdmin");
                }
                else if (user.MaQuyen == 3)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.Error = "Email hoặc mật khẩu không chính xác!";
            return View();
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(NguoiDung model)
        {
            if (!ModelState.IsValid) return View();

            if (_context.NguoiDungs.Any(u => u.Email == model.Email))
            {
                ViewBag.Error = "Email đã được sử dụng!";
                return View();
            }

            model.MaQuyen = 3;
            _context.NguoiDungs.Add(model);
            _context.SaveChanges();

            TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult Profile()
        {
            var userJson = HttpContext.Session.GetString("UserInfo");
            if (userJson == null) return RedirectToAction("Login");

            var user = JsonConvert.DeserializeObject<NguoiDung>(userJson);
            ViewData["UserName"] = HttpContext.Session.GetString("UserName");
            return View(user);
        }

        [HttpGet]
        public IActionResult EditProfile(int id)
        {
            var user = _context.NguoiDungs.FirstOrDefault(u => u.Id == id);
            if (user == null) return NotFound();

            ViewData["UserName"] = HttpContext.Session.GetString("UserName");
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProfile(NguoiDung user)
        {
            if (!ModelState.IsValid) return View(user);

            var currentUser = _context.NguoiDungs.FirstOrDefault(u => u.Id == user.Id);
            if (currentUser == null) return NotFound();

            // Cập nhật thông tin
            currentUser.HoTen = user.HoTen;
            currentUser.Email = user.Email;
            currentUser.Sdt = user.Sdt;
            currentUser.MatKhau = user.MatKhau;
            currentUser.NgaySinh = user.NgaySinh;
            currentUser.DiaChi = user.DiaChi;
            currentUser.HinhAnh = user.HinhAnh;

            _context.SaveChanges();
            HttpContext.Session.Clear();
            TempData["Success"] = "Cập nhật thành công! Vui lòng đăng nhập lại.";

            return RedirectToAction("Login");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
