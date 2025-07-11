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

        [Required(ErrorMessage = "Vui lòng nhập Email.")]
        [RegularExpression(@"^[\w\.\-]+@([\w\-]+\.)+[a-zA-Z]{2,4}$", ErrorMessage = "Email không hợp lệ.")]
        [DisplayName("Email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Nội dung không được để trống")]
        [DisplayName("Nội Dung")]
        public string NoiDung { get; set; } = null!;

        [DisplayName("Ngày Gửi")]
        public DateTime? NgayGui { get; set; }


        [Required(ErrorMessage = "Vui lòng nhập Số điện thoại.")]
        [DisplayName("Số Điện Thoại")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại phải đủ 10 chữ số.")]
        [RegularExpression(@"^(0[3|5|7|8|9])+([0-9]{8})$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string Sdt { get; set; } = null!;
    }
}
