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
    public class PlantillasCertificadosMedicosController : Controller
    {
        private readonly HCOdontologicasContext _context;

        public PlantillasCertificadosMedicosController(HCOdontologicasContext context)
        {
            _context = context;
        }

        // GET: PlantillasCertificadosMedicos
        public async Task<IActionResult> Index()
        {
            return View(await _context.PlantillaCertificadoMedico.ToListAsync());
        }

        // GET: PlantillasCertificadosMedicos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantillaCertificadoMedico = await _context.PlantillaCertificadoMedico
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (plantillaCertificadoMedico == null)
            {
                return NotFound();
            }

            return View(plantillaCertificadoMedico);
        }

        // GET: PlantillasCertificadosMedicos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: PlantillasCertificadosMedicos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,Nombre,Descripcion")] PlantillaCertificadoMedico plantillaCertificadoMedico)
        {
            if (ModelState.IsValid)
            {
                _context.Add(plantillaCertificadoMedico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(plantillaCertificadoMedico);
        }

        // GET: PlantillasCertificadosMedicos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantillaCertificadoMedico = await _context.PlantillaCertificadoMedico.FindAsync(id);
            if (plantillaCertificadoMedico == null)
            {
                return NotFound();
            }
            return View(plantillaCertificadoMedico);
        }

        // POST: PlantillasCertificadosMedicos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Codigo,Nombre,Descripcion")] PlantillaCertificadoMedico plantillaCertificadoMedico)
        {
            if (id != plantillaCertificadoMedico.Codigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(plantillaCertificadoMedico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlantillaCertificadoMedicoExists(plantillaCertificadoMedico.Codigo))
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
            return View(plantillaCertificadoMedico);
        }

        // GET: PlantillasCertificadosMedicos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plantillaCertificadoMedico = await _context.PlantillaCertificadoMedico
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (plantillaCertificadoMedico == null)
            {
                return NotFound();
            }

            return View(plantillaCertificadoMedico);
        }

        // POST: PlantillasCertificadosMedicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plantillaCertificadoMedico = await _context.PlantillaCertificadoMedico.FindAsync(id);
            _context.PlantillaCertificadoMedico.Remove(plantillaCertificadoMedico);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlantillaCertificadoMedicoExists(int id)
        {
            return _context.PlantillaCertificadoMedico.Any(e => e.Codigo == id);
        }
    }
}
