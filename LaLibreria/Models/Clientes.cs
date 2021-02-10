using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace LaLibreria.Models
{
    public partial class Clientes
    {
        public Clientes()
        {
            Ventas = new HashSet<Ventas>();
        }

        public decimal Idclientes { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Mail { get; set; }
        public string Rut { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public DateTime FechaInsert { get; set; }
        public bool? Estado { get; set; }

        public virtual ICollection<Ventas> Ventas { get; set; }
    }
}
