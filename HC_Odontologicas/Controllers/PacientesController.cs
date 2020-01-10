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
	public class PacientesController : Controller
	{
		private readonly HCOdontologicasContext _context;
		private ValidacionesController validaciones;
		private readonly AuditoriaController _auditoria;
		SelectListItem vacio = new SelectListItem(value: "0", text: "Seleccione...");

		public PacientesController(HCOdontologicasContext context)
		{
			_context = context;
			validaciones = new ValidacionesController(_context);
			_auditoria = new AuditoriaController(context);
		}

		// GET: Pacientes
		public async Task<IActionResult> Index(string sortOrder, string Filter, int? page, string search)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				//Permisos de usuario
				var permisos = i.Claims.Where(c => c.Type == "Pacientes").Select(c => c.Value).SingleOrDefault().Split(";");
				ViewData["Crear"] = Convert.ToBoolean(permisos[1]);
				ViewData["Editar"] = Convert.ToBoolean(permisos[2]);
				ViewData["Eliminar"] = Convert.ToBoolean(permisos[3]);
				ViewData["Exportar"] = Convert.ToBoolean(permisos[4]);

				if (Convert.ToBoolean(permisos[0]))
				{
					ViewData["NombreSortParam"] = string.IsNullOrEmpty(sortOrder) ? "nombre_desc" : "";					

					//permite mantener la busqueda introducida en el filtro de busqueda
					if (search != null)
						page = 1;
					else
						search = Filter;

					ViewData["Filter"] = search;
					ViewData["CurrentSort"] = sortOrder;
					var pacientes = from c in _context.Paciente.OrderBy(p => p.NombreCompleto) select c;

					//if (!String.IsNullOrEmpty(search))
						pacientes = pacientes.Where(s => s.NombreCompleto.Contains(search) || s.Identificacion.Contains(search) || s.NumeroUnico.Contains(search));

					switch (sortOrder)
					{
						case "nombre_desc":
							pacientes = pacientes.OrderByDescending(s => s.NombreCompleto);
							break;			
						
						default:
							pacientes = pacientes.OrderBy(s => s.NombreCompleto);
							break;
					}
					int pageSize = 10;
					return View(await Paginacion<Paciente>.CreateAsync(pacientes, page ?? 1, pageSize)); // page devuelve valor si lo tiene caso contrario devuelve 1
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


		// GET: Pacientes/Create
		public IActionResult Create()
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				//Permisos de usuario
				var permisos = i.Claims.Where(c => c.Type == "Pacientes").Select(c => c.Value).SingleOrDefault().Split(";");
				if (Convert.ToBoolean(permisos[1]))
				{

					List<SelectListItem> TipoIdentificacion = new SelectList(_context.TipoIdentificacion.OrderBy(f => f.Nombre), "Codigo", "Nombre").ToList();
					TipoIdentificacion.Insert(0, vacio);
					ViewData["CodigoTipoIdentificacion"] = TipoIdentificacion;

					List<SelectListItem> Facultad = new SelectList(_context.Facultad.OrderBy(f => f.Nombre), "Codigo", "Nombre").ToList();
					Facultad.Insert(0, vacio);
					ViewData["CodigoFacultad"] = Facultad;

					return View();
				}
				else
					return Redirect("../Pacientes");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// POST: Pacientes/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<IActionResult> Create(Paciente paciente)
		{
			var i = (ClaimsIdentity)User.Identity;
			List<SelectListItem> TipoIdentificacion = new SelectList(_context.TipoIdentificacion.OrderBy(f => f.Nombre), "Codigo", "Nombre").ToList();
			List<SelectListItem> Facultad = new SelectList(_context.Facultad.OrderBy(f => f.Nombre), "Codigo", "Nombre").ToList();
			List<SelectListItem> Carrera = new SelectList(_context.Carrera.OrderBy(f => f.Nombre).Where(p => p.CodigoFacultad == paciente.CodigoFacultad), "Codigo", "Nombre", paciente.CodigoCarrera).ToList();

			if (i.IsAuthenticated)
			{				
				try
				{
					if (ModelState.IsValid)
					{
						Int64 maxCodigo = 0;
						maxCodigo = Convert.ToInt64(_context.Paciente.Max(f => f.Codigo));
						maxCodigo += 1;
						paciente.Codigo = maxCodigo.ToString("D8");
						_context.Add(paciente);
						await _context.SaveChangesAsync();
						await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "Paciente", paciente.Codigo, "I");

						TipoIdentificacion.Insert(0, vacio);
						ViewData["CodigoTipoIdentificacion"] = TipoIdentificacion;

						Facultad.Insert(0, vacio);
						ViewData["CodigoFacultad"] = Facultad;

						Carrera.Insert(0, vacio);
						ViewData["CodigoCarrera"] = Carrera;

						ViewBag.Message = "Save";

						return View(paciente);
					}					

					return View(paciente);
				}
				catch (Exception e)
				{
					string mensaje = e.Message;
					if (e.InnerException != null)
						mensaje = MensajesError.UniqueKey(e.InnerException.Message);
										
					TipoIdentificacion.Insert(0, vacio);
					ViewData["CodigoTipoIdentificacion"] = TipoIdentificacion;

					Facultad.Insert(0, vacio);
					ViewData["CodigoFacultad"] = Facultad;

					Carrera.Insert(0, vacio);
					ViewData["CodigoCarrera"] = Carrera;

					ViewBag.Message = mensaje;

					return View(paciente);
				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// GET: Pacientes/Edit/5
		public async Task<IActionResult> Edit(string codigo)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "Pacientes").Select(c => c.Value).SingleOrDefault().Split(";");
				codigo = Encriptacion.Decrypt(codigo);
				if (Convert.ToBoolean(permisos[2]))
				{
					if (codigo == null)
						return NotFound();

					var paciente = await _context.Paciente.SingleOrDefaultAsync(f => f.Codigo == codigo);

					if (paciente == null)
						return NotFound();


					List<SelectListItem> TipoIdentificacion = new SelectList(_context.TipoIdentificacion.OrderBy(t => t.Nombre), "Codigo", "Nombre",  paciente.CodigoTipoIdentificacion).ToList();
					TipoIdentificacion.Insert(0, vacio);
					ViewData["CodigoTipoIdentificacion"] = TipoIdentificacion;

					List<SelectListItem> Facultad = new SelectList(_context.Facultad.OrderBy(f => f.Nombre), "Codigo", "Nombre" , paciente.CodigoFacultad).ToList();
					Facultad.Insert(0, vacio);
					ViewData["CodigoFacultad"] = Facultad;

					List<SelectListItem> Carrera = new SelectList(_context.Carrera.OrderBy(f => f.Nombre), "Codigo", "Nombre" , paciente.CodigoCarrera).ToList();
					Carrera.Insert(0, vacio);
					ViewData["CodigoCarrera"] = Carrera;

					return View(paciente);
				}
				else
					return Redirect("../Pacientes");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// POST: Pacientes/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<IActionResult> Edit(Paciente paciente)
		{
			var i = (ClaimsIdentity)User.Identity;
			List<SelectListItem> TipoIdentificacion = new SelectList(_context.TipoIdentificacion.OrderBy(f => f.Nombre), "Codigo", "Nombre").ToList();
			List<SelectListItem> Facultad = new SelectList(_context.Facultad.OrderBy(f => f.Nombre), "Codigo", "Nombre").ToList();
			List<SelectListItem> Carrera = new SelectList(_context.Carrera.OrderBy(f => f.Nombre).Where(p => p.CodigoFacultad== paciente.CodigoFacultad), "Codigo", "Nombre").ToList();

			if (i.IsAuthenticated)
			{
				try
				{
					if (ModelState.IsValid)
					{
						try
						{
							paciente.Codigo = Encriptacion.Decrypt(paciente.Codigo);
							_context.Update(paciente);
							await _context.SaveChangesAsync();
							await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "Paciente", paciente.Codigo, "U");
							ViewBag.Message = "Save";

							TipoIdentificacion.Insert(0, vacio);
							ViewData["CodigoTipoIdentificacion"] = TipoIdentificacion;

							Facultad.Insert(0, vacio);
							ViewData["CodigoFacultad"] = Facultad;

							Carrera.Insert(0, vacio);
							ViewData["CodigoCarrera"] = Carrera;

							return View(paciente);
						}
						catch (DbUpdateConcurrencyException)
						{
							throw;
						}
					}

					TipoIdentificacion.Insert(0, vacio);
					ViewData["CodigoTipoIdentificacion"] = TipoIdentificacion;

					Facultad.Insert(0, vacio);
					ViewData["CodigoFacultad"] = Facultad;

					Carrera.Insert(0, vacio);
					ViewData["CodigoCarrera"] = Carrera;

					return View(paciente);
				}
				catch (Exception e)
				{
					string mensaje = e.Message;
					if (e.InnerException != null)
						mensaje = MensajesError.UniqueKey(e.InnerException.Message);

					ViewBag.Message = mensaje;

					TipoIdentificacion.Insert(0, vacio);
					ViewData["CodigoTipoIdentificacion"] = TipoIdentificacion;

					Facultad.Insert(0, vacio);
					ViewData["CodigoFacultad"] = Facultad;

					Carrera.Insert(0, vacio);
					ViewData["CodigoCarrera"] = Carrera;

					return View(paciente);
				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// POST: Pacientes/Delete/5
		[HttpPost]
		public async Task<String> DeleteConfirmed(string codigo)
		{
			try
			{
				var i = (ClaimsIdentity)User.Identity;
				var paciente = await _context.Paciente.SingleOrDefaultAsync(f => f.Codigo == codigo);
				_context.Paciente.Remove(paciente);
				await _context.SaveChangesAsync();
				await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "Paciente", paciente.Codigo, "D");
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

		[HttpPost]
		public async Task<List<SelectListItem>> CargarDatosCarrera(String CodigoFacultad)
		{			
			List<SelectListItem> list = new List<SelectListItem>();			
			var Carrera = await _context.Carrera.OrderBy(f => f.Nombre).Where(p=> p.CodigoFacultad == CodigoFacultad).ToListAsync();
			list.Insert(0, new SelectListItem("Seleccione...", "0"));
			foreach (Carrera item in Carrera.ToList())
				list.Add(new SelectListItem(item.Nombre, item.Codigo));			
			return list;
		}
	}
} 
