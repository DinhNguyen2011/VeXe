using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Asp.netWebDatVe.Models
{
    public partial class Loaixe
    {
        public Loaixe()
        {
            Xes = new HashSet<Xe>();
        }

        [DisplayName("Mã Loại Xe")]
        public int IdLoai { get; set; }

        [DisplayName("Tên Loại Xe")]
        public string? Tenloai { get; set; }

        [DisplayName("Số Ghế")]
        public int Soghe { get; set; }


        public virtual ICollection<Xe> Xes { get; set; }
    }
}
