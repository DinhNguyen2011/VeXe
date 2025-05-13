using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Asp.netWebDatVe.Models
{
    public partial class LienHe
    {
        [DisplayName("Mã Liên Hệ")]
        public int Id { get; set; }

        [DisplayName("Họ và Tên")]
        public string HoVaTen { get; set; } = null!;

        [DisplayName("Email")]
        public string Email { get; set; } = null!;

        [DisplayName("Nội Dung")]
        public string NoiDung { get; set; } = null!;

        [DisplayName("Ngày Gửi")]
        public DateTime? NgayGui { get; set; }

        [DisplayName("Số Điện Thoại")]
        public string? Sdt { get; set; }
    }
}
