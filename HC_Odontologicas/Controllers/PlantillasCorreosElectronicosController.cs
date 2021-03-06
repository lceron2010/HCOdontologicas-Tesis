﻿using HC_Odontologicas.FuncionesGenerales;
using HC_Odontologicas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HC_Odontologicas.Controllers
{
	public class PlantillasCorreosElectronicosController : Controller
	{
		private readonly HCOdontologicasContext _context;
		private ValidacionesController validaciones;
		private readonly AuditoriaController _auditoria;
		public PlantillasCorreosElectronicosController(HCOdontologicasContext context)
		{
			_context = context;
			validaciones = new ValidacionesController(_context);
			_auditoria = new AuditoriaController(context);
		}

		// GET: PlantillasCorreosElectronicos
		public async Task<IActionResult> Index(string search, string Filter, string sortOrder, int? page)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				//Permisos de usuario
				var permisos = i.Claims.Where(c => c.Type == "PlantillasCorreosElectronicos").Select(c => c.Value).SingleOrDefault().Split(";");
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

					var plantillaCorreo = from c in _context.PlantillaCorreoElectronico select c;
					if (!String.IsNullOrEmpty(search))
						plantillaCorreo = plantillaCorreo.Where(s => s.Asunto.Contains(search));

					switch (sortOrder)
					{
						case "nombre_desc":
							plantillaCorreo = plantillaCorreo.OrderByDescending(s => s.Asunto);
							break;
						default:
							plantillaCorreo = plantillaCorreo.OrderBy(s => s.Asunto);
							break;

					}
					int pageSize = 10;
					return View(await Paginacion<PlantillaCorreoElectronico>.CreateAsync(plantillaCorreo, page ?? 1, pageSize));

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

		// GET: PlantillasCorreosElectronicos/Create
		public IActionResult Create()
		{

			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "PlantillasCorreosElectronicos").Select(c => c.Value).SingleOrDefault().Split(";");

				if (Convert.ToBoolean(permisos[1]))
				{

					return View();
				}
				else
					return Redirect("../PlantillasCorreosElectronicos");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// POST: PlantillasCorreosElectronicos/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<IActionResult> Create(PlantillaCorreoElectronico plantillaCorreoElectronico)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				try
				{
					if (ModelState.IsValid)
					{
						Int64 maxCodigo = 0;
						maxCodigo = Convert.ToInt64(_context.PlantillaCorreoElectronico.Max(f => f.Codigo));
						maxCodigo += 1;
						plantillaCorreoElectronico.Codigo = maxCodigo.ToString("D4");
						_context.Add(plantillaCorreoElectronico);
						await _context.SaveChangesAsync();
						string clave = plantillaCorreoElectronico.Asunto;
						if (clave.Length > 54)
						{
							clave.Substring(0, 53);
						}

						await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "PlantillaCorreoElectronico", clave, "I");
						ViewBag.Message = "Save";
						return View(plantillaCorreoElectronico);
					}
					return View(plantillaCorreoElectronico);

				}
				catch (Exception e)
				{
					string mensaje = e.Message;
					if (e.InnerException != null)
						mensaje = MensajesError.UniqueKey(e.InnerException.Message);

					ViewBag.Message = mensaje;
					return View(plantillaCorreoElectronico);
				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}

		}

		// GET: PlantillasCorreosElectronicos/Edit/5
		public async Task<IActionResult> Edit(string codigo)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "Cie10").Select(c => c.Value).SingleOrDefault().Split(";");
				codigo = Encriptacion.Decrypt(codigo);
				if (Convert.ToBoolean(permisos[2]))
				{
					if (codigo == null)
						return NotFound();

					var plantillaCorreoElectronico = await _context.PlantillaCorreoElectronico.SingleOrDefaultAsync(f => f.Codigo == codigo);

					if (plantillaCorreoElectronico == null)
						return NotFound();

					return View(plantillaCorreoElectronico);
				}
				else
					return Redirect("../PlantillasCorreosElectronicos");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// POST: PlantillasCorreosElectronicos/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<IActionResult> Edit(PlantillaCorreoElectronico plantillaCorreoElectronico)
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
							plantillaCorreoElectronico.Codigo = Encriptacion.Decrypt(plantillaCorreoElectronico.Codigo);
							_context.Update(plantillaCorreoElectronico);
							await _context.SaveChangesAsync();
							await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "PlantillaCorreoElectronico", plantillaCorreoElectronico.Codigo, "U");
							ViewBag.Message = "Save";

							return View(plantillaCorreoElectronico);
						}
						catch (DbUpdateConcurrencyException)
						{
							throw;
						}
					}

					return View(plantillaCorreoElectronico);
				}
				catch (Exception e)
				{
					string mensaje = e.Message;
					if (e.InnerException != null)
						mensaje = MensajesError.UniqueKey(e.InnerException.Message);

					ViewBag.Message = mensaje;

					return View(plantillaCorreoElectronico);
				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}



		// POST: PlantillasCorreosElectronicos/Delete/5
		[HttpPost]
		public async Task<string> DeleteConfirmed(string codigo)
		{
			try
			{
				var i = (ClaimsIdentity)User.Identity;
				var plantillaCorreoElectronico = await _context.PlantillaCorreoElectronico.SingleOrDefaultAsync(f => f.Codigo == codigo);
				_context.PlantillaCorreoElectronico.Remove(plantillaCorreoElectronico);
				await _context.SaveChangesAsync();
				await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "PlantillaCorreoElectronico", plantillaCorreoElectronico.Codigo.ToString(), "D");
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
