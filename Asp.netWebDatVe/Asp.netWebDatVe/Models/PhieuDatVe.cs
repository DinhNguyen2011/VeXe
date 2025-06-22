using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Asp.netWebDatVe.Models
{
    public partial class PhieuDatVe
    {
        public PhieuDatVe()
        {
            ThanhToans = new HashSet<ThanhToan>();
            VeXes = new HashSet<VeXe>();
        }
        [DisplayName("Mã Phiếu Đặt Vé")]
        public int MaPhieu { get; set; }

        [DisplayName("Email Khách Hàng")]
        public string? Email { get; set; }

        [DisplayName("Ngày Đặt")]
        public DateTime? NgayDat { get; set; }

        [DisplayName("Tổng Tiền")]
        public decimal? TongTien { get; set; }

        [DisplayName("Trạng Thái")]
        public string? TrangThai { get; set; }

        [DisplayName("Mã Giao Dịch (VNPAY)")]
        public string? VnpTransactionId { get; set; }

        [DisplayName("Mã Khuyến Mãi")]
        public int? MaKhuyenMai { get; set; }
        public string? MoMoTransactionId { get; set; } // Thêm trường này

        [DisplayName("Khuyến Mãi Áp Dụng")]


        public virtual KhuyenMai? MaKhuyenMaiNavigation { get; set; }
        public virtual ICollection<ThanhToan> ThanhToans { get; set; }
        public virtual ICollection<VeXe> VeXes { get; set; }
    }
}
