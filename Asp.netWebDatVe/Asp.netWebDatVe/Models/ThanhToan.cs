using System;
using System.Collections.Generic;

namespace Asp.netWebDatVe.Models
{
    public partial class ThanhToan
    {
        public int MaThanhToan { get; set; }
        public int MaPhieu { get; set; }
        public string PhuongThuc { get; set; } = null!;
        public decimal SoTien { get; set; }
        public DateTime NgayThanhToan { get; set; }
        public string? MaGiaoDich { get; set; }
        public string TrangThai { get; set; } = null!;

        public virtual PhieuDatVe MaPhieuNavigation { get; set; } = null!;
    }
}
