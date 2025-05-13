using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Asp.netWebDatVe.Models
{
    public partial class ThanhToan
    {
        public int MaThanhToan { get; set; }
        [Display(Name = "Mã phiếu đặt vé")]
        [Required(ErrorMessage = "Mã phiếu đặt vé không được để trống")]
        public int MaPhieu { get; set; }

        [Display(Name = "Phương thức thanh toán")]
        [Required(ErrorMessage = "Phương thức thanh toán không được để trống")]
        [StringLength(50, ErrorMessage = "Phương thức thanh toán không được vượt quá 50 ký tự")]
        public string PhuongThuc { get; set; } = null!;

        [Display(Name = "Số tiền")]
        [Required(ErrorMessage = "Số tiền không được để trống")]
        [Range(0, double.MaxValue, ErrorMessage = "Số tiền phải lớn hơn hoặc bằng 0")]
        public decimal SoTien { get; set; }

        [Display(Name = "Ngày thanh toán")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Ngày thanh toán không được để trống")]
        public DateTime NgayThanhToan { get; set; }

        [Display(Name = "Mã giao dịch")]
        [StringLength(100, ErrorMessage = "Mã giao dịch không được vượt quá 100 ký tự")]
        public string? MaGiaoDich { get; set; }

        [Display(Name = "Trạng thái")]
        [Required(ErrorMessage = "Trạng thái không được để trống")]
        [StringLength(50, ErrorMessage = "Trạng thái không được vượt quá 50 ký tự")]
        public string TrangThai { get; set; } = null!;


        public virtual PhieuDatVe MaPhieuNavigation { get; set; } = null!;
    }
}
