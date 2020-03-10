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
	public class CitasOdontologicasController : Controller
	{
		private readonly HCOdontologicasContext _context;

		public CitasOdontologicasController(HCOdontologicasContext context)
		{
			_context = context;
		}

		// GET: CitasOdontologicas
		public async Task<IActionResult> Index(string sortOrder, string Filter, int? page, string search)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				//Permisos de usuario
				var permisos = i.Claims.Where(c => c.Type == "CitasOdontologicas").Select(c => c.Value).SingleOrDefault().Split(";");
				ViewData["Crear"] = Convert.ToBoolean(permisos[1]);
				ViewData["Editar"] = Convert.ToBoolean(permisos[2]);
				ViewData["Eliminar"] = Convert.ToBoolean(permisos[3]);
				ViewData["Exportar"] = Convert.ToBoolean(permisos[4]);

				if (Convert.ToBoolean(permisos[0]))
				{

					ViewData["NombreSortParam"] = String.IsNullOrEmpty(sortOrder) ? "nombre_desc" : "";

					//permite mantener la busqueda introducida en el filtro de busqueda
					if (search != null)
						page = 1;
					else
						search = Filter;

					ViewData["Filter"] = search;
					ViewData["CurrentSort"] = sortOrder;
					var fecha = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
					var fechaInicioDia = new DateTime(fecha.Year, fecha.Month, fecha.Day, 00, 00, 00);
					var fechaInicioFinDia = new DateTime(fecha.Year, fecha.Month, fecha.Day, 23, 59, 59);
					var citaOdontologica = from c in _context.CitaOdontologica.Include(a => a.Personal).Include(a => a.Paciente).Where(a => a.FechaInicio > fechaInicioDia && a.FechaInicio < fechaInicioFinDia).OrderBy(p => p.FechaInicio) select c; //Include(h => h.Paciente).Include(hc => hc.Personal)


					if (!String.IsNullOrEmpty(search))
						citaOdontologica = citaOdontologica.Where(s => s.Paciente.NombreCompleto.Contains(search) || s.Paciente.NombreCompleto.Contains(search));

					switch (sortOrder)
					{
						case "nombre_desc":
							citaOdontologica = citaOdontologica.OrderByDescending(s => s.FechaInicio);
							break;
						default:
							citaOdontologica = citaOdontologica.OrderBy(s => s.FechaInicio);
							break;

					}
					int pageSize = 10;

					return View(await Paginacion<CitaOdontologica>.CreateAsync(citaOdontologica, page ?? 1, pageSize)); // page devuelve valor si lo tiene caso contrario devuelve 1
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


		public async Task<IActionResult> CitaOdontologica()
		{
			

			var citaOdontologica = await _context.CitaOdontologica
				.Include(c => c.Paciente)
				.Include(c => c.Personal)
				.FirstOrDefaultAsync();
			if (citaOdontologica == null)
			{
				return NotFound();
			}

			return View(citaOdontologica);
		}

		// GET: CitasOdontologicas/Details/5
		public async Task<IActionResult> Details()
		{
			
			var citaOdontologica = await _context.CitaOdontologica
				.Include(c => c.Paciente)
				.Include(c => c.Personal)
				.FirstOrDefaultAsync();
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


		//[TempData]
		//public string CodigoCitaAlAtender { get; set; }
		//public void AtenderCita(string codigo)
		//{
		//	CodigoCitaAlAtender = $"codigo cita para atender {codigo} added";
		//}


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
