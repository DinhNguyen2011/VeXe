using System;
using System.Collections.Generic;
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

        public int MaNhanVien { get; set; }
        [Display(Name = "Họ tên")]
        [Required(ErrorMessage = "Họ tên không được để trống")]
        [StringLength(100, ErrorMessage = "Họ tên không được vượt quá 100 ký tự")]
        public string HoTen { get; set; } = null!;

        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(15, ErrorMessage = "Số điện thoại không được vượt quá 15 ký tự")]
        public string Sdt { get; set; } = null!;

        [Display(Name = "Địa chỉ")]
        [StringLength(200, ErrorMessage = "Địa chỉ không được vượt quá 200 ký tự")]
        public string? DiaChi { get; set; }

        [Display(Name = "Vai trò")]
        [Required(ErrorMessage = "Vai trò không được để trống")]
        [StringLength(50, ErrorMessage = "Vai trò không được vượt quá 50 ký tự")]
        public string VaiTro { get; set; } = null!;

        [Display(Name = "CCCD")]
        [Range(100000000000, 9999999999999, ErrorMessage = "CCCD phải gồm 12 đến 13 chữ số")]
        public long? Cccd { get; set; }

        public virtual ICollection<ChuyenXe> ChuyenXeMaNhanVien1Navigations { get; set; }
        public virtual ICollection<ChuyenXe> ChuyenXeMaNhanVienNavigations { get; set; }
        public virtual ICollection<ChuyenXe> ChuyenXeMaTaiXeNavigations { get; set; }
    }
}
