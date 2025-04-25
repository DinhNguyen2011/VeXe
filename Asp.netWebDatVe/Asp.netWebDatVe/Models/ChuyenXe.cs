using System;
using System.Collections.Generic;

namespace Asp.netWebDatVe.Models
{
    public partial class ChuyenXe
    {
        public ChuyenXe()
        {
            VeXes = new HashSet<VeXe>();
        }

        public int MaChuyen { get; set; }
        public int? MaTuyen { get; set; }
        public DateTime? ThoiDiemKhoiHanh { get; set; }
        public DateTime? ThoiDiemDenDuKien { get; set; }
        public decimal? GiaVe { get; set; }
        public string? BienSoXe { get; set; }
        public string? TenChuyenXe { get; set; }

        public virtual Xe? BienSoXeNavigation { get; set; }
        public virtual TuyenXe? MaTuyenNavigation { get; set; }
        public virtual ICollection<VeXe> VeXes { get; set; }
    }
}
