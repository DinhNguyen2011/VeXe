using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Asp.netWebDatVe.Models
{
    public partial class BenXe
    {
        public BenXe()
        {
            TuyenXeMaBenXeDenNavigations = new HashSet<TuyenXe>();
            TuyenXeMaBenXeDiNavigations = new HashSet<TuyenXe>();
        }

        [DisplayName("Mã Bến Xe")]
        public int MaBenXe { get; set; }

        [DisplayName("Tên Bến Xe")]
        public string TenBenXe { get; set; } = null!;

        [DisplayName("Địa Chỉ")]
        public string DiaChi { get; set; } = null!;

        [DisplayName("Số Điện Thoại")]
        public string Sdt { get; set; } = null!;

        [DisplayName("Thành Phố")]
        public string? ThanhPho { get; set; }
        public virtual ICollection<TuyenXe> TuyenXeMaBenXeDenNavigations { get; set; }
        public virtual ICollection<TuyenXe> TuyenXeMaBenXeDiNavigations { get; set; }
    }
}
