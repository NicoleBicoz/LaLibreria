using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace LaLibreria.Models
{
    public partial class DctoLibros
    {
        public decimal IddctoLibros { get; set; }
        public decimal Idlibros { get; set; }
        public decimal Iddescuentos { get; set; }
        public DateTime FechaInsert { get; set; }

        public virtual Descuentos IddescuentosNavigation { get; set; }
        public virtual Libros IdlibrosNavigation { get; set; }
    }
}
