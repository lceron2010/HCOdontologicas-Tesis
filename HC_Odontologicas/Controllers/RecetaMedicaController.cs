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
	public class RecetaMedicaController : Controller
	{
		private readonly HCOdontologicasContext _context;
		private ValidacionesController validaciones;
		private readonly AuditoriaController _auditoria;
		SelectListItem vacio = new SelectListItem(value: "0", text: "Seleccione...");
		DateTime fecha = Funciones.ObtenerFechaActual("SA Pacific Standard Time");

		public RecetaMedicaController(HCOdontologicasContext context)
		{
			_context = context;
			validaciones = new ValidacionesController(_context);
			_auditoria = new AuditoriaController(context);
		}

		// GET: RecetaMedica
		public async Task<IActionResult> Index(string sortOrder, string Filter, int? page, string search)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				//Permisos de usuario
				var permisos = i.Claims.Where(c => c.Type == "RecetaMedica").Select(c => c.Value).SingleOrDefault().Split(";");
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
					var receta = from c in _context.RecetaMedica.Include(a => a.CitaOdontologica).ThenInclude(h => h.Paciente).Include(an => an.CitaOdontologica).ThenInclude(hc => hc.Personal) select c;
					if (!String.IsNullOrEmpty(search))
						receta = receta.Where(s => s.Descripcion.Contains(search));

					switch (sortOrder)
					{
						case "nombre_desc":
							receta = receta.OrderByDescending(s => s.Descripcion);
							break;
						default:
							receta = receta.OrderBy(s => s.Descripcion);
							break;

					}
					int pageSize = 10;
					return View(await Paginacion<RecetaMedica>.CreateAsync(receta, page ?? 1, pageSize)); // page devuelve valor si lo tiene caso contrario devuelve 1
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

		// GET: RecetaMedica/Create
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

					List<SelectListItem> PlantillaRM = new SelectList(_context.PlantillaRecetaMedica.OrderBy(c => c.Nombre), "Codigo", "Nombre").ToList();
					PlantillaRM.Insert(0, vacio);
					ViewData["CodigoPlantillaReceta"] = PlantillaRM;

					return View();
				}
				else
					return Redirect("../RecetaMedica");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// POST: RecetaMedica/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<IActionResult> Create(RecetaMedica recetaMedica)
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

					List<SelectListItem> PlantillaRM = new SelectList(_context.PlantillaRecetaMedica.OrderBy(c => c.Nombre), "Codigo", "Nombre").ToList(); ;
					PlantillaRM.Insert(0, vacio);
					ViewData["CodigoPlantillaReceta"] = PlantillaRM;

					if (ModelState.IsValid)
					{
						//cita odontologica						
						CitaOdontologica citaOdontologica = _context.CitaOdontologica.Where(ci => ci.Codigo == recetaMedica.CodigoCitaOdontologica).SingleOrDefault();//_context.CitaOdontologica.Where(ci => ci.FechaInicio.Date == fecha.Date && ci.HoraInicio <= intInicial && ci.HoraFin >= intFinal && ci.CodigoPaciente == anamnesis.CodigoPaciente && ci.CodigoPersonal == anamnesis.CodigoPersonal).FirstOrDefault();
						DateTime FechaCitaCreacion = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
						var transaction = _context.Database.BeginTransaction();
						if (citaOdontologica == null)
						{
							CitaOdontologica cita = new CitaOdontologica();
							Int64 maxCodigoHC = 0;
							maxCodigoHC = Convert.ToInt64(_context.CitaOdontologica.Max(f => f.Codigo));
							maxCodigoHC += 1;
							cita.Codigo = maxCodigoHC.ToString("D8");
							cita.CodigoPaciente = recetaMedica.CodigoPaciente;
							cita.CodigoPersonal = recetaMedica.CodigoPersonal;
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
							recetaMedica.CodigoCitaOdontologica = cita.Codigo;
						}
						else
						{
							recetaMedica.CodigoCitaOdontologica = citaOdontologica.Codigo;
						}
						//guardar receta medica						
						Int64 maxCodigo = 0;
						maxCodigo = Convert.ToInt64(_context.RecetaMedica.Max(f => f.Codigo));
						maxCodigo += 1;
						recetaMedica.Codigo = maxCodigo.ToString("D8");
						recetaMedica.Fecha = FechaCitaCreacion;
						if (recetaMedica.CodigoPlantillaRecetaMedica == "0")
						{
							recetaMedica.CodigoPlantillaRecetaMedica = null;
						}
						_context.Add(recetaMedica);
						await _context.SaveChangesAsync();
						transaction.Commit();
						await _auditoria.GuardarLogAuditoria(recetaMedica.Fecha, i.Name, "RecetaMedica", recetaMedica.Codigo, "I");

						ViewBag.Message = "Save";
						return View(recetaMedica);

					}

					return View(recetaMedica);

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

					List<SelectListItem> PlantillaRM = new SelectList(_context.PlantillaRecetaMedica.OrderBy(c => c.Nombre), "Codigo", "Nombre").ToList(); ;
					PlantillaRM.Insert(0, vacio);
					ViewData["CodigoPlantillaReceta"] = PlantillaRM;

					return View(recetaMedica);

				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// GET: RecetaMedica/Edit/5
		public async Task<IActionResult> Edit(string codigo)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "RecetaMedica").Select(c => c.Value).SingleOrDefault().Split(";");
				codigo = Encriptacion.Decrypt(codigo);
				if (Convert.ToBoolean(permisos[2]))
				{
					if (codigo == null)
						return NotFound();

					var receta = await _context.RecetaMedica.Include(a => a.CitaOdontologica).ThenInclude(h => h.Paciente)
						.Include(an => an.CitaOdontologica).ThenInclude(hc => hc.Personal).SingleOrDefaultAsync(f => f.Codigo == codigo);

					if (receta == null)
						return NotFound();


					List<SelectListItem> Personal = new SelectList(_context.Personal.OrderBy(c => c.NombreCompleto).Where(c => c.Estado == true), "Codigo", "NombreCompleto", receta.CitaOdontologica.Personal.Codigo).ToList();
					Personal.Insert(0, vacio);
					ViewData["CodigoPersonal"] = Personal;

					List<SelectListItem> Paciente = new SelectList(_context.Paciente.OrderBy(p => p.NombreCompleto).Where(p => p.Estado == true), "Codigo", "NombreCompleto", receta.CitaOdontologica.Paciente.Codigo).ToList();
					Paciente.Insert(0, vacio);
					ViewData["CodigoPaciente"] = Paciente;

					List<SelectListItem> PlantillaRM = new SelectList(_context.PlantillaRecetaMedica.OrderBy(c => c.Nombre), "Codigo", "Nombre", receta.CodigoPlantillaRecetaMedica).ToList(); ;
					PlantillaRM.Insert(0, vacio);
					ViewData["CodigoPlantillaReceta"] = PlantillaRM;

					return View(receta);
				}
				else
					return Redirect("../RecetaMedica");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// POST: RecetaMedica/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<IActionResult> Edit(RecetaMedica recetaMedica)
		{
			var i = (ClaimsIdentity)User.Identity;

			List<SelectListItem> Personal = new SelectList(_context.Personal.OrderBy(c => c.NombreCompleto).Where(c => c.Estado == true), "Codigo", "NombreCompleto", recetaMedica.CodigoPersonal).ToList();
			List<SelectListItem> Paciente = new SelectList(_context.Paciente.OrderBy(p => p.NombreCompleto).Where(p => p.Estado == true), "Codigo", "NombreCompleto", recetaMedica.CodigoPaciente).ToList();
			List<SelectListItem> PlantillaRM = new SelectList(_context.PlantillaRecetaMedica.OrderBy(c => c.Nombre), "Codigo", "Nombre", recetaMedica.CodigoPlantillaRecetaMedica).ToList();
			if (i.IsAuthenticated)
			{
				try
				{
					if (ModelState.IsValid)
					{
						try
						{
							recetaMedica.Codigo = Encriptacion.Decrypt(recetaMedica.Codigo);
							recetaMedica.Fecha = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
							if (recetaMedica.CodigoPlantillaRecetaMedica == "0")
							{
								recetaMedica.CodigoPlantillaRecetaMedica = null;
							}
							_context.Update(recetaMedica);
							await _context.SaveChangesAsync();
							await _auditoria.GuardarLogAuditoria(recetaMedica.Fecha, i.Name, "RecetaMedica", recetaMedica.Codigo, "U");
							ViewBag.Message = "Save";

							Personal.Insert(0, vacio);
							ViewData["CodigoPersonal"] = Personal;

							Paciente.Insert(0, vacio);
							ViewData["CodigoPaciente"] = Paciente;

							PlantillaRM.Insert(0, vacio);
							ViewData["CodigoPlantillaReceta"] = PlantillaRM;

							return View(recetaMedica);
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

					PlantillaRM.Insert(0, vacio);
					ViewData["CodigoPlantillaReceta"] = PlantillaRM;

					return View(recetaMedica);
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

					PlantillaRM.Insert(0, vacio);
					ViewData["CodigoPlantillaReceta"] = PlantillaRM;

					return View(recetaMedica);
				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// POST: RecetaMedica/Delete/5
		[HttpPost]
		public async Task<String> DeleteConfirmed(string codigo)
		{
			try
			{
				var i = (ClaimsIdentity)User.Identity;
				var receta = await _context.RecetaMedica.SingleOrDefaultAsync(f => f.Codigo == codigo);
				_context.RecetaMedica.Remove(receta);
				await _context.SaveChangesAsync();
				await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "RecetaMedica", receta.Codigo, "D");
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
		public async Task<PlantillaRecetaMedica> CargarDatosPlantillaReceta(String CodigoPlantilla)
		{
			try
			{
				var plantillaRecetaMedica = await _context.PlantillaRecetaMedica.Where(f => f.Codigo == CodigoPlantilla).SingleOrDefaultAsync();
				return plantillaRecetaMedica;
			}
			catch (Exception e)
			{
				return null;
			}
		}
	}
}

