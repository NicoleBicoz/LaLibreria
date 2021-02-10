using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace LaLibreria.Models
{
    public partial class Ventas
    {
        public Ventas()
        {
            DetalleVentas = new HashSet<DetalleVentas>();
        }

        public decimal Idventas { get; set; }
        public byte IdestadosVentas { get; set; }
        public decimal Idcliente { get; set; }
        public string Rutcliente { get; set; }
        public byte IdtipoDocumento { get; set; }
        public DateTime? FechaEmision { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? Descuento { get; set; }
        public decimal Total { get; set; }
        public DateTime FechaInsert { get; set; }
        public DateTime FechaUpdate { get; set; }
        public int FolioDocumento { get; set; }

        public virtual Clientes Clientes { get; set; }
        public virtual EstadosVentas IdestadosVentasNavigation { get; set; }
        public virtual TipoDocumento IdtipoDocumentoNavigation { get; set; }
        public virtual ICollection<DetalleVentas> DetalleVentas { get; set; }
    }
}
