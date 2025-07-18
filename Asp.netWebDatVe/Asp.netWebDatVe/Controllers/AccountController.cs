using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using BCrypt.Net;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

namespace Asp.netWebDatVe.Controllers
{
    public class AccountController : Controller
    {
        private readonly QLDatVeContext _context;
        public AccountController(QLDatVeContext context)
        {
            _context = context; 
        }
        //mã hóa chuỗi khi ng dùng dn
        private bool IsBCryptHash(string password)
        {           
            return password != null && password.StartsWith("$2") && password.Length >= 50;
        }

        /*----- Đăng nhập -> VIEW */
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(NguoiDung model, string returnUrl = null)
        {
            //Tìm người dùng trong database dựa trên email.
            var user = _context.NguoiDungs
                .FirstOrDefault(u => u.Email == model.Email);

            if (user == null)
            {
                ViewBag.Error = "Email hoặc mật khẩu không chính xác!";
                ViewData["ReturnUrl"] = returnUrl;
                return View();
            }

            bool isPasswordValid = false;
            bool needsPasswordUpdate = false;

            //Nếu mật khẩu trong database là BCrypt(IsBCryptHash trả về true), dùng BCrypt.Verify để so sánh mật khẩu người dùng nhập với mật khẩu mã hóa.
            //Nếu không phải BCrypt(mật khẩu dạng plaintext), so sánh trực tiếp và đánh dấu needsPasswordUpdate = true.
            if (IsBCryptHash(user.MatKhau))
            {
                isPasswordValid = BCrypt.Net.BCrypt.Verify(model.MatKhau, user.MatKhau);
            }
            else
            {
                isPasswordValid = user.MatKhau == model.MatKhau;
                needsPasswordUpdate = true;
            }

            if (!isPasswordValid)
            {
                ViewBag.Error = "Email hoặc mật khẩu không chính xác!";
                ViewData["ReturnUrl"] = returnUrl;
                return View();
            }

            //Mã hóa mật khẩu mới bằng BCrypt và lưu vào database.
            if (needsPasswordUpdate)
            {
                user.MatKhau = BCrypt.Net.BCrypt.HashPassword(model.MatKhau);
                _context.Update(user);
                await _context.SaveChangesAsync();
            }

            //tránh link truy câp
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.HoTen ?? user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),           
                new Claim(ClaimTypes.Role, user.MaQuyen.ToString() ?? "3")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
            {
                IsPersistent = false,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
            });

            HttpContext.Session.SetString("UserInfo", JsonConvert.SerializeObject(user));
            HttpContext.Session.SetString("UserName", user.HoTen ?? user.Email);
            HttpContext.Session.SetInt32("UserId", user.Id);

            TempData["Successs"] = "Đăng nhập!";

            // Xử lý ReturnUrl
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            //chuyển hướng quyền
            if (user.MaQuyen == 1 || user.MaQuyen == 2 )
            /*if (user.MaQuyen == 1 || user.MaQuyen == 2  || user.MaQuyen == 3)*/
            {
                return RedirectToAction("Index", "HomeAdmin");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        /*----- Đăng Ký -> VIEW */
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

            model.MatKhau = BCrypt.Net.BCrypt.HashPassword(model.MatKhau);
   
            model.MaQuyen = 3;
            _context.NguoiDungs.Add(model);
            _context.SaveChanges();

            TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("Login");
        }


        /*----- đăng  xuất */
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }



        /*----- Từ chối truy cập - VIEW */
        public IActionResult AccessDenied()
        {
            return View();
        }


        /*----- Thông Tin Người dùng-> VIEW */
        public IActionResult Profile()
        {
            var userJson = HttpContext.Session.GetString("UserInfo");
            if (userJson == null) return RedirectToAction("Login");

            var user = JsonConvert.DeserializeObject<NguoiDung>(userJson);
            ViewData["UserName"] = HttpContext.Session.GetString("UserName");     
            var tickets = _context.VeXes
                .Where(v => v.Email == user.Email)
                .Select(v => new
                {
                    v.MaVe,
                    v.TenVe,
                    v.NgayDat,
                    v.TrangThai
                })
                .ToList();
            ViewBag.Tickets = tickets;
            return View(user);
        }


        /*----- Sửa TT -> VIEW */
        [HttpGet]
        [Authorize]
        public IActionResult EditProfile(int id)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            // Lấy ID người dùng từ claims
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (userId != id)
            {
                return Unauthorized(); // Ngăn truy cập hồ sơ của người khác
            }

            var user = _context.NguoiDungs.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            ViewData["UserName"] = User.Identity.Name;
            return View(user);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(NguoiDung model, IFormFile? hinhAnh)
        {
            var userName = HttpContext.Session.GetString("UserName");
            ViewData["UserName"] = userName;
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            if (userId != model.Id)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                ViewData["UserName"] = User.Identity.Name;
                return View(model);
            }

            var user = _context.NguoiDungs.Find(model.Id);
            if (user == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin
            user.HoTen = model.HoTen;
            user.Sdt = model.Sdt;
            user.NgaySinh = model.NgaySinh;
            user.DiaChi = model.DiaChi;

            // Xử lý upload ảnh
            if (hinhAnh != null && hinhAnh.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(hinhAnh.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("hinhAnh", "Chỉ chấp nhận file ảnh (.jpg, .jpeg, .png, .gif).");
                    return View(model);
                }
                if (hinhAnh.Length > 2 * 1024 * 1024) 
                {
                    ModelState.AddModelError("hinhAnh", "File ảnh không được lớn hơn 2MB.");
                    return View(model);
                }

                var fileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await hinhAnh.CopyToAsync(stream);
                }
                user.HinhAnh = "/images/" + fileName;
            }

            _context.Update(user);
            await _context.SaveChangesAsync();

            // Cập nhật session
            HttpContext.Session.SetString("UserInfo", JsonConvert.SerializeObject(user));
            HttpContext.Session.SetString("UserName", user.HoTen ?? user.Email);

            TempData["Success"] = "Cập nhật hồ sơ thành công!";
            return RedirectToAction("Profile");
        }
        /*----- Đổi Mật Khẩu  -> VIEW */
        [HttpGet]
        public IActionResult ChangePassword()
        {
            var userJson = HttpContext.Session.GetString("UserInfo");
            if (userJson == null) return RedirectToAction("Login");

            ViewData["UserName"] = HttpContext.Session.GetString("UserName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(string oldPassword, string newPassword, string confirmPassword)
        {
            var userJson = HttpContext.Session.GetString("UserInfo");
            if (userJson == null) return RedirectToAction("Login");

            var user = JsonConvert.DeserializeObject<NguoiDung>(userJson);
            var currentUser = _context.NguoiDungs.FirstOrDefault(u => u.Id == user.Id);
            if (currentUser == null) return NotFound();

            // Kiểm tra mật khẩu cũ
            bool isPasswordValid = false;
            if (IsBCryptHash(currentUser.MatKhau))
            {
                isPasswordValid = BCrypt.Net.BCrypt.Verify(oldPassword, currentUser.MatKhau);
            }
            else
            {
                isPasswordValid = currentUser.MatKhau == oldPassword;
            }

            if (!isPasswordValid)
            {
                ViewBag.Error = "Mật khẩu cũ không chính xác!";
                ViewData["UserName"] = HttpContext.Session.GetString("UserName");
                return View();
            }

            // Kiểm tra mật khẩu xác nhận
            if (newPassword != confirmPassword)
            {
                ViewBag.Error = "Mật khẩu xác nhận không khớp!";
                ViewData["UserName"] = HttpContext.Session.GetString("UserName");
                return View();
            }

            // Kiểm tra định dạng mật khẩu mới
            if (newPassword.Length < 8 || !newPassword.Any(char.IsDigit) || !newPassword.Any(char.IsLetter))
            {
                ViewBag.Error = "Mật khẩu mới phải có ít nhất 8 ký tự, bao gồm cả chữ cái và số!";
                ViewData["UserName"] = HttpContext.Session.GetString("UserName");
                return View();
            }

            // Mã hóa mật khẩu mới bằng BCrypt
            currentUser.MatKhau = BCrypt.Net.BCrypt.HashPassword(newPassword);
            _context.Update(currentUser);
            _context.SaveChanges();

            // Xóa session và yêu cầu đăng nhập lại
            HttpContext.Session.Clear();
            TempData["Success"] = "Đổi mật khẩu thành công! Vui lòng đăng nhập lại.";
            return RedirectToAction("Login");
        }

        /*-----*/
        public IActionResult Index()
        {
            //ViewBag.accname = HttpContext.Session.GetString("UserName");//hiển thị tên                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             
            return View();
        }
    }
}