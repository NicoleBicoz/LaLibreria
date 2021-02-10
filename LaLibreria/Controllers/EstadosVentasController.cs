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
    public class EstadosVentasController : Controller
    {
        private readonly ModelContext _context;

        public EstadosVentasController(ModelContext context)
        {
            _context = context;
        }

        // GET: EstadosVentas
        public async Task<IActionResult> Index()
        {
            return View(await _context.EstadosVentas.ToListAsync());
        }

        // GET: EstadosVentas/Details/5
        public async Task<IActionResult> Details(byte? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadosVentas = await _context.EstadosVentas
                .FirstOrDefaultAsync(m => m.IdestadosVentas == id);
            if (estadosVentas == null)
            {
                return NotFound();
            }

            return View(estadosVentas);
        }

        // GET: EstadosVentas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: EstadosVentas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdestadosVentas,CodEstado,Descripcion,FechaInsert")] EstadosVentas estadosVentas)
        {
            if (ModelState.IsValid)
            {
                using (OracleConnection objConexion = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));Persist Security Info=True;User Id=system;Password=Angels123;"))
                {

                    OracleCommand objComando = new OracleCommand();
                    objComando.Connection = objConexion;
                    objComando.CommandText = "INSERT INTO ESTADOS_VENTAS (IDESTADOS_VENTAS, COD_ESTADO, DESCRIPCION) VALUES (SEC_ESTADOSVENTAS.nextval," + estadosVentas.CodEstado + ",'" + estadosVentas.Descripcion + "')";

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
            return View(estadosVentas);
        }

        // GET: EstadosVentas/Edit/5
        public async Task<IActionResult> Edit(byte? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadosVentas = await _context.EstadosVentas.FindAsync(id);
            if (estadosVentas == null)
            {
                return NotFound();
            }
            return View(estadosVentas);
        }

        // POST: EstadosVentas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(byte id, [Bind("IdestadosVentas,CodEstado,Descripcion,FechaInsert")] EstadosVentas estadosVentas)
        {
            if (id != estadosVentas.IdestadosVentas)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                using (OracleConnection objConexion = new OracleConnection("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=xe)));Persist Security Info=True;User Id=system;Password=Angels123;"))
                {
                    string sql = "UPDATE ESTADOS_VENTAS SET COD_ESTADO=" + estadosVentas.CodEstado + ", DESCRIPCION='" + estadosVentas.Descripcion + "' WHERE IDESTADOS_VENTAS=" + estadosVentas.IdestadosVentas + "";
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
            return View(estadosVentas);
        }

        // GET: EstadosVentas/Delete/5
        public async Task<IActionResult> Delete(byte? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var estadosVentas = await _context.EstadosVentas
                .FirstOrDefaultAsync(m => m.IdestadosVentas == id);
            if (estadosVentas == null)
            {
                return NotFound();
            }

            return View(estadosVentas);
        }

        // POST: EstadosVentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(byte id)
        {
            var estadosVentas = await _context.EstadosVentas.FindAsync(id);
            _context.EstadosVentas.Remove(estadosVentas);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstadosVentasExists(byte id)
        {
            return _context.EstadosVentas.Any(e => e.IdestadosVentas == id);
        }
    }
}
