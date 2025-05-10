using System;
using System.Collections.Generic;

namespace Asp.netWebDatVe.Models
{
    public partial class PhieuDatVe
    {
        public PhieuDatVe()
        {
            ThanhToans = new HashSet<ThanhToan>();
            VeXes = new HashSet<VeXe>();
        }

        public int MaPhieu { get; set; }
        public string? Email { get; set; }
        public DateTime? NgayDat { get; set; }
        public decimal? TongTien { get; set; }
        public string? TrangThai { get; set; }
        public string? VnpTransactionId { get; set; }
        public int? MaKhuyenMai { get; set; }

        public virtual KhuyenMai? MaKhuyenMaiNavigation { get; set; }
        public virtual ICollection<ThanhToan> ThanhToans { get; set; }
        public virtual ICollection<VeXe> VeXes { get; set; }
    }
}
