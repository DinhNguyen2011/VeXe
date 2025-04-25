using System;
using System.Collections.Generic;

namespace Asp.netWebDatVe.Models
{
    public partial class Vitrighe
    {
        public Vitrighe()
        {
            VeXes = new HashSet<VeXe>();
        }

        public int IdVitri { get; set; }
        public string Bienso { get; set; } = null!;
        public string? Tenvitri { get; set; }
        public bool? Trangthai { get; set; }

        public virtual Xe BiensoNavigation { get; set; } = null!;
        public virtual ICollection<VeXe> VeXes { get; set; }
    }
}
