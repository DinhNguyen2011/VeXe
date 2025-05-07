using System;
using System.Collections.Generic;

namespace Asp.netWebDatVe.Models
{
    public partial class LienHe
    {
        public int Id { get; set; }
        public string HoVaTen { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string NoiDung { get; set; } = null!;
        public DateTime? NgayGui { get; set; }
    }
}
