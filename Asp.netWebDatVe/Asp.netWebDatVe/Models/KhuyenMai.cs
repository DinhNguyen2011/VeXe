using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Asp.netWebDatVe.Models
{
    public partial class KhuyenMai
    {
        public KhuyenMai()
        {
            PhieuDatVes = new HashSet<PhieuDatVe>();
        }

        public int MaKhuyenMai { get; set; }
        [Required(ErrorMessage = "Tên khuyến mãi không được để trống")]
        [Display(Name = "Tên khuyến mãi")]
        [StringLength(100, ErrorMessage = "Tên khuyến mãi không được vượt quá 100 ký tự")]
        public string TenKhuyenMai { get; set; } = null!;

        [Display(Name = "Mô tả")]
        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string? MoTa { get; set; }

        [Display(Name = "Phần trăm giảm")]
        [Range(0, 100, ErrorMessage = "Phần trăm giảm phải nằm trong khoảng từ 0 đến 100")]
        public decimal PhanTramGiam { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Ngày bắt đầu không được để trống")]
        public DateTime NgayBatDau { get; set; }

        [Display(Name = "Ngày kết thúc")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Ngày kết thúc không được để trống")]
        public DateTime NgayKetThuc { get; set; }

        public virtual ICollection<PhieuDatVe> PhieuDatVes { get; set; }
    }
}
