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
    public class PlantillaConsentimientosInformadosController : Controller
    {
        private readonly HCOdontologicasContext _context;

        public PlantillaConsentimientosInformadosController(HCOdontologicasContext context)
        {
            _context = context;
        }

        // GET: PlantillaConsentimientosInformados
        public async Task<IActionResult> Index()
        {
            return View(await _context.PlantillaConsentimientoInformado.ToListAsync());
        }

        // GET: PlantillaConsentimientosInformados/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantillaConsentimientoInformado = await _context.PlantillaConsentimientoInformado
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (plantillaConsentimientoInformado == null)
            {
                return NotFound();
            }

            return View(plantillaConsentimientoInformado);
        }

        // GET: PlantillaConsentimientosInformados/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PlantillaConsentimientosInformados/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,Nombre,Descripcion")] PlantillaConsentimientoInformado plantillaConsentimientoInformado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plantillaConsentimientoInformado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(plantillaConsentimientoInformado);
        }

        // GET: PlantillaConsentimientosInformados/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantillaConsentimientoInformado = await _context.PlantillaConsentimientoInformado.FindAsync(id);
            if (plantillaConsentimientoInformado == null)
            {
                return NotFound();
            }
            return View(plantillaConsentimientoInformado);
        }

        // POST: PlantillaConsentimientosInformados/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Codigo,Nombre,Descripcion")] PlantillaConsentimientoInformado plantillaConsentimientoInformado)
        {
            if (id != plantillaConsentimientoInformado.Codigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plantillaConsentimientoInformado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlantillaConsentimientoInformadoExists(plantillaConsentimientoInformado.Codigo))
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
            return View(plantillaConsentimientoInformado);
        }

        // GET: PlantillaConsentimientosInformados/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantillaConsentimientoInformado = await _context.PlantillaConsentimientoInformado
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (plantillaConsentimientoInformado == null)
            {
                return NotFound();
            }

            return View(plantillaConsentimientoInformado);
        }

        // POST: PlantillaConsentimientosInformados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var plantillaConsentimientoInformado = await _context.PlantillaConsentimientoInformado.FindAsync(id);
            _context.PlantillaConsentimientoInformado.Remove(plantillaConsentimientoInformado);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlantillaConsentimientoInformadoExists(string id)
        {
            return _context.PlantillaConsentimientoInformado.Any(e => e.Codigo == id);
        }
    }
}
