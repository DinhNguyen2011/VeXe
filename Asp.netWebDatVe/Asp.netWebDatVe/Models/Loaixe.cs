using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
        [Required(ErrorMessage = "Vui lòng nhập tên loại xe.")]
        public string? Tenloai { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số ghế.")]

        [DisplayName("Số Ghế")]
        public int Soghe { get; set; }


        public virtual ICollection<Xe> Xes { get; set; }
    }
}
