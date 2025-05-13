using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Asp.netWebDatVe.Models
{
    public partial class PhieuDatVe
    {
        public PhieuDatVe()
        {
            ThanhToans = new HashSet<ThanhToan>();
            VeXes = new HashSet<VeXe>();
        }

        public int MaPhieu { get; set; }
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        public string? Email { get; set; }

        [Display(Name = "Ngày đặt")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Ngày đặt không được để trống")]
        public DateTime? NgayDat { get; set; }

        [Display(Name = "Tổng tiền")]
        [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền phải lớn hơn hoặc bằng 0")]
        public decimal? TongTien { get; set; }

        [Display(Name = "Trạng thái")]
        [StringLength(50, ErrorMessage = "Trạng thái không được vượt quá 50 ký tự")]
        public string? TrangThai { get; set; }

        [Display(Name = "Mã giao dịch VNPAY")]
        [StringLength(100, ErrorMessage = "Mã giao dịch không được vượt quá 100 ký tự")]
        public string? VnpTransactionId { get; set; }

        [Display(Name = "Mã khuyến mãi")]
        public int? MaKhuyenMai { get; set; }
        public virtual KhuyenMai? MaKhuyenMaiNavigation { get; set; }
        public virtual ICollection<ThanhToan> ThanhToans { get; set; }
        public virtual ICollection<VeXe> VeXes { get; set; }
    }
}
