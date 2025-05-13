using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Asp.netWebDatVe.Models
{
    public partial class Loaixe
    {
        public Loaixe()
        {
            Xes = new HashSet<Xe>();
        }

        public int IdLoai { get; set; }
        [Display(Name = "Tên loại xe")]
        [Required(ErrorMessage = "Tên loại xe không được để trống")]
        [StringLength(100, ErrorMessage = "Tên loại xe không được vượt quá 100 ký tự")]
        public string? Tenloai { get; set; }

        [Display(Name = "Số ghế")]
        [Required(ErrorMessage = "Số ghế không được để trống")]
        [Range(1, 100, ErrorMessage = "Số ghế phải nằm trong khoảng từ 1 đến 100")]
        public int Soghe { get; set; }

        public virtual ICollection<Xe> Xes { get; set; }
    }
}
