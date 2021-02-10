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
    public class DctoLibrosController : Controller
    {
        private readonly ModelContext _context;

        public DctoLibrosController(ModelContext context)
        {
            _context = context;
        }

        // GET: DctoLibros
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.DctoLibros.Include(d => d.IddescuentosNavigation).Include(d => d.IdlibrosNavigation);
            return View(await modelContext.ToListAsync());
        }

        // GET: DctoLibros/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dctoLibros = await _context.DctoLibros
                .Include(d => d.IddescuentosNavigation)
                .Include(d => d.IdlibrosNavigation)
                .FirstOrDefaultAsync(m => m.IddctoLibros == id);
            if (dctoLibros == null)
            {
                return NotFound();
            }

            return View(dctoLibros);
        }

        // GET: DctoLibros/Create
        public IActionResult Create()
        {
            ViewData["Iddescuentos"] = new SelectList(_context.Descuentos, "Iddescuentos", "CodDescuento");
            ViewData["Idlibros"] = new SelectList(_context.Libros, "Idlibros", "Titulo");
            return View();
        }

        // POST: DctoLibros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IddctoLibros,Idlibros,Iddescuentos,FechaInsert")] DctoLibros dctoLibros)
        {
            if (ModelState.IsValid)
            {
                using (OracleConnection objConexion = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));Persist Security Info=True;User Id=system;Password=Angels123;"))
                {

                    OracleCommand objComando = new OracleCommand();
                    objComando.Connection = objConexion;
                    objComando.CommandText = "PRC_ASOCIA_DCTO_LIBROS";
                    objComando.CommandType = System.Data.CommandType.StoredProcedure;
                    objComando.Parameters.Add("idDescuentoLibros", OracleDbType.Decimal).Value = 0;
                    objComando.Parameters["idDescuentoLibros"].Size = 3;
                    objComando.Parameters.Add("idLibro", OracleDbType.Decimal).Value = dctoLibros.Idlibros;
                    objComando.Parameters["idLibro"].Size = 3;
                    objComando.Parameters.Add("idDescuento", OracleDbType.Decimal).Value = dctoLibros.Iddescuentos;
                    objComando.Parameters["idDescuento"].Size = 3;
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
                            ModelState.AddModelError("DctoLibros", Convert.ToString(objComando.Parameters["stringStatus"].Value));
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
            ViewData["Iddescuentos"] = new SelectList(_context.Descuentos, "Iddescuentos", "CodDescuento", dctoLibros.Iddescuentos);
            ViewData["Idlibros"] = new SelectList(_context.Libros, "Idlibros", "Titulo", dctoLibros.Idlibros);
            return View(dctoLibros);
        }

        // GET: DctoLibros/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dctoLibros = await _context.DctoLibros.FindAsync(id);
            if (dctoLibros == null)
            {
                return NotFound();
            }
            ViewData["Iddescuentos"] = new SelectList(_context.Descuentos, "Iddescuentos", "CodDescuento", dctoLibros.Iddescuentos);
            ViewData["Idlibros"] = new SelectList(_context.Libros, "Idlibros", "Titulo", dctoLibros.Idlibros);
            return View(dctoLibros);
        }

        // POST: DctoLibros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("IddctoLibros,Idlibros,Iddescuentos,FechaInsert")] DctoLibros dctoLibros)
        {
            if (id != dctoLibros.IddctoLibros)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (OracleConnection objConexion = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));Persist Security Info=True;User Id=system;Password=Angels123;"))
                {

                    OracleCommand objComando = new OracleCommand();
                    objComando.Connection = objConexion;
                    objComando.CommandText = "PRC_ASOCIA_DCTO_LIBROS";
                    objComando.CommandType = System.Data.CommandType.StoredProcedure;
                    objComando.Parameters.Add("idDescuentoLibros", OracleDbType.Decimal).Value = dctoLibros.IddctoLibros;
                    objComando.Parameters["idDescuentoLibros"].Size = 3;
                    objComando.Parameters.Add("idLibro", OracleDbType.Decimal).Value = dctoLibros.Idlibros;
                    objComando.Parameters["idLibro"].Size = 3;
                    objComando.Parameters.Add("idDescuento", OracleDbType.Decimal).Value = dctoLibros.Iddescuentos;
                    objComando.Parameters["idDescuento"].Size = 3;
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
                            ModelState.AddModelError("DctoLibros", Convert.ToString(objComando.Parameters["stringStatus"].Value));
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
            ViewData["Iddescuentos"] = new SelectList(_context.Descuentos, "Iddescuentos", "CodDescuento", dctoLibros.Iddescuentos);
            ViewData["Idlibros"] = new SelectList(_context.Libros, "Idlibros", "Autor", dctoLibros.Idlibros);
            return View(dctoLibros);
        }

        // GET: DctoLibros/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dctoLibros = await _context.DctoLibros
                .Include(d => d.IddescuentosNavigation)
                .Include(d => d.IdlibrosNavigation)
                .FirstOrDefaultAsync(m => m.IddctoLibros == id);
            if (dctoLibros == null)
            {
                return NotFound();
            }

            return View(dctoLibros);
        }

        // POST: DctoLibros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var dctoLibros = await _context.DctoLibros.FindAsync(id);
            _context.DctoLibros.Remove(dctoLibros);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DctoLibrosExists(decimal id)
        {
            return _context.DctoLibros.Any(e => e.IddctoLibros == id);
        }
    }
}
