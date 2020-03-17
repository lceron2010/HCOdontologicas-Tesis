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
	public class CarrerasController : Controller
	{
		private readonly HCOdontologicasContext _context;
		private ValidacionesController validaciones;
		private readonly AuditoriaController _auditoria;
		SelectListItem vacio = new SelectListItem(value: "0", text: "Seleccione...");
		public CarrerasController(HCOdontologicasContext context)
		{
			_context = context;
			validaciones = new ValidacionesController(_context);
			_auditoria = new AuditoriaController(context);
		}

		// GET: Carrera
		public async Task<IActionResult> Index(string search, string Filter, string sortOrder, int? page)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				//Permisos de usuario
				var permisos = i.Claims.Where(c => c.Type == "Carreras").Select(c => c.Value).SingleOrDefault().Split(";");
				ViewData["Crear"] = Convert.ToBoolean(permisos[1]);
				ViewData["Editar"] = Convert.ToBoolean(permisos[2]);
				ViewData["Eliminar"] = Convert.ToBoolean(permisos[3]);
				ViewData["Exportar"] = Convert.ToBoolean(permisos[4]);

				if (Convert.ToBoolean(permisos[0]))
				{
					//permite mantener la busqueda introducida en el filtro de busqueda
					if (search != null)
						page = 1;
					else
						search = Filter;

					ViewData["Filter"] = search;
					ViewData["CurrentSort"] = sortOrder;

					var carrera = from c in _context.Carrera.Include(c => c.Facultad) select c;
					if (!String.IsNullOrEmpty(search))
						carrera = carrera.Where(s => s.Nombre.Contains(search));

					switch (sortOrder)
					{
						case "nombre_desc":
							carrera = carrera.OrderByDescending(s => s.Nombre);
							break;
						default:
							carrera = carrera.OrderBy(s => s.Nombre);
							break;

					}
					int pageSize = 10;
					return View(await Paginacion<Carrera>.CreateAsync(carrera, page ?? 1, pageSize)); // page devuelve valor si lo tiene caso contrario devuelve 1
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

		// GET: Carrera/Create
		public IActionResult Create()
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "Carreras").Select(c => c.Value).SingleOrDefault().Split(";");

				if (Convert.ToBoolean(permisos[1]))
				{
					List<SelectListItem> Facultad = new SelectList(_context.Facultad.OrderBy(f => f.Nombre), "Codigo", "Nombre").ToList();
					Facultad.Insert(0, vacio);
					ViewData["CodigoFacultad"] = Facultad;

					return View();
				}
				else
					return Redirect("../Carreras");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// POST: carrera/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<IActionResult> Create(Carrera carrera)
		{
			var i = (ClaimsIdentity)User.Identity;
			List<SelectListItem> Facultad = new SelectList(_context.Facultad.OrderBy(f => f.Nombre), "Codigo", "Nombre").ToList();
			if (i.IsAuthenticated)
			{
				try
				{
					if (ModelState.IsValid)
					{
						Int64 maxCodigo = 0;
						maxCodigo = Convert.ToInt64(_context.Carrera.Max(f => f.Codigo));
						maxCodigo += 1;
						carrera.Codigo = maxCodigo.ToString("D4");
						_context.Add(carrera);
						await _context.SaveChangesAsync();
						await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "Carrera", carrera.Codigo, "I");

						Facultad.Insert(0, vacio);
						ViewData["CodigoFacultad"] = Facultad;

						ViewBag.Message = "Save";
						return View(carrera);
					}
					return View(carrera);

				}
				catch (Exception e)
				{
					string mensaje = e.Message;
					if (e.InnerException != null)
						mensaje = MensajesError.UniqueKey(e.InnerException.Message);

					ViewBag.Message = mensaje;

					Facultad.Insert(0, vacio);
					ViewData["CodigoFacultad"] = Facultad;

					return View(carrera);
				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}

		}
		// GET: Carrera/Edit/5
		public async Task<IActionResult> Edit(String codigo)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "Carreras").Select(c => c.Value).SingleOrDefault().Split(";");
				codigo = Encriptacion.Decrypt(codigo);
				if (Convert.ToBoolean(permisos[2]))
				{
					if (codigo == null)
						return NotFound();

					var carrera = await _context.Carrera.SingleOrDefaultAsync(f => f.Codigo == codigo);

					if (carrera == null)
						return NotFound();

					List<SelectListItem> Facultad = new SelectList(_context.Facultad.OrderBy(f => f.Nombre), "Codigo", "Nombre", carrera.CodigoFacultad).ToList();
					Facultad.Insert(0, vacio);
					ViewData["CodigoFacultad"] = Facultad;

					return View(carrera);
				}
				else
					return Redirect("../Carreras");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}
		// POST: Carrera/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<IActionResult> Edit(Carrera carrera)
		{
			var i = (ClaimsIdentity)User.Identity;
			List<SelectListItem> Facultad = new SelectList(_context.Facultad.OrderBy(f => f.Nombre), "Codigo", "Nombre").ToList();
			if (i.IsAuthenticated)
			{
				try
				{

					if (ModelState.IsValid)
					{
						try
						{
							carrera.Codigo = Encriptacion.Decrypt(carrera.Codigo);
							_context.Update(carrera);
							await _context.SaveChangesAsync();
							await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "Carrera", carrera.Codigo, "U");

							Facultad.Insert(0, vacio);
							ViewData["CodigoFacultad"] = Facultad;

							ViewBag.Message = "Save";

							return View(carrera);
						}
						catch (DbUpdateConcurrencyException)
						{
							throw;
						}
					}

					Facultad.Insert(0, vacio);
					ViewData["CodigoFacultad"] = Facultad;

					return View(carrera);
				}
				catch (Exception e)
				{
					string mensaje = e.Message;
					if (e.InnerException != null)
						mensaje = MensajesError.UniqueKey(e.InnerException.Message);

					Facultad.Insert(0, vacio);
					ViewData["CodigoFacultad"] = Facultad;

					ViewBag.Message = mensaje;

					return View(carrera);
				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}


		// POST: Carrera/Delete/5
		[HttpPost]
		public async Task<String> DeleteConfirmed(string codigo)
		{
			try
			{
				var i = (ClaimsIdentity)User.Identity;
				var carrera = await _context.Carrera.SingleOrDefaultAsync(f => f.Codigo == codigo);
				_context.Carrera.Remove(carrera);
				await _context.SaveChangesAsync();
				await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "Carrera", carrera.Codigo, "D");
				return "Delete";

			}
			catch (Exception e)
			{
				string mensaje = e.Message;
				if (e.InnerException != null)
					mensaje = MensajesError.ForeignKey(e.InnerException.Message);
				return mensaje;
			}

		}

	}
}
