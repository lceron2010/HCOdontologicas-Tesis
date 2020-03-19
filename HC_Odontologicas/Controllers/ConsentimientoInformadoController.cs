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
		DateTime fecha = Funciones.ObtenerFechaActual("SA Pacific Standard Time");

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
					ViewData["FechaSortParam"] = sortOrder == "fecha_desc" ? "fecha_asc" : "fecha_desc";
					//permite mantener la busqueda introducida en el filtro de busqueda
					if (search != null)
						page = 1;
					else
						search = Filter;

					ViewData["Filter"] = search;
					ViewData["CurrentSort"] = sortOrder;
					var consentimiento = from c in _context.ConsentimientoInformado.Include(a => a.CitaOdontologica).ThenInclude(h => h.Paciente).Include(an => an.CitaOdontologica).ThenInclude(hc => hc.Personal) select c;
					if (!String.IsNullOrEmpty(search))
						 consentimiento = from c in _context.ConsentimientoInformado
									.Include(a => a.CitaOdontologica).ThenInclude(h => h.Paciente)
									.Include(an => an.CitaOdontologica).ThenInclude(hc => hc.Personal)
									.Where(s => s.CitaOdontologica.Paciente.Nombres.Contains(search)
									|| s.CitaOdontologica.Paciente.Apellidos.Contains(search)
									|| s.CitaOdontologica.Paciente.NombreCompleto.Contains(search)
									|| s.CitaOdontologica.Paciente.Identificacion.Contains(search)) select c;

					switch (sortOrder)
					{
						case "nombre_desc":
							consentimiento = consentimiento.OrderByDescending(s => s.CitaOdontologica.Paciente.NombreCompleto);
							break;
						case "fecha_asc":
							consentimiento = consentimiento.OrderBy(s => s.Fecha);
							break;
						case "fecha_desc":
							consentimiento = consentimiento.OrderByDescending(s => s.Fecha);
							break;
						default:
							consentimiento = consentimiento.OrderBy(s => s.CitaOdontologica.Paciente.NombreCompleto);
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
					ConsentimientoInformado consentimientoInformado = new ConsentimientoInformado();

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
						consentimientoInformado.CodigoCitaOdontologica = null;
					}
					else
					{
						Personal = new SelectList(_context.Personal.OrderBy(c => c.NombreCompleto).Where(c => c.Estado == true), "Codigo", "NombreCompleto", cita.CodigoPersonal).ToList();
						Paciente = new SelectList(_context.Paciente.OrderBy(p => p.NombreCompleto).Where(p => p.Estado == true), "Codigo", "NombreCompleto", cita.CodigoPaciente).ToList();
						consentimientoInformado.CodigoCitaOdontologica = cita.Codigo;
					}

					ViewData["CodigoPersonal"] = Personal;
					ViewData["CodigoPaciente"] = Paciente;

					var PlantillaCI = _context.PlantillaConsentimientoInformado.SingleOrDefault();					
					ViewData["Descripcion"] = PlantillaCI.Descripcion;

					return View(consentimientoInformado);
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

					var PlantillaCI = _context.PlantillaConsentimientoInformado.SingleOrDefault();
					ViewData["Descripcion"] = PlantillaCI.Descripcion;

					if (ModelState.IsValid)
					{
						//cita odontologica						
						CitaOdontologica citaOdontologica = _context.CitaOdontologica.Where(ci => ci.Codigo == consentimientoInformado.CodigoCitaOdontologica).SingleOrDefault();//_context.CitaOdontologica.Where(ci => ci.FechaInicio.Date == fecha.Date && ci.HoraInicio <= intInicial && ci.HoraFin >= intFinal && ci.CodigoPaciente == anamnesis.CodigoPaciente && ci.CodigoPersonal == anamnesis.CodigoPersonal).FirstOrDefault();
						DateTime FechaCitaCreacion = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
						var transaction = _context.Database.BeginTransaction();
						if (citaOdontologica == null)
						{
							CitaOdontologica cita = new CitaOdontologica();
							Int64 maxCodigoHC = 0;
							maxCodigoHC = Convert.ToInt64(_context.CitaOdontologica.Max(f => f.Codigo));
							maxCodigoHC += 1;
							cita.Codigo = maxCodigoHC.ToString("D8");
							cita.CodigoPaciente = consentimientoInformado.CodigoPaciente;
							cita.CodigoPersonal = consentimientoInformado.CodigoPersonal;
							cita.FechaCreacion = FechaCitaCreacion;
							cita.Observaciones = null;
							cita.Estado = "N";
							cita.FechaInicio = FechaCitaCreacion;
							cita.FechaFin = FechaCitaCreacion;
							cita.HoraInicio = new TimeSpan(FechaCitaCreacion.Hour, FechaCitaCreacion.Minute, 00);
							cita.HoraFin = new TimeSpan(FechaCitaCreacion.Hour, FechaCitaCreacion.Minute, 00);
							cita.UsuarioCreacion = i.Name;
							_context.Add(cita);
							await _context.SaveChangesAsync();
							await _auditoria.GuardarLogAuditoria(cita.FechaCreacion, i.Name, "CitaOdontologica", cita.Codigo, "I");
							consentimientoInformado.CodigoCitaOdontologica = cita.Codigo;
						}
						else
						{
							consentimientoInformado.CodigoCitaOdontologica = citaOdontologica.Codigo;
						}
						//consentimientoInformado
						Int64 maxCodigo = 0;
						maxCodigo = Convert.ToInt64(_context.ConsentimientoInformado.Max(f => f.Codigo));
						maxCodigo += 1;
						consentimientoInformado.Codigo = maxCodigo.ToString("D8");
						consentimientoInformado.Fecha = FechaCitaCreacion;
						
						_context.Add(consentimientoInformado);
						await _context.SaveChangesAsync();
						transaction.Commit();
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

					var PlantillaCI = _context.PlantillaConsentimientoInformado.SingleOrDefault();
					ViewData["Descripcion"] = PlantillaCI.Descripcion;


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

					ViewData["Descripcion"] = consentimiento.Descripcion;

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
			List<SelectListItem> Personal = new SelectList(_context.Personal.OrderBy(c => c.NombreCompleto).Where(c => c.Estado == true), "Codigo", "NombreCompleto", consentimientoInformado.CodigoPersonal).ToList();
			List<SelectListItem> Paciente = new SelectList(_context.Paciente.OrderBy(p => p.NombreCompleto).Where(p => p.Estado == true), "Codigo", "NombreCompleto", consentimientoInformado.CodigoPaciente).ToList();
			var PlantillaCI = _context.PlantillaConsentimientoInformado.SingleOrDefault();
			ViewData["Descripcion"] = PlantillaCI.Descripcion;

			if (i.IsAuthenticated)
			{
				try
				{

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

							Personal.Insert(0, vacio);
							ViewData["CodigoPersonal"] = Personal;

							Paciente.Insert(0, vacio);
							ViewData["CodigoPaciente"] = Paciente;

							ViewData["Descripcion"] = PlantillaCI.Descripcion;

							return View(consentimientoInformado);
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

					ViewData["Descripcion"] = PlantillaCI.Descripcion;

					return View(consentimientoInformado);
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

					ViewData["Descripcion"] = PlantillaCI.Descripcion;

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

		[HttpPost]
		public async Task<PlantillaConsentimientoInformado> CargarDatosPlantillaConsentimiento(String CodigoPlantilla)
		{
			try
			{

				//PlantillaConsentimientoInformado plantillaConsentimientoInformado = new PlantillaConsentimientoInformado();
				var plantillaConsentimientoInformado = await _context.PlantillaConsentimientoInformado.Where(f => f.Codigo == CodigoPlantilla).SingleOrDefaultAsync();
				return plantillaConsentimientoInformado;
			}
			catch (Exception e)
			{
				return null;
			}
		}

	}
}
