using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;

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

        [DisplayName("Mã Tuyến")]
        public int? MaTuyen { get; set; }

        [DisplayName("Thời Điểm Khởi Hành")]
        public DateTime? ThoiDiemKhoiHanh { get; set; }

        [DisplayName("Thời Điểm Đến Dự Kiến")]
        public DateTime? ThoiDiemDenDuKien { get; set; }

        [DisplayName("Giá Vé")]
        public decimal? GiaVe { get; set; }

        [DisplayName("Biển Số Xe")]
        public string? BienSoXe { get; set; }

        [DisplayName("Tên Chuyến Xe")]
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
