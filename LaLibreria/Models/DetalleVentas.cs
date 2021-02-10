using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace LaLibreria.Models
{
    public partial class DetalleVentas
    {
        public decimal IddetalleVentas { get; set; }
        public decimal Idventas { get; set; }
        public decimal Idlibros { get; set; }
        public decimal? IddctoLibros { get; set; }
        public int PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }

        public virtual Libros IdlibrosNavigation { get; set; }
        public virtual Ventas IdventasNavigation { get; set; }
    }
}
