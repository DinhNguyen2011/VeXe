using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Asp.netWebDatVe.Models
{
    public partial class NguoiDung
    {
        [DisplayName("Mã Người Dùng")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Email.")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
        [DisplayName("Email")]
        public string Email { get; set; } = null!;

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [DisplayName("Số Điện Thoại")]
        public string? Sdt { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Họ tên.")]
        [DisplayName("Họ và Tên")]
        public string? HoTen { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập Mật khẩu.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        [DisplayName("Mật Khẩu")]
        public string MatKhau { get; set; } = null!;

        [DisplayName("Ngày Sinh")]
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }

        [DisplayName("Địa Chỉ")]
        public string? DiaChi { get; set; }

        [DisplayName("Mã Quyền")]
        public int? MaQuyen { get; set; }

        [DisplayName("Hình Ảnh")]
        public string? HinhAnh { get; set; }

        [DisplayName("Chú Thích")]
        public string? ChuThich { get; set; }

        public virtual PhanQuyen? MaQuyenNavigation { get; set; }
    }
}
