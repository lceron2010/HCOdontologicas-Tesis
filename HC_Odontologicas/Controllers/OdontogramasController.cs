using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HC_Odontologicas.Models;
using HC_Odontologicas.FuncionesGenerales;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace HC_Odontologicas.Controllers
{
	public class OdontogramasController : Controller
	{
		private readonly HCOdontologicasContext _context;
		private ValidacionesController validaciones;
		private readonly AuditoriaController _auditoria;
		SelectListItem vacio = new SelectListItem(value: "0", text: "Seleccione...");
		DateTime fecha = Funciones.ObtenerFechaActual("SA Pacific Standard Time");

		public OdontogramasController(HCOdontologicasContext context)
		{
			_context = context;
			validaciones = new ValidacionesController(_context);
			_auditoria = new AuditoriaController(context);
		}

		// GET: Odontogramas
		public async Task<IActionResult> Index(string sortOrder, string Filter, int? page, string search)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				//Permisos de usuario
				var permisos = i.Claims.Where(c => c.Type == "Odontogramas").Select(c => c.Value).SingleOrDefault().Split(";");
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
					//var personal = from c in _context.Personal.Include(a => a.Cargo).OrderBy(p => p.NombreCompleto) select c;
					var odontograma = from c in _context.Odontograma.Include(a => a.CitaOdontologica).ThenInclude(h => h.Paciente).Include(an => an.CitaOdontologica).ThenInclude(hc => hc.Personal) select c;

					if (!String.IsNullOrEmpty(search))
						//anamnesis = anamnesis.Where(s => s.Nombres.Contains(search));

						switch (sortOrder)
						{
							case "nombre_desc":
								odontograma = odontograma.OrderByDescending(s => s.CitaOdontologica.Paciente.NombreCompleto);
								break;
							default:
								odontograma = odontograma.OrderBy(s => s.CitaOdontologica.Paciente.NombreCompleto);
								break;

						}
					int pageSize = 10;
					return View(await Paginacion<Odontograma>.CreateAsync(odontograma, page ?? 1, pageSize)); // page devuelve valor si lo tiene caso contrario devuelve 1
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


		// GET: Odontogramas/Create
		public IActionResult Create()
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "RecetaMedica").Select(c => c.Value).SingleOrDefault().Split(";");

				if (Convert.ToBoolean(permisos[1]))
				{

					RecetaMedica recetaMedica = new RecetaMedica();

					//llenar combos de paciente y doctor select * from citaodontologica where HoraInicio >= '9:00' and HoraFin <= '10:30'

					//TimeSpan intInicial = new TimeSpan(fecha.Hour, fecha.Minute, 00);
					TimeSpan intInicial = new TimeSpan(19, 30, 00);
					TimeSpan intFinal = new TimeSpan(22, 30, 00);

					//ver estos condiciones.
					var c = _context.CitaOdontologica.Where(ci => ci.FechaInicio.Date == fecha.Date);
					c = c.Where(ci => ci.HoraInicio >= intInicial || ci.HoraFin <= intFinal);
					CitaOdontologica cita = c.FirstOrDefault();
					//CitaOdontologica cita = _context.CitaOdontologica.Where(ci => ci.FechaInicio.Date == fecha.Date && ci.HoraInicio >= intInicial || ci.HoraFin <= intFinal).SingleOrDefault();
					//-- fin ver las condiciones
					List<SelectListItem> Personal = null;
					List<SelectListItem> Paciente = null;
					if (cita == null)
					{
						Personal = new SelectList(_context.Personal.OrderBy(c => c.NombreCompleto).Where(c => c.Estado == true), "Codigo", "NombreCompleto").ToList();
						Personal.Insert(0, vacio);
						Paciente = new SelectList(_context.Paciente.OrderBy(p => p.NombreCompleto).Where(p => p.Estado == true), "Codigo", "NombreCompleto").ToList();
						Paciente.Insert(0, vacio);
						recetaMedica.CodigoCitaOdontologica = null;
					}
					else
					{
						Personal = new SelectList(_context.Personal.OrderBy(c => c.NombreCompleto).Where(c => c.Estado == true), "Codigo", "NombreCompleto", cita.CodigoPersonal).ToList();
						Paciente = new SelectList(_context.Paciente.OrderBy(p => p.NombreCompleto).Where(p => p.Estado == true), "Codigo", "NombreCompleto", cita.CodigoPaciente).ToList();
						recetaMedica.CodigoCitaOdontologica = cita.Codigo;
					}

					ViewData["CodigoPersonal"] = Personal;
					ViewData["CodigoPaciente"] = Paciente;

					
					return View();
				}
				else
					return Redirect("../Odontogramas");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}

		}


		[HttpPost]      //public async Task<string> Create(Odontograma odontograma , IFormFile svg742, string hola, IFormFile imagen)
		public async Task<string> GuardarOdontograma(Odontograma odontograma, IFormFile imagen)
		{


			//if (ModelState.IsValid)
			//{
			//	//_context.Add(odontograma);
			await _context.SaveChangesAsync();
			//	//return RedirectToAction(nameof(Index));
			//}
			//ViewData["CodigoCitaOdontologica"] = new SelectList(_context.CitaOdontologica, "Codigo", "Codigo", odontograma.CodigoCitaOdontologica);

			return imagen.ToString();// View(odontograma);
		}


		// POST: Odontogramas/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]      //public async Task<string> Create(Odontograma odontograma , IFormFile svg742, string hola, IFormFile imagen)
		public async Task<string> Create(Odontograma odontograma, List<TipoIdentificacion> array)
		{


			//if (ModelState.IsValid)
			//{
			//	//_context.Add(odontograma);
				await _context.SaveChangesAsync();
			//	//return RedirectToAction(nameof(Index));
			//}
			//ViewData["CodigoCitaOdontologica"] = new SelectList(_context.CitaOdontologica, "Codigo", "Codigo", odontograma.CodigoCitaOdontologica);
			
		return array.ToString();// View(odontograma);
		}


		[HttpPost]
		public ActionResult ExportarExcel(Odontograma odontograma, List<TipoIdentificacion> array)
		{
			var resultado = array;
			return View(resultado);
		}


		// GET: Odontogramas/Edit/5
		public async Task<IActionResult> Edit(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var odontograma = await _context.Odontograma.FindAsync(id);
			if (odontograma == null)
			{
				return NotFound();
			}
			ViewData["CodigoCitaOdontologica"] = new SelectList(_context.CitaOdontologica, "Codigo", "Codigo", odontograma.CodigoCitaOdontologica);
			return View(odontograma);
		}

		// POST: Odontogramas/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(string id, [Bind("Codigo,CodigoCitaOdontologica,FechaActualizacion,Observaciones,Estado")] Odontograma odontograma)
		{
			if (id != odontograma.Codigo)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(odontograma);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!OdontogramaExists(odontograma.Codigo))
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
			ViewData["CodigoCitaOdontologica"] = new SelectList(_context.CitaOdontologica, "Codigo", "Codigo", odontograma.CodigoCitaOdontologica);
			return View(odontograma);
		}

		// GET: Odontogramas/Delete/5
		public async Task<IActionResult> Delete(string id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var odontograma = await _context.Odontograma
				.Include(o => o.CitaOdontologica)
				.FirstOrDefaultAsync(m => m.Codigo == id);
			if (odontograma == null)
			{
				return NotFound();
			}

			return View(odontograma);
		}

		// POST: Odontogramas/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(string id)
		{
			var odontograma = await _context.Odontograma.FindAsync(id);
			_context.Odontograma.Remove(odontograma);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool OdontogramaExists(string id)
		{
			return _context.Odontograma.Any(e => e.Codigo == id);
		}
	}
}
