using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LaLibreria.Models;

namespace LaLibreria.Controllers
{
    public class ListadoDescuentosController : Controller
    {
        private readonly ModelContext _context;

        public ListadoDescuentosController(ModelContext context)
        {
            _context = context;
        }

        // GET: ListadoDescuentos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Descuentos.ToListAsync());
        }

        // GET: ListadoDescuentos/Details/5
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

        // GET: ListadoDescuentos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ListadoDescuentos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Iddescuentos,CodDescuento,Descripcion,Porcentaje,Estado,FechaInsert,FechaUpdate")] Descuentos descuentos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(descuentos);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(descuentos);
        }

        // GET: ListadoDescuentos/Edit/5
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

        // POST: ListadoDescuentos/Edit/5
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
                try
                {
                    _context.Update(descuentos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DescuentosExists(descuentos.Iddescuentos))
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
            return View(descuentos);
        }

        // GET: ListadoDescuentos/Delete/5
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

        // POST: ListadoDescuentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(decimal id)
        {
            var descuentos = await _context.Descuentos.FindAsync(id);
            _context.Descuentos.Remove(descuentos);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DescuentosExists(decimal id)
        {
            return _context.Descuentos.Any(e => e.Iddescuentos == id);
        }
    }
}
