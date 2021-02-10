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
    public class ListadoLibrosController : Controller
    {
        private readonly ModelContext _context;

        public ListadoLibrosController(ModelContext context)
        {
            _context = context;
        }

        // GET: ListadoLibros
        public async Task<IActionResult> Index()
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

                            nueva = nueva + "<td style='border: 1px solid black;'>" + reader.GetOracleValue(i) + "</td>";

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
            var modelContext = _context.Libros.Include(l => l.IdeditorialNavigation);
            return View(await modelContext.ToListAsync());
        }

        // GET: ListadoLibros/Details/5
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

        // GET: ListadoLibros/Create
        public IActionResult Create()
        {
            ViewData["Ideditorial"] = new SelectList(_context.Editorial, "Ideditorial", "NombreEditorial");
            return View();
        }

        // POST: ListadoLibros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idlibros,Ideditorial,Titulo,Autor,Precio,Stock,FechaInsert,Estado")] Libros libros)
        {
            if (ModelState.IsValid)
            {
                _context.Add(libros);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Ideditorial"] = new SelectList(_context.Editorial, "Ideditorial", "NombreEditorial", libros.Ideditorial);
            return View(libros);
        }

        // GET: ListadoLibros/Edit/5
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

        // POST: ListadoLibros/Edit/5
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
                try
                {
                    _context.Update(libros);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibrosExists(libros.Idlibros))
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
            ViewData["Ideditorial"] = new SelectList(_context.Editorial, "Ideditorial", "NombreEditorial", libros.Ideditorial);
            return View(libros);
        }

        // GET: ListadoLibros/Delete/5
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

        // POST: ListadoLibros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var libros = await _context.Libros.FindAsync(id);
            _context.Libros.Remove(libros);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibrosExists(decimal id)
        {
            return _context.Libros.Any(e => e.Idlibros == id);
        }
    }
}
