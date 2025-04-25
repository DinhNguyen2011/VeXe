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
            ViewBag.LoginCheck = null;
            return View();
        }

        [HttpPost]
        public IActionResult Login(NguoiDung model)
        {
            ViewBag.LoginCheck = false;

            var user = _context.NguoiDungs
                .FirstOrDefault(u => u.Email == model.Email && u.MatKhau == model.MatKhau);

            if (user != null)
            {
                string userJson = JsonConvert.SerializeObject(user);
                HttpContext.Session.SetString("UserInfo", userJson);
                HttpContext.Session.SetString("UserName", user.HoTen ?? user.Email);
                HttpContext.Session.SetInt32("UserId", user.Id);

                ViewBag.LoginCheck = true;
                TempData["Success"] = "Đăng nhập thành công!";

                if (user.MaQuyen == 1)
                {
                    //var cl = new List<Claim> {
                    //    new Claim(ClaimTypes.Name,"Admin"),
                    //    new  Claim(ClaimTypes.Role,"Admin")

                    //};
                    return RedirectToAction("Index", "HomeAdmin");
                    //HttpContext.Session.SetString("Role", "Admin");
                }
                else if (user.MaQuyen == 3)
                {
                    //var cl = new List<Claim> {
                    //    new Claim(ClaimTypes.Name,"Admin"),
                    //    new  Claim(ClaimTypes.Role,"Admin")

                    //};
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
            if (ModelState.IsValid)
            {
                var existingUser = _context.NguoiDungs.FirstOrDefault(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ViewBag.Error = "Email đã được sử dụng!";
                    return View();
                }

                var newUser = new NguoiDung
                {
                    HoTen = model.HoTen,
                    Email = model.Email,
                    Sdt = model.Sdt,
                    MatKhau = model.MatKhau,
                    NgaySinh = model.NgaySinh,
                    DiaChi = model.DiaChi,
                    HinhAnh = model.HinhAnh,
                    MaQuyen = 3
                };

                _context.NguoiDungs.Add(newUser);
                _context.SaveChanges();

                TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login");
            }

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }


        public IActionResult Profile()
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;

            var userJson = HttpContext.Session.GetString("UserInfo");
            if (userJson == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var user = JsonConvert.DeserializeObject<NguoiDung>(userJson);
            return View(user);
        }


        public IActionResult EditProfile(int id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            var user = _context.NguoiDungs.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProfile(NguoiDung user)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;


            if (ModelState.IsValid)
            {
                var currentUser = _context.NguoiDungs.FirstOrDefault(u => u.Id == user.Id);

                if (currentUser != null)
                {
                    currentUser.Email = user.Email;
                    currentUser.Sdt = user.Sdt;
                    currentUser.HoTen = user.HoTen;
                    currentUser.MatKhau = user.MatKhau;
                    currentUser.NgaySinh = user.NgaySinh;
                    currentUser.DiaChi = user.DiaChi;
                    currentUser.HinhAnh = user.HinhAnh;



                    _context.SaveChanges();


                    HttpContext.Session.Clear();

                    TempData["Success"] = "Cập nhật thông tin thành công! Vui lòng đăng nhập lại để sử dụng thông tin mới.";

                    return RedirectToAction("Index", "Home");
                }
            }


            return View(user);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
