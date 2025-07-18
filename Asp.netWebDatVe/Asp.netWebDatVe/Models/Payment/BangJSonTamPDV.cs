using System.ComponentModel.DataAnnotations;

namespace Asp.netWebDatVe.Models.Payment
{
    public class BangJSonTamPDV
    {
        public int MaChuyen { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ít nhất một ghế.")]
        public List<int> SeatIds { get; set; } = new List<int>();

        [Required(ErrorMessage = "Vui lòng nhập tên khách hàng.")]
        public string TenKhachHang { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
        [RegularExpression("^[0-9]{10}$", ErrorMessage = "Số điện thoại phải có 10 chữ số.")]
        //[RegularExpression(@"^0[35789][0-9]{8}$", ErrorMessage = "Số điện thoại phải có 10 chữ số và bắt đầu bằng 03, 05, 07, 08, hoặc 09.")]
        public string SoDienThoai { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }


        //[StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự.")]
        public string GhiChu { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime NgayDat { get; set; }

        public string TenChuyenXe { get; set; }
        public string PaymentMethod { get; set; }
    }
}
