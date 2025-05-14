using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Asp.netWebDatVe.Models
{
    public partial class NguoiDung
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Email theo đúng định dạng.")]
        public string Email { get; set; } = null!;
        public string? Sdt { get; set; }
        public string? HoTen { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]

        public string MatKhau { get; set; } = null!;
        public DateTime? NgaySinh { get; set; }
        public string? DiaChi { get; set; }
        public int? MaQuyen { get; set; }
        public string? HinhAnh { get; set; }
        public string? ChuThich { get; set; }

        public virtual PhanQuyen? MaQuyenNavigation { get; set; }
    }
}
