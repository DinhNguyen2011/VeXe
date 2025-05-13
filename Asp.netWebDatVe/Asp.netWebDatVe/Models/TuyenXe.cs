using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Asp.netWebDatVe.Models
{
    public partial class TuyenXe
    {
        public TuyenXe()
        {
            ChuyenXes = new HashSet<ChuyenXe>();
        }
        [Display(Name = "Mã tuyến")]
        public int MaTuyen { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập điểm đi")]
        [Display(Name = "Điểm đi")]
        public string DiemDi { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập điểm đến")]
        [Display(Name = "Điểm đến")]
        public string DiemDen { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập số ngày chạy trong tuần")]
        [Display(Name = "Số ngày chạy trong tuần")]
        public int? SoNgayChayTrongTuan { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập giá hiện hành")]
        [Display(Name = "Giá hiện hành")]
        public decimal? GiaHienHanh { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập quãng đường")]
        [Display(Name = "Quãng đường (km)")]
        public int? QuangDuong { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn bến xe đi")]
        [Display(Name = "Bến xe đi")]
        public int MaBenXeDi { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn bến xe đến")]
        [Display(Name = "Bến xe đến")]
        public int MaBenXeDen { get; set; }
        [ValidateNever]
        public virtual BenXe MaBenXeDenNavigation { get; set; } = null!;
        [ValidateNever]
        public virtual BenXe MaBenXeDiNavigation { get; set; } = null!;
        public virtual ICollection<ChuyenXe> ChuyenXes { get; set; }
    }
}
