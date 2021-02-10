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
    public class VentasController : Controller
    {
        private readonly ModelContext _context;

        public VentasController(ModelContext context)
        {
            _context = context;
        }


        // GET: Ventas
        public async Task<IActionResult> Index()
        {
            var modelContext = _context.Ventas.Include(v => v.Clientes).Include(v => v.IdestadosVentasNavigation).Include(v => v.IdtipoDocumentoNavigation);
            return View(await modelContext.ToListAsync());
        }

        // GET: Ventas/Details/5
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

        // GET: Ventas/Create
        public IActionResult Create()
        {
            ViewData["Rutcliente"] = new SelectList(_context.Clientes, "Rut", "Rut");
            ViewData["IdestadosVentas"] = new SelectList(_context.EstadosVentas, "IdestadosVentas", "Descripcion");
            ViewData["IdtipoDocumento"] = new SelectList(_context.TipoDocumento, "IdtipoDocumento", "Descripcion");
            return View();
        }

        // POST: Ventas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Idventas,IdestadosVentas,Idcliente,Rutcliente,IdtipoDocumento,FechaEmision,FechaVencimiento,Subtotal,Descuento,Total,FechaInsert,FechaUpdate,FolioDocumento")] Ventas ventas)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("Create", "DetalleVentas", new { rut = ventas.Rutcliente, fechaEmision = ventas.FechaEmision, FechaVencimiento = ventas.FechaVencimiento, idTipoDoc = ventas.IdtipoDocumento, idEstadoVenta = ventas.IdestadosVentas, idCliente = ventas.Idcliente });
                
                //return RedirectToAction(nameof(Index));
            }
            ViewData["Rutcliente"] = new SelectList(_context.Clientes, "Rut", "Rut", ventas.Rutcliente);
            ViewData["IdestadosVentas"] = new SelectList(_context.EstadosVentas, "IdestadosVentas", "Descripcion", ventas.IdestadosVentas);
            ViewData["IdtipoDocumento"] = new SelectList(_context.TipoDocumento, "IdtipoDocumento", "Descripcion", ventas.IdtipoDocumento);
            return View(ventas);
        }

        // GET: Ventas/Edit/5
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
            ViewData["IdestadosVentas"] = new SelectList(_context.EstadosVentas, "IdestadosVentas", "Descripcion", ventas.IdestadosVentas);
            ViewData["IdtipoDocumento"] = new SelectList(_context.TipoDocumento, "IdtipoDocumento", "Descripcion", ventas.IdtipoDocumento);
            return View(ventas);
        }

        // POST: Ventas/Edit/5
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
            ViewData["IdestadosVentas"] = new SelectList(_context.EstadosVentas, "IdestadosVentas", "Descripcion", ventas.IdestadosVentas);
            ViewData["IdtipoDocumento"] = new SelectList(_context.TipoDocumento, "IdtipoDocumento", "Descripcion", ventas.IdtipoDocumento);
            return View(ventas);
        }

        // GET: Ventas/Delete/5
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

        // POST: Ventas/Delete/5
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
