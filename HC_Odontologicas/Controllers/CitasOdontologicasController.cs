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
    public class CitasOdontologicasController : Controller
    {
        private readonly HCOdontologicasContext _context;

        public CitasOdontologicasController(HCOdontologicasContext context)
        {
            _context = context;
        }

        // GET: CitasOdontologicas
        public async Task<IActionResult> Index()
        {
            var hCOdontologicasContext = _context.CitaOdontologica.Include(c => c.Paciente).Include(c => c.Personal);
            return View(await hCOdontologicasContext.ToListAsync());
        }

        // GET: CitasOdontologicas/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var citaOdontologica = await _context.CitaOdontologica
                .Include(c => c.Paciente)
                .Include(c => c.Personal)
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (citaOdontologica == null)
            {
                return NotFound();
            }

            return View(citaOdontologica);
        }

        // GET: CitasOdontologicas/Create
        public IActionResult Create()
        {
            ViewData["CodigoPaciente"] = new SelectList(_context.Paciente, "Codigo", "Codigo");
            ViewData["CodigoPersonal"] = new SelectList(_context.Personal, "Codigo", "Codigo");
            return View();
        }

        // POST: CitasOdontologicas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,CodigoPaciente,CodigoPersonal,FechaCreacion,Observaciones,Estado,FechaInicio,FechaFin,HoraInicio,HoraFin,UsuarioCreacion")] CitaOdontologica citaOdontologica)
        {
            if (ModelState.IsValid)
            {
                _context.Add(citaOdontologica);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodigoPaciente"] = new SelectList(_context.Paciente, "Codigo", "Codigo", citaOdontologica.CodigoPaciente);
            ViewData["CodigoPersonal"] = new SelectList(_context.Personal, "Codigo", "Codigo", citaOdontologica.CodigoPersonal);
            return View(citaOdontologica);
        }

        // GET: CitasOdontologicas/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var citaOdontologica = await _context.CitaOdontologica.FindAsync(id);
            if (citaOdontologica == null)
            {
                return NotFound();
            }
            ViewData["CodigoPaciente"] = new SelectList(_context.Paciente, "Codigo", "Codigo", citaOdontologica.CodigoPaciente);
            ViewData["CodigoPersonal"] = new SelectList(_context.Personal, "Codigo", "Codigo", citaOdontologica.CodigoPersonal);
            return View(citaOdontologica);
        }

        // POST: CitasOdontologicas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Codigo,CodigoPaciente,CodigoPersonal,FechaCreacion,Observaciones,Estado,FechaInicio,FechaFin,HoraInicio,HoraFin,UsuarioCreacion")] CitaOdontologica citaOdontologica)
        {
            if (id != citaOdontologica.Codigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(citaOdontologica);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CitaOdontologicaExists(citaOdontologica.Codigo))
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
            ViewData["CodigoPaciente"] = new SelectList(_context.Paciente, "Codigo", "Codigo", citaOdontologica.CodigoPaciente);
            ViewData["CodigoPersonal"] = new SelectList(_context.Personal, "Codigo", "Codigo", citaOdontologica.CodigoPersonal);
            return View(citaOdontologica);
        }

        // GET: CitasOdontologicas/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var citaOdontologica = await _context.CitaOdontologica
                .Include(c => c.Paciente)
                .Include(c => c.Personal)
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (citaOdontologica == null)
            {
                return NotFound();
            }

            return View(citaOdontologica);
        }

        // POST: CitasOdontologicas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var citaOdontologica = await _context.CitaOdontologica.FindAsync(id);
            _context.CitaOdontologica.Remove(citaOdontologica);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CitaOdontologicaExists(string id)
        {
            return _context.CitaOdontologica.Any(e => e.Codigo == id);
        }
    }
}
