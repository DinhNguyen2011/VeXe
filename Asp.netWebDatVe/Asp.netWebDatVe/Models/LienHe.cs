using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Asp.netWebDatVe.Models
{
    public partial class LienHe
    {
        public int Id { get; set; }

        [DisplayName("Họ và tên")]
        [Required(ErrorMessage = "Họ và tên là bắt buộc.")]
        [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự.")]
        public string HoVaTen { get; set; } = null!;

        [DisplayName("Email")]
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; } = null!;

        [DisplayName("Nội dung")]
        [Required(ErrorMessage = "Nội dung là bắt buộc.")]
        [StringLength(1000, ErrorMessage = "Nội dung không được vượt quá 1000 ký tự.")]
        public string NoiDung { get; set; } = null!;

        [DisplayName("Ngày gửi")]
        [DataType(DataType.DateTime)]
        public DateTime? NgayGui { get; set; }

        [DisplayName("Số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại phải có đúng 10 ký tự.")]
        [RegularExpression(@"^0[0-9]{9}$", ErrorMessage = "Số điện thoại phải bắt đầu bằng 0 và có đúng 10 chữ số.")]
        public string? Sdt { get; set; }
    }
}
