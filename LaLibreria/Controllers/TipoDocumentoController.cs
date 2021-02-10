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
    public class TipoDocumentoController : Controller
    {
        private readonly ModelContext _context;

        public TipoDocumentoController(ModelContext context)
        {
            _context = context;
        }

        // GET: TipoDocumento
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoDocumento.ToListAsync());
        }

        // GET: TipoDocumento/Details/5
        public async Task<IActionResult> Details(byte? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoDocumento = await _context.TipoDocumento
                .FirstOrDefaultAsync(m => m.IdtipoDocumento == id);
            if (tipoDocumento == null)
            {
                return NotFound();
            }

            return View(tipoDocumento);
        }

        // GET: TipoDocumento/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoDocumento/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdtipoDocumento,CodigoSii,Descripcion,FechaInsert")] TipoDocumento tipoDocumento)
        {
            if (ModelState.IsValid)
            {
                using (OracleConnection objConexion = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));Persist Security Info=True;User Id=system;Password=Angels123;"))
                {

                    OracleCommand objComando = new OracleCommand();
                    objComando.Connection = objConexion;
                    objComando.CommandText = "INSERT INTO TIPO_DOCUMENTO (IDTIPO_DOCUMENTO, CODIGO_SII, DESCRIPCION) VALUES (SEC_TIPODOCUMENTO.nextval," + tipoDocumento.CodigoSii +",'"+ tipoDocumento.Descripcion +"')";

                    try
                    {
                        objConexion.Open();
                        objComando.ExecuteNonQuery();
                        objConexion.Close();
                            return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Exception: {0}", ex.ToString());
                    }

                }
                return RedirectToAction(nameof(Index));
            }
            return View(tipoDocumento);
        }

        // GET: TipoDocumento/Edit/5
        public async Task<IActionResult> Edit(byte? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoDocumento = await _context.TipoDocumento.FindAsync(id);
            if (tipoDocumento == null)
            {
                return NotFound();
            }
            return View(tipoDocumento);
        }

        // POST: TipoDocumento/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(byte id, [Bind("IdtipoDocumento,CodigoSii,Descripcion,FechaInsert")] TipoDocumento tipoDocumento)
        {
            if (id != tipoDocumento.IdtipoDocumento)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (OracleConnection objConexion = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));Persist Security Info=True;User Id=system;Password=Angels123;"))
                {

                    string sql = "UPDATE TIPO_DOCUMENTO SET CODIGO_SII = "+ tipoDocumento.CodigoSii + ", DESCRIPCION = '"+ tipoDocumento.Descripcion + "' WHERE IDTIPO_DOCUMENTO = "+tipoDocumento.IdtipoDocumento+"";
                    OracleCommand objComando = new OracleCommand(sql, objConexion);

                    try
                    {
                        objConexion.Open();
                        objComando.ExecuteNonQuery();
                        objConexion.Close();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        objConexion.Close();
                        System.Console.WriteLine("Exception: {0}", ex.ToString());
                    }

                }
                return RedirectToAction(nameof(Index));
            }
            return View(tipoDocumento);
        }

        // GET: TipoDocumento/Delete/5
        public async Task<IActionResult> Delete(byte? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoDocumento = await _context.TipoDocumento
                .FirstOrDefaultAsync(m => m.IdtipoDocumento == id);
            if (tipoDocumento == null)
            {
                return NotFound();
            }

            return View(tipoDocumento);
        }

        // POST: TipoDocumento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(byte id)
        {
            var tipoDocumento = await _context.TipoDocumento.FindAsync(id);
            _context.TipoDocumento.Remove(tipoDocumento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoDocumentoExists(byte id)
        {
            return _context.TipoDocumento.Any(e => e.IdtipoDocumento == id);
        }
    }
}
