using System;
using System.Collections.Generic;

namespace Asp.netWebDatVe.Models
{
    public partial class TuyenXe
    {
        public TuyenXe()
        {
            ChuyenXes = new HashSet<ChuyenXe>();
        }

        public int MaTuyen { get; set; }
        public string DiemDi { get; set; } = null!;
        public string DiemDen { get; set; } = null!;
        public int? SoNgayChayTrongTuan { get; set; }
        public decimal? GiaHienHanh { get; set; }
        public int? QuangDuong { get; set; }
        public int? MaBenXe { get; set; }
        public int? MaBenXeDen { get; set; }

        public virtual BenXeDen? MaBenXeDenNavigation { get; set; }
        public virtual BenXe? MaBenXeNavigation { get; set; }
        public virtual ICollection<ChuyenXe> ChuyenXes { get; set; }
    }
}
