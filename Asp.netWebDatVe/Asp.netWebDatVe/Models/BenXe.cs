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

        [Required(ErrorMessage = "Vui lòng nhập Tên bến xe ")]
        [DisplayName("Tên Bến Xe")]
        public string TenBenXe { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập Địa chỉ")]
        [DisplayName("Địa Chỉ")]
        public string DiaChi { get; set; } = null!;

      

        [Required(ErrorMessage = "Vui lòng nhập Số điện thoại.")]
        [DisplayName("Số Điện Thoại")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Số điện thoại phải đủ 10 chữ số.")]
        [RegularExpression(@"^(0[3|5|7|8|9])+([0-9]{8})$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string Sdt { get; set; } = null!;

        [DisplayName("Thành Phố")]
        public string? ThanhPho { get; set; }

        public virtual ICollection<TuyenXe> TuyenXeMaBenXeDenNavigations { get; set; }
        public virtual ICollection<TuyenXe> TuyenXeMaBenXeDiNavigations { get; set; }
    }
}
