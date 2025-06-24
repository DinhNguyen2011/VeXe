using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Asp.netWebDatVe.Models
{
    public partial class ThanhToan
    {
        [DisplayName("Mã Thanh Toán")]
        public int MaThanhToan { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn phiếu đặt vé.")]
        [DisplayName("Mã Phiếu Đặt Vé")]
        public int MaPhieu { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập phương thức thanh toán.")]
        [DisplayName("Phương Thức Thanh Toán")]
        public string PhuongThuc { get; set; } = null!;

        [Required(ErrorMessage = "Vui lòng nhập số tiền thanh toán.")]
        [DisplayName("Số Tiền")]
        public decimal SoTien { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày thanh toán.")]
        [DisplayName("Ngày Thanh Toán")]
        public DateTime NgayThanhToan { get; set; }

        [DisplayName("Mã Giao Dịch")]
        public string? MaGiaoDich { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập trạng thái thanh toán.")]
        [DisplayName("Trạng Thái")]
        public string TrangThai { get; set; } = null!;

        [DisplayName("Phiếu Đặt Vé")]
        public virtual PhieuDatVe MaPhieuNavigation { get; set; } = null!;
    }
}
