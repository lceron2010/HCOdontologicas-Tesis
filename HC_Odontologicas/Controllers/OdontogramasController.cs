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
using Newtonsoft.Json;

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





		// POST: Odontogramas/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]      //public async Task<string> Create(Odontograma odontograma , IFormFile svg742, string hola, IFormFile imagen)
		public async Task<string> Create(List<Odontograma> odontograma)
		{
			var i = (ClaimsIdentity)User.Identity;
				try
				{
					List<SelectListItem> Personal = new SelectList(_context.Personal.OrderBy(c => c.NombreCompleto).Where(c => c.Estado == true), "Codigo", "NombreCompleto").ToList();
					Personal.Insert(0, vacio);
					ViewData["CodigoPersonal"] = Personal;

					List<SelectListItem> Paciente = new SelectList(_context.Paciente.OrderBy(p => p.NombreCompleto).Where(p => p.Estado == true), "Codigo", "NombreCompleto").ToList();
					Paciente.Insert(0, vacio);
					ViewData["CodigoPaciente"] = Paciente;

					//if (ModelState.IsValid)
					//{

						//cita odontologica						
						CitaOdontologica citaOdontologica = _context.CitaOdontologica.Where(ci => ci.Codigo == odontograma[0].CodigoCitaOdontologica).SingleOrDefault();
						DateTime FechaCitaCreacion = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
						var transaction = _context.Database.BeginTransaction();
						if (citaOdontologica == null)
						{
							CitaOdontologica cita = new CitaOdontologica();
							Int64 maxCodigoHC = 0;
							maxCodigoHC = Convert.ToInt64(_context.CitaOdontologica.Max(f => f.Codigo));
							maxCodigoHC += 1;
							cita.Codigo = maxCodigoHC.ToString("D8");
							cita.CodigoPaciente = odontograma[0].CodigoPaciente;
							cita.CodigoPersonal = odontograma[0].CodigoPersonal;
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
							odontograma[0].CodigoCitaOdontologica = cita.Codigo;
						}
						else
						{
							odontograma[0].CodigoCitaOdontologica = citaOdontologica.Codigo;
						}



						//guardar el odontograma
						Odontograma odont = new Odontograma();
						Int64 maxCodigo = 0;
						maxCodigo = Convert.ToInt64(_context.Odontograma.Max(f => f.Codigo));
						maxCodigo += 1;

						odont.Codigo = maxCodigo.ToString("D8");
						odont.CodigoCitaOdontologica = odontograma[0].CodigoCitaOdontologica;
						odont.FechaActualizacion = fecha;
						odont.Observaciones = null; 
						odont.Estado = "I";

						_context.Odontograma.Add(odont);

						//guardar odontogramaDetalle
						Int64 maxCodigoOd = 0;
						maxCodigoOd = Convert.ToInt64(_context.OdontogramaDetalle.Max(f => f.Codigo));
						foreach (var detalle in odontograma[0].OdontogramaDetalle)
						{

							OdontogramaDetalle odontDetalle = new OdontogramaDetalle();
							maxCodigoOd += 1;
							odontDetalle.Codigo = maxCodigoOd.ToString("D8");
							odontDetalle.CodigoOdontograma = odont.Codigo;
							odontDetalle.Pieza = detalle.Pieza;
							odontDetalle.Region = detalle.Region;
							odontDetalle.Enfermedad = detalle.Enfermedad;
							odontDetalle.Valor = detalle.Valor;
							odontDetalle.Diagnostico = detalle.Diagnostico;
							_context.OdontogramaDetalle.Add(odontDetalle);

						}

						await _context.SaveChangesAsync();
						transaction.Commit();
						await _auditoria.GuardarLogAuditoria(fecha, i.Name, "Odontograma", odont.Codigo, "I");
						ViewBag.Message = "Save";
						
				return "Save";					
					
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

					return mensaje;//e.InnerException.ToString();
				}
			
		}

		public string ObtenerDatosOdontogramaDetalle(string codigoOdontograma)
		{
			string codigoD = Encriptacion.Decrypt(codigoOdontograma);

			Odontograma odontograma = _context.Odontograma.Include(a => a.OdontogramaDetalle)				
				.SingleOrDefault(f => f.Codigo == codigoD);

			List<OdontogramaDetalle> listaDetalle = new List<OdontogramaDetalle>();

			foreach (var item in odontograma.OdontogramaDetalle)
			{
				OdontogramaDetalle detalle = new OdontogramaDetalle();
				detalle.Codigo = item.Codigo;
				detalle.CodigoOdontograma = item.CodigoOdontograma;
				detalle.Pieza = item.Pieza;
				detalle.Region = item.Region;
				detalle.Enfermedad = item.Enfermedad;
				detalle.Valor = item.Valor;
				detalle.Diagnostico = item.Diagnostico;
				listaDetalle.Add(detalle);
			}



			return JsonConvert.SerializeObject(listaDetalle);
			//return View(odontograma);

		}

		// GET: Pacientes/Edit/5
		public async Task<IActionResult> Edit(string codigo)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "Odontogramas").Select(c => c.Value).SingleOrDefault().Split(";");
				codigo = Encriptacion.Decrypt(codigo);
				if (Convert.ToBoolean(permisos[2]))
				{
					if (codigo == null)
						return NotFound();

					var odontograma = await _context.Odontograma.Include(a => a.OdontogramaDetalle)
						.Include(a => a.CitaOdontologica).ThenInclude(h => h.Paciente)
						.Include(an => an.CitaOdontologica).ThenInclude(hc => hc.Personal)

						.SingleOrDefaultAsync(f => f.Codigo == codigo);

					if (odontograma == null)
						return NotFound();


					List<SelectListItem> Personal = new SelectList(_context.Personal.OrderBy(c => c.NombreCompleto).Where(c => c.Estado == true), "Codigo", "NombreCompleto", odontograma.CitaOdontologica.Personal.Codigo).ToList();
					Personal.Insert(0, vacio);
					ViewData["CodigoPersonal"] = Personal;

					List<SelectListItem> Paciente = new SelectList(_context.Paciente.OrderBy(p => p.NombreCompleto).Where(p => p.Estado == true), "Codigo", "NombreCompleto", odontograma.CitaOdontologica.Paciente.Codigo).ToList();
					Paciente.Insert(0, vacio);
					ViewData["CodigoPaciente"] = Paciente;

					return View(odontograma);
				}
				else
					return Redirect("../Odontogramas");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// POST: Odontogramas/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<string> Edit(List<Odontograma> odontograma)
		{
			var i = (ClaimsIdentity)User.Identity;
			List<SelectListItem> Personal = new SelectList(_context.Personal.OrderBy(c => c.NombreCompleto).Where(c => c.Estado == true), "Codigo", "NombreCompleto", odontograma[0].CodigoPersonal).ToList();
			List<SelectListItem> Paciente = new SelectList(_context.Paciente.OrderBy(p => p.NombreCompleto).Where(p => p.Estado == true), "Codigo", "NombreCompleto", odontograma[0].CodigoPaciente).ToList();

			if (i.IsAuthenticated)
			{
				try
				{
					if (ModelState.IsValid)
					{
						try
						{
							var transaction = _context.Database.BeginTransaction();
							//actualizar odontograma
							odontograma[0].Codigo = Encriptacion.Decrypt(odontograma[0].Codigo);
							Odontograma odontogramaAntiguo = _context.Odontograma.SingleOrDefault(p => p.Codigo == odontograma[0].Codigo);							
							odontogramaAntiguo.Codigo = odontograma[0].Codigo;
							odontogramaAntiguo.CodigoCitaOdontologica = odontograma[0].CodigoCitaOdontologica;
							odontogramaAntiguo.FechaActualizacion = fecha;
							odontogramaAntiguo.Observaciones = null;
							odontogramaAntiguo.Estado = "A";
							

							var tipoComprobantesImpuesto = _context.OdontogramaDetalle.Where(a => a.CodigoOdontograma == odontograma[0].Codigo).ToList();
							foreach (var item in tipoComprobantesImpuesto)
								_context.OdontogramaDetalle.Remove(item);
							_context.SaveChanges();

							//guardar odontogramaDetalle
							Int64 maxCodigoOd = 0;
							maxCodigoOd = Convert.ToInt64(_context.OdontogramaDetalle.Max(f => f.Codigo));
							foreach (var detalle in odontograma[0].OdontogramaDetalle)
							{
								OdontogramaDetalle odontDetalle = new OdontogramaDetalle();
								maxCodigoOd += 1;
								odontDetalle.Codigo = maxCodigoOd.ToString("D8");
								odontDetalle.CodigoOdontograma = odontograma[0].Codigo;
								odontDetalle.Pieza = detalle.Pieza;
								odontDetalle.Region = detalle.Region;
								odontDetalle.Enfermedad = detalle.Enfermedad;
								odontDetalle.Valor = detalle.Valor;
								odontDetalle.Diagnostico = detalle.Diagnostico;
								_context.OdontogramaDetalle.Add(odontDetalle);
							}

							_context.Update(odontogramaAntiguo);
							_context.SaveChanges();
							transaction.Commit();
							await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "Odontograma", odontograma[0].Codigo, "U");
							ViewBag.Message = "Save";

							Personal.Insert(0, vacio);
							ViewData["CodigoPersonal"] = Personal;

							Paciente.Insert(0, vacio);
							ViewData["CodigoPaciente"] = Paciente;

							return "Save";

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

					return "No es valido";
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

					return e.InnerException.Message;
				}
			}
			else
			{
				return "No autenticado";
			}
		}

		// POST: Odontogramas/Delete/5
		[HttpPost]		
		public async Task<string> DeleteConfirmed(string codigo)
		{
			try
			{
				var i = (ClaimsIdentity)User.Identity;
				var odontograma = await _context.Odontograma.SingleOrDefaultAsync(f => f.Codigo == codigo);
				_context.Odontograma.Remove(odontograma);
				await _context.SaveChangesAsync();
				await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "Odontograma", odontograma.Codigo, "D");
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
