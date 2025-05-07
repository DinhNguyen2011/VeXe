using System.ComponentModel.DataAnnotations;

namespace Asp.netWebDatVe.Models
{
    public class XeModel
    {
        [Required(ErrorMessage = "Biển số không được để trống.")]
        public string Bienso { get; set; } = null!;

        [Required(ErrorMessage = "Loại xe không được để trống.")]
        public int IdLoai { get; set; }

        [Display(Name = "Tên xe")]
        public string? Tenxe { get; set; }

        [Display(Name = "Hình ảnh mới")]
        public IFormFile? HinhAnh { get; set; } // File ảnh mới từ người dùng

        [Display(Name = "Hình ảnh hiện tại")]
        public string? HinhAnhUrl { get; set; } // Đường dẫn ảnh cũ để hiển thị

    }
}
