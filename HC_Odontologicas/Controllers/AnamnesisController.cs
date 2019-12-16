using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HC_Odontologicas.Models;
using System.Security.Claims;
using HC_Odontologicas.FuncionesGenerales;

namespace HC_Odontologicas.Controllers
{
    public class AnamnesisController : Controller
    {
		private readonly HCOdontologicasContext _context;
		private ValidacionesController validaciones;
		private readonly AuditoriaController _auditoria;

		public AnamnesisController(HCOdontologicasContext context)
        {
			_context = context;
			validaciones = new ValidacionesController(_context);
			_auditoria = new AuditoriaController(context);
		}

        // GET: Anamnesis
        public async Task<IActionResult> Index(string sortOrder, string Filter, int? page, string search)
        {
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				//Permisos de usuario
				var permisos = i.Claims.Where(c => c.Type == "Anamnesis").Select(c => c.Value).SingleOrDefault().Split(";");
				ViewData["Crear"] = Convert.ToBoolean(permisos[1]);
				ViewData["Editar"] = Convert.ToBoolean(permisos[2]);
				ViewData["Eliminar"] = Convert.ToBoolean(permisos[3]);
				ViewData["Exportar"] = Convert.ToBoolean(permisos[4]);

				if (Convert.ToBoolean(permisos[0]))
				{
					//ViewData["NombreSortParam"] = String.IsNullOrEmpty(sortOrder) ? "nombre_desc" : "";

					//permite mantener la busqueda introducida en el filtro de busqueda
					if (search != null)
						page = 1;
					else
						search = Filter;

					ViewData["Filter"] = search;
					ViewData["CurrentSort"] = sortOrder;
					var anamnesis = from c in _context.Anamnesis select c;

					if (!String.IsNullOrEmpty(search))
						//anamnesis = anamnesis.Where(s => s.Nombres.Contains(search));

					switch (sortOrder)
					{
						case "nombre_desc":
							anamnesis = anamnesis.OrderByDescending(s => s.MotivoConsulta);
							break;
						default:
							anamnesis = anamnesis.OrderBy(s => s.MotivoConsulta);
							break;

					}
					int pageSize = 10;
					return View(await Paginacion<Anamnesis>.CreateAsync(anamnesis, page ?? 1, pageSize)); // page devuelve valor si lo tiene caso contrario devuelve 1
				}
				else
				{
					return Redirect("../Home");
				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}
      
        // GET: Anamnesis/Create
        public IActionResult Create()
        {
            ViewData["CodigoHistoriaClinica"] = new SelectList(_context.HistoriaClinica, "Codigo", "Codigo");
            return View();
        }

        // POST: Anamnesis/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Codigo,CodigoHistoriaClinica,MotivoConsulta,EnfermedadActual,Alerta,AntecedentesQuirurgicos,Alergico,Medicamentos,Habitos,AntecedentesFamiliares,Fuma,Embarazada,GrupoSanguineo,Endocrino,Traumatologico,Fecha")] Anamnesis anamnesis)
        {
            if (ModelState.IsValid)
            {
                _context.Add(anamnesis);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CodigoHistoriaClinica"] = new SelectList(_context.HistoriaClinica, "Codigo", "Codigo", anamnesis.CodigoHistoriaClinica);
            return View(anamnesis);
        }

        // GET: Anamnesis/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anamnesis = await _context.Anamnesis.FindAsync(id);
            if (anamnesis == null)
            {
                return NotFound();
            }
            ViewData["CodigoHistoriaClinica"] = new SelectList(_context.HistoriaClinica, "Codigo", "Codigo", anamnesis.CodigoHistoriaClinica);
            return View(anamnesis);
        }

        // POST: Anamnesis/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Codigo,CodigoHistoriaClinica,MotivoConsulta,EnfermedadActual,Alerta,AntecedentesQuirurgicos,Alergico,Medicamentos,Habitos,AntecedentesFamiliares,Fuma,Embarazada,GrupoSanguineo,Endocrino,Traumatologico,Fecha")] Anamnesis anamnesis)
        {
            if (id != anamnesis.Codigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(anamnesis);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnamnesisExists(anamnesis.Codigo))
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
            ViewData["CodigoHistoriaClinica"] = new SelectList(_context.HistoriaClinica, "Codigo", "Codigo", anamnesis.CodigoHistoriaClinica);
            return View(anamnesis);
        }

        // GET: Anamnesis/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anamnesis = await _context.Anamnesis
                .Include(a => a.CodigoHistoriaClinicaNavigation)
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (anamnesis == null)
            {
                return NotFound();
            }

            return View(anamnesis);
        }

        // POST: Anamnesis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var anamnesis = await _context.Anamnesis.FindAsync(id);
            _context.Anamnesis.Remove(anamnesis);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnamnesisExists(string id)
        {
            return _context.Anamnesis.Any(e => e.Codigo == id);
        }
    }
}
