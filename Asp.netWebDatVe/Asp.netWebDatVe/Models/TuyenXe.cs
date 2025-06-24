using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Asp.netWebDatVe.Models
{
    public partial class TuyenXe
    {
        public TuyenXe()
        {
            ChuyenXes = new HashSet<ChuyenXe>();
        }

        [DisplayName("Mã Tuyến")]
        public int MaTuyen { get; set; }

        [Required(ErrorMessage = "Điểm đi không được để trống")]
        [DisplayName("Điểm Đi")]
        public string DiemDi { get; set; } = null!;

        [Required(ErrorMessage = "Điểm đến không được để trống")]
        [DisplayName("Điểm Đến")]
        public string DiemDen { get; set; } = null!;

        [Range(1, 7, ErrorMessage = "Số ngày chạy phải từ 1 đến 7")]
        [Required(ErrorMessage =" Số Ngày Chạy không được để trống")]

        [DisplayName("Số Ngày Chạy Trong Tuần")]
        public int? SoNgayChayTrongTuan { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0")]
        [Required(ErrorMessage = "Giá Hiện Hành không được để trống")]

        [DisplayName("Giá Hiện Hành")]
        public decimal? GiaHienHanh { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quãng đường phải lớn hơn 0")]
        [Required(ErrorMessage = "Quãng Đường không được để trống")]

        [DisplayName("Quãng Đường (km)")]
        public int? QuangDuong { get; set; }

        [Required(ErrorMessage = "Mã bến xe đi không được để trống")]
        [DisplayName("Mã Bến Xe Đi")]
        public int MaBenXeDi { get; set; }

        [Required(ErrorMessage = "Mã bến xe đến không được để trống")]
        [DisplayName("Mã Bến Xe Đến")]
        public int MaBenXeDen { get; set; }

        [ValidateNever]
        public virtual BenXe MaBenXeDenNavigation { get; set; } = null!;

        [ValidateNever]
        public virtual BenXe MaBenXeDiNavigation { get; set; } = null!;

        public virtual ICollection<ChuyenXe> ChuyenXes { get; set; }
    }
}
