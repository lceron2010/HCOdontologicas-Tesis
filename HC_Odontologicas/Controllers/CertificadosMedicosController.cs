﻿using HC_Odontologicas.FuncionesGenerales;
using HC_Odontologicas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HC_Odontologicas.Controllers
{
	public class CertificadosMedicosController : Controller
	{
		private readonly HCOdontologicasContext _context;
		private ValidacionesController validaciones;
		private readonly AuditoriaController _auditoria;
		SelectListItem vacio = new SelectListItem(value: "0", text: "Seleccione...");


		public CertificadosMedicosController(HCOdontologicasContext context)
		{
			_context = context;
			validaciones = new ValidacionesController(_context);
			_auditoria = new AuditoriaController(context);
		}

		// GET: CertificadosMedicos
		public async Task<IActionResult> Index(string sortOrder, string Filter, int? page, string search)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				//Permisos de usuario
				var permisos = i.Claims.Where(c => c.Type == "CertificadosMedicos").Select(c => c.Value).SingleOrDefault().Split(";");
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

					var fechaInicioDia = new DateTime();
					var fechaInicioFinDia = new DateTime();



					if (String.IsNullOrEmpty(search))
					{
						var fecha = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
						//fechaInicioDia = new DateTime(fecha.Year, fecha.Month, fecha.Day, 00, 00, 00);
						//fechaInicioFinDia = new DateTime(fecha.Year, fecha.Month, fecha.Day, 23, 59, 59);
						fechaInicioDia = new DateTime(fecha.Year, fecha.Month, 11, 00, 00, 00);
						fechaInicioFinDia = new DateTime(fecha.Year, fecha.Month, 11, 23, 59, 59);
					}
					else
					{
						var fecha = Convert.ToDateTime(search);
						fechaInicioDia = new DateTime(fecha.Year, fecha.Month, fecha.Day, 00, 00, 00);
						fechaInicioFinDia = new DateTime(fecha.Year, fecha.Month, fecha.Day, 23, 59, 59);

						
					}//poner liuego esto

					var citaOdontologica = from c in _context.CitaOdontologica.Include(a => a.Personal).Include(a => a.Paciente)
										   .Where(a => a.FechaInicio > fechaInicioDia && a.FechaInicio < fechaInicioFinDia && (a.Estado == "A"))
										   .OrderBy(p => p.FechaInicio)
										   select c;


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

		//MostrarCertificado
		[HttpGet]
		public IActionResult CertificadoMedico(string Codigo)
		{
			Codigo = Encriptacion.Decrypt(Codigo);
			return new ViewAsPdf("CertificadoPDF", GetOne(Codigo));
		}

		public CertificadoMedicoPdf GetOne(string Codigo)
		{
			PlantillaCertificadoMedico certificado = _context.PlantillaCertificadoMedico.Where(c => c.Codigo == Codigo).SingleOrDefault();
			CertificadoMedicoPdf ti = new CertificadoMedicoPdf();
			ti.Contenido = certificado.Descripcion;
			var indice = certificado.Nombre.IndexOf("-");
			ti.Odontologo = certificado.Nombre.Substring(0, indice).Trim();
			ti.FechaActual = Funciones.ObtenerFechaActual("SA Pacific Standard Time").ToString("dd/MM/yyyy");
			return ti;

		}

		// GET: CertificadosMedicos/Edit/5
		public async Task<IActionResult> Edit(string Codigo)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "CertificadosMedicos").Select(c => c.Value).SingleOrDefault().Split(";");
				Codigo = Encriptacion.Decrypt(Codigo);

				if (Convert.ToBoolean(permisos[2]))
				{
					if (Codigo == null)
						return NotFound();


					var cita = await _context.CitaOdontologica.Include(c => c.Paciente).Include(c => c.Personal).SingleOrDefaultAsync(f => f.Codigo == Codigo);

					var diag = _context.Diagnostico.Include(d => d.DiagnosticoCie10).ThenInclude(d => d.Cie10).Where(d => d.CodigoCitaOdontologica == cita.Codigo).SingleOrDefault();
					if (cita == null)
						return NotFound();

					CertificadoMedicoImprimir cmi = new CertificadoMedicoImprimir();

					cmi.CedulaPaciente = cita.Paciente.Identificacion;
					cmi.NombrePaciente = cita.Paciente.NombreCompleto;
					cmi.FechaCita = cita.FechaInicio;//new DateTime(cita.FechaInicio.Day, cita.FechaInicio.Month, cita.FechaInicio.Year);
					cmi.HoraInicio = cita.HoraInicio;
					cmi.HoraFin = cita.HoraFin;
					List<SelectListItem> Cie10 = new SelectList(_context.Cie10.OrderBy(f => f.CodigoInterno), "Codigo", "CodigoNombre", diag.DiagnosticoCie10[0].Cie10.Codigo).ToList();
					Cie10.Insert(0, vacio);
					ViewData["CIE10"] = Cie10;
					cmi.CIE10Nombre = diag.DiagnosticoCie10[0].Cie10.CodigoInterno + " - " + diag.DiagnosticoCie10[0].Cie10.Nombre;
					cmi.Procedimiento = "";
					cmi.Pieza = diag.Pieza;
					cmi.Reposo = false;
					//cmi.FechaInicioReposo = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
				//	cmi.FechaFinReposo = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
					//cmi.FechaReincorporarse = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
					cmi.Fecha = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
					cmi.NombreMedico = cita.Personal.NombreCompleto;

					return View(cmi);
				}
				else
					return Redirect("../CertificadosMedicos");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}

		}

		[HttpPost]
		public async Task<IActionResult> Edit(CertificadoMedicoImprimir cmi)
		{
			var i = (ClaimsIdentity)User.Identity;

			if (i.IsAuthenticated)
			{
				try
				{
					if (ModelState.IsValid)
					{
						try
						{
							var plantilla = _context.PlantillaCertificadoMedico.Where(p => p.Nombre.Contains("Certificado Asistencia Reposo")).SingleOrDefault();
							var contenido = plantilla.Descripcion;
							var sub = "NO";
							if (cmi.CitasSubsecuentes)
							{
								sub = "SI";
							}
							var rep = "NO";
							DateTime fechaInicioReposo = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
							DateTime fechaFinReposo = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
							DateTime fechaReincorporarse = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
							//var Fechareincorporarse =
							if (cmi.Reposo)
							{
								rep = "SI";
								if (cmi.NumdiasReposo > 0)
								{
									fechaFinReposo = Funciones.ObtenerFechaActual("SA Pacific Standard Time").AddDays(cmi.NumdiasReposo - 1);
									
								}
								fechaReincorporarse = fechaFinReposo.AddDays(1);

							}
							else
							{
								var index = contenido.IndexOf("[@ReposoInicio]");
								contenido = contenido.Substring(0, index - 6);
							}

							string fir = fechaInicioReposo.ToString("dd/MM/yyyy");
							string ffr = fechaFinReposo.ToString("dd/MM/yyyy");
							string fr = fechaReincorporarse.ToString("dd/MM/yyyy");

							var final = contenido.Replace("[@FechaCita]", cmi.FechaCita.ToString("dd/MM/yyyy"))
								.Replace("[@Paciente]", cmi.NombrePaciente)
								.Replace("[@Cedula]", cmi.CedulaPaciente)
								.Replace("[@HoraInicio]", cmi.HoraInicio.ToString())
								.Replace("[@HoraFin]", cmi.HoraFin.ToString())
								.Replace("[@Cie10]", cmi.CIE10Nombre)
								.Replace("[@Pieza]", cmi.Pieza.ToString())
								.Replace("[@Procedimiento]", cmi.Procedimiento)
								.Replace("[@CitasSubsecuentes]", sub)
								.Replace("[@Reposo]", rep)
								.Replace("[@ReposoInicio]", fir)
								.Replace("[@ReposoFin]", ffr)
								.Replace("[@ReposoReincorpararse]", fr);

							//.ToString("dd/MM/yyyy"))
							//guardar el contenido

							PlantillaCertificadoMedico plantillaCertificadoMedico = new PlantillaCertificadoMedico();
							Int64 maxCodigo = 0;
							maxCodigo = Convert.ToInt64(_context.PlantillaCertificadoMedico.Max(f => f.Codigo));
							maxCodigo += 1;
							plantillaCertificadoMedico.Codigo = maxCodigo.ToString("D4");
							plantillaCertificadoMedico.Nombre = cmi.NombreMedico + " - " + cmi.NombrePaciente;
							plantillaCertificadoMedico.Descripcion = final;

							_context.Add(plantillaCertificadoMedico);
							await _context.SaveChangesAsync();
							await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "CertifidoMedicoPaciente", plantillaCertificadoMedico.Codigo, "I");

							string Codigo = Encriptacion.Encrypt(plantillaCertificadoMedico.Codigo);

							return Redirect("../CertificadosMedicos/CertificadoMedico?Codigo=" + Codigo);

						}
						catch (DbUpdateConcurrencyException)
						{
							throw;
						}
					}

					return View(cmi);
				}
				catch (Exception e)
				{
					string mensaje = e.Message;
					if (e.InnerException != null)
						mensaje = MensajesError.UniqueKey(e.InnerException.Message);

					ViewBag.Message = mensaje;

					return View(cmi);
				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}

		}


	}

}
