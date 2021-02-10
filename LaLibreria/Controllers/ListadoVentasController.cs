using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaLibreria.Models;
using Oracle.ManagedDataAccess.Client;
using System.Text;

namespace LaLibreria.Controllers
{
    public class ListadoVentasController : Controller
    {
        private readonly ModelContext _context;

        public ListadoVentasController(ModelContext context)
        {
            _context = context;
        }

        // GET: ListadoVentas
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Ventas.Include(v => v.Clientes).Include(v => v.IdestadosVentasNavigation).Include(v => v.IdtipoDocumentoNavigation);
            return View(await modelContext.ToListAsync());
        }

        // GET: ListadoVentas/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await _context.Ventas
                .Include(v => v.Clientes)
                .Include(v => v.IdestadosVentasNavigation)
                .Include(v => v.IdtipoDocumentoNavigation)
                .FirstOrDefaultAsync(m => m.Idventas == id);
            if (ventas == null)
            {
                return NotFound();
            }

            return View(ventas);
        }

        // GET: ListadoVentas/Create
        public IActionResult Create()
        {
            ViewData["Rutcliente"] = new SelectList(_context.Clientes, "Rut", "Rut");
            ViewData["IdestadosVentas"] = new SelectList(_context.EstadosVentas, "IdestadosVentas", "IdestadosVentas");
            ViewData["IdtipoDocumento"] = new SelectList(_context.TipoDocumento, "IdtipoDocumento", "IdtipoDocumento");
            return View();
        }

        public IActionResult Listado()
        {
            using (OracleConnection objConexion = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));Persist Security Info=True;User Id=system;Password=Angels123;"))
            {

                try
                {
                    objConexion.Open();
                    OracleCommand cmd = new OracleCommand();
                    cmd.Connection = objConexion;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "PRC_LISTALIBROS";
                    cmd.Parameters.Add("cursor_listado", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
                    OracleDataReader reader = cmd.ExecuteReader();

                    var nueva = "<table class='table' style='border: 1px solid black;'><thead><tr><th>Titulo</th><th>Autor</th><th>Editorial</th><th>Precio</th><th>Stock</th></tr></thead><tbody>";

                    while (reader.Read())
                    {

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (i == 0)
                            {
                                nueva = nueva + "<tr>";
                            }

                            nueva = nueva + "<td>" + reader.GetOracleValue(i) + "</td>";

                            if (i == 4)
                            {
                                nueva = nueva + "</tr>";
                            }

                        }


                    }
                    reader.Close();

                    return Content(nueva + "</tbody></table>", "text/html");


                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Exception: {0}", ex.ToString());

                    objConexion.Close();
                }
                objConexion.Close();

            }

            return View();
        }

        // POST: ListadoVentas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idventas,IdestadosVentas,Idcliente,Rutcliente,IdtipoDocumento,FechaEmision,FechaVencimiento,Subtotal,Descuento,Total,FechaInsert,FechaUpdate,FolioDocumento")] Ventas ventas)
        {
            using (OracleConnection objConexion = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));Persist Security Info=True;User Id=system;Password=Angels123;"))
            {

                try
                {
                    objConexion.Open();
                    OracleCommand listadoVentas = new OracleCommand();
                    listadoVentas.Connection = objConexion;
                    listadoVentas.CommandType = System.Data.CommandType.StoredProcedure;
                    listadoVentas.CommandText = "PRC_LISTAVENTAS";
                    listadoVentas.Parameters.Add("fechaEmis", OracleDbType.Date).Value = ventas.FechaEmision;
                    listadoVentas.Parameters.Add("cursor_listado", OracleDbType.RefCursor).Direction = System.Data.ParameterDirection.Output;
                    OracleDataReader reader = listadoVentas.ExecuteReader();

                    var nueva = "<html><head><meta charset = 'utf-8'></head><body><table class='table' style='border: 1px solid black;'><thead><tr style='border: 1px solid black;'><th>Estado Venta</th><th>Fecha Emisión</th><th>Fecha Vencimiento</th><th>Subtotal</th><th>Descuento</th><th>Total</th><th>Tipo Documento</th><th>Nombre</th><th>Apellido</th></tr></thead><tbody>";

                    while (reader.Read())
                    {

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (i == 0)
                            {
                                nueva = nueva + "<tr>";
                            }
                            nueva = nueva + "<td style='border: 1px solid black;'>" + reader.GetOracleValue(i) + "</td>";

                            if (i == 8)
                            {
                                nueva = nueva + "</tr>";
                            }

                        }


                    }
                    reader.Close();

                    return Content(nueva + "</tbody></table></body></html>", "text/html");


                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Exception: {0}", ex.ToString());

                    objConexion.Close();
                }
                objConexion.Close();

            }
            return View(ventas);
        }

        // GET: ListadoVentas/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await _context.Ventas.FindAsync(id);
            if (ventas == null)
            {
                return NotFound();
            }
            ViewData["Rutcliente"] = new SelectList(_context.Clientes, "Rut", "Rut", ventas.Rutcliente);
            ViewData["IdestadosVentas"] = new SelectList(_context.EstadosVentas, "IdestadosVentas", "IdestadosVentas", ventas.IdestadosVentas);
            ViewData["IdtipoDocumento"] = new SelectList(_context.TipoDocumento, "IdtipoDocumento", "IdtipoDocumento", ventas.IdtipoDocumento);
            return View(ventas);
        }

        // POST: ListadoVentas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Idventas,IdestadosVentas,Idcliente,Rutcliente,IdtipoDocumento,FechaEmision,FechaVencimiento,Subtotal,Descuento,Total,FechaInsert,FechaUpdate,FolioDocumento")] Ventas ventas)
        {
            if (id != ventas.Idventas)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ventas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentasExists(ventas.Idventas))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Rutcliente"] = new SelectList(_context.Clientes, "Rut", "Rut", ventas.Rutcliente);
            ViewData["IdestadosVentas"] = new SelectList(_context.EstadosVentas, "IdestadosVentas", "IdestadosVentas", ventas.IdestadosVentas);
            ViewData["IdtipoDocumento"] = new SelectList(_context.TipoDocumento, "IdtipoDocumento", "IdtipoDocumento", ventas.IdtipoDocumento);
            return View(ventas);
        }

        // GET: ListadoVentas/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ventas = await _context.Ventas
                .Include(v => v.Clientes)
                .Include(v => v.IdestadosVentasNavigation)
                .Include(v => v.IdtipoDocumentoNavigation)
                .FirstOrDefaultAsync(m => m.Idventas == id);
            if (ventas == null)
            {
                return NotFound();
            }

            return View(ventas);
        }

        // POST: ListadoVentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var ventas = await _context.Ventas.FindAsync(id);
            _context.Ventas.Remove(ventas);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VentasExists(decimal id)
        {
            return _context.Ventas.Any(e => e.Idventas == id);
        }
    }
}
