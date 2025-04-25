using System;
using System.Collections.Generic;

namespace Asp.netWebDatVe.Models
{
    public partial class PhanQuyen
    {
        public PhanQuyen()
        {
            NguoiDungs = new HashSet<NguoiDung>();
        }

        public int MaQuyen { get; set; }
        public string? TenQuyen { get; set; }

        public virtual ICollection<NguoiDung> NguoiDungs { get; set; }
    }
}
