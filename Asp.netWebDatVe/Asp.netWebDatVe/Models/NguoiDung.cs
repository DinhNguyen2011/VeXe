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
        [RegularExpression(@"^[\w\.\-]+@([\w\-]+\.)+[a-zA-Z]{2,4}$", ErrorMessage = "Email không hợp lệ.")]

        [DisplayName("Email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập Số điện thoại.")]
        [DisplayName("Số Điện Thoại")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại phải đủ 10 chữ số.")]
        [RegularExpression(@"^(0[3|5|7|8|9])+([0-9]{8})$", ErrorMessage = "Số điện thoại không hợp lệ.")]
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
