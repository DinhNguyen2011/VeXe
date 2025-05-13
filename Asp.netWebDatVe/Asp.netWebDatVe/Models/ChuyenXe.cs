using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Asp.netWebDatVe.Models
{
    public partial class ChuyenXe
    {
        public ChuyenXe()
        {
            VeXes = new HashSet<VeXe>();
        }

        public int MaChuyen { get; set; }
        [Display(Name = "Mã tuyến")]
        [Required(ErrorMessage = "Mã tuyến không được để trống")]
        public int? MaTuyen { get; set; }

        [Display(Name = "Thời điểm khởi hành")]
        [Required(ErrorMessage = "Thời điểm khởi hành không được để trống")]
        public DateTime? ThoiDiemKhoiHanh { get; set; }

        [Display(Name = "Thời điểm đến dự kiến")]
        public DateTime? ThoiDiemDenDuKien { get; set; }

        [Display(Name = "Giá vé")]
        [Required(ErrorMessage = "Giá vé không được để trống")]
       
        public decimal? GiaVe { get; set; }

        [Display(Name = "Biển số xe")]
        [Required(ErrorMessage = "Biển số xe không được để trống")]
        public string? BienSoXe { get; set; }

        [Display(Name = "Tên chuyến xe")]
        [Required(ErrorMessage = "Tên chuyến xe không được để trống")]
        public string? TenChuyenXe { get; set; }

        [Display(Name = "Ghi chú")]
        public string? GhiChu { get; set; }

        [Display(Name = "Mã nhân viên hỗ trợ")]
        public int? MaNhanVien { get; set; }

        [Display(Name = "Mã tài xế")]
      
        public int? MaTaiXe { get; set; }

        [Display(Name = "Mã nhân viên phụ xe")]
        public int? MaNhanVien1 { get; set; }


        public virtual Xe? BienSoXeNavigation { get; set; }
        public virtual NhanVien? MaNhanVien1Navigation { get; set; }
        public virtual NhanVien? MaNhanVienNavigation { get; set; }
        public virtual NhanVien? MaTaiXeNavigation { get; set; }
        public virtual TuyenXe? MaTuyenNavigation { get; set; }
        public virtual ICollection<VeXe> VeXes { get; set; }
    }
}
