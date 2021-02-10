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
    public class LibrosController : Controller
    {
        private readonly ModelContext _context;

        public LibrosController(ModelContext context)
        {
            _context = context;
        }

        // GET: Libros
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Libros.Include(l => l.IdeditorialNavigation);
            return View(await modelContext.ToListAsync());
        }

        // GET: Libros/Details/5
        public async Task<IActionResult> Details(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libros = await _context.Libros
                .Include(l => l.IdeditorialNavigation)
                .FirstOrDefaultAsync(m => m.Idlibros == id);
            if (libros == null)
            {
                return NotFound();
            }

            return View(libros);
        }

        // GET: Libros/Create
        public IActionResult Create()
        {
            ViewData["Ideditorial"] = new SelectList(_context.Editorial, "Ideditorial", "NombreEditorial");
            return View();
        }

        // POST: Libros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idlibros,Ideditorial,Titulo,Autor,Precio,Stock,FechaInsert,Estado")] Libros libros)
        {
            if (ModelState.IsValid)
            {
                using (OracleConnection objConexion = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));Persist Security Info=True;User Id=system;Password=Angels123;"))
                {

                    OracleCommand objComando = new OracleCommand();
                    objComando.Connection = objConexion;
                    objComando.CommandText = "PRC_CREA_ACTUALIZA_LIBROS";
                    objComando.CommandType = System.Data.CommandType.StoredProcedure;
                    objComando.Parameters.Add("codLibro", OracleDbType.Int16).Value = 0;
                    objComando.Parameters["codLibro"].Size = 3;
                    objComando.Parameters.Add("codEditorial", OracleDbType.Int16).Value = libros.Ideditorial;
                    objComando.Parameters["codEditorial"].Size = 3;
                    objComando.Parameters.Add("tituloLibro", OracleDbType.Varchar2).Value = libros.Titulo;
                    objComando.Parameters["tituloLibro"].Size = 250;
                    objComando.Parameters.Add("autorLibro", OracleDbType.Varchar2).Value = libros.Autor;
                    objComando.Parameters["autorLibro"].Size = 80;
                    objComando.Parameters.Add("precioLibro", OracleDbType.Int16).Value = libros.Precio;
                    objComando.Parameters["precioLibro"].Size = 10;
                    objComando.Parameters.Add("stockLibro", OracleDbType.Int16).Value = libros.Stock;
                    objComando.Parameters["stockLibro"].Size = 10;
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
                            ModelState.AddModelError("Libros", Convert.ToString(objComando.Parameters["stringStatus"].Value));
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
            ViewData["Ideditorial"] = new SelectList(_context.Editorial, "Ideditorial", "NombreEditorial", libros.Ideditorial);
            return View(libros);
        }

        // GET: Libros/Edit/5
        public async Task<IActionResult> Edit(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libros = await _context.Libros.FindAsync(id);
            if (libros == null)
            {
                return NotFound();
            }
            ViewData["Ideditorial"] = new SelectList(_context.Editorial, "Ideditorial", "NombreEditorial", libros.Ideditorial);
            return View(libros);
        }

        // POST: Libros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(decimal id, [Bind("Idlibros,Ideditorial,Titulo,Autor,Precio,Stock,FechaInsert,Estado")] Libros libros)
        {
            if (id != libros.Idlibros)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (OracleConnection objConexion = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));Persist Security Info=True;User Id=system;Password=Angels123;"))
                {

                    OracleCommand objComando = new OracleCommand();
                    objComando.Connection = objConexion;
                    objComando.CommandText = "PRC_CREA_ACTUALIZA_LIBROS";
                    objComando.CommandType = System.Data.CommandType.StoredProcedure;
                    objComando.Parameters.Add("codLibro", OracleDbType.Int16).Value = id;
                    objComando.Parameters["codLibro"].Size = 3;
                    objComando.Parameters.Add("codEditorial", OracleDbType.Int16).Value = libros.Ideditorial;
                    objComando.Parameters["codEditorial"].Size = 3;
                    objComando.Parameters.Add("tituloLibro", OracleDbType.Varchar2).Value = libros.Titulo;
                    objComando.Parameters["tituloLibro"].Size = 250;
                    objComando.Parameters.Add("autorLibro", OracleDbType.Varchar2).Value = libros.Autor;
                    objComando.Parameters["autorLibro"].Size = 80;
                    objComando.Parameters.Add("precioLibro", OracleDbType.Int16).Value = libros.Precio;
                    objComando.Parameters["precioLibro"].Size = 10;
                    objComando.Parameters.Add("stockLibro", OracleDbType.Int16).Value = libros.Stock;
                    objComando.Parameters["stockLibro"].Size = 10;
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
                            ModelState.AddModelError("Libros", Convert.ToString(objComando.Parameters["stringStatus"].Value));
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
            ViewData["Ideditorial"] = new SelectList(_context.Editorial, "Ideditorial", "NombreEditorial", libros.Ideditorial);
            return View(libros);
        }

        // GET: Libros/Delete/5
        public async Task<IActionResult> Delete(decimal? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libros = await _context.Libros
                .Include(l => l.IdeditorialNavigation)
                .FirstOrDefaultAsync(m => m.Idlibros == id);
            if (libros == null)
            {
                return NotFound();
            }

            return View(libros);
        }

        // POST: Libros/Delete/5
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
                objComando.CommandText = "PRC_DESHABILITA_LIBRO";
                objComando.CommandType = System.Data.CommandType.StoredProcedure;
                objComando.Parameters.Add("codLibro", OracleDbType.Int16).Value = id;
                objComando.Parameters["codLibro"].Size = 3;
                objComando.Parameters.Add("estadoLibro", OracleDbType.Int16).Value = nuevoEstado;
                objComando.Parameters["estadoLibro"].Size = 1;
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
                        ModelState.AddModelError("Libros", Convert.ToString(objComando.Parameters["stringStatus"].Value));
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

        private bool LibrosExists(decimal id)
        {
            return _context.Libros.Any(e => e.Idlibros == id);
        }
    }
}
