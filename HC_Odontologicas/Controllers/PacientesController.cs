using HC_Odontologicas.FuncionesGenerales;
using HC_Odontologicas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HC_Odontologicas.Controllers
{
	public class PacientesController : Controller
	{
		private readonly HCOdontologicasContext _context;
		private ValidacionesController validaciones;
		private readonly AuditoriaController _auditoria;

		private readonly String pathRootDocumentos = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + "\\";

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
				ViewData["Importar"] = Convert.ToBoolean(permisos[5]);

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

					if (!String.IsNullOrEmpty(search)) //para no mostrar nada al iniciar
						pacientes = pacientes.Where(s => s.NombreCompleto.Contains(search) || s.Identificacion.Contains(search)
						|| s.NumeroUnico.Contains(search) || s.Nombres.Contains(search) || s.Apellidos.Contains(search));

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
					if (paciente.FechaNacimiento >= Funciones.ObtenerFechaActual("SA Pacific Standard Time").Date)
					{
						var mensajeR = "La fecha debe ser menor a la actual.";
						if (!string.IsNullOrEmpty(mensajeR))
							ModelState.AddModelError("FechaNacimiento", mensajeR);
					}


					if (paciente.TipoPaciente == "E" || paciente.TipoPaciente == "EB" || paciente.TipoPaciente == "EC" || paciente.TipoPaciente == "EN")
					{
						var mensajeR = validaciones.VerifyCampoRequerido(paciente.NumeroUnico);
						if (!string.IsNullOrEmpty(mensajeR))
							ModelState.AddModelError("NumeroUnico", mensajeR);

						mensajeR = validaciones.VerifyComboRequerido(paciente.CodigoFacultad);
						if (!string.IsNullOrEmpty(mensajeR))
							ModelState.AddModelError("Facultad", mensajeR);

						mensajeR = validaciones.VerifyComboRequerido(paciente.CodigoCarrera);
						if (!string.IsNullOrEmpty(mensajeR))
							ModelState.AddModelError("Carrera", mensajeR);
					}
					else if (paciente.TipoPaciente == "D" || paciente.TipoPaciente == "PA")
					{
						var mensajeR = validaciones.VerifyCampoRequerido(paciente.Cargo);
						if (!string.IsNullOrEmpty(mensajeR))
							ModelState.AddModelError("Cargo", mensajeR);
					}
					else
					{
						var mensajeR = validaciones.VerifyComboRequerido(paciente.TipoPaciente);
						if (!string.IsNullOrEmpty(mensajeR))
							ModelState.AddModelError("TipoPaciente", mensajeR);
					}

					if (ModelState.IsValid)
					{
						Int64 maxCodigo = 0;
						maxCodigo = Convert.ToInt64(_context.Paciente.Max(f => f.Codigo));
						maxCodigo += 1;
						paciente.Codigo = maxCodigo.ToString("D8");
						paciente.Nombres = paciente.Nombres.ToUpper();
						paciente.Apellidos = paciente.Apellidos.ToUpper();
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


					List<SelectListItem> TipoIdentificacion = new SelectList(_context.TipoIdentificacion.OrderBy(t => t.Nombre), "Codigo", "Nombre", paciente.CodigoTipoIdentificacion).ToList();
					TipoIdentificacion.Insert(0, vacio);
					ViewData["CodigoTipoIdentificacion"] = TipoIdentificacion;

					List<SelectListItem> Facultad = new SelectList(_context.Facultad.OrderBy(f => f.Nombre), "Codigo", "Nombre", paciente.CodigoFacultad).ToList();
					Facultad.Insert(0, vacio);
					ViewData["CodigoFacultad"] = Facultad;

					List<SelectListItem> Carrera = new SelectList(_context.Carrera.OrderBy(f => f.Nombre), "Codigo", "Nombre", paciente.CodigoCarrera).ToList();
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
			List<SelectListItem> Carrera = new SelectList(_context.Carrera.OrderBy(f => f.Nombre).Where(p => p.CodigoFacultad == paciente.CodigoFacultad), "Codigo", "Nombre").ToList();

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

		//listar las carreras de acuerdo a la facultad
		[HttpPost]
		public async Task<List<SelectListItem>> CargarDatosCarrera(String CodigoFacultad)
		{
			List<SelectListItem> list = new List<SelectListItem>();
			var Carrera = await _context.Carrera.OrderBy(f => f.Nombre).Where(p => p.CodigoFacultad == CodigoFacultad).ToListAsync();
			list.Insert(0, new SelectListItem("Seleccione...", "0"));
			foreach (Carrera item in Carrera.ToList())
				list.Add(new SelectListItem(item.Nombre, item.Codigo));
			return list;
		}

		//impotar datos
		//mostrar datos en el model.
		public String CargarDatosTabla(IFormFile Documento)
		{

			try
			{
				DataTable table = new DataTable();
				if (Documento != null)
				{
					if (GetMimeTypes().SingleOrDefault(p => p.Value == Documento.ContentType && p.Key == "." + Documento.FileName.Split(".")[Documento.FileName.Split(".").Count() - 1]).Value != null)
					{
						UploadFile(Documento);
					}
					else
					{
						Documento = null;
						ViewData["ExtensionArchivo"] = "Extensión del documento incorrecto";
					}

				}
				if ((ViewData["ExtensionArchivo"]) is null)
				{
					string fileName = Documento.FileName;
					FileInfo file = new FileInfo(Path.Combine(pathRootDocumentos, fileName));
					//llenar la dataTable con el excel

					ExcelPackage package = new ExcelPackage(file);
					ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
					foreach (var firstRowCell in workSheet.Cells[1, 1, 1, workSheet.Dimension.End.Column])
					{
						int indice = firstRowCell.Text.IndexOf(".");
						if (indice == firstRowCell.Text.Length - 1)
						{
							table.Columns.Add(firstRowCell.Text.Substring(0, indice));
						}
						else
						{
							table.Columns.Add(firstRowCell.Text);
						}
					}

					for (var rowNumber = 2; rowNumber <= workSheet.Dimension.End.Row; rowNumber++)
					{
						var row = workSheet.Cells[rowNumber, 1, rowNumber, workSheet.Dimension.End.Column];
						var newRow = table.NewRow();

						foreach (var cell in row)
						{
							if (cell.Start.Column == 1)
							{
								newRow[cell.Start.Column - 1] = cell.Text.TrimEnd();
							}
							else
							{
								newRow[cell.Start.Column - 2] = cell.Text.TrimEnd();
							}

						}
						table.Rows.Add(newRow);
					}

					//newRow[cell.Start.Column - 1] = cell.Text;

					//añadir la nueva columna de observaciones
					table.Columns.Add("Observaciones");

					//validar los datos
					foreach (DataRow dr in table.Rows)
					{
						dr["Observaciones"] = validarDatos(dr);
					}

					DeleteFile(Documento.FileName);
				}

				String tabla = JsonConvert.SerializeObject(table);

				return tabla;
			}

			catch (Exception ex)
			{
				DeleteFile(Documento.FileName);
				throw new Exception(ex.Message, ex);

			}
		}

		private string validarDatos(DataRow dr)
		{
			string mensaje = "";

			if (string.IsNullOrEmpty(dr["Código"].ToString()))
			{
				mensaje += " Debe tener un Código";
			}

			if (string.IsNullOrEmpty(dr["Nombre"].ToString()))
			{
				mensaje += " Debe tener un Nombre";
			}


			if (string.IsNullOrEmpty(dr["Cédula"].ToString()))
			{
				mensaje += " Debe tener una Cédula";
			}
			if (string.IsNullOrEmpty(dr["FechaNac"].ToString()))
			{
				mensaje += " Debe tener una FechaNac";
			}
			if (string.IsNullOrEmpty(dr["Genero"].ToString()))
			{
				mensaje += " Debe tener un Genero";
			}

			if (string.IsNullOrEmpty(dr["Dirección"].ToString()))
			{
				mensaje += " Debe tener una Dirección";
			}
			if (string.IsNullOrEmpty(dr["EmailEPN"].ToString()))
			{
				mensaje += " Debe tener un EmailEPN";
			}

			return mensaje;
		}


		//guardar los datos en la base
		[HttpPost]
		public string GuardarDatosImportados(IFormFile Documento)
		{
			string codigo = null;
			var j = (ClaimsIdentity)User.Identity;
			try
			{
				string fileName = Documento.FileName;
				FileInfo file = new FileInfo(Path.Combine(pathRootDocumentos, fileName));

				if (Documento != null)
				{
					if (GetMimeTypes().SingleOrDefault(p => p.Value == Documento.ContentType && p.Key == "." + Documento.FileName.Split(".")[Documento.FileName.Split(".").Count() - 1]).Value != null)
					{
						UploadFile(Documento);
					}
					else
					{
						Documento = null;
						ViewData["ExtensionArchivo"] = "Extensión del documento incorrecto";
					}

				}

				//if ((ViewData["ExtensionArchivo"]) is null)
				//{

				ExcelPackage package = new ExcelPackage(file);
				ExcelWorksheet workSheet = package.Workbook.Worksheets.First();
				int totalRows = workSheet.Dimension.Rows;

				Int64 maxCodigo = 0;
				maxCodigo = Convert.ToInt64(_context.Paciente.Max(f => f.Codigo));
				maxCodigo += 1;
				codigo = maxCodigo.ToString("D8");
				DateTime fechaPacienteGuardado = Funciones.ObtenerFechaActual("SA Pacific Standard Time");


				List<Paciente> listaPacientes = new List<Paciente>();
				for (int i = 2; i <= totalRows; i++)
				{
					Paciente pc = new Paciente();
					if (workSheet.Cells[i, 1].Value != null)
					{
						string nombreCompleto = workSheet.Cells[i, 3].Value.ToString();
						string[] nombres = nombreCompleto.Split(" ");
						pc.Codigo = maxCodigo.ToString("D8");
						pc.NumeroUnico = workSheet.Cells[i, 2].Value.ToString();
						pc.Nombres = nombres[2] + " " + nombres[3];
						pc.Apellidos = nombres[0] + " " + nombres[1];
						pc.Identificacion = workSheet.Cells[i, 4].Value.ToString();
						pc.FechaNacimiento = Convert.ToDateTime(workSheet.Cells[i, 5].Value.ToString());
						pc.Genero = workSheet.Cells[i, 6].Value.ToString();
						pc.Direccion = workSheet.Cells[i, 7].Value.ToString();
						pc.Telefono = workSheet.Cells[i, 8].Value.ToString();
						pc.Celular = workSheet.Cells[i, 9].Value.ToString();
						pc.MailPersonal = workSheet.Cells[i, 10].Value.ToString();
						pc.MailEpn = workSheet.Cells[i, 21].Value.ToString();
						pc.EstadoCivil = null;
						pc.DependenciaDondeTrabaja = null;
						pc.Cargo = null;
						pc.Procedencia = null;
						pc.TipoPaciente = null;
						pc.Estado = true;
						pc.CodigoTipoIdentificacion = null;
						pc.CodigoFacultad = null;// buscarFacultad(workSheet.Cells[i, 12].Value.ToString());
						pc.CodigoCarrera = null;//buscarCarrrera(pc.CodigoFacultad, workSheet.Cells[i, 11].Value.ToString());
						_context.Add(pc);
						//_auditoria.GuardarLogAuditoria(fechaPacienteGuardado, j.Name, "Paciente", pc.Codigo, "I");

						//listaPacientes.Add(pc);
						maxCodigo = maxCodigo + 1;
						//}
					}
					else
					{
						string nombreCompleto = workSheet.Cells[i, 4].Value.ToString();
						string[] nombres = nombreCompleto.Split(" ");

						pc.Codigo = maxCodigo.ToString("D8");
						pc.NumeroUnico = workSheet.Cells[i, 3].Value.ToString();
						pc.Nombres = nombres[2] + " " + nombres[3];
						pc.Apellidos = nombres[0] + " " + nombres[1];
						pc.Identificacion = workSheet.Cells[i, 5].Value.ToString();
						pc.FechaNacimiento = Convert.ToDateTime(workSheet.Cells[i, 6].Value.ToString());
						pc.Genero = workSheet.Cells[i, 7].Value.ToString();
						pc.Direccion = workSheet.Cells[i, 8].Value.ToString();
						pc.Telefono = workSheet.Cells[i, 9].Value.ToString();
						pc.Celular = workSheet.Cells[i, 10].Value.ToString();
						pc.MailPersonal = workSheet.Cells[i, 11].Value.ToString();
						pc.MailEpn = workSheet.Cells[i, 22].Value.ToString();
						pc.EstadoCivil = null;
						pc.DependenciaDondeTrabaja = null;
						pc.Cargo = null;
						pc.Procedencia = null;
						pc.TipoPaciente = null;
						pc.Estado = true;
						pc.CodigoTipoIdentificacion = null;
						pc.CodigoFacultad = null; //buscarFacultad(workSheet.Cells[i, 13].Value.ToString());
						pc.CodigoCarrera = null;//buscarCarrrera(pc.CodigoFacultad, workSheet.Cells[i, 11].Value.ToString());
						_context.Add(pc);
						//_auditoria.GuardarLogAuditoria(fechaPacienteGuardado, j.Name, "Paciente", pc.Codigo, "I");

						//listaPacientes.Add(pc);
						maxCodigo = maxCodigo + 1;
						//}
					}

				}
				//_context.Paciente.AddRangeAsync(listaPacientes);
				_context.SaveChanges();


				DeleteFile(Documento.FileName);

				return "Save";
			}

			catch (Exception ex)
			{
				DeleteFile(Documento.FileName);

				return ex.InnerException.Message.ToString();
			}
		}

		//private string buscarCarrrera(string codigoFacultad, string nombreCarrera)
		//{
		//	try
		//	{
		//		string h = _context.Carrera.Where(s => s.Codigo == "0002").SingleOrDefault().Nombre.ToUpper();
		//		var g = h.CompareTo(nombreCarrera);

		//		string codigoCarrera = null;
		//		var carrera = _context.Carrera.Where(s =>s.CodigoFacultad == codigoFacultad && s.Nombre.ToUpper().Contains(nombreCarrera)).FirstOrDefault();
		//		if (carrera != null)
		//			codigoCarrera = carrera.Codigo;

		//		return codigoCarrera;
		//	}
		//	catch (Exception e)
		//	{
		//		return e.Message;
		//	}
		//}

		private string buscarFacultad(string nombreFacultad)
		{
			try
			{
				string h = _context.Facultad.Where(s => s.Codigo == "0008").SingleOrDefault().Nombre;
				var g = h.Contains(nombreFacultad);

				string codigoFacultad = null;
				var facultad = _context.Facultad.Where(s => s.Nombre.Contains(nombreFacultad)).FirstOrDefault();
				if (facultad != null)
					codigoFacultad = facultad.Codigo;

				return codigoFacultad;
			}
			catch (Exception e)
			{
				return e.Message;
			}
		}

		public void UploadFile(IFormFile archivo)
		{
			var path = "";
			var nombreArchivo = archivo.FileName;

			if (archivo != null)
			{
				path = @"" + pathRootDocumentos + nombreArchivo;

				using (var stream = new FileStream(path, FileMode.Create))
				{
					archivo.CopyTo(stream);
				}
			}

		}

		private void DeleteFile(String path)
		{
			var patDocumento = @"" + pathRootDocumentos + path;
			if (!String.IsNullOrEmpty(path))
				System.IO.File.Delete(patDocumento);
		}

		private Dictionary<string, string> GetMimeTypes()
		{
			return new Dictionary<string, string>
			{
				 {".xls", "application/vnd.ms-excel"},
				{".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
			};
		}

		[HttpGet]
		public async Task<List<SelectListItem>> CargarPacientes()
		{
			List<SelectListItem> list = new List<SelectListItem>();
			var Pacientes = await _context.Paciente.OrderBy(f => f.NombreCompleto).Where(p => p.Estado == true).ToListAsync();
			list.Insert(0, new SelectListItem("Seleccione...", "0"));
			foreach (Paciente item in Pacientes.ToList())
				list.Add(new SelectListItem(item.NombreCompleto, item.Codigo));
			return list;
		}

		[HttpGet]
		public async Task<string> CargarPacienteNombre(string Identificacion)
		{
			if (!String.IsNullOrEmpty(Identificacion))
			{
				var paciente = await _context.Paciente.Where(p => p.Estado == true && p.Identificacion == Identificacion).SingleOrDefaultAsync();
				if (paciente == null)
				{
					return "Paciente No Registrado o Inactivo";
				}
				else
				{
					return paciente.NombreCompleto;
				}
			}

			return "";

		}


		//importar datos

		public IActionResult Importar()
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				//Permisos de usuario
				var permisos = i.Claims.Where(c => c.Type == "Pacientes").Select(c => c.Value).SingleOrDefault().Split(";");
				if (Convert.ToBoolean(permisos[5]))
				{


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

	}
}
