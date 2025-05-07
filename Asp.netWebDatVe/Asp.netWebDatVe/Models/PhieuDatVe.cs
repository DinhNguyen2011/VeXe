using System;
using System.Collections.Generic;

namespace Asp.netWebDatVe.Models
{
    public partial class PhieuDatVe
    {
        public PhieuDatVe()
        {
            VeXes = new HashSet<VeXe>();
        }

        public int MaPhieu { get; set; }
        public string? Email { get; set; }
        public DateTime? NgayDat { get; set; }
        public decimal? TongTien { get; set; }
        public string? TrangThai { get; set; }
        public string? VnpTransactionId { get; set; }

        public virtual ICollection<VeXe> VeXes { get; set; }
    }
}
