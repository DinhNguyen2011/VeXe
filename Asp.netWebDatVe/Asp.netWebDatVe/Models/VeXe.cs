using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Asp.netWebDatVe.Models
{
    public partial class VeXe
    {
        public int MaVe { get; set; }
        [Display(Name = "Mã phiếu đặt vé")]
        public int? MaPhieu { get; set; }

        [Display(Name = "Mã chuyến xe")]
        public int? MaChuyen { get; set; }

        [Display(Name = "Tên vé")]
        [StringLength(100, ErrorMessage = "Tên vé không được vượt quá 100 ký tự")]
        public string? TenVe { get; set; }

        [Display(Name = "Trạng thái")]
        [StringLength(50, ErrorMessage = "Trạng thái không được vượt quá 50 ký tự")]
        public string? TrangThai { get; set; }

        [Display(Name = "Ghi chú")]
        [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự")]
        public string? GhiChu { get; set; }

        [Display(Name = "Tên khách hàng")]
        [StringLength(100, ErrorMessage = "Tên khách hàng không được vượt quá 100 ký tự")]
        public string? TenKh { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        public string? Email { get; set; }

        [Display(Name = "Vị trí ghế")]
        public int? IdVitri { get; set; }

        [Display(Name = "Ngày đặt")]
        [DataType(DataType.Date)]
        public DateTime? NgayDat { get; set; }

        [Display(Name = "Số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [StringLength(15, ErrorMessage = "Số điện thoại không được vượt quá 15 ký tự")]
        public string? Sđt { get; set; }

        public virtual Vitrighe? IdVitriNavigation { get; set; }
        public virtual ChuyenXe? MaChuyenNavigation { get; set; }
        public virtual PhieuDatVe? MaPhieuNavigation { get; set; }
    }
}
