using System;
using System.Collections.Generic;

namespace Asp.netWebDatVe.Models
{
    public partial class NguoiDung
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string? Sdt { get; set; }
        public string? HoTen { get; set; }
        public string MatKhau { get; set; } = null!;
        public DateTime? NgaySinh { get; set; }
        public string? DiaChi { get; set; }
        public int? MaQuyen { get; set; }
        public string? HinhAnh { get; set; }

        public virtual PhanQuyen? MaQuyenNavigation { get; set; }
    }
}
