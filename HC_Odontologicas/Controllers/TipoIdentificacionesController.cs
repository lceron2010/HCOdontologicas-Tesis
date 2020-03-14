using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HC_Odontologicas.Models;

namespace HC_Odontologicas.Controllers
{
    public class TipoIdentificacionesController : Controller
    {
        private readonly HCOdontologicasContext _context;

        public TipoIdentificacionesController(HCOdontologicasContext context)
        {
            _context = context;
        }

        // GET: TipoIdentificaciones
        public async Task<IActionResult> Index()
        {
            return View(await _context.TipoIdentificacion.ToListAsync());
        }

        // GET: TipoIdentificaciones/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoIdentificacion = await _context.TipoIdentificacion
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (tipoIdentificacion == null)
            {
                return NotFound();
            }

            return View(tipoIdentificacion);
        }

        // GET: TipoIdentificaciones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TipoIdentificaciones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,Nombre,Descripcion")] TipoIdentificacion tipoIdentificacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tipoIdentificacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tipoIdentificacion);
        }

        // GET: TipoIdentificaciones/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoIdentificacion = await _context.TipoIdentificacion.FindAsync(id);
            if (tipoIdentificacion == null)
            {
                return NotFound();
            }
            return View(tipoIdentificacion);
        }

        // POST: TipoIdentificaciones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Codigo,Nombre,Descripcion")] TipoIdentificacion tipoIdentificacion)
        {
            if (id != tipoIdentificacion.Codigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tipoIdentificacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TipoIdentificacionExists(tipoIdentificacion.Codigo))
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
            return View(tipoIdentificacion);
        }

        // GET: TipoIdentificaciones/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipoIdentificacion = await _context.TipoIdentificacion
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (tipoIdentificacion == null)
            {
                return NotFound();
            }

            return View(tipoIdentificacion);
        }

        // POST: TipoIdentificaciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var tipoIdentificacion = await _context.TipoIdentificacion.FindAsync(id);
            _context.TipoIdentificacion.Remove(tipoIdentificacion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TipoIdentificacionExists(string id)
        {
            return _context.TipoIdentificacion.Any(e => e.Codigo == id);
        }
    }
}
