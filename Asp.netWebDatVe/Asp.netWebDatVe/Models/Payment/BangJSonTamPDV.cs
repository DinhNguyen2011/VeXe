namespace Asp.netWebDatVe.Models.Payment
{
    public class BangJSonTamPDV
    {
        public int MaChuyen { get; set; }
        public List<int> SeatIds { get; set; } = new List<int>();
        public string TenKhachHang { get; set; }
        public string SoDienThoai { get; set; }
        public string Email { get; set; }
        public string GhiChu { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime NgayDat { get; set; }
        public string TenChuyenXe { get; set; }
    }
}
