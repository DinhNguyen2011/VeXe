using Asp.netWebDatVe.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Asp.netWebDatVe.Controllers
{
    [Authorize(Roles = "1,2")]
    public class HomeAdminController : Controller
    {
        private readonly QLDatVeContext _context;

        public HomeAdminController(QLDatVeContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewData["UserName"] = User.Identity.Name;

            var tongChuyenXe = _context.ChuyenXes.Count();
            var tongXe = _context.Xes.Count();
            var tongLoaiXe = _context.Loaixes.Count();
            var tongNhanVien = _context.NhanViens.Count();
            var tongKhachHang = _context.NguoiDungs.Count(u => u.MaQuyen == 3);
            var doanhThuThang = _context.PhieuDatVes
            .Where(p => p.TrangThai != "Đã hủy" && p.NgayDat.HasValue && p.NgayDat.Value.Month == DateTime.Now.Month)
            .Sum(p => (decimal?)p.TongTien) ?? 0;

            ViewBag.TongChuyenXe = tongChuyenXe;
            ViewBag.TongXe = tongXe;
            ViewBag.TongLoaiXe = tongLoaiXe;
            ViewBag.TongNhanVien = tongNhanVien;
            ViewBag.TongKhachHang = tongKhachHang;
            ViewBag.DoanhThuThang = doanhThuThang.ToString("N0") + " VNĐ";

            // Doanh thu từng tháng trong năm
            var doanhThuTheoThang = Enumerable.Range(1, 12).Select(month => new
            {
                Thang = month,
                Tong = _context.PhieuDatVes
        .Where(p => p.NgayDat.HasValue && p.NgayDat.Value.Month == month && p.NgayDat.Value.Year == DateTime.Now.Year && p.TrangThai != "Đã hủy")
        .Sum(p => (decimal?)p.TongTien) ?? 0
            }).ToList();

            ViewBag.LabelsThang = string.Join(",", doanhThuTheoThang.Select(x => $"\"Tháng {x.Thang}\""));
            ViewBag.DataThang = string.Join(",", doanhThuTheoThang.Select(x => x.Tong));

            // Phân bố chuyến xe theo loại xe
            var chuyenTheoLoai = _context.Loaixes.Select(l => new
            {
                TenLoai = l.Tenloai,
                SoLuong = l.Xes.SelectMany(x => x.ChuyenXes).Count()
            }).ToList();

            ViewBag.LabelsLoai = string.Join(",", chuyenTheoLoai.Select(x => $"\"{x.TenLoai}\""));
            ViewBag.DataLoai = string.Join(",", chuyenTheoLoai.Select(x => x.SoLuong));

            return View();
        }

    }
}
