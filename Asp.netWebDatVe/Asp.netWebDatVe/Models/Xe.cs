using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Asp.netWebDatVe.Models
{
    public partial class Xe
    {
        public Xe()
        {
            ChuyenXes = new HashSet<ChuyenXe>();
            Vitrighes = new HashSet<Vitrighe>();
        }

        [Display(Name = "Biển Số Xe")]
        [Required(ErrorMessage = "Vui lòng nhập Biển số xe")]
        public string Bienso { get; set; } = null!;

        [Display(Name = "Loại Xe")]
        [Required(ErrorMessage = "Vui lòng chọn Loại xe")]
        public int IdLoai { get; set; }

        [Display(Name = "Tên Xe")]
        [Required(ErrorMessage = "Vui lòng nhập Tên xe")]
        public string? Tenxe { get; set; }

        [Display(Name = "Hình Ảnh")]
        public string? HinhAnh { get; set; } 

        public virtual Loaixe IdLoaiNavigation { get; set; } = null!;
        public virtual ICollection<ChuyenXe> ChuyenXes { get; set; }
        public virtual ICollection<Vitrighe> Vitrighes { get; set; }
    }
}
