using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Asp.netWebDatVe.Models
{
    public partial class LienHe
    {
        [DisplayName("Mã Liên Hệ")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [DisplayName("Họ và Tên")]
        public string HoVaTen { get; set; } = null!;

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [DisplayName("Email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Nội dung không được để trống")]
        [DisplayName("Nội Dung")]
        public string NoiDung { get; set; } = null!;

        [DisplayName("Ngày Gửi")]
        public DateTime? NgayGui { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [DisplayName("Số Điện Thoại")]
        public string? Sdt { get; set; }
    }
}
