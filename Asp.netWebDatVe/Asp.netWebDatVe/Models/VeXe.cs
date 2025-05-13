using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Asp.netWebDatVe.Models
{
    public partial class VeXe
    {
        [DisplayName("Mã Vé")]
        public int MaVe { get; set; }

        [DisplayName("Mã Phiếu Đặt Vé")]
        public int? MaPhieu { get; set; }

        [DisplayName("Mã Chuyến Xe")]
        public int? MaChuyen { get; set; }

        [DisplayName("Tên Vé")]
        public string? TenVe { get; set; }

        [DisplayName("Trạng Thái")]
        public string? TrangThai { get; set; }

        [DisplayName("Ghi Chú")]
        public string? GhiChu { get; set; }

        [DisplayName("Tên Khách Hàng")]
        public string? TenKh { get; set; }

        [DisplayName("Email Khách Hàng")]
        public string? Email { get; set; }

        [DisplayName("Ngày Đặt")]
        public DateTime? NgayDat { get; set; }

        [DisplayName("Số Điện Thoại")]
        public string? Sđt { get; set; }

        [DisplayName("Vị Trí Ghế")]
        public int? IdVitri { get; set; }


        public virtual Vitrighe? IdVitriNavigation { get; set; }
        public virtual ChuyenXe? MaChuyenNavigation { get; set; }
        public virtual PhieuDatVe? MaPhieuNavigation { get; set; }
    }
}
