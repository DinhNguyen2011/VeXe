using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Asp.netWebDatVe.Models
{
    public partial class Vitrighe
    {
        public Vitrighe()
        {
            VeXes = new HashSet<VeXe>();
        }

        [Key]
        public int IdVitri { get; set; }
        [DisplayName("Biển số")]
        [Required(ErrorMessage = "Vui lòng chọn biển số xe")]
        [StringLength(20, ErrorMessage = "Biển số xe không được vượt quá 20 ký tự")]
        public string Bienso { get; set; } = null!;
        [DisplayName("Tên ghế")]

        [StringLength(50, ErrorMessage = "Tên vị trí không được vượt quá 50 ký tự")]
        [RegularExpression(@"^G\d+$", ErrorMessage = "Tên vị trí phải có định dạng G + số (ví dụ: G1, G2)")]
        public string? Tenvitri { get; set; }
        [DisplayName("Trạng thái")]

        [Required(ErrorMessage = "Vui lòng chọn trạng thái")]
        public bool? Trangthai { get; set; }
        [ValidateNever]
        public virtual Xe BiensoNavigation { get; set; } = null!;
        public virtual ICollection<VeXe> VeXes { get; set; }
    }
}