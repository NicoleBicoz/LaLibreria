using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace LaLibreria.Models
{
    public partial class ModelContext : DbContext
    {
        public ModelContext()
        {
        }

        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Clientes> Clientes { get; set; }
        public virtual DbSet<DctoLibros> DctoLibros { get; set; }
        public virtual DbSet<Descuentos> Descuentos { get; set; }
        public virtual DbSet<DetalleVentas> DetalleVentas { get; set; }
        public virtual DbSet<Editorial> Editorial { get; set; }
        public virtual DbSet<EstadosVentas> EstadosVentas { get; set; }
        public virtual DbSet<Libros> Libros { get; set; }
        public virtual DbSet<TipoDocumento> TipoDocumento { get; set; }
        public virtual DbSet<Ventas> Ventas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseOracle("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));Persist Security Info=True;User Id=system;Password=Angels123;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Clientes>(entity =>
            {
                entity.HasKey(e => new { e.Rut, e.Idclientes })
                    .HasName("PK_IDCLIENTES_RUT");

                entity.ToTable("CLIENTES");

                entity.Property(e => e.Rut)
                    .HasColumnName("RUT")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Idclientes)
                    .HasColumnName("IDCLIENTES")
                    .HasColumnType("NUMBER(20)");

                entity.Property(e => e.Apellidos)
                    .HasColumnName("APELLIDOS")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasColumnName("ESTADO")
                    .HasDefaultValueSql("1 ");

                entity.Property(e => e.FechaInsert)
                    .HasColumnName("FECHA_INSERT")
                    .HasColumnType("TIMESTAMP(6)")
                    .HasDefaultValueSql("sysdate ");

                entity.Property(e => e.FechaNacimiento)
                    .HasColumnName("FECHA_NACIMIENTO")
                    .HasColumnType("DATE");

                entity.Property(e => e.Mail)
                    .HasColumnName("MAIL")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nombres)
                    .IsRequired()
                    .HasColumnName("NOMBRES")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DctoLibros>(entity =>
            {
                entity.HasKey(e => e.IddctoLibros)
                    .HasName("PK_IDDCTO_LIBROS");

                entity.ToTable("DCTO_LIBROS");

                entity.Property(e => e.IddctoLibros)
                    .HasColumnName("IDDCTO_LIBROS")
                    .HasColumnType("NUMBER(20)");

                entity.Property(e => e.FechaInsert)
                    .HasColumnName("FECHA_INSERT")
                    .HasColumnType("TIMESTAMP(6)")
                    .HasDefaultValueSql("sysdate ");

                entity.Property(e => e.Iddescuentos)
                    .HasColumnName("IDDESCUENTOS")
                    .HasColumnType("NUMBER(20)");

                entity.Property(e => e.Idlibros)
                    .HasColumnName("IDLIBROS")
                    .HasColumnType("NUMBER(20)");

                entity.HasOne(d => d.IddescuentosNavigation)
                    .WithMany(p => p.DctoLibros)
                    .HasForeignKey(d => d.Iddescuentos)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IDDESCUENTOS_DCTO");

                entity.HasOne(d => d.IdlibrosNavigation)
                    .WithMany(p => p.DctoLibros)
                    .HasForeignKey(d => d.Idlibros)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IDLIBROS_DCTO");
            });

            modelBuilder.Entity<Descuentos>(entity =>
            {
                entity.HasKey(e => e.Iddescuentos)
                    .HasName("PK_IDDESCUENTOS");

                entity.ToTable("DESCUENTOS");

                entity.Property(e => e.Iddescuentos)
                    .HasColumnName("IDDESCUENTOS")
                    .HasColumnType("NUMBER(20)");

                entity.Property(e => e.CodDescuento)
                    .IsRequired()
                    .HasColumnName("COD_DESCUENTO")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Descripcion)
                    .IsRequired()
                    .HasColumnName("DESCRIPCION")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Estado).HasColumnName("ESTADO");

                entity.Property(e => e.FechaInsert)
                    .HasColumnName("FECHA_INSERT")
                    .HasColumnType("TIMESTAMP(6)")
                    .HasDefaultValueSql("sysdate ");

                entity.Property(e => e.FechaUpdate)
                    .HasColumnName("FECHA_UPDATE")
                    .HasColumnType("TIMESTAMP(6)");

                entity.Property(e => e.Porcentaje).HasColumnName("PORCENTAJE");
            });

            modelBuilder.Entity<DetalleVentas>(entity =>
            {
                entity.HasKey(e => e.IddetalleVentas)
                    .HasName("PK_IDDETALLE_VENTAS");

                entity.ToTable("DETALLE_VENTAS");

                entity.Property(e => e.IddetalleVentas)
                    .HasColumnName("IDDETALLE_VENTAS")
                    .HasColumnType("NUMBER(20)");

                entity.Property(e => e.Cantidad)
                    .HasColumnName("CANTIDAD")
                    .HasDefaultValueSql("1 ");

                entity.Property(e => e.IddctoLibros)
                    .HasColumnName("IDDCTO_LIBROS")
                    .HasColumnType("NUMBER(20)");

                entity.Property(e => e.Idlibros)
                    .HasColumnName("IDLIBROS")
                    .HasColumnType("NUMBER(20)");

                entity.Property(e => e.Idventas)
                    .HasColumnName("IDVENTAS")
                    .HasColumnType("NUMBER(20)");

                entity.Property(e => e.PrecioUnitario).HasColumnName("PRECIO_UNITARIO");

                entity.Property(e => e.Total)
                    .HasColumnName("TOTAL")
                    .HasColumnType("NUMBER(20)");

                entity.HasOne(d => d.IdlibrosNavigation)
                    .WithMany(p => p.DetalleVentas)
                    .HasForeignKey(d => d.Idlibros)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IDLIBROS_DETALLE");

                entity.HasOne(d => d.IdventasNavigation)
                    .WithMany(p => p.DetalleVentas)
                    .HasForeignKey(d => d.Idventas)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IDVENTAS");
            });

            modelBuilder.Entity<Editorial>(entity =>
            {
                entity.HasKey(e => e.Ideditorial)
                    .HasName("PK_IDEDITORIAL");

                entity.ToTable("EDITORIAL");

                entity.Property(e => e.Ideditorial)
                    .HasColumnName("IDEDITORIAL")
                    .HasColumnType("NUMBER(20)");

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasColumnName("ESTADO")
                    .HasDefaultValueSql("1 ");

                entity.Property(e => e.FechaInsert)
                    .HasColumnName("FECHA_INSERT")
                    .HasColumnType("TIMESTAMP(6)")
                    .HasDefaultValueSql("sysdate ");

                entity.Property(e => e.NombreEditorial)
                    .IsRequired()
                    .HasColumnName("NOMBRE_EDITORIAL")
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'Desconocida' ");
            });

            modelBuilder.Entity<EstadosVentas>(entity =>
            {
                entity.HasKey(e => e.IdestadosVentas)
                    .HasName("PK_IDESTADOS_VENTAS");

                entity.ToTable("ESTADOS_VENTAS");

                entity.Property(e => e.IdestadosVentas).HasColumnName("IDESTADOS_VENTAS");

                entity.Property(e => e.CodEstado).HasColumnName("COD_ESTADO");

                entity.Property(e => e.Descripcion)
                    .HasColumnName("DESCRIPCION")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FechaInsert)
                    .HasColumnName("FECHA_INSERT")
                    .HasColumnType("TIMESTAMP(6)")
                    .HasDefaultValueSql("sysdate ");
            });

            modelBuilder.Entity<Libros>(entity =>
            {
                entity.HasKey(e => e.Idlibros)
                    .HasName("PK_IDLIBROS");

                entity.ToTable("LIBROS");

                entity.Property(e => e.Idlibros)
                    .HasColumnName("IDLIBROS")
                    .HasColumnType("NUMBER(20)");

                entity.Property(e => e.Autor)
                    .IsRequired()
                    .HasColumnName("AUTOR")
                    .HasMaxLength(80)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'Desconocido' ");

                entity.Property(e => e.Estado)
                    .IsRequired()
                    .HasColumnName("ESTADO")
                    .HasDefaultValueSql("1 ");

                entity.Property(e => e.FechaInsert)
                    .HasColumnName("FECHA_INSERT")
                    .HasColumnType("TIMESTAMP(6)")
                    .HasDefaultValueSql("sysdate ");

                entity.Property(e => e.Ideditorial)
                    .HasColumnName("IDEDITORIAL")
                    .HasColumnType("NUMBER(20)");

                entity.Property(e => e.Precio)
                    .HasColumnName("PRECIO")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.Stock)
                    .HasColumnName("STOCK")
                    .HasDefaultValueSql("0 ");

                entity.Property(e => e.Titulo)
                    .IsRequired()
                    .HasColumnName("TITULO")
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'Sin título' ");

                entity.HasOne(d => d.IdeditorialNavigation)
                    .WithMany(p => p.Libros)
                    .HasForeignKey(d => d.Ideditorial)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IDEDITORIAL_LIBROS");
            });

            modelBuilder.Entity<TipoDocumento>(entity =>
            {
                entity.HasKey(e => e.IdtipoDocumento)
                    .HasName("PK_IDTIPO_DOCUMENTO");

                entity.ToTable("TIPO_DOCUMENTO");

                entity.Property(e => e.IdtipoDocumento).HasColumnName("IDTIPO_DOCUMENTO");

                entity.Property(e => e.CodigoSii)
                    .HasColumnName("CODIGO_SII")
                    .HasColumnType("NUMBER(2)");

                entity.Property(e => e.Descripcion)
                    .HasColumnName("DESCRIPCION")
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.FechaInsert)
                    .HasColumnName("FECHA_INSERT")
                    .HasColumnType("TIMESTAMP(6)")
                    .HasDefaultValueSql("sysdate ");
            });

            modelBuilder.Entity<Ventas>(entity =>
            {
                entity.HasKey(e => e.Idventas)
                    .HasName("PK_IDVENTAS");

                entity.ToTable("VENTAS");

                entity.Property(e => e.Idventas)
                    .HasColumnName("IDVENTAS")
                    .HasColumnType("NUMBER(20)");

                entity.Property(e => e.Descuento)
                    .HasColumnName("DESCUENTO")
                    .HasColumnType("NUMBER(20)");

                entity.Property(e => e.FechaEmision)
                    .HasColumnName("FECHA_EMISION")
                    .HasColumnType("DATE");

                entity.Property(e => e.FechaInsert)
                    .HasColumnName("FECHA_INSERT")
                    .HasColumnType("DATE")
                    .HasDefaultValueSql("sysdate ");

                entity.Property(e => e.FechaUpdate)
                    .HasColumnName("FECHA_UPDATE")
                    .HasColumnType("DATE")
                    .HasDefaultValueSql("sysdate ");

                entity.Property(e => e.FechaVencimiento)
                    .HasColumnName("FECHA_VENCIMIENTO")
                    .HasColumnType("DATE");

                entity.Property(e => e.FolioDocumento)
                    .HasColumnName("FOLIO_DOCUMENTO")
                    .HasColumnType("NUMBER(6)");

                entity.Property(e => e.Idcliente)
                    .HasColumnName("IDCLIENTE")
                    .HasColumnType("NUMBER(20)");

                entity.Property(e => e.IdestadosVentas).HasColumnName("IDESTADOS_VENTAS");

                entity.Property(e => e.IdtipoDocumento).HasColumnName("IDTIPO_DOCUMENTO");

                entity.Property(e => e.Rutcliente)
                    .IsRequired()
                    .HasColumnName("RUTCLIENTE")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Subtotal)
                    .HasColumnName("SUBTOTAL")
                    .HasColumnType("NUMBER(20)");

                entity.Property(e => e.Total)
                    .HasColumnName("TOTAL")
                    .HasColumnType("NUMBER(20)");

                entity.HasOne(d => d.IdestadosVentasNavigation)
                    .WithMany(p => p.Ventas)
                    .HasForeignKey(d => d.IdestadosVentas)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IDESTADOS_VENTAS");

                entity.HasOne(d => d.IdtipoDocumentoNavigation)
                    .WithMany(p => p.Ventas)
                    .HasForeignKey(d => d.IdtipoDocumento)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IDTIPODOC_VENTAS");

                entity.HasOne(d => d.Clientes)
                    .WithMany(p => p.Ventas)
                    .HasForeignKey(d => new { d.Rutcliente, d.Idcliente })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IDCLIENTE_VENTAS");
            });

            modelBuilder.HasSequence("LOGMNR_DIDS$");

            modelBuilder.HasSequence("LOGMNR_EVOLVE_SEQ$");

            modelBuilder.HasSequence("LOGMNR_SEQ$");

            modelBuilder.HasSequence("LOGMNR_UIDS$");

            modelBuilder.HasSequence("MVIEW$_ADVSEQ_GENERIC");

            modelBuilder.HasSequence("MVIEW$_ADVSEQ_ID");

            modelBuilder.HasSequence("ROLLING_EVENT_SEQ$");

            modelBuilder.HasSequence("SEC_CLIENTES");

            modelBuilder.HasSequence("SEC_CODIGOLIBROS");

            modelBuilder.HasSequence("SEC_DCTO_LIBROS");

            modelBuilder.HasSequence("SEC_DESCUENTOS");

            modelBuilder.HasSequence("SEC_DETALLE_VENTAS");

            modelBuilder.HasSequence("SEC_EDITORIAL");

            modelBuilder.HasSequence("SEC_ESTADOSVENTAS");

            modelBuilder.HasSequence("SEC_FOLIO_BOLETA");

            modelBuilder.HasSequence("SEC_FOLIO_FACTURA");

            modelBuilder.HasSequence("SEC_LIBROS");

            modelBuilder.HasSequence("SEC_MASCOTA");

            modelBuilder.HasSequence("SEC_TIPODOCUMENTO");

            modelBuilder.HasSequence("SEC_VENTAS");

            modelBuilder.HasSequence("SQ_TIPOMASCOTA");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
