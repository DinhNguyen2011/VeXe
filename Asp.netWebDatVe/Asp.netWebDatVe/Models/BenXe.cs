using System;
using System.Collections.Generic;

namespace Asp.netWebDatVe.Models
{
    public partial class BenXe
    {
        public BenXe()
        {
            TuyenXeMaBenXeDenNavigations = new HashSet<TuyenXe>();
            TuyenXeMaBenXeDiNavigations = new HashSet<TuyenXe>();
        }

        public int MaBenXe { get; set; }
        public string TenBenXe { get; set; } = null!;
        public string DiaChi { get; set; } = null!;
        public string Sdt { get; set; } = null!;
        public string? ThanhPho { get; set; }

        public virtual ICollection<TuyenXe> TuyenXeMaBenXeDenNavigations { get; set; }
        public virtual ICollection<TuyenXe> TuyenXeMaBenXeDiNavigations { get; set; }
    }
}
