using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Asp.netWebDatVe.Models
{
    public partial class Vitrighe
    {
        public Vitrighe()
        {
            VeXes = new HashSet<VeXe>();
        }

        public int IdVitri { get; set; }
        [Display(Name = "Biển số xe")]
        [Required(ErrorMessage = "Biển số xe không được để trống")]
        [StringLength(20, ErrorMessage = "Biển số xe không được vượt quá 20 ký tự")]
        public string Bienso { get; set; } = null!;

        [Display(Name = "Tên vị trí")]
        [StringLength(50, ErrorMessage = "Tên vị trí không được vượt quá 50 ký tự")]
        public string? Tenvitri { get; set; }

        [Display(Name = "Trạng thái")]
        public bool? Trangthai { get; set; }

        public virtual Xe BiensoNavigation { get; set; } = null!;
        public virtual ICollection<VeXe> VeXes { get; set; }
    }
}
