using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Asp.netWebDatVe.Models
{
    public partial class ChuyenXe
    {
        public ChuyenXe()
        {
            VeXes = new HashSet<VeXe>();
        }

        [DisplayName("Mã Chuyến")]
        public int MaChuyen { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn tuyến xe")]

        [DisplayName("Mã Tuyến")]
        public int? MaTuyen { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập thời điểm khởi hành")]

        [DisplayName("Thời Điểm Khởi Hành")]
        public DateTime? ThoiDiemKhoiHanh { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập thời điểm đến dự kiến")]

        [DisplayName("Thời Điểm Đến Dự Kiến")]
        public DateTime? ThoiDiemDenDuKien { get; set; }

        [DisplayName("Giá Vé")]
        public decimal? GiaVe { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập biển số xe")]

        [DisplayName("Biển Số Xe")]
        public string? BienSoXe { get; set; }

        [DisplayName("Tên Chuyến Xe")]
           [Required(ErrorMessage = "Vui lòng nhập tên chuyến xe")]
        public string? TenChuyenXe { get; set; }

        [DisplayName("Ghi Chú")]
        public string? GhiChu { get; set; }

        [DisplayName("Mã Nhân Viên")]
        public int? MaNhanVien { get; set; }

        [DisplayName("Mã Tài Xế")]
        public int? MaTaiXe { get; set; }

        [DisplayName("Mã Nhân Viên (Phụ)")]
        public int? MaNhanVien1 { get; set; }
        [ValidateNever]
        public virtual Xe? BienSoXeNavigation { get; set; }
        [ValidateNever]
        public virtual NhanVien? MaNhanVien1Navigation { get; set; }
        [ValidateNever]
        public virtual NhanVien? MaNhanVienNavigation { get; set; }
        [ValidateNever]
        public virtual NhanVien? MaTaiXeNavigation { get; set; }
        [ValidateNever]
        public virtual TuyenXe? MaTuyenNavigation { get; set; }
        public virtual ICollection<VeXe> VeXes { get; set; }
    }
}
