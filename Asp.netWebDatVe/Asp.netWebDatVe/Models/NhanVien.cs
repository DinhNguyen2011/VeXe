using System;
using System.Collections.Generic;

namespace Asp.netWebDatVe.Models
{
    public partial class NhanVien
    {
        public NhanVien()
        {
            ChuyenXeMaNhanVien1Navigations = new HashSet<ChuyenXe>();
            ChuyenXeMaNhanVienNavigations = new HashSet<ChuyenXe>();
            ChuyenXeMaTaiXeNavigations = new HashSet<ChuyenXe>();
        }

        public int MaNhanVien { get; set; }
        public string HoTen { get; set; } = null!;
        public string Sdt { get; set; } = null!;
        public string? DiaChi { get; set; }
        public string VaiTro { get; set; } = null!;
        public long? Cccd { get; set; }

        public virtual ICollection<ChuyenXe> ChuyenXeMaNhanVien1Navigations { get; set; }
        public virtual ICollection<ChuyenXe> ChuyenXeMaNhanVienNavigations { get; set; }
        public virtual ICollection<ChuyenXe> ChuyenXeMaTaiXeNavigations { get; set; }
    }
}
