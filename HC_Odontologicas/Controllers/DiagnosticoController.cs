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

namespace HC_Odontologicas.Controllers
{
	public class DiagnosticoController : Controller
	{
		private readonly HCOdontologicasContext _context;
		private ValidacionesController validaciones;
		private readonly AuditoriaController _auditoria;
		SelectListItem vacio = new SelectListItem(value: "0", text: "Seleccione...");

		DateTime fecha = Funciones.ObtenerFechaActual("SA Pacific Standard Time");

		public DiagnosticoController(HCOdontologicasContext context)
		{
			_context = context;
			validaciones = new ValidacionesController(_context);
			_auditoria = new AuditoriaController(context);
		}

		// GET: Diagnostico
		public async Task<IActionResult> Index(string sortOrder, string Filter, int? page, string search)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				//Permisos de usuario
				var permisos = i.Claims.Where(c => c.Type == "Diagnostico").Select(c => c.Value).SingleOrDefault().Split(";");
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
					var diagnostico = from c in _context.Diagnostico.Include(a => a.CitaOdontologica).ThenInclude(h => h.Paciente).Include(an => an.CitaOdontologica).ThenInclude(hc => hc.Personal) select c;

					if (!String.IsNullOrEmpty(search))
						//dianostico = diagnostico.Where(s => s.Nombres.Contains(search));

						switch (sortOrder)
						{
							case "nombre_desc":
								diagnostico = diagnostico.OrderByDescending(s => s.Fecha);
								break;
							default:
								diagnostico = diagnostico.OrderBy(s => s.Fecha);
								break;

						}
					int pageSize = 10;
					return View(await Paginacion<Diagnostico>.CreateAsync(diagnostico, page ?? 1, pageSize)); // page devuelve valor si lo tiene caso contrario devuelve 1
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

		// GET: Diagnostico/Create
		public IActionResult Create()
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "Diagnostico").Select(c => c.Value).SingleOrDefault().Split(";");

				if (Convert.ToBoolean(permisos[1]))
				{
					//lista de enfermedades
					List<DiagnosticoCie10> diagnosticoC = new List<DiagnosticoCie10>();
					var enfermedad = _context.Cie10.OrderBy(f => f.Nombre).Where(f => f.Nombre.StartsWith("C")).ToList(); //QUITAR LUEGO					
					//agregar los cie10 a la lista de diagnosticoCIE10
					foreach (Cie10 item in enfermedad)
					{
						DiagnosticoCie10 aenfermedad = new DiagnosticoCie10();
						aenfermedad.Cie10 = item;
						aenfermedad.Seleccionado = false;
						diagnosticoC.Add(aenfermedad);
					}

					Diagnostico diagnostico = new Diagnostico();
					diagnostico.DiagnosticoCie10 = diagnosticoC;

					//llenar combos de paciente y doctor select * from citaodontologica where HoraInicio >= '9:00' and HoraFin <= '10:30'

					//TimeSpan intInicial = new TimeSpan(fecha.Hour, fecha.Minute, 00);
					TimeSpan intInicial = new TimeSpan(19, 30, 00);
					TimeSpan intFinal = new TimeSpan(22, 30, 00);
					var c = _context.CitaOdontologica;
					//ver estos condiciones.
					var ct = _context.CitaOdontologica.Where(ci => ci.FechaInicio.Date == fecha.Date);
					ct = ct.Where(ci => ci.HoraInicio >= intInicial || ci.HoraFin <= intFinal);
					CitaOdontologica cita = ct.FirstOrDefault();
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
						diagnostico.CodigoCitaOdontologica = null;
					}
					else
					{
						Personal = new SelectList(_context.Personal.OrderBy(c => c.NombreCompleto).Where(c => c.Estado == true), "Codigo", "NombreCompleto", cita.CodigoPersonal).ToList();
						Paciente = new SelectList(_context.Paciente.OrderBy(p => p.NombreCompleto).Where(p => p.Estado == true), "Codigo", "NombreCompleto", cita.CodigoPaciente).ToList();
						diagnostico.CodigoCitaOdontologica = cita.Codigo;
					}

					ViewData["CodigoPersonal"] = Personal;
					ViewData["CodigoPaciente"] = Paciente;


					return View(diagnostico);
				}
				else
					return Redirect("../Diagnostico");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// POST: Diagnostico/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<IActionResult> Create(Diagnostico diagnostico)
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

						//cita odontologica						
						CitaOdontologica citaOdontologica = _context.CitaOdontologica.Where(ci => ci.Codigo == diagnostico.CodigoCitaOdontologica).SingleOrDefault();
						DateTime FechaCitaCreacion = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
						if (citaOdontologica == null)
						{
							CitaOdontologica cita = new CitaOdontologica();
							Int64 maxCodigoHC = 0;
							maxCodigoHC = Convert.ToInt64(_context.CitaOdontologica.Max(f => f.Codigo));
							maxCodigoHC += 1;
							cita.Codigo = maxCodigoHC.ToString("D8");
							cita.CodigoPaciente = diagnostico.CodigoPaciente;
							cita.CodigoPersonal = diagnostico.CodigoPersonal;
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
							diagnostico.CodigoCitaOdontologica = cita.Codigo;
						}
						else
						{
							diagnostico.CodigoCitaOdontologica = citaOdontologica.Codigo;
						}


						var transaction = _context.Database.BeginTransaction();
						//guardar el tipoComprobante
						Diagnostico diag = new Diagnostico();
						Int64 maxCodigo = 0;
						maxCodigo = Convert.ToInt64(_context.Diagnostico.Max(f => f.Codigo));
						maxCodigo += 1;

						diag.Codigo = maxCodigo.ToString("D8");
						diag.CodigoCitaOdontologica = diagnostico.CodigoCitaOdontologica;
						diag.Fecha = fecha;
						diag.Pieza = diagnostico.Pieza;
						diag.Observacion = diagnostico.Observacion;
						diag.Firma = diagnostico.Firma;
						
						_context.Diagnostico.Add(diag);

						//guardar diagnosticoCie10
						Int64 maxCodigoAe = 0;
						maxCodigoAe = Convert.ToInt64(_context.DiagnosticoCie10.Max(f => f.Codigo));
						foreach (var enf in diagnostico.DiagnosticoCie10)
						{
							if (enf.Seleccionado)
							{
								DiagnosticoCie10 diagnosticoCie10 = new DiagnosticoCie10();
								maxCodigoAe += 1;
								diagnosticoCie10.Codigo = maxCodigoAe.ToString("D8");
								//diagnosticoCie10.CodigoDiagnostico = diagnostico.Codigo;
								diagnosticoCie10.CodigoDiagnostico = diag.Codigo;
								diagnosticoCie10.CodigoCie10 = enf.Cie10.Codigo;
								_context.DiagnosticoCie10.Add(diagnosticoCie10);
							}
						}

						await _context.SaveChangesAsync();
						transaction.Commit();
						await _auditoria.GuardarLogAuditoria(diag.Fecha, i.Name, "Diagnostico", diag.Codigo, "I");
						ViewBag.Message = "Save";
						return View(diagnostico);
					}
					return View(diagnostico);
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
					return View(diagnostico);
				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// GET: Diagnostico/Edit/5
		public async Task<IActionResult> Edit(string codigo)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "Diagnostico").Select(c => c.Value).SingleOrDefault().Split(";");
				codigo = Encriptacion.Decrypt(codigo);
				if (Convert.ToBoolean(permisos[2]))
				{
					if (codigo == null)
						return NotFound();

					var diagnostico = await _context.Diagnostico.Include(a => a.DiagnosticoCie10)
						.ThenInclude(a => a.Cie10)
						.Include(a => a.CitaOdontologica).ThenInclude(h => h.Paciente)
						.Include(an => an.CitaOdontologica).ThenInclude(hc => hc.Personal)
						.SingleOrDefaultAsync(f => f.Codigo == codigo);

					if (diagnostico == null)
						return NotFound();

					//lista de cie10
					var listaCie10 = _context.Cie10.OrderBy(f => f.Nombre).ToList();
					
					List<DiagnosticoCie10> tci = new List<DiagnosticoCie10>();
					tci = diagnostico.DiagnosticoCie10;
					for (int l = 0; l < tci.Count(); l++)
					{
						tci[l].Seleccionado = true;
					}
					//cie10 que estan en diagnosticoCie10
					List<Cie10> listaImpuestosTCI = new List<Cie10>();
					foreach (var item in tci)
					{
						listaImpuestosTCI.Add(item.Cie10);
					}
					//cie10 que faltan en diagnosticoCie10
					var listaCie10Agregar = (from t in listaCie10 where !listaImpuestosTCI.Any(x => x.Codigo == t.Codigo) select t).ToList();

					//agregar a la lista de diagnosticoCie10
					foreach (var l in listaCie10Agregar)
					{
						DiagnosticoCie10 ti = new DiagnosticoCie10();
						ti.Cie10 = l;
						ti.Seleccionado = false;
						diagnostico.DiagnosticoCie10.Add(ti);
					}

					diagnostico.DiagnosticoCie10.OrderBy(p => p.Cie10.Nombre);

					List<SelectListItem> Personal = new SelectList(_context.Personal.OrderBy(c => c.NombreCompleto).Where(c => c.Estado == true), "Codigo", "NombreCompleto", diagnostico.CitaOdontologica.Personal.Codigo).ToList();
					Personal.Insert(0, vacio);
					ViewData["CodigoPersonal"] = Personal;

					List<SelectListItem> Paciente = new SelectList(_context.Paciente.OrderBy(p => p.NombreCompleto).Where(p => p.Estado == true), "Codigo", "NombreCompleto", diagnostico.CitaOdontologica.Paciente.Codigo).ToList();
					Paciente.Insert(0, vacio);
					ViewData["CodigoPaciente"] = Paciente;

					return View(diagnostico);
				}
				else
					return Redirect("../Diagnostico");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// POST: Diagnostico/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<IActionResult> Edit(Diagnostico diagnostico)
		{
			var i = (ClaimsIdentity)User.Identity;
			List<SelectListItem> Personal = new SelectList(_context.Personal.OrderBy(c => c.NombreCompleto).Where(c => c.Estado == true), "Codigo", "NombreCompleto", diagnostico.CodigoPersonal).ToList();
			List<SelectListItem> Paciente = new SelectList(_context.Paciente.OrderBy(p => p.NombreCompleto).Where(p => p.Estado == true), "Codigo", "NombreCompleto", diagnostico.CodigoPaciente).ToList();

			if (i.IsAuthenticated)
			{
				try
				{
					if (ModelState.IsValid)
					{
						try
						{
							var transaction = _context.Database.BeginTransaction();
							//actualizar tipocomprobante
							diagnostico.Codigo = Encriptacion.Decrypt(diagnostico.Codigo);
							Diagnostico diagnosticoAntiguo = _context.Diagnostico.SingleOrDefault(p => p.Codigo == diagnostico.Codigo);
							diagnosticoAntiguo.Codigo = diagnostico.Codigo;
							diagnosticoAntiguo.CodigoCitaOdontologica = diagnostico.CodigoCitaOdontologica;
							diagnosticoAntiguo.Fecha = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
							diagnosticoAntiguo.Pieza = diagnostico.Pieza;
							diagnosticoAntiguo.Observacion = diagnostico.Observacion;
							diagnosticoAntiguo.Firma = diagnostico.Firma;													

							var diagnosticoCie10 = _context.DiagnosticoCie10.Where(a => a.CodigoDiagnostico == diagnostico.Codigo).ToList();
							foreach (var item in diagnosticoCie10)
								_context.DiagnosticoCie10.Remove(item);
							_context.SaveChanges();

							//guardar AnamenesisEnefermedad
							Int64 maxCodigoAe = 0;
							maxCodigoAe = Convert.ToInt64(_context.DiagnosticoCie10.Max(f => f.Codigo));
							foreach (var diag in diagnostico.DiagnosticoCie10)
							{
								if (diag.Seleccionado)
								{
									DiagnosticoCie10 diagCie10 = new DiagnosticoCie10();
									maxCodigoAe += 1;
									diagCie10.Codigo = maxCodigoAe.ToString("D8");
									//diagCie10.CodigoDiagnostico = null;
									diagCie10.CodigoDiagnostico = diagnostico.Codigo;
									diagCie10.CodigoCie10 = diag.Cie10.Codigo;
									_context.DiagnosticoCie10.Add(diagCie10);
								}
							}

							_context.Update(diagnosticoAntiguo);
							_context.SaveChanges();
							transaction.Commit();
							await _auditoria.GuardarLogAuditoria(diagnosticoAntiguo.Fecha, i.Name, "Diagnostico", diagnostico.Codigo, "U");
							ViewBag.Message = "Save";

							Personal.Insert(0, vacio);
							ViewData["CodigoPersonal"] = Personal;

							Paciente.Insert(0, vacio);
							ViewData["CodigoPaciente"] = Paciente;

							return View(diagnostico);

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

					return View(diagnostico);
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

					return View(diagnostico);
				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// POST: Diagnostico/Delete/5
		[HttpPost]
		public async Task<string> DeleteConfirmed(string codigo)
		{
			try
			{
				var i = (ClaimsIdentity)User.Identity;
				var diagnostico = await _context.Diagnostico.SingleOrDefaultAsync(f => f.Codigo == codigo);
				_context.Diagnostico.Remove(diagnostico);
				await _context.SaveChangesAsync();
				await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "Diagnostico", diagnostico.Codigo, "D");
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
