using System;
using System.Collections.Generic;

namespace Asp.netWebDatVe.Models
{
    public partial class VeXe
    {
        public int MaVe { get; set; }
        public int? MaPhieu { get; set; }
        public int? MaChuyen { get; set; }
        public string? TenVe { get; set; }
        public string? TrangThai { get; set; }
        public string? GhiChu { get; set; }
        public string? TenKh { get; set; }
        public string? Email { get; set; }
        public int? IdVitri { get; set; }

        public virtual Vitrighe? IdVitriNavigation { get; set; }
        public virtual ChuyenXe? MaChuyenNavigation { get; set; }
        public virtual PhieuDatVe? MaPhieuNavigation { get; set; }
    }
}
