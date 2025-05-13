using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Asp.netWebDatVe.Models
{
    public partial class PhanQuyen
    {
        public PhanQuyen()
        {
            NguoiDungs = new HashSet<NguoiDung>();
        }

        public int MaQuyen { get; set; }
        [Display(Name = "Tên quyền")]
        [Required(ErrorMessage = "Tên quyền không được để trống")]
        [StringLength(50, ErrorMessage = "Tên quyền không được vượt quá 50 ký tự")]
        public string? TenQuyen { get; set; }
        public virtual ICollection<NguoiDung> NguoiDungs { get; set; }
    }
}
