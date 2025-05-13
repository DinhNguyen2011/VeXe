using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Asp.netWebDatVe.Models
{
    public partial class Xe
    {
        public Xe()
        {
            ChuyenXes = new HashSet<ChuyenXe>();
            Vitrighes = new HashSet<Vitrighe>();
        }

        [DisplayName("Biển Số Xe")]
        public string Bienso { get; set; } = null!;

        [DisplayName("Loại Xe")]
        public int IdLoai { get; set; }

        [DisplayName("Tên Xe")]
        public string? Tenxe { get; set; }

        [DisplayName("Hình Ảnh Xe")]
        public string? HinhAnh { get; set; }

        public virtual Loaixe IdLoaiNavigation { get; set; } = null!;
        public virtual ICollection<ChuyenXe> ChuyenXes { get; set; }
        public virtual ICollection<Vitrighe> Vitrighes { get; set; }
    }
}
