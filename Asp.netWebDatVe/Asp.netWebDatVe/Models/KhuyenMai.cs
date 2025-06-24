using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Asp.netWebDatVe.Models
{
    public partial class KhuyenMai
    {
        public KhuyenMai()
        {
            PhieuDatVes = new HashSet<PhieuDatVe>();
        }

        [DisplayName("Mã Khuyến Mãi")]
        public int MaKhuyenMai { get; set; }

        [Required(ErrorMessage = "Tên khuyến mãi không được để trống")]
        [DisplayName("Tên Khuyến Mãi")]
        public string TenKhuyenMai { get; set; } = null!;

        [DisplayName("Mô Tả")]
        public string? MoTa { get; set; }

        [Required(ErrorMessage = "Phần trăm giảm không được để trống")]
        [Range(0, 100, ErrorMessage = "Phần trăm giảm phải từ 0 đến 100")]
        [DisplayName("Phần Trăm Giảm")]
        public decimal PhanTramGiam { get; set; }

        [Required(ErrorMessage = "Ngày bắt đầu không được để trống")]
        [DisplayName("Ngày Bắt Đầu")]
        public DateTime NgayBatDau { get; set; }

        [Required(ErrorMessage = "Ngày kết thúc không được để trống")]
        [DisplayName("Ngày Kết Thúc")]
        public DateTime NgayKetThuc { get; set; }

        public string? HinhAnh { get; set; }

        public virtual ICollection<PhieuDatVe> PhieuDatVes { get; set; }
    }
}
