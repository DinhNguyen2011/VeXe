using System;
using System.Collections.Generic;

namespace Asp.netWebDatVe.Models
{
    public partial class KhuyenMai
    {
        public KhuyenMai()
        {
            PhieuDatVes = new HashSet<PhieuDatVe>();
        }

        public int MaKhuyenMai { get; set; }
        public string TenKhuyenMai { get; set; } = null!;
        public string? MoTa { get; set; }
        public decimal PhanTramGiam { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }

        public virtual ICollection<PhieuDatVe> PhieuDatVes { get; set; }
    }
}
