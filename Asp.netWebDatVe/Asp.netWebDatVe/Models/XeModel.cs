using System.ComponentModel.DataAnnotations;

namespace Asp.netWebDatVe.Models
{
    public class XeModel
    {
        [Display(Name = "Biển Số Xe")]
        [Required(ErrorMessage = "Vui lòng nhập Biển số xe")]
        public string Bienso { get; set; } = null!;

        [Display(Name = "Loại Xe")]
        [Required(ErrorMessage = "Vui lòng chọn Loại xe")]
        public int IdLoai { get; set; }

        [Display(Name = "Tên Xe")]
        [Required(ErrorMessage = "Vui lòng nhập Tên xe")]
        public string? Tenxe { get; set; }

        [Display(Name = "Hình Ảnh")]
        public IFormFile? HinhAnh { get; set; }

        public string? HinhAnhUrl { get; set; }

    }
}
