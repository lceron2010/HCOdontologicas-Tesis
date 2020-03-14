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
    public class PlantillasCorreosElectronicosController : Controller
    {
        private readonly HCOdontologicasContext _context;

        public PlantillasCorreosElectronicosController(HCOdontologicasContext context)
        {
            _context = context;
        }

        // GET: PlantillasCorreosElectronicos
        public async Task<IActionResult> Index()
        {
            return View(await _context.PlantillaCorreoElectronico.ToListAsync());
        }

        // GET: PlantillasCorreosElectronicos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantillaCorreoElectronico = await _context.PlantillaCorreoElectronico
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (plantillaCorreoElectronico == null)
            {
                return NotFound();
            }

            return View(plantillaCorreoElectronico);
        }

        // GET: PlantillasCorreosElectronicos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PlantillasCorreosElectronicos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,Nombre,Asunto,Cuerpo,Comentario")] PlantillaCorreoElectronico plantillaCorreoElectronico)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plantillaCorreoElectronico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(plantillaCorreoElectronico);
        }

        // GET: PlantillasCorreosElectronicos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantillaCorreoElectronico = await _context.PlantillaCorreoElectronico.FindAsync(id);
            if (plantillaCorreoElectronico == null)
            {
                return NotFound();
            }
            return View(plantillaCorreoElectronico);
        }

        // POST: PlantillasCorreosElectronicos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Codigo,Nombre,Asunto,Cuerpo,Comentario")] PlantillaCorreoElectronico plantillaCorreoElectronico)
        {
            if (id != plantillaCorreoElectronico.Codigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plantillaCorreoElectronico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlantillaCorreoElectronicoExists(plantillaCorreoElectronico.Codigo))
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
            return View(plantillaCorreoElectronico);
        }

        // GET: PlantillasCorreosElectronicos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantillaCorreoElectronico = await _context.PlantillaCorreoElectronico
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (plantillaCorreoElectronico == null)
            {
                return NotFound();
            }

            return View(plantillaCorreoElectronico);
        }

        // POST: PlantillasCorreosElectronicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plantillaCorreoElectronico = await _context.PlantillaCorreoElectronico.FindAsync(id);
            _context.PlantillaCorreoElectronico.Remove(plantillaCorreoElectronico);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlantillaCorreoElectronicoExists(int id)
        {
            return _context.PlantillaCorreoElectronico.Any(e => e.Codigo == id);
        }
    }
}
