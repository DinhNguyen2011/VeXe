using System;
using System.Collections.Generic;

namespace Asp.netWebDatVe.Models
{
    public partial class ChuyenXe
    {
        public ChuyenXe()
        {
            VeXes = new HashSet<VeXe>();
        }

        public int MaChuyen { get; set; }
        public int? MaTuyen { get; set; }
        public DateTime? ThoiDiemKhoiHanh { get; set; }
        public DateTime? ThoiDiemDenDuKien { get; set; }
        public decimal? GiaVe { get; set; }
        public string? BienSoXe { get; set; }
        public string? TenChuyenXe { get; set; }
        public string? GhiChu { get; set; }
        public int? MaNhanVien { get; set; }
        public int? MaTaiXe { get; set; }
        public int? MaNhanVien1 { get; set; }

        public virtual Xe? BienSoXeNavigation { get; set; }
        public virtual NhanVien? MaNhanVien1Navigation { get; set; }
        public virtual NhanVien? MaNhanVienNavigation { get; set; }
        public virtual NhanVien? MaTaiXeNavigation { get; set; }
        public virtual TuyenXe? MaTuyenNavigation { get; set; }
        public virtual ICollection<VeXe> VeXes { get; set; }
    }
}
