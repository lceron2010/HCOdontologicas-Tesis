using HC_Odontologicas.FuncionesGenerales;
using HC_Odontologicas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HC_Odontologicas.Controllers
{
	public class AnamnesisController : Controller
	{
		private readonly HCOdontologicasContext _context;
		private ValidacionesController validaciones;
		private readonly AuditoriaController _auditoria;
		SelectListItem vacio = new SelectListItem(value: "0", text: "Seleccione...");

		DateTime fecha = Funciones.ObtenerFechaActual("SA Pacific Standard Time");

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
					ViewData["NombreSortParam"] = String.IsNullOrEmpty(sortOrder) ? "nombre_desc" : "";
					ViewData["FechaSortParam"] = sortOrder == "fecha_desc" ? "fecha_asc" : "fecha_desc";
					//permite mantener la busqueda introducida en el filtro de busqueda
					if (search != null)
						page = 1;
					else
						search = Filter;

					ViewData["Filter"] = search;
					ViewData["CurrentSort"] = sortOrder;
					//var personal = from c in _context.Personal.Include(a => a.Cargo).OrderBy(p => p.NombreCompleto) select c;
					var anamnesis = from c in _context.Anamnesis.Include(a => a.CitaOdontologica).ThenInclude(h => h.Paciente).Include(an => an.CitaOdontologica).ThenInclude(hc => hc.Personal).OrderBy(c => c.CitaOdontologica.Paciente.NombreCompleto) select c;

					if (!String.IsNullOrEmpty(search))
						anamnesis = from c in _context.Anamnesis.Include(a => a.CitaOdontologica).ThenInclude(h => h.Paciente)
									.Include(an => an.CitaOdontologica).ThenInclude(hc => hc.Personal)
									.OrderBy(c => c.CitaOdontologica.Paciente.NombreCompleto)
									.Where(s => s.CitaOdontologica.Paciente.Nombres.Contains(search)
									|| s.CitaOdontologica.Paciente.Apellidos.Contains(search)
									|| s.CitaOdontologica.Paciente.NombreCompleto.Contains(search)
									|| s.CitaOdontologica.Paciente.Identificacion.Contains(search))
									select c;

					switch (sortOrder)
					{

						case "nombre_desc":
							anamnesis = anamnesis.OrderByDescending(s => s.CitaOdontologica.Paciente.NombreCompleto);
							break;
						case "fecha_asc":
							anamnesis = anamnesis.OrderBy(s => s.Fecha);
							break;
						case "fecha_desc":
							anamnesis = anamnesis.OrderByDescending(s => s.Fecha);
							break;
						default:
							anamnesis = anamnesis.OrderBy(s => s.CitaOdontologica.Paciente.NombreCompleto);
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
		public IActionResult Create(string codigo)
		{

			TempData["CodigoCitaAlAtender"] = codigo;


			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "Anamnesis").Select(c => c.Value).SingleOrDefault().Split(";");

				if (Convert.ToBoolean(permisos[1]))
				{
					//lista de enfermedades
					List<AnamnesisEnfermedad> ae = new List<AnamnesisEnfermedad>();
					var enfermedad = _context.Enfermedad.OrderBy(f => f.Nombre).ToList();
					enfermedad = enfermedad.FindAll(f => f.Estado == true);
					//agregar las enferemedades a la lista de anamnesisEnfermedad.
					foreach (Enfermedad item in enfermedad)
					{
						AnamnesisEnfermedad aenfermedad = new AnamnesisEnfermedad();
						aenfermedad.Enfermedad = item;
						aenfermedad.Seleccionado = false;
						ae.Add(aenfermedad);
					}

					Anamnesis anamnesis = new Anamnesis();
					anamnesis.AnamnesisEnfermedad = ae;

					List<SelectListItem> Enfermedades = null;
					Enfermedades = new SelectList(_context.Enfermedad.OrderBy(c => c.Nombre).Where(c => c.Estado == true), "Codigo", "Nombre").ToList();

					ViewData["AnamnesisEnfermedad"] = Enfermedades;

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
						anamnesis.CodigoCitaOdontologica = null;
					}
					else
					{
						Personal = new SelectList(_context.Personal.OrderBy(c => c.NombreCompleto).Where(c => c.Estado == true), "Codigo", "NombreCompleto", cita.CodigoPersonal).ToList();
						Paciente = new SelectList(_context.Paciente.OrderBy(p => p.NombreCompleto).Where(p => p.Estado == true), "Codigo", "NombreCompleto", cita.CodigoPaciente).ToList();
						anamnesis.CodigoCitaOdontologica = cita.Codigo;
					}

					ViewData["CodigoPersonal"] = Personal;
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

		// POST: Anamnesis/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<IActionResult> Create(Anamnesis anamnesis, List<string> enfermedades)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				try
				{
					if (ModelState.IsValid)
					{

						//cita odontologica						
						CitaOdontologica citaOdontologica = _context.CitaOdontologica.Where(ci => ci.Codigo == anamnesis.CodigoCitaOdontologica).SingleOrDefault();//_context.CitaOdontologica.Where(ci => ci.FechaInicio.Date == fecha.Date && ci.HoraInicio <= intInicial && ci.HoraFin >= intFinal && ci.CodigoPaciente == anamnesis.CodigoPaciente && ci.CodigoPersonal == anamnesis.CodigoPersonal).FirstOrDefault();
						DateTime FechaCitaCreacion = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
						var transaction = _context.Database.BeginTransaction();
						if (citaOdontologica == null)
						{
							CitaOdontologica cita = new CitaOdontologica();
							Int64 maxCodigoHC = 0;
							maxCodigoHC = Convert.ToInt64(_context.CitaOdontologica.Max(f => f.Codigo));
							maxCodigoHC += 1;
							cita.Codigo = maxCodigoHC.ToString("D8");
							cita.CodigoPaciente = anamnesis.CodigoPaciente;
							cita.CodigoPersonal = anamnesis.CodigoPersonal;
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
							anamnesis.CodigoCitaOdontologica = cita.Codigo;
						}
						else
						{
							anamnesis.CodigoCitaOdontologica = citaOdontologica.Codigo;
						}

						//guardar el anamnesos
						Anamnesis anm = new Anamnesis();
						Int64 maxCodigo = 0;
						maxCodigo = Convert.ToInt64(_context.Anamnesis.Max(f => f.Codigo));
						maxCodigo += 1;
						anm.Codigo = maxCodigo.ToString("D8");
						anm.CodigoCitaOdontologica = anamnesis.CodigoCitaOdontologica;
						anm.MotivoConsulta = anamnesis.MotivoConsulta;
						anm.EnfermedadActual = anamnesis.EnfermedadActual;
						anm.Alerta = anamnesis.Alerta;
						anm.Alergico = anamnesis.Alergico;
						anm.AntecedentesQuirurgicos = anamnesis.AntecedentesQuirurgicos;
						anm.Alergico = anamnesis.Alergico;
						anm.Medicamentos = anamnesis.Medicamentos;
						anm.Habitos = anamnesis.Habitos;
						anm.AntecedentesFamiliares = anamnesis.AntecedentesFamiliares;
						anm.Fuma = anamnesis.Fuma;
						anm.Embarazada = anamnesis.Embarazada;
						anm.UltimaVisitaOdontologo = anamnesis.UltimaVisitaOdontologo;
						anm.Endocrino = anamnesis.Endocrino;
						anm.Traumatologico = anamnesis.Traumatologico;
						anm.Fecha = fecha;

						_context.Anamnesis.Add(anm);

						//guardar AnamenesisEnefermedad
						Int64 maxCodigoAe = 0;
						maxCodigoAe = Convert.ToInt64(_context.AnamnesisEnfermedad.Max(f => f.Codigo));

						foreach (var enf in enfermedades)
						{
							AnamnesisEnfermedad anamnesisEnfermedad = new AnamnesisEnfermedad();
							maxCodigoAe += 1;
							anamnesisEnfermedad.Codigo = maxCodigoAe.ToString("D8");
							anamnesisEnfermedad.CodigoAnamnesis = anm.Codigo;
							anamnesisEnfermedad.CodigoEnfermedad = enf;
							_context.AnamnesisEnfermedad.Add(anamnesisEnfermedad);
						}

						//foreach (var enf in anamnesis.AnamnesisEnfermedad)
						//{
						//	if (enf.Seleccionado)
						//	{
						//		AnamnesisEnfermedad anamnesisEnfermedad = new AnamnesisEnfermedad();
						//		maxCodigoAe += 1;
						//		anamnesisEnfermedad.Codigo = maxCodigoAe.ToString("D8");
						//		//anamnesisEnfermedad.CodigoAnamnesis = anamnesis.Codigo;
						//		anamnesisEnfermedad.CodigoAnamnesis = anm.Codigo;
						//		anamnesisEnfermedad.CodigoEnfermedad = enf.Enfermedad.Codigo;
						//		_context.AnamnesisEnfermedad.Add(anamnesisEnfermedad);
						//	}
						//}

						await _context.SaveChangesAsync();
						transaction.Commit();
						await _auditoria.GuardarLogAuditoria(anm.Fecha, i.Name, "Anamnesis", anm.Codigo, "I");
						ViewBag.Message = "Save";
						return View(anamnesis);
					}
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

					var anamnesis = await _context.Anamnesis.Include(a => a.AnamnesisEnfermedad)
						.ThenInclude(a => a.Enfermedad)
						.Include(a => a.CitaOdontologica).ThenInclude(h => h.Paciente)
						.Include(an => an.CitaOdontologica).ThenInclude(hc => hc.Personal)

						.SingleOrDefaultAsync(f => f.Codigo == codigo);

					if (anamnesis == null)
						return NotFound();

					//lista de enfermedades
					var listaEnfermedades = _context.Enfermedad.OrderBy(f => f.Nombre).ToList();
					listaEnfermedades = listaEnfermedades.FindAll(f => f.Estado == true);

					List<AnamnesisEnfermedad> tci = new List<AnamnesisEnfermedad>();
					tci = anamnesis.AnamnesisEnfermedad;
					for (int l = 0; l < tci.Count(); l++)
					{
						tci[l].Seleccionado = true;
					}
					//enfermedades que estan en anamnesisenfermedad
					List<Enfermedad> listaImpuestosTCI = new List<Enfermedad>();
					List<string> listaE = new List<string>();
					string listaEE = "";
					foreach (var item in tci)
					{
						listaImpuestosTCI.Add(item.Enfermedad);
						listaE.Add(item.Codigo);
						listaEE = listaEE + item.Codigo + ",";
					}
					//enfermedad que faltan en anamnesisEnfermedad
					var listaImpuestosAgregar = (from t in listaEnfermedades where !listaImpuestosTCI.Any(x => x.Codigo == t.Codigo) select t).ToList();

					//agregar a la lista de anamnesisEnfermedad
					foreach (var l in listaImpuestosAgregar)
					{
						AnamnesisEnfermedad ti = new AnamnesisEnfermedad();
						ti.Enfermedad = l;
						ti.Seleccionado = false;
						anamnesis.AnamnesisEnfermedad.Add(ti);
					}

					anamnesis.AnamnesisEnfermedad.OrderBy(p => p.Enfermedad.Nombre);

					ViewData["AnamnesisEnfermedadSeleccionadas"] = listaEE;//listaE.ToString();

					List<SelectListItem> Enfermedades = null;
					Enfermedades = new SelectList(_context.Enfermedad.OrderBy(c => c.Nombre).Where(c => c.Estado == true), "Codigo", "Nombre").ToList();
					//SelectListItem test = new SelectListItem(value: "9", text: "test", selected: true );
					//Enfermedades.Insert(8, test);

					ViewData["AnamnesisEnfermedad"] = Enfermedades;


					//ViewData["AnamnesisEnfermedad"] = anamnesis.AnamnesisEnfermedad;


					List<SelectListItem> Personal = new SelectList(_context.Personal.OrderBy(c => c.NombreCompleto).Where(c => c.Estado == true), "Codigo", "NombreCompleto", anamnesis.CitaOdontologica.Personal.Codigo).ToList();
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
		public async Task<IActionResult> Edit(Anamnesis anamnesis, List<string> enfermedades)
		{
			var i = (ClaimsIdentity)User.Identity;
			List<SelectListItem> Personal = new SelectList(_context.Personal.OrderBy(c => c.NombreCompleto).Where(c => c.Estado == true), "Codigo", "NombreCompleto", anamnesis.CodigoPersonal).ToList();
			List<SelectListItem> Paciente = new SelectList(_context.Paciente.OrderBy(p => p.NombreCompleto).Where(p => p.Estado == true), "Codigo", "NombreCompleto", anamnesis.CodigoPaciente).ToList();

			if (i.IsAuthenticated)
			{
				try
				{
					if (ModelState.IsValid)
					{
						try
						{
							var transaction = _context.Database.BeginTransaction();
							//actualizar anamnesis
							anamnesis.Codigo = Encriptacion.Decrypt(anamnesis.Codigo);
							Anamnesis anamnesisAntiguo = _context.Anamnesis.SingleOrDefault(p => p.Codigo == anamnesis.Codigo);
							anamnesisAntiguo.Codigo = anamnesis.Codigo;
							anamnesisAntiguo.CodigoCitaOdontologica = anamnesis.CodigoCitaOdontologica;
							anamnesisAntiguo.MotivoConsulta = anamnesis.MotivoConsulta;
							anamnesisAntiguo.EnfermedadActual = anamnesis.EnfermedadActual;
							anamnesisAntiguo.Alerta = anamnesis.Alerta;
							anamnesisAntiguo.Alergico = anamnesis.Alergico;
							anamnesisAntiguo.AntecedentesQuirurgicos = anamnesis.AntecedentesQuirurgicos;
							anamnesisAntiguo.Alergico = anamnesis.Alergico;
							anamnesisAntiguo.Medicamentos = anamnesis.Medicamentos;
							anamnesisAntiguo.Habitos = anamnesis.Habitos;
							anamnesisAntiguo.AntecedentesFamiliares = anamnesis.AntecedentesFamiliares;
							anamnesisAntiguo.Fuma = anamnesis.Fuma;
							anamnesisAntiguo.Embarazada = anamnesis.Embarazada;
							anamnesisAntiguo.UltimaVisitaOdontologo = anamnesis.UltimaVisitaOdontologo;
							anamnesisAntiguo.Endocrino = anamnesis.Endocrino;
							anamnesisAntiguo.Traumatologico = anamnesis.Traumatologico;
							anamnesisAntiguo.Fecha = fecha;

							var anamnesisEnf = _context.AnamnesisEnfermedad.Where(a => a.CodigoAnamnesis == anamnesis.Codigo).ToList();
							foreach (var item in anamnesisEnf)
								_context.AnamnesisEnfermedad.Remove(item);
							_context.SaveChanges();

							//guardar AnamenesisEnefermedad
							Int64 maxCodigoAe = 0;
							maxCodigoAe = Convert.ToInt64(_context.AnamnesisEnfermedad.Max(f => f.Codigo));

							foreach (var enf in enfermedades)
							{
								AnamnesisEnfermedad anamnesisEnfermedad = new AnamnesisEnfermedad();
								maxCodigoAe += 1;
								anamnesisEnfermedad.Codigo = maxCodigoAe.ToString("D8");
								anamnesisEnfermedad.CodigoAnamnesis = anamnesis.Codigo;
								anamnesisEnfermedad.CodigoEnfermedad = enf;
								_context.AnamnesisEnfermedad.Add(anamnesisEnfermedad);
							}

							//foreach (var enf in anamnesis.AnamnesisEnfermedad)
							//{
							//	if (enf.Seleccionado)
							//	{
							//		AnamnesisEnfermedad anamnesisEnfermedad = new AnamnesisEnfermedad();
							//		maxCodigoAe += 1;
							//		anamnesisEnfermedad.Codigo = maxCodigoAe.ToString("D8");
							//		//anamnesisEnfermedad.CodigoAnamnesis = null;
							//		anamnesisEnfermedad.CodigoAnamnesis = anamnesis.Codigo;
							//		anamnesisEnfermedad.CodigoEnfermedad = enf.Enfermedad.Codigo;
							//		_context.AnamnesisEnfermedad.Add(anamnesisEnfermedad);
							//	}
							//}

							_context.Update(anamnesisAntiguo);
							_context.SaveChanges();
							transaction.Commit();
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
