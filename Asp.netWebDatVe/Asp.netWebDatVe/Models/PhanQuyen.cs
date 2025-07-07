using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Asp.netWebDatVe.Models
{
    public partial class PhanQuyen
    {
        public PhanQuyen()
        {
            NguoiDungs = new HashSet<NguoiDung>();
        }
        [DisplayName("Mã Quyền")]
      

        public int MaQuyen { get; set; }

        [DisplayName("Tên Quyền")]
        [Required(ErrorMessage = "Vui lòng nhập tên Quyền.")]

        public string? TenQuyen { get; set; }
        public virtual ICollection<NguoiDung> NguoiDungs { get; set; }
    }
}
