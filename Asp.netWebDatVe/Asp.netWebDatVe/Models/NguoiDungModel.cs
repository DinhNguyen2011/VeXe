using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Asp.netWebDatVe.Models
{
    public class NguoiDungModel
    {
        [DisplayName("ID")]
        public int Id { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        public string Email { get; set; } = null!;

        [DisplayName("SĐT")]
        [StringLength(10, ErrorMessage = "Số điện thoại không được vượt quá 10 ký tự.")]
        [Required(ErrorMessage = "Vui lòng nhập SĐT.")]

        public string? Sdt { get; set; }

        [DisplayName("Họ và tên")]
        [Required(ErrorMessage = "Vui lòng nhập họ tên.")]
        public string? HoTen { get; set; }

        [DisplayName("Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        public string MatKhau { get; set; } = null!;

        [DisplayName("Ngày sinh")]
        public DateTime? NgaySinh { get; set; }

        [DisplayName("Địa chỉ")]
        [StringLength(250, ErrorMessage = "Địa chỉ không được vượt quá 250 ký tự.")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ.")]

        public string? DiaChi { get; set; }

        [DisplayName("Mã Quyền")]
        [Required(ErrorMessage = "Mã quyền là bắt buộc.")]
        public int? MaQuyen { get; set; } = 0;

        [DisplayName("Hình ảnh")]
        [FileExtensions(Extensions = "jpg,jpeg,png", ErrorMessage = "Chỉ cho phép các định dạng ảnh: jpg, jpeg, png.")]
        public IFormFile? HinhAnh { get; set; }

        [DisplayName("Đường dẫn hình ảnh")]
        public string? HinhAnhUrl { get; set; }

    }
}