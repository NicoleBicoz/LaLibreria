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
    public class DescuentosController : Controller
    {
        private readonly ModelContext _context;

        public DescuentosController(ModelContext context)
        {
            _context = context;
        }

        // GET: Descuentos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Descuentos.ToListAsync());
        }

        // GET: Descuentos/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var descuentos = await _context.Descuentos
                .FirstOrDefaultAsync(m => m.Iddescuentos == id);
            if (descuentos == null)
            {
                return NotFound();
            }

            return View(descuentos);
        }

        // GET: Descuentos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Descuentos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Iddescuentos,CodDescuento,Descripcion,Porcentaje,Estado,FechaInsert,FechaUpdate")] Descuentos descuentos)
        {
            if (ModelState.IsValid)
            {
                using (OracleConnection objConexion = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));Persist Security Info=True;User Id=system;Password=Angels123;"))
                {
                    var nuevoEstado = 0;

                    if (descuentos.Estado == true)
                    {
                        nuevoEstado = 1;
                    }

                    OracleCommand objComando = new OracleCommand();
                    objComando.Connection = objConexion;
                    objComando.CommandText = "PRC_CREA_ACTUALIZA_DESCUENTOS";
                    objComando.CommandType = System.Data.CommandType.StoredProcedure;
                    objComando.Parameters.Add("codigoDescuento", OracleDbType.Varchar2).Value = descuentos.CodDescuento;
                    objComando.Parameters["codigoDescuento"].Size = 10;
                    objComando.Parameters.Add("descripcionDescuento", OracleDbType.Varchar2).Value = descuentos.Descripcion;
                    objComando.Parameters["descripcionDescuento"].Size = 20;
                    objComando.Parameters.Add("porcentajeDescuento", OracleDbType.Decimal).Value = descuentos.Porcentaje;
                    objComando.Parameters["porcentajeDescuento"].Size = 3;
                    objComando.Parameters.Add("estadoDescuento", OracleDbType.Decimal).Value = nuevoEstado;
                    objComando.Parameters["estadoDescuento"].Size = 1;
                    objComando.Parameters.Add("codStatus", OracleDbType.Int16).Direction = System.Data.ParameterDirection.Output;
                    objComando.Parameters["codStatus"].Value = 500;
                    objComando.Parameters["codStatus"].Size = 3;
                    objComando.Parameters.Add("stringStatus", OracleDbType.NVarchar2).Direction = System.Data.ParameterDirection.Output;
                    objComando.Parameters["stringStatus"].Value = "Error. Editorial no encontrada para su actualización, verifique código interno.";
                    objComando.Parameters["stringStatus"].Size = 100;

                    try
                    {
                        objConexion.Open();
                        objComando.ExecuteNonQuery();
                        var codigo = objComando.Parameters["codStatus"].Value;

                        if (Convert.ToString(codigo) == "200")
                        {
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            ModelState.AddModelError("Descuentos", Convert.ToString(objComando.Parameters["stringStatus"].Value));
                            return BadRequest(ModelState);
                        }
                        //objReader.NextResult();
                        //Console.WriteLine(objReader);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Exception: {0}", ex.ToString());
                    }
                    objConexion.Close();

                }
            }
            return View(descuentos);
        }

        // GET: Descuentos/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var descuentos = await _context.Descuentos.FindAsync(id);
            if (descuentos == null)
            {
                return NotFound();
            }
            return View(descuentos);
        }

        // POST: Descuentos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Iddescuentos,CodDescuento,Descripcion,Porcentaje,Estado,FechaInsert,FechaUpdate")] Descuentos descuentos)
        {
            if (id != descuentos.Iddescuentos)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (OracleConnection objConexion = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));Persist Security Info=True;User Id=system;Password=Angels123;"))
                {
                    var nuevoEstado = 0;

                    if (descuentos.Estado == true)
                    {
                        nuevoEstado = 1;
                    }

                    OracleCommand objComando = new OracleCommand();
                    objComando.Connection = objConexion;
                    objComando.CommandText = "PRC_CREA_ACTUALIZA_DESCUENTOS";
                    objComando.CommandType = System.Data.CommandType.StoredProcedure;
                    objComando.Parameters.Add("codigoDescuento", OracleDbType.Varchar2).Value = descuentos.CodDescuento;
                    objComando.Parameters["codigoDescuento"].Size = 10;
                    objComando.Parameters.Add("descripcionDescuento", OracleDbType.Varchar2).Value = descuentos.Descripcion;
                    objComando.Parameters["descripcionDescuento"].Size = 20;
                    objComando.Parameters.Add("porcentajeDescuento", OracleDbType.Decimal).Value = descuentos.Porcentaje;
                    objComando.Parameters["porcentajeDescuento"].Size = 3;
                    objComando.Parameters.Add("estadoDescuento", OracleDbType.Decimal).Value = nuevoEstado;
                    objComando.Parameters["estadoDescuento"].Size = 1;
                    objComando.Parameters.Add("codStatus", OracleDbType.Int16).Direction = System.Data.ParameterDirection.Output;
                    objComando.Parameters["codStatus"].Value = 500;
                    objComando.Parameters["codStatus"].Size = 3;
                    objComando.Parameters.Add("stringStatus", OracleDbType.NVarchar2).Direction = System.Data.ParameterDirection.Output;
                    objComando.Parameters["stringStatus"].Value = "Error. Editorial no encontrada para su actualización, verifique código interno.";
                    objComando.Parameters["stringStatus"].Size = 100;

                    try
                    {
                        objConexion.Open();
                        objComando.ExecuteNonQuery();
                        var codigo = objComando.Parameters["codStatus"].Value;

                        if (Convert.ToString(codigo) == "200")
                        {
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            ModelState.AddModelError("Descuentos", Convert.ToString(objComando.Parameters["stringStatus"].Value));
                            return BadRequest(ModelState);
                        }
                        //objReader.NextResult();
                        //Console.WriteLine(objReader);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Exception: {0}", ex.ToString());
                    }
                    objConexion.Close();

                }
                return RedirectToAction(nameof(Index));
            }
            return View(descuentos);
        }

        // GET: Descuentos/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var descuentos = await _context.Descuentos
                .FirstOrDefaultAsync(m => m.Iddescuentos == id);
            if (descuentos == null)
            {
                return NotFound();
            }

            return View(descuentos);
        }

        // POST: Descuentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id, bool estado)
        {
            using (OracleConnection objConexion = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));Persist Security Info=True;User Id=system;Password=Angels123;"))
            {
                var nuevoEstado = 0;

                if (estado == false)
                {
                    nuevoEstado = 1;
                }

                OracleCommand objComando = new OracleCommand();
                objComando.Connection = objConexion;
                objComando.CommandText = "PRC_DESHABILITA_DESCUENTO";
                objComando.CommandType = System.Data.CommandType.StoredProcedure;
                objComando.Parameters.Add("idDescuento", OracleDbType.Int16).Value = id;
                objComando.Parameters["idDescuento"].Size = 3;
                objComando.Parameters.Add("estadoDescuento", OracleDbType.Int16).Value = nuevoEstado;
                objComando.Parameters["estadoDescuento"].Size = 1;
                objComando.Parameters.Add("codStatus", OracleDbType.Int16).Direction = System.Data.ParameterDirection.Output;
                objComando.Parameters["codStatus"].Value = 500;
                objComando.Parameters["codStatus"].Size = 3;
                objComando.Parameters.Add("stringStatus", OracleDbType.NVarchar2).Direction = System.Data.ParameterDirection.Output;
                objComando.Parameters["stringStatus"].Value = "Error. Editorial no encontrada para su actualización, verifique código interno.";
                objComando.Parameters["stringStatus"].Size = 100;

                try
                {
                    objConexion.Open();
                    objComando.ExecuteNonQuery();
                    var codigo = objComando.Parameters["codStatus"].Value;

                    if (Convert.ToString(codigo) == "200")
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("Descuentos", Convert.ToString(objComando.Parameters["stringStatus"].Value));
                        return BadRequest(ModelState);
                    }
                    //objReader.NextResult();
                    //Console.WriteLine(objReader);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Exception: {0}", ex.ToString());
                }
                objConexion.Close();

            }
            return RedirectToAction(nameof(Index));
        }

        private bool DescuentosExists(decimal id)
        {
            return _context.Descuentos.Any(e => e.Iddescuentos == id);
        }
    }
}
