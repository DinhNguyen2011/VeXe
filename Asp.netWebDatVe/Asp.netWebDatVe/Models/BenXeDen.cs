using System;
using System.Collections.Generic;

namespace Asp.netWebDatVe.Models
{
    public partial class BenXeDen
    {
        public BenXeDen()
        {
            TuyenXes = new HashSet<TuyenXe>();
        }

        public int MaBenXeDen { get; set; }
        public string? TenBenXeDen { get; set; }
        public string? DiaChi { get; set; }
        public string? Sdt { get; set; }

        public virtual ICollection<TuyenXe> TuyenXes { get; set; }
    }
}
