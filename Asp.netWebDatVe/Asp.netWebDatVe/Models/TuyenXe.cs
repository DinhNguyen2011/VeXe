using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;

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

        [DisplayName("Điểm Đi")]
        public string DiemDi { get; set; } = null!;

        [DisplayName("Điểm Đến")]
        public string DiemDen { get; set; } = null!;

        [DisplayName("Số Ngày Chạy Trong Tuần")]
        public int? SoNgayChayTrongTuan { get; set; }

        [DisplayName("Giá Hiện Hành")]
        public decimal? GiaHienHanh { get; set; }

        [DisplayName("Quãng Đường (km)")]
        public int? QuangDuong { get; set; }

        [DisplayName("Mã Bến Xe Đi")]
        public int MaBenXeDi { get; set; }

        [DisplayName("Mã Bến Xe Đến")]
        public int MaBenXeDen { get; set; }
        [ValidateNever]
        public virtual BenXe MaBenXeDenNavigation { get; set; } = null!;
        [ValidateNever]
        public virtual BenXe MaBenXeDiNavigation { get; set; } = null!;
        public virtual ICollection<ChuyenXe> ChuyenXes { get; set; }
    }
}
