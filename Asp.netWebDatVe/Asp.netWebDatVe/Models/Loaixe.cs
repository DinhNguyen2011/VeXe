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
        [Required(ErrorMessage = "Vui lòng nhập mã loại.")]

        public int IdLoai { get; set; }

        [DisplayName("Tên Loại Xe")]
        [Required(ErrorMessage = "Vui lòng nhập tên loại xe.")]
        public string? Tenloai { get; set; }

        [DisplayName("Số Ghế")]
        [Required(ErrorMessage = "Vui lòng nhập số ghế.")]
        public int Soghe { get; set; }


        public virtual ICollection<Xe> Xes { get; set; }
    }
}
