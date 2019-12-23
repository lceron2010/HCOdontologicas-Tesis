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
		SelectListItem vacio = new SelectListItem(value: "0", text: "Seleccione...");

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
					//var personal = from c in _context.Personal.Include(a => a.Cargo).OrderBy(p => p.NombreCompleto) select c;
					var anamnesis = from c in _context.Anamnesis.Include(a => a.CitaOdontologica).ThenInclude(h => h.Paciente).Include(an => an.CitaOdontologica).ThenInclude(hc => hc.Personal) select c;

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
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "Anamnesis").Select(c => c.Value).SingleOrDefault().Split(";");

				if (Convert.ToBoolean(permisos[1]))
				{

					List<SelectListItem> Personal = new SelectList(_context.Personal.OrderBy(c => c.NombreCompleto).Where(c => c.Estado == true), "Codigo", "NombreCompleto").ToList();
					Personal.Insert(0, vacio);
					ViewData["CodigoPersonal"] = Personal;

					List<SelectListItem> Paciente = new SelectList(_context.Paciente.OrderBy(p => p.NombreCompleto).Where(p => p.Estado == true), "Codigo", "NombreCompleto").ToList();
					Paciente.Insert(0, vacio);
					ViewData["CodigoPaciente"] = Paciente;

					return View();
				}
				else
					return Redirect("../Personal");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// POST: Anamnesis/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<IActionResult> Create(Anamnesis anamnesis)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				try
				{

					if (ModelState.IsValid)
					{
						CitaOdontologica historiaClinica = new CitaOdontologica();
						Int64 maxCodigo = 0;
						maxCodigo = Convert.ToInt64(_context.Anamnesis.Max(f => f.Codigo));
						maxCodigo += 1;
						anamnesis.Codigo = maxCodigo.ToString("D8");
						//
						var codigoHC = _context.CitaOdontologica.SingleOrDefault(h => h.CodigoPaciente == anamnesis.CodigoPaciente && h.CodigoPersonal == anamnesis.CodigoPersonal);
						if (codigoHC == null)
						{

							Int64 maxCodigoHC = 0;
							maxCodigoHC = Convert.ToInt64(_context.CitaOdontologica.Max(f => f.Codigo));
							maxCodigoHC += 1;
							historiaClinica.Codigo = maxCodigo.ToString("D8");
							historiaClinica.CodigoPaciente = anamnesis.CodigoPaciente;
							historiaClinica.CodigoPersonal = anamnesis.CodigoPersonal;
							historiaClinica.FechaCreacion = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
							historiaClinica.Observaciones = null;
							historiaClinica.Estado = true;
							_context.Add(historiaClinica);
							await _context.SaveChangesAsync();
							await _auditoria.GuardarLogAuditoria(historiaClinica.FechaCreacion, i.Name, "HistoriaClinica", historiaClinica.Codigo, "I");
							anamnesis.CodigoCitaOdontologica = historiaClinica.Codigo;
						}
						else
						{
							anamnesis.CodigoCitaOdontologica = codigoHC.Codigo;
						}

						anamnesis.Fecha = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
						_context.Add(anamnesis);
						await _context.SaveChangesAsync();
						await _auditoria.GuardarLogAuditoria(anamnesis.Fecha, i.Name, "Anamnesis", anamnesis.Codigo, "I");


						ViewBag.Message = "Save";
						return View(anamnesis);

					}
					List<SelectListItem> Personal = new SelectList(_context.Personal.OrderBy(c => c.NombreCompleto).Where(c => c.Estado == true), "Codigo", "NombreCompleto").ToList();
					Personal.Insert(0, vacio);
					ViewData["CodigoPersonal"] = Personal;

					List<SelectListItem> Paciente = new SelectList(_context.Paciente.OrderBy(p => p.NombreCompleto).Where(p => p.Estado == true), "Codigo", "NombreCompleto").ToList();
					Paciente.Insert(0, vacio);
					ViewData["CodigoPaciente"] = Paciente;
					return View(anamnesis);
				}
				catch (Exception e)
				{
					string mensaje = e.Message;
					if (e.InnerException != null)
						mensaje = MensajesError.UniqueKey(e.InnerException.Message);

					ViewBag.Message = mensaje;
					List<SelectListItem> Personal = new SelectList(_context.Personal.OrderBy(c => c.NombreCompleto).Where(c => c.Estado == true), "Codigo", "NombreCompleto").ToList();
					Personal.Insert(0, vacio);
					ViewData["CodigoPersonal"] = Personal;

					List<SelectListItem> Paciente = new SelectList(_context.Paciente.OrderBy(p => p.NombreCompleto).Where(p => p.Estado == true), "Codigo", "NombreCompleto").ToList();
					Paciente.Insert(0, vacio);
					ViewData["CodigoPaciente"] = Paciente;
					return View(anamnesis);
				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// GET: Anamnesis/Edit/5
		public async Task<IActionResult> Edit(string codigo)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "Anamnesis").Select(c => c.Value).SingleOrDefault().Split(";");
				codigo = Encriptacion.Decrypt(codigo);
				if (Convert.ToBoolean(permisos[2]))
				{
					if (codigo == null)
						return NotFound();

					var anamnesis = await _context.Anamnesis.Include(a => a.CitaOdontologica).ThenInclude(h => h.Paciente)
						.Include(an => an.CitaOdontologica).ThenInclude(hc => hc.Personal).SingleOrDefaultAsync(f => f.Codigo == codigo);

					if (anamnesis == null)
						return NotFound();


					List<SelectListItem> Personal = new SelectList(_context.Personal.OrderBy(c => c.NombreCompleto).Where(c => c.Estado == true), "Codigo", "NombreCompleto" , anamnesis.CitaOdontologica.Personal.Codigo).ToList();
					Personal.Insert(0, vacio);
					ViewData["CodigoPersonal"] = Personal;

					List<SelectListItem> Paciente = new SelectList(_context.Paciente.OrderBy(p => p.NombreCompleto).Where(p => p.Estado == true), "Codigo", "NombreCompleto", anamnesis.CitaOdontologica.Paciente.Codigo).ToList();
					Paciente.Insert(0, vacio);
					ViewData["CodigoPaciente"] = Paciente;


					return View(anamnesis);
				}
				else
					return Redirect("../Anamnesis");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// POST: Anamnesis/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<IActionResult> Edit(Anamnesis anamnesis)
		{
			var i = (ClaimsIdentity)User.Identity;
			List<SelectListItem> Personal = new SelectList(_context.Personal.OrderBy(c => c.NombreCompleto).Where(c => c.Estado == true), "Codigo", "NombreCompleto").ToList();
			List<SelectListItem> Paciente = new SelectList(_context.Paciente.OrderBy(p => p.NombreCompleto).Where(p => p.Estado == true), "Codigo", "NombreCompleto").ToList();
			if (i.IsAuthenticated)
			{
				try
				{
					if (ModelState.IsValid)
					{
						try
						{
							anamnesis.Codigo = Encriptacion.Decrypt(anamnesis.Codigo);
							_context.Update(anamnesis);
							await _context.SaveChangesAsync();
							await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "Anamnesis", anamnesis.Codigo, "U");
							ViewBag.Message = "Save";

							Personal.Insert(0, vacio);
							ViewData["CodigoPersonal"] = Personal;

							Paciente.Insert(0, vacio);
							ViewData["CodigoPaciente"] = Paciente;

							return View(anamnesis);
						}
						catch (DbUpdateConcurrencyException)
						{
							throw;
						}
					}

					Personal.Insert(0, vacio);
					ViewData["CodigoPersonal"] = Personal;

					Paciente.Insert(0, vacio);
					ViewData["CodigoPaciente"] = Paciente;

					return View(anamnesis);
				}
				catch (Exception e)
				{
					string mensaje = e.Message;
					if (e.InnerException != null)
						mensaje = MensajesError.UniqueKey(e.InnerException.Message);

					ViewBag.Message = mensaje;

					Personal.Insert(0, vacio);
					ViewData["CodigoPersonal"] = Personal;

					Paciente.Insert(0, vacio);
					ViewData["CodigoPaciente"] = Paciente;

					return View(anamnesis);
				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}



		// POST: Anamnesis/Delete/5
		[HttpPost]

		public async Task<string> DeleteConfirmed(string codigo)
		{
			try
			{
				var i = (ClaimsIdentity)User.Identity;
				var anamnesis = await _context.Anamnesis.SingleOrDefaultAsync(f => f.Codigo == codigo);
				_context.Anamnesis.Remove(anamnesis);
				await _context.SaveChangesAsync();
				await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "Anmnesis", anamnesis.Codigo, "D");
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
