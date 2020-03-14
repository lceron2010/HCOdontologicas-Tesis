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
    public class PlantillasRecetasMedicasController : Controller
    {
        private readonly HCOdontologicasContext _context;

        public PlantillasRecetasMedicasController(HCOdontologicasContext context)
        {
            _context = context;
        }

        // GET: PlantillasRecetasMedicas
        public async Task<IActionResult> Index()
        {
            return View(await _context.PlantillaRecetaMedica.ToListAsync());
        }

        // GET: PlantillasRecetasMedicas/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantillaRecetaMedica = await _context.PlantillaRecetaMedica
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (plantillaRecetaMedica == null)
            {
                return NotFound();
            }

            return View(plantillaRecetaMedica);
        }

        // GET: PlantillasRecetasMedicas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PlantillasRecetasMedicas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,Nombre,Descripcion")] PlantillaRecetaMedica plantillaRecetaMedica)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plantillaRecetaMedica);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(plantillaRecetaMedica);
        }

        // GET: PlantillasRecetasMedicas/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantillaRecetaMedica = await _context.PlantillaRecetaMedica.FindAsync(id);
            if (plantillaRecetaMedica == null)
            {
                return NotFound();
            }
            return View(plantillaRecetaMedica);
        }

        // POST: PlantillasRecetasMedicas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Codigo,Nombre,Descripcion")] PlantillaRecetaMedica plantillaRecetaMedica)
        {
            if (id != plantillaRecetaMedica.Codigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plantillaRecetaMedica);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlantillaRecetaMedicaExists(plantillaRecetaMedica.Codigo))
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
            return View(plantillaRecetaMedica);
        }

        // GET: PlantillasRecetasMedicas/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantillaRecetaMedica = await _context.PlantillaRecetaMedica
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (plantillaRecetaMedica == null)
            {
                return NotFound();
            }

            return View(plantillaRecetaMedica);
        }

        // POST: PlantillasRecetasMedicas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var plantillaRecetaMedica = await _context.PlantillaRecetaMedica.FindAsync(id);
            _context.PlantillaRecetaMedica.Remove(plantillaRecetaMedica);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlantillaRecetaMedicaExists(string id)
        {
            return _context.PlantillaRecetaMedica.Any(e => e.Codigo == id);
        }
    }
}
