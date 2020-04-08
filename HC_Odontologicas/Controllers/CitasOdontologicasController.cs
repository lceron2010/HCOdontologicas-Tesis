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
	public class CitasOdontologicasController : Controller
	{
		private readonly HCOdontologicasContext _context;
		SelectListItem vacio = new SelectListItem(value: "0", text: "Seleccione...");
		private readonly AuditoriaController _auditoria;

		public CitasOdontologicasController(HCOdontologicasContext context)
		{
			_context = context;
			_auditoria = new AuditoriaController(context);
		}

		// GET: CitasOdontologicas
		public async Task<IActionResult> Index(string Filter, int? page, string search)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				//Permisos de usuario
				var permisos = i.Claims.Where(c => c.Type == "CitasOdontologicas").Select(c => c.Value).SingleOrDefault().Split(";");
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
					
					var fecha = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
					var fechaInicioDia = new DateTime(fecha.Year, fecha.Month, fecha.Day, 00, 00, 00);
					var fechaInicioFinDia = new DateTime(fecha.Year, fecha.Month, fecha.Day, 23, 59, 59);

					var citaOdontologica = from c in _context.CitaOdontologica.Include(a => a.Personal).Include(a => a.Paciente)
										   .Where(a => a.FechaInicio > fechaInicioDia && a.FechaInicio < fechaInicioFinDia && (a.Estado=="C" || a.Estado=="M"))
										   .OrderBy(p => p.FechaInicio) select c; 


					if (!String.IsNullOrEmpty(search))
						citaOdontologica = citaOdontologica
							.Where(s => s.Paciente.NombreCompleto.Contains(search) 
							|| s.Paciente.Nombres.Contains(search)
							|| s.Paciente.Apellidos.Contains(search)
							|| s.Paciente.Identificacion.Contains(search));

					
					int pageSize = 10;

					return View(await Paginacion<CitaOdontologica>.CreateAsync(citaOdontologica, page ?? 1, pageSize)); // page devuelve valor si lo tiene caso contrario devuelve 1
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


		// GET: CitasOdontologicas/Create
		public IActionResult Create()
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "CitasOdontologicas").Select(c => c.Value).SingleOrDefault().Split(";");

				if (Convert.ToBoolean(permisos[1]))
				{
					//ViewData["CodigoPaciente"] = new SelectList(_context.Paciente, "Codigo", "Codigo");
					//ViewData["CodigoPersonal"] = new SelectList(_context.Personal, "Codigo", "Codigo");
					return View();
				}
				else
					return Redirect("../CitasOdontologicas");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}			

		}

		// POST: CitasOdontologicas/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<IActionResult> Create(CitaOdontologica citaOdontologica)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				try
				{


					if (ModelState.IsValid)
					{
						_context.Add(citaOdontologica);
						await _context.SaveChangesAsync();
						return RedirectToAction(nameof(Index));
					}
					ViewData["CodigoPaciente"] = new SelectList(_context.Paciente, "Codigo", "Codigo");
					ViewData["CodigoPersonal"] = new SelectList(_context.Personal, "Codigo", "Codigo");
					return View(citaOdontologica);

				}
				catch (Exception e)
				{
					string mensaje = e.Message;
					if (e.InnerException != null)
						mensaje = MensajesError.UniqueKey(e.InnerException.Message);

					ViewBag.Message = mensaje;

					return View();

				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// GET: CitasOdontologicas/Edit/5
		public async Task<IActionResult> Edit(string Codigo)
		{

			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "CitasOdontologicas").Select(c => c.Value).SingleOrDefault().Split(";");
				Codigo = Encriptacion.Decrypt(Codigo);
				if (Convert.ToBoolean(permisos[2]))
				{
					if (Codigo == null)
						return NotFound();

					var citaOdontologica = await _context.CitaOdontologica.SingleOrDefaultAsync(f => f.Codigo == Codigo);

					if (citaOdontologica == null)
						return NotFound();

					//var cita = _context.CitaOdontologica.Where(c => c.Codigo == codigo).SingleOrDefault();
					var paciente = _context.Paciente.Where(p => p.Codigo == citaOdontologica.CodigoPaciente).Include(p => p.Facultad).Include(p => p.Carrera).SingleOrDefault();

					//datos del paciente
					ViewData["Cedula"] = paciente.Identificacion;
					ViewData["Nombre"] = paciente.NombreCompleto;
					ViewData["Direccion"] = paciente.Direccion;
					ViewData["Correo"] = paciente.MailEpn;
					ViewData["Telefono"] = paciente.Celular;
					if (paciente.Facultad == null)
					{
						ViewData["Facultad"] = "";
						ViewData["Carrera"] = "";
					}
					else
					{
						ViewData["Facultad"] = paciente.Facultad.Nombre;
						ViewData["Carrera"] = paciente.Carrera.Nombre;
					}

					ViewData["CodigoCitaOdontologica"] = Codigo;

					//paciente
					List<SelectListItem> TipoIdentificacion = new SelectList(_context.TipoIdentificacion.OrderBy(f => f.Nombre), "Codigo", "Nombre").ToList();
					ViewData["CodigoTipoIdentificacion"] = TipoIdentificacion;

					List<SelectListItem> Facultad = new SelectList(_context.Facultad.OrderBy(f => f.Nombre), "Codigo", "Nombre").ToList();
					Facultad.Insert(0, vacio);
					ViewData["CodigoFacultad"] = Facultad;

					//anamnesis

					List<SelectListItem> Enfermedades = null;
					Enfermedades = new SelectList(_context.Enfermedad.OrderBy(c => c.Nombre).Where(c => c.Estado == true), "Codigo", "Nombre").ToList();
					ViewData["AnamnesisEnfermedad"] = Enfermedades;

					//diagnostico
					List<SelectListItem> Cie10 = new SelectList(_context.Cie10.OrderBy(f => f.CodigoInterno), "Codigo", "CodigoNombre").ToList();//.Where(f => f.Nombre.StartsWith("C")).ToList(); //QUITAR LUEGO					;
					Cie10.Insert(0, vacio);
					ViewData["CIE10"] = Cie10;

					//concentimiento informado
					var PlantillaCI = _context.PlantillaConsentimientoInformado.Where(c => c.Nombre.Contains("Consentimiento Informado")).SingleOrDefault();
					ViewData["Descripcion"] = PlantillaCI.Descripcion;

					//receta medica
					List<SelectListItem> PlantillaRM = new SelectList(_context.PlantillaRecetaMedica.OrderBy(c => c.Nombre), "Codigo", "Nombre").ToList();
					PlantillaRM.Insert(0, vacio);
					ViewData["CodigoPlantillaReceta"] = PlantillaRM;

					return View(citaOdontologica);
				}
				else
					return Redirect("../Anamnesis");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}

		}

		// POST: CitasOdontologicas/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]

		public async Task<IActionResult> Edit(CitaOdontologica citaOdontologica, List<string> enfermedades, List<Odontograma> ListaOdontogramas)
		{
			var i = (ClaimsIdentity)User.Identity;
			var transaction = _context.Database.BeginTransaction();
			if (i.IsAuthenticated)
			{
				try
				{


					if (ModelState.IsValid)
					{
						DateTime fecha = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
						citaOdontologica.Codigo = Encriptacion.Decrypt(citaOdontologica.Codigo);
						
						//anamnesis
						Anamnesis anamnesis = new Anamnesis();
						Int64 maxCodigoAnamnesis = 0;
						maxCodigoAnamnesis = Convert.ToInt64(_context.Anamnesis.Max(f => f.Codigo));
						maxCodigoAnamnesis += 1;						
						anamnesis.Codigo = maxCodigoAnamnesis.ToString("D8");
						anamnesis.CodigoCitaOdontologica = citaOdontologica.Codigo;
						anamnesis.MotivoConsulta = citaOdontologica.MotivoConsulta;
						anamnesis.EnfermedadActual = citaOdontologica.EnfermedadActual;
						anamnesis.Alerta = citaOdontologica.Alerta;
						anamnesis.Alergico = citaOdontologica.Alergico;
						anamnesis.AntecedentesQuirurgicos = citaOdontologica.AntecedentesQuirurgicos;
						anamnesis.Alergico = citaOdontologica.Alergico;
						anamnesis.Medicamentos = citaOdontologica.Medicamentos;
						anamnesis.Habitos = citaOdontologica.Habitos;
						anamnesis.AntecedentesFamiliares = citaOdontologica.AntecedentesFamiliares;
						anamnesis.Fuma = citaOdontologica.Fuma;
						anamnesis.Embarazada = citaOdontologica.Embarazada;
						anamnesis.UltimaVisitaOdontologo = citaOdontologica.UltimaVisitaOdontologo;
						anamnesis.Endocrino = citaOdontologica.Endocrino;
						anamnesis.Traumatologico = citaOdontologica.Traumatologico;
						anamnesis.Fecha = fecha;
						_context.Anamnesis.Add(anamnesis);

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

						_context.SaveChanges();

						//diagnostico
						Diagnostico diagnostico = new Diagnostico();
						Int64 maxCodigoDiag = 0;
						maxCodigoDiag = Convert.ToInt64(_context.Diagnostico.Max(f => f.Codigo));
						maxCodigoDiag += 1;
						diagnostico.Codigo = maxCodigoDiag.ToString("D8");
						diagnostico.CodigoCitaOdontologica = citaOdontologica.Codigo;
						diagnostico.Fecha = fecha;
						diagnostico.Pieza = citaOdontologica.Pieza;
						diagnostico.Observacion = citaOdontologica.Observacion;
						diagnostico.Firma = citaOdontologica.Firma;
						diagnostico.Recomendacion = citaOdontologica.Recomendacion;
						_context.Diagnostico.Add(diagnostico);

						var diagnosticoCie10 = _context.DiagnosticoCie10.Where(a => a.CodigoDiagnostico == diagnostico.Codigo).ToList();
						foreach (var item in diagnosticoCie10)
							_context.DiagnosticoCie10.Remove(item);
						_context.SaveChanges();

						//guardar diagnosticoCie10
						Int64 maxCodigoCie = 0;
						maxCodigoCie = Convert.ToInt64(_context.DiagnosticoCie10.Max(f => f.Codigo));
						maxCodigoCie += 1;
						DiagnosticoCie10 diagCie10 = new DiagnosticoCie10();
						diagCie10.Codigo = maxCodigoCie.ToString("D8");
						diagCie10.CodigoDiagnostico = diagnostico.Codigo;
						diagCie10.CodigoCie10 = citaOdontologica.CodigoDiagnosticoCie10;
						_context.DiagnosticoCie10.Add(diagCie10);
						_context.SaveChanges();

						//consentimiento informado
						ConsentimientoInformado consentimientoInformado = new ConsentimientoInformado();
						Int64 maxCodigoCi = 0;
						maxCodigoCi = Convert.ToInt64(_context.ConsentimientoInformado.Max(f => f.Codigo));
						maxCodigoCi += 1;
						consentimientoInformado.Codigo = maxCodigoCi.ToString("D8");
						consentimientoInformado.CodigoCitaOdontologica = citaOdontologica.Codigo;
						consentimientoInformado.Descripcion = citaOdontologica.Descripcion;
						consentimientoInformado.Firma = citaOdontologica.FirmaConcentimiento;
						consentimientoInformado.Fecha = fecha;
						_context.Add(consentimientoInformado);
						_context.SaveChanges();

						var codigoReceta = "";
						if (! ( (string.IsNullOrEmpty(citaOdontologica.DescripcionReceta)) &&( string.IsNullOrEmpty(citaOdontologica.Indicaciones))))
						{
							//receta medica
							RecetaMedica recetaMedica = new RecetaMedica();
							Int64 maxCodigoR = 0;
							maxCodigoR = Convert.ToInt64(_context.RecetaMedica.Max(f => f.Codigo));
							maxCodigoR += 1;
							recetaMedica.Codigo = maxCodigoR.ToString("D8");
							recetaMedica.CodigoCitaOdontologica = citaOdontologica.Codigo;
							recetaMedica.Descripcion = citaOdontologica.DescripcionReceta;
							recetaMedica.Fecha = fecha;
							if (citaOdontologica.CodigoPlantillaRecetaMedica == "0")
							{
								recetaMedica.CodigoPlantillaRecetaMedica = null;
							}
							else
							{
								recetaMedica.CodigoPlantillaRecetaMedica = citaOdontologica.CodigoPlantillaRecetaMedica;
							}
							
							recetaMedica.Indicaciones = citaOdontologica.Indicaciones;
							codigoReceta = recetaMedica.Codigo;
							_context.Add(recetaMedica);
							_context.SaveChanges();
						}
																		
						CitaOdontologica citaAntigua = _context.CitaOdontologica.SingleOrDefault(p => p.Codigo == citaOdontologica.Codigo);
						citaAntigua.Codigo = citaOdontologica.Codigo;
						citaAntigua.CodigoPaciente = citaOdontologica.CodigoPaciente;
						citaAntigua.CodigoPersonal = citaOdontologica.CodigoPersonal;
						citaAntigua.FechaCreacion = citaOdontologica.FechaCreacion;
						citaAntigua.Observaciones = citaOdontologica.Observaciones;
						citaAntigua.Estado = "A";
						citaAntigua.FechaInicio = citaOdontologica.FechaInicio;
						citaAntigua.FechaFin = citaOdontologica.FechaFin;
						citaAntigua.HoraInicio = citaOdontologica.HoraInicio;
						citaAntigua.HoraFin = citaOdontologica.HoraFin;
						citaAntigua.UsuarioCreacion = i.Name;
						
						_context.Update(citaAntigua);

						_context.SaveChanges();

						await _auditoria.GuardarLogAuditoria(fecha, i.Name, "Anamnesis", anamnesis.Codigo, "I");
						
						await _auditoria.GuardarLogAuditoria(fecha, i.Name, "Diagnostico", diagnostico.Codigo, "I");
						await _auditoria.GuardarLogAuditoria(fecha, i.Name, "ConsentmientoInformado", consentimientoInformado.Codigo, "I");
						if (!string.IsNullOrEmpty(codigoReceta))
						{
							await _auditoria.GuardarLogAuditoria(fecha, i.Name, "Receta", codigoReceta, "I");
						}
						
						await _auditoria.GuardarLogAuditoria(fecha, i.Name, "CitaOdontologica", citaOdontologica.Codigo, "I");

						transaction.Commit();
						

						ViewBag.Message = "Save";

						return View(citaOdontologica);

					}
					else
					{
						ViewBag.Message = "Algo salio mal";

						return View(citaOdontologica);
					}

				}
				catch (Exception e)
				{
					string mensaje = e.Message;
					if (e.InnerException != null)
						mensaje = MensajesError.UniqueKey(e.InnerException.Message);
					ViewBag.Message = mensaje;

					transaction.Rollback();

					//eliminar el odontograma si hay algun error
					//citaOdontologica.Codigo = Encriptacion.Decrypt(citaOdontologica.Codigo);
					var odontograma = await _context.Odontograma.SingleOrDefaultAsync(f => f.CodigoCitaOdontologica == citaOdontologica.Codigo);
					_context.Odontograma.Remove(odontograma);
					await _context.SaveChangesAsync();

					return View(citaOdontologica);
				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}


		}
	}
}
