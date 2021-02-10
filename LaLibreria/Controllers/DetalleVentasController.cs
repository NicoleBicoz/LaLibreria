using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaLibreria.Models;
using Oracle.ManagedDataAccess.Client;

namespace LaLibreria.Controllers
{
    public class DetalleVentasController : Controller
    {
        private readonly ModelContext _context;

        public DetalleVentasController(ModelContext context)
        {
            _context = context;
        }

        // GET: DetalleVentas
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.DetalleVentas.Include(d => d.IdlibrosNavigation).Include(d => d.IdventasNavigation);
            return View(await modelContext.ToListAsync());
        }

        // GET: DetalleVentas/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleVentas = await _context.DetalleVentas
                .Include(d => d.IdlibrosNavigation)
                .Include(d => d.IdventasNavigation)
                .FirstOrDefaultAsync(m => m.IddetalleVentas == id);
            if (detalleVentas == null)
            {
                return NotFound();
            }

            return View(detalleVentas);
        }

        // GET: DetalleVentas/Create
        public IActionResult Create1()
        {
            ViewData["Idlibros"] = new SelectList(_context.Libros, "Idlibros", "Autor");
            ViewData["Idventas"] = new SelectList(_context.Ventas, "Idventas", "Rutcliente");
            return View();
        }


        public ActionResult Create(string rut, DateTime fechaEmision, DateTime fechaVencimiento, string tipoDoc, int idTipoDoc, int idEstadoVenta, int idCliente)
        {

            ViewData["Rut"] = rut;
            ViewData["tipoDoc"] = tipoDoc;
            ViewData["idTipoDoc"] = idTipoDoc;
            ViewData["FechaEmision"] = fechaEmision;
            ViewData["FechaVcto"] = fechaVencimiento;
            ViewData["idEstadoVenta"] = idEstadoVenta;
            ViewData["idCliente"] = idCliente;
            ViewData["Idlibros"] = new SelectList(_context.Libros, "Idlibros", "Titulo");
            ViewData["Idventas"] = new SelectList(_context.Ventas, "Idventas", "Rutcliente");

            traeDatosLibro(1);

            return View();
        }

        public void traeDatosLibro(int Idlibros) 
        {
            using (OracleConnection objConexion = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));Persist Security Info=True;User Id=system;Password=Angels123;"))
            {

                OracleCommand objComando = new OracleCommand();
                objComando.Connection = objConexion;
                objComando.CommandText = "select libros.precio as PrecioUnitario, NVL(dcto_libros.iddcto_libros,0) as idDescuento, NVL(descuentos.porcentaje,0) as Porcentaje from libros left join dcto_libros on(libros.idlibros = dcto_libros.idlibros) left join descuentos on(descuentos.iddescuentos = dcto_libros.iddescuentos) where libros.idlibros=" + Idlibros;

                try
                {
                    objConexion.Open();
                    OracleDataReader reader = objComando.ExecuteReader();
                    string idDescuento = ""; string precioUnitario = ""; string porcentaje = ""; decimal total; decimal descuento;

                    while (reader.Read())
                    {
                        porcentaje = reader["Porcentaje"].ToString();
                        idDescuento = reader["idDescuento"].ToString();
                        precioUnitario = reader["PrecioUnitario"].ToString();

                    }

                    descuento = Math.Round((Decimal.Parse(porcentaje) / 100) * Decimal.Parse(precioUnitario));
                    total = Decimal.Parse(precioUnitario) - descuento;

                    ViewData["totalLibro"] = Math.Round(total);
                    ViewData["Descuento"] = porcentaje;
                    ViewData["subtotal"] = precioUnitario;
                    ViewData["precioLibro"] = precioUnitario;
                    ViewData["montoDcto"] = descuento;
                    ViewData["idDcto"] = idDescuento;

                    objConexion.Close();
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Exception: {0}", ex.ToString());
                }

            }
        }

        // POST: DetalleVentas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IddetalleVentas,Idventas,Idlibros,IddctoLibros,PrecioUnitario,Cantidad,Total")] DetalleVentas detalleVentas, string[] ventas)
        {
            if (ModelState.IsValid)
            {
                string hola = ventas[0] + " - " + ventas[1];
                /*
                ventas[0]: subtotal
                ventas[1]: descuento
                ventas[2]: rut
                ventas[3]: idDocumento
                ventas[4]: fechaemision
                ventas[5]: fechavcto
                ventas[6]: idCliente
                ventas[7]: montoDcto
                ventas[8]: idDcto
                ventas[9]: PrecioUnitario
                ventas[10]: Total

                 */

                using (OracleConnection objConexion = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));Persist Security Info=True;User Id=system;Password=Angels123;"))
                {

                    OracleCommand objCliente = new OracleCommand();
                    objCliente.Connection = objConexion;
                    objCliente.CommandText = "select idclientes from clientes where rut='"+ ventas[2] + "'";
                    string idCliente = "";
                    try
                    {
                        objConexion.Open();
                        OracleDataReader reader = objCliente.ExecuteReader();

                        while (reader.Read())
                        {

                            idCliente = reader["idclientes"].ToString();

                        }


                        objConexion.Close();
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Exception: {0}", ex.ToString());
                    }


                    OracleCommand objComando = new OracleCommand();
                    objComando.Connection = objConexion;
                    objComando.CommandText = "PRC_GENERA_VENTA";
                    objComando.CommandType = System.Data.CommandType.StoredProcedure;
                    objComando.Parameters.Add("idEstadoVentas", OracleDbType.Decimal).Value = detalleVentas.Cantidad;
                    objComando.Parameters["idEstadoVentas"].Size = 3;
                    objComando.Parameters.Add("idCliente", OracleDbType.Decimal).Value = Convert.ToInt32(idCliente);
                    objComando.Parameters["idCliente"].Size = 3;
                    objComando.Parameters.Add("rut", OracleDbType.Varchar2).Value = Convert.ToString(ventas[2]);
                    objComando.Parameters["rut"].Size = 10;
                    objComando.Parameters.Add("idTipoDoc", OracleDbType.Decimal).Value = Convert.ToInt32(ventas[3]);
                    objComando.Parameters["idTipoDoc"].Size = 3;
                    objComando.Parameters.Add("fechaEmis", OracleDbType.Date).Value = Convert.ToDateTime(ventas[4]);
                    objComando.Parameters.Add("fechaVcto", OracleDbType.Date).Value = Convert.ToDateTime(ventas[5]);
                    objComando.Parameters.Add("subtotal", OracleDbType.Decimal).Value = Convert.ToInt32(ventas[0]);
                    objComando.Parameters["subtotal"].Size = 10;
                    objComando.Parameters.Add("dcto", OracleDbType.Decimal).Value = Convert.ToInt32(ventas[7]);
                    objComando.Parameters["dcto"].Size = 10;
                    objComando.Parameters.Add("total", OracleDbType.Decimal).Value = Convert.ToInt32(ventas[10]);
                    objComando.Parameters["total"].Size = 10;
                    objComando.Parameters.Add("responseIdVenta", OracleDbType.Int16).Direction = System.Data.ParameterDirection.Output;
                    objComando.Parameters["responseIdVenta"].Value = 0;
                    objComando.Parameters["responseIdVenta"].Size = 3;
                    objComando.Parameters.Add("mensajeProcedimiento", OracleDbType.NVarchar2).Direction = System.Data.ParameterDirection.Output;
                    objComando.Parameters["mensajeProcedimiento"].Value = "Error. Editorial no encontrada para su actualización, verifique código interno.";
                    objComando.Parameters["mensajeProcedimiento"].Size = 100;

                    try
                    {
                        objConexion.Open();
                        objComando.ExecuteNonQuery();
                        string codigo = Convert.ToString(objComando.Parameters["responseIdVenta"].Value);

                        objConexion.Close();
                        if (Convert.ToDecimal(codigo) > 0)
                        {
                            OracleCommand nuevoObjeto = new OracleCommand();
                            nuevoObjeto.Connection = objConexion;
                            nuevoObjeto.CommandText = "PRC_DETALLE_VENTA";
                            nuevoObjeto.CommandType = System.Data.CommandType.StoredProcedure;
                            nuevoObjeto.Parameters.Add("idVenta", OracleDbType.Decimal).Value = codigo;
                            nuevoObjeto.Parameters["idVenta"].Size = 3;
                            nuevoObjeto.Parameters.Add("idLibro", OracleDbType.Decimal).Value = detalleVentas.Idlibros;
                            nuevoObjeto.Parameters["idLibro"].Size = 3;
                            nuevoObjeto.Parameters.Add("idDctoLibro", OracleDbType.Decimal).Value = ventas[8];
                            nuevoObjeto.Parameters["idDctoLibro"].Size = 3;
                            nuevoObjeto.Parameters.Add("precioUnitario", OracleDbType.Decimal).Value = ventas[9];
                            nuevoObjeto.Parameters["precioUnitario"].Size = 3;
                            nuevoObjeto.Parameters.Add("cantidad", OracleDbType.Decimal).Value = detalleVentas.Cantidad;
                            nuevoObjeto.Parameters["cantidad"].Size = 10;
                            nuevoObjeto.Parameters.Add("total", OracleDbType.Decimal).Value = ventas[10];
                            nuevoObjeto.Parameters["total"].Size = 10;
                            nuevoObjeto.Parameters.Add("codigoRespuesta", OracleDbType.Int16).Direction = System.Data.ParameterDirection.Output;
                            nuevoObjeto.Parameters["codigoRespuesta"].Value = 500;
                            nuevoObjeto.Parameters["codigoRespuesta"].Size = 3;
                            nuevoObjeto.Parameters.Add("mensajeProcedimiento", OracleDbType.NVarchar2).Direction = System.Data.ParameterDirection.Output;
                            nuevoObjeto.Parameters["mensajeProcedimiento"].Value = "Error. Editorial no encontrada para su actualización, verifique código interno.";
                            nuevoObjeto.Parameters["mensajeProcedimiento"].Size = 100;


                            objConexion.Open();
                            nuevoObjeto.ExecuteNonQuery();
                            var codRespuesta = nuevoObjeto.Parameters["codigoRespuesta"].Value;

                            objConexion.Close();

                            if (Convert.ToString(codRespuesta) == "200")
                            {
                                OracleCommand actualizaStock = new OracleCommand();
                                actualizaStock.Connection = objConexion;
                                actualizaStock.CommandText = "PRC_ACTUALIZASTOCK";
                                actualizaStock.CommandType = System.Data.CommandType.StoredProcedure;
                                actualizaStock.Parameters.Add("idLibro", OracleDbType.Decimal).Value = detalleVentas.Idlibros;
                                actualizaStock.Parameters["idLibro"].Size = 3;
                                actualizaStock.Parameters.Add("cantidad", OracleDbType.Decimal).Value = detalleVentas.Cantidad;
                                actualizaStock.Parameters["cantidad"].Size = 3;
                                actualizaStock.Parameters.Add("codigoRespuesta", OracleDbType.Int16).Direction = System.Data.ParameterDirection.Output;
                                actualizaStock.Parameters["codigoRespuesta"].Value = 500;
                                actualizaStock.Parameters["codigoRespuesta"].Size = 3;
                                actualizaStock.Parameters.Add("mensajeProcedimiento", OracleDbType.NVarchar2).Direction = System.Data.ParameterDirection.Output;
                                actualizaStock.Parameters["mensajeProcedimiento"].Value = "Error. Editorial no encontrada para su actualización, verifique código interno.";
                                actualizaStock.Parameters["mensajeProcedimiento"].Size = 100;


                                objConexion.Open();
                                actualizaStock.ExecuteNonQuery();
                                return RedirectToAction("Index", "Ventas");
                                //return RedirectToAction(nameof(Index));
                            }
                            else
                            {
                                ModelState.AddModelError("DetalleVentas", Convert.ToString(objComando.Parameters["mensajeProcedimiento"].Value));
                                return BadRequest(ModelState);
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Exception: {0}", ex.ToString());
                    }

                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idlibros"] = new SelectList(_context.Libros, "Idlibros", "Autor", detalleVentas.Idlibros);
            ViewData["Idventas"] = new SelectList(_context.Ventas, "Idventas", "Rutcliente", detalleVentas.Idventas);
            return View(detalleVentas);
        }

        // GET: DetalleVentas/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleVentas = await _context.DetalleVentas.FindAsync(id);
            if (detalleVentas == null)
            {
                return NotFound();
            }
            ViewData["Idlibros"] = new SelectList(_context.Libros, "Idlibros", "Autor", detalleVentas.Idlibros);
            ViewData["Idventas"] = new SelectList(_context.Ventas, "Idventas", "Rutcliente", detalleVentas.Idventas);
            return View(detalleVentas);
        }

        // POST: DetalleVentas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("IddetalleVentas,Idventas,Idlibros,IddctoLibros,PrecioUnitario,Cantidad,Total")] DetalleVentas detalleVentas)
        {
            if (id != detalleVentas.IddetalleVentas)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detalleVentas);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetalleVentasExists(detalleVentas.IddetalleVentas))
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
            ViewData["Idlibros"] = new SelectList(_context.Libros, "Idlibros", "Autor", detalleVentas.Idlibros);
            ViewData["Idventas"] = new SelectList(_context.Ventas, "Idventas", "Rutcliente", detalleVentas.Idventas);
            return View(detalleVentas);
        }

        // GET: DetalleVentas/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detalleVentas = await _context.DetalleVentas
                .Include(d => d.IdlibrosNavigation)
                .Include(d => d.IdventasNavigation)
                .FirstOrDefaultAsync(m => m.IddetalleVentas == id);
            if (detalleVentas == null)
            {
                return NotFound();
            }

            return View(detalleVentas);
        }

        // POST: DetalleVentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var detalleVentas = await _context.DetalleVentas.FindAsync(id);
            _context.DetalleVentas.Remove(detalleVentas);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DetalleVentasExists(decimal id)
        {
            return _context.DetalleVentas.Any(e => e.IddetalleVentas == id);
        }
    }
}
