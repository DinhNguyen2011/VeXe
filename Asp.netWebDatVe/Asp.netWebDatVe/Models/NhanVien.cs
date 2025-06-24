using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Asp.netWebDatVe.Models
{
    public partial class NhanVien
    {
        public NhanVien()
        {
            ChuyenXeMaNhanVien1Navigations = new HashSet<ChuyenXe>();
            ChuyenXeMaNhanVienNavigations = new HashSet<ChuyenXe>();
            ChuyenXeMaTaiXeNavigations = new HashSet<ChuyenXe>();
        }

        [DisplayName("Mã Nhân Viên")]
        public int MaNhanVien { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Họ và Tên.")]
        [DisplayName("Họ và Tên")]
        public string HoTen { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập Số điện thoại.")]
        [DisplayName("Số Điện Thoại")]
        public string Sdt { get; set; } = null!;

        [DisplayName("Địa Chỉ")]
        public string? DiaChi { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Vai trò.")]
        [DisplayName("Vai Trò")]
        public string VaiTro { get; set; } = null!;

        [DisplayName("CCCD")]
        public long? Cccd { get; set; }

        [DisplayName("Hình Ảnh")]
        public string? HinhAnh { get; set; }

        public virtual ICollection<ChuyenXe> ChuyenXeMaNhanVien1Navigations { get; set; }
        public virtual ICollection<ChuyenXe> ChuyenXeMaNhanVienNavigations { get; set; }
        public virtual ICollection<ChuyenXe> ChuyenXeMaTaiXeNavigations { get; set; }
    }
}
