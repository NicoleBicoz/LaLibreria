using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace LaLibreria.Models
{
    public partial class EstadosVentas
    {
        public EstadosVentas()
        {
            Ventas = new HashSet<Ventas>();
        }

        public byte IdestadosVentas { get; set; }
        public byte CodEstado { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaInsert { get; set; }

        public virtual ICollection<Ventas> Ventas { get; set; }
    }
}
