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
	public class ConsentimientoInformadoController : Controller
	{
		private readonly HCOdontologicasContext _context;
		private ValidacionesController validaciones;
		private readonly AuditoriaController _auditoria;
		SelectListItem vacio = new SelectListItem(value: "0", text: "Seleccione...");

		public ConsentimientoInformadoController(HCOdontologicasContext context)
		{
			_context = context;
			validaciones = new ValidacionesController(_context);
			_auditoria = new AuditoriaController(context);
		}

		// GET: ConsentimientoInformado
		public async Task<IActionResult> Index(string sortOrder, string Filter, int? page, string search)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				//Permisos de usuario
				var permisos = i.Claims.Where(c => c.Type == "ConsentimientoInformado").Select(c => c.Value).SingleOrDefault().Split(";");
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
					var consentimiento = from c in _context.ConsentimientoInformado.Include(a => a.CitaOdontologica).ThenInclude(h => h.Paciente).Include(an => an.CitaOdontologica).ThenInclude(hc => hc.Personal) select c;
					if (!String.IsNullOrEmpty(search))
						consentimiento = consentimiento.Where(s => s.Nombre.Contains(search));

					switch (sortOrder)
					{
						case "nombre_desc":
							consentimiento = consentimiento.OrderByDescending(s => s.Nombre);
							break;
						default:
							consentimiento = consentimiento.OrderBy(s => s.Nombre);
							break;

					}
					int pageSize = 10;
					return View(await Paginacion<ConsentimientoInformado>.CreateAsync(consentimiento, page ?? 1, pageSize)); // page devuelve valor si lo tiene caso contrario devuelve 1
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


		// GET: ConsentimientoInformado/Create
		public IActionResult Create()
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "ConsentimientoInformado").Select(c => c.Value).SingleOrDefault().Split(";");

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
					return Redirect("../ConsentimientoInformado");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// POST: ConsentimientoInformado/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<IActionResult> Create(ConsentimientoInformado consentimientoInformado)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				try
				{
					List<SelectListItem> Personal = new SelectList(_context.Personal.OrderBy(c => c.NombreCompleto).Where(c => c.Estado == true), "Codigo", "NombreCompleto").ToList();
					Personal.Insert(0, vacio);
					ViewData["CodigoPersonal"] = Personal;

					List<SelectListItem> Paciente = new SelectList(_context.Paciente.OrderBy(p => p.NombreCompleto).Where(p => p.Estado == true), "Codigo", "NombreCompleto").ToList();
					Paciente.Insert(0, vacio);
					ViewData["CodigoPaciente"] = Paciente;

					if (ModelState.IsValid)
					{
						CitaOdontologica historiaClinica = new CitaOdontologica();
						Int64 maxCodigo = 0;
						maxCodigo = Convert.ToInt64(_context.ConsentimientoInformado.Max(f => f.Codigo));
						maxCodigo += 1;
						consentimientoInformado.Codigo = maxCodigo.ToString("D8");
						//
						var codigoHC = _context.CitaOdontologica.SingleOrDefault(h => h.CodigoPaciente == consentimientoInformado.CodigoPaciente && h.CodigoPersonal == consentimientoInformado.CodigoPersonal);
						if (codigoHC == null)
						{

							Int64 maxCodigoHC = 0;
							maxCodigoHC = Convert.ToInt64(_context.CitaOdontologica.Max(f => f.Codigo));
							maxCodigoHC += 1;
							historiaClinica.Codigo = maxCodigoHC.ToString("D8");
							historiaClinica.CodigoPaciente = consentimientoInformado.CodigoPaciente;
							historiaClinica.CodigoPersonal = consentimientoInformado.CodigoPersonal;
							historiaClinica.FechaCreacion = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
							historiaClinica.Observaciones = null;
							historiaClinica.Estado = true;
							_context.Add(historiaClinica);
							await _context.SaveChangesAsync();
							await _auditoria.GuardarLogAuditoria(historiaClinica.FechaCreacion, i.Name, "HistoriaClinica", historiaClinica.Codigo, "I");
							consentimientoInformado.CodigoCitaOdontologica = historiaClinica.Codigo;
						}
						else
						{
							consentimientoInformado.CodigoCitaOdontologica = codigoHC.Codigo;
						}

						consentimientoInformado.Fecha = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
						_context.Add(consentimientoInformado);
						await _context.SaveChangesAsync();
						await _auditoria.GuardarLogAuditoria(consentimientoInformado.Fecha, i.Name, "ConsentimientoInformado", consentimientoInformado.Codigo, "I");
						ViewBag.Message = "Save";
						return View(consentimientoInformado);
					}

					return View(consentimientoInformado);
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
					return View(consentimientoInformado);
				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// GET: ConsentimientoInformado/Edit/5
		public async Task<IActionResult> Edit(string codigo)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "ConsentimientoInformado").Select(c => c.Value).SingleOrDefault().Split(";");
				codigo = Encriptacion.Decrypt(codigo);
				if (Convert.ToBoolean(permisos[2]))
				{
					if (codigo == null)
						return NotFound();

					var consentimiento = await _context.ConsentimientoInformado.Include(a => a.CitaOdontologica).ThenInclude(h => h.Paciente)
						.Include(an => an.CitaOdontologica).ThenInclude(hc => hc.Personal).SingleOrDefaultAsync(f => f.Codigo == codigo);

					if (consentimiento == null)
						return NotFound();


					List<SelectListItem> Personal = new SelectList(_context.Personal.OrderBy(c => c.NombreCompleto).Where(c => c.Estado == true), "Codigo", "NombreCompleto", consentimiento.CitaOdontologica.Personal.Codigo).ToList();
					Personal.Insert(0, vacio);
					ViewData["CodigoPersonal"] = Personal;

					List<SelectListItem> Paciente = new SelectList(_context.Paciente.OrderBy(p => p.NombreCompleto).Where(p => p.Estado == true), "Codigo", "NombreCompleto", consentimiento.CitaOdontologica.Paciente.Codigo).ToList();
					Paciente.Insert(0, vacio);
					ViewData["CodigoPaciente"] = Paciente;


					return View(consentimiento);
				}
				else
					return Redirect("../ConsentimientoInformado");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// POST: ConsentimientoInformado/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<IActionResult> Edit(ConsentimientoInformado consentimientoInformado)
		{
			var i = (ClaimsIdentity)User.Identity;

			if (i.IsAuthenticated)
			{

				try
				{
					List<SelectListItem> Personal = new SelectList(_context.Personal.OrderBy(c => c.NombreCompleto).Where(c => c.Estado == true), "Codigo", "NombreCompleto").ToList();
					Personal.Insert(0, vacio);
					ViewData["CodigoPersonal"] = Personal;

					List<SelectListItem> Paciente = new SelectList(_context.Paciente.OrderBy(p => p.NombreCompleto).Where(p => p.Estado == true), "Codigo", "NombreCompleto").ToList();
					Paciente.Insert(0, vacio);
					ViewData["CodigoPaciente"] = Paciente;
					if (ModelState.IsValid)
					{
						try
						{
							consentimientoInformado.Codigo = Encriptacion.Decrypt(consentimientoInformado.Codigo);
							consentimientoInformado.Fecha = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
							_context.Update(consentimientoInformado);
							await _context.SaveChangesAsync();
							await _auditoria.GuardarLogAuditoria(consentimientoInformado.Fecha, i.Name, "ConsentimientoInformado", consentimientoInformado.Codigo, "U");
							ViewBag.Message = "Save";

							//Personal.Insert(0, vacio);
							//ViewData["CodigoPersonal"] = Personal;

							//Paciente.Insert(0, vacio);
							//ViewData["CodigoPaciente"] = Paciente;

							return View(consentimientoInformado);
						}
						catch (DbUpdateConcurrencyException)
						{
							throw;
						}
					}




					return View(consentimientoInformado);
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

					//Personal.Insert(0, vacio);
					//ViewData["CodigoPersonal"] = Personal;

					//Paciente.Insert(0, vacio);
					//ViewData["CodigoPaciente"] = Paciente;

					return View(consentimientoInformado);
				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// POST: ConsentimientoInformado/Delete/5
		[HttpPost]
		public async Task<String> DeleteConfirmed(string codigo)
		{
			try
			{
				var i = (ClaimsIdentity)User.Identity;
				var consentimiento = await _context.ConsentimientoInformado.SingleOrDefaultAsync(f => f.Codigo == codigo);
				_context.ConsentimientoInformado.Remove(consentimiento);
				await _context.SaveChangesAsync();
				await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "ConcentimientoInformado", consentimiento.Codigo, "D");
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
