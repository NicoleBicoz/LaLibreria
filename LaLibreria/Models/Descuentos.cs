using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace LaLibreria.Models
{
    public partial class Descuentos
    {
        public Descuentos()
        {
            DctoLibros = new HashSet<DctoLibros>();
        }

        public decimal Iddescuentos { get; set; }
        public string CodDescuento { get; set; }
        public string Descripcion { get; set; }
        public byte Porcentaje { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaInsert { get; set; }
        public DateTime? FechaUpdate { get; set; }

        public virtual ICollection<DctoLibros> DctoLibros { get; set; }
    }
}
