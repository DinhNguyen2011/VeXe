using System;
using System.Collections.Generic;
using System.ComponentModel;

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

        [DisplayName("Tên Khuyến Mãi")]
        public string TenKhuyenMai { get; set; } = null!;

        [DisplayName("Mô Tả")]
        public string? MoTa { get; set; }

        [DisplayName("Phần Trăm Giảm")]
        public decimal PhanTramGiam { get; set; }

        [DisplayName("Ngày Bắt Đầu")]
        public DateTime NgayBatDau { get; set; }

        [DisplayName("Ngày Kết Thúc")]
        public DateTime NgayKetThuc { get; set; }

        public virtual ICollection<PhieuDatVe> PhieuDatVes { get; set; }
    }
}
