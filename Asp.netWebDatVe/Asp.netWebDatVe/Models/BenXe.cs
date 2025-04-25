using System;
using System.Collections.Generic;

namespace Asp.netWebDatVe.Models
{
    public partial class BenXe
    {
        public BenXe()
        {
            TuyenXes = new HashSet<TuyenXe>();
        }

        public int MaBenXe { get; set; }
        public string? TenBenXe { get; set; }
        public string? DiaChi { get; set; }
        public string? Sdt { get; set; }

        public virtual ICollection<TuyenXe> TuyenXes { get; set; }
    }
}
