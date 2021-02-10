using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace LaLibreria.Models
{
    public partial class Libros
    {
        public Libros()
        {
            DctoLibros = new HashSet<DctoLibros>();
            DetalleVentas = new HashSet<DetalleVentas>();
        }

        public decimal Idlibros { get; set; }
        public decimal Ideditorial { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public int Precio { get; set; }
        public int Stock { get; set; }
        public DateTime FechaInsert { get; set; }
        public bool? Estado { get; set; }

        public virtual Editorial IdeditorialNavigation { get; set; }
        public virtual ICollection<DctoLibros> DctoLibros { get; set; }
        public virtual ICollection<DetalleVentas> DetalleVentas { get; set; }
    }
}
