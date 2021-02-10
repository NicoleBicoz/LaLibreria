using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace LaLibreria.Models
{
    public partial class Editorial
    {
        public Editorial()
        {
            Libros = new HashSet<Libros>();
        }

        public decimal Ideditorial { get; set; }
        public string NombreEditorial { get; set; }
        public bool? Estado { get; set; }
        public DateTime FechaInsert { get; set; }

        public virtual ICollection<Libros> Libros { get; set; }
    }
}
