using System;
using System.Collections.Generic;

namespace Asp.netWebDatVe.Models
{
    public partial class Xe
    {
        public Xe()
        {
            ChuyenXes = new HashSet<ChuyenXe>();
            Vitrighes = new HashSet<Vitrighe>();
        }

        public string Bienso { get; set; } = null!;
        public int IdLoai { get; set; }
        public string? Tenxe { get; set; }
        public string? HinhAnh { get; set; }

        public virtual Loaixe IdLoaiNavigation { get; set; } = null!;
        public virtual ICollection<ChuyenXe> ChuyenXes { get; set; }
        public virtual ICollection<Vitrighe> Vitrighes { get; set; }
    }
}
