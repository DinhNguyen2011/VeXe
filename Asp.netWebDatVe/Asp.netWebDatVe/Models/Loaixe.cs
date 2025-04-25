using System;
using System.Collections.Generic;

namespace Asp.netWebDatVe.Models
{
    public partial class Loaixe
    {
        public Loaixe()
        {
            Xes = new HashSet<Xe>();
        }

        public int IdLoai { get; set; }
        public string? Tenloai { get; set; }
        public int Soghe { get; set; }

        public virtual ICollection<Xe> Xes { get; set; }
    }
}
