using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Asp.netWebDatVe.Models
{
    public partial class BenXe
    {
        public BenXe()
        {
            TuyenXeMaBenXeDenNavigations = new HashSet<TuyenXe>();
            TuyenXeMaBenXeDiNavigations = new HashSet<TuyenXe>();
        }
        [Display(Name = "Mã bến xe")]

        public int MaBenXe { get; set; }
        [Required(ErrorMessage = "Tên bến xe không được để trống")]
        [Display(Name = "Tên bến xe")]
        public string TenBenXe { get; set; } = null!;

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        [Display(Name = "Địa chỉ")]
        public string DiaChi { get; set; } = null!;

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Display(Name = "Số điện thoại")]
        public string Sdt { get; set; } = null!;

        [Display(Name = "Thành phố")]
        public string? ThanhPho { get; set; }

        public virtual ICollection<TuyenXe> TuyenXeMaBenXeDenNavigations { get; set; }
        public virtual ICollection<TuyenXe> TuyenXeMaBenXeDiNavigations { get; set; }
    }
}
