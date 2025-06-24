using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        [DisplayName("Mã Bến Xe")]
        public int MaBenXe { get; set; }

        [Required(ErrorMessage = "Tên bến xe không được để trống")]
        [DisplayName("Tên Bến Xe")]
        public string TenBenXe { get; set; } = null!;

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        [DisplayName("Địa Chỉ")]
        public string DiaChi { get; set; } = null!;

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [DisplayName("Số Điện Thoại")]
        public string Sdt { get; set; } = null!;

        [DisplayName("Thành Phố")]
        public string? ThanhPho { get; set; }

        public virtual ICollection<TuyenXe> TuyenXeMaBenXeDenNavigations { get; set; }
        public virtual ICollection<TuyenXe> TuyenXeMaBenXeDiNavigations { get; set; }
    }
}
