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
    public class Cie10Controller : Controller
    {
        private readonly HCOdontologicasContext _context;

        public Cie10Controller(HCOdontologicasContext context)
        {
            _context = context;
        }

        // GET: Cie10
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cie10.ToListAsync());
        }

        // GET: Cie10/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cie10 = await _context.Cie10
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (cie10 == null)
            {
                return NotFound();
            }

            return View(cie10);
        }

        // GET: Cie10/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Cie10/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,CodigoInterno,Nombre,Descripcion")] Cie10 cie10)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cie10);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cie10);
        }

        // GET: Cie10/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cie10 = await _context.Cie10.FindAsync(id);
            if (cie10 == null)
            {
                return NotFound();
            }
            return View(cie10);
        }

        // POST: Cie10/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Codigo,CodigoInterno,Nombre,Descripcion")] Cie10 cie10)
        {
            if (id != cie10.Codigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cie10);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Cie10Exists(cie10.Codigo))
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
            return View(cie10);
        }

        // GET: Cie10/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cie10 = await _context.Cie10
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (cie10 == null)
            {
                return NotFound();
            }

            return View(cie10);
        }

        // POST: Cie10/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var cie10 = await _context.Cie10.FindAsync(id);
            _context.Cie10.Remove(cie10);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Cie10Exists(string id)
        {
            return _context.Cie10.Any(e => e.Codigo == id);
        }
    }
}
