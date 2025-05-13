using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Asp.netWebDatVe.Models
{
    public partial class ThanhToan
    {
        [DisplayName("Mã Thanh Toán")]
        public int MaThanhToan { get; set; }

        [DisplayName("Mã Phiếu Đặt Vé")]
        public int MaPhieu { get; set; }

        [DisplayName("Phương Thức Thanh Toán")]
        public string PhuongThuc { get; set; } = null!;

        [DisplayName("Số Tiền")]
        public decimal SoTien { get; set; }

        [DisplayName("Ngày Thanh Toán")]
        public DateTime NgayThanhToan { get; set; }

        [DisplayName("Mã Giao Dịch")]
        public string? MaGiaoDich { get; set; }

        [DisplayName("Trạng Thái")]
        public string TrangThai { get; set; } = null!;

        public virtual PhieuDatVe MaPhieuNavigation { get; set; } = null!;
    }
}
