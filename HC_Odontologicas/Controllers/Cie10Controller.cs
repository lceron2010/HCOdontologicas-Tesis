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
	public class Cie10Controller : Controller
	{
		private readonly HCOdontologicasContext _context;
		private ValidacionesController validaciones;
		private readonly AuditoriaController _auditoria;
		public Cie10Controller(HCOdontologicasContext context)
		{
			_context = context;
			validaciones = new ValidacionesController(_context);
			_auditoria = new AuditoriaController(context);
		}

		// GET: Cie10
		public async Task<IActionResult> Index(string search, string Filter, string sortOrder, int? page)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				//Permisos de usuario
				var permisos = i.Claims.Where(c => c.Type == "Cie10").Select(c => c.Value).SingleOrDefault().Split(";");
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
					ViewData["CurrentSort"] = sortOrder;

					var cie10 = from c in _context.Cie10 select c;
					if (!String.IsNullOrEmpty(search))
						cie10 = cie10.Where(s => s.Nombre.Contains(search) || s.CodigoInterno.Contains(search));

					switch (sortOrder)
					{
						case "nombre_desc":
							cie10 = cie10.OrderByDescending(s => s.Nombre);
							break;
						default:
							cie10 = cie10.OrderBy(s => s.CodigoInterno);
							break;

					}
					//int pageSize = 10;
					// return View(await Paginacion<Anamnesis>.CreateAsync(cie10, page ?? 1, pageSize));
					return View(cie10);
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

		// GET: Cie10/Create
		public IActionResult Create()
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "Cie10").Select(c => c.Value).SingleOrDefault().Split(";");

				if (Convert.ToBoolean(permisos[1]))
				{

					return View();
				}
				else
					return Redirect("../Cie10");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// POST: Cie10/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<IActionResult> Create(Cie10 cie10)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				try
				{
					if (ModelState.IsValid)
					{
						Int64 maxCodigo = 0;
						maxCodigo = Convert.ToInt64(_context.Cie10.Max(f => f.Codigo));
						maxCodigo += 1;
						cie10.Codigo = maxCodigo.ToString("D8");
						_context.Add(cie10);
						await _context.SaveChangesAsync();
						await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "CIE10", cie10.Codigo, "I");
						ViewBag.Message = "Save";
						return View(cie10);
					}
					return View(cie10);
					
				}
				catch (Exception e)
				{
					string mensaje = e.Message;
					if (e.InnerException != null)
						mensaje = MensajesError.UniqueKey(e.InnerException.Message);

					ViewBag.Message = mensaje;

					return View(cie10);
				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}

		}   
		// GET: Cie10/Edit/5
		public async Task<IActionResult> Edit(String codigo)
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

					var cie10 = await _context.Cie10.SingleOrDefaultAsync(f => f.Codigo == codigo);

					if (cie10 == null)
						return NotFound();

					return View(cie10);
				}
				else
					return Redirect("../Cie10");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}
		// POST: Cie10/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]		
		public async Task<IActionResult> Edit(Cie10 cie10)
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
							cie10.Codigo = Encriptacion.Decrypt(cie10.Codigo);							
							_context.Update(cie10);
							await _context.SaveChangesAsync();
							await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "CIE10", cie10.Codigo, "U");
							ViewBag.Message = "Save";					

							return View(cie10);
						}
						catch (DbUpdateConcurrencyException)
						{
							throw;
						}
					}					

					return View(cie10);
				}
				catch (Exception e)
				{
					string mensaje = e.Message;
					if (e.InnerException != null)
						mensaje = MensajesError.UniqueKey(e.InnerException.Message);

					ViewBag.Message = mensaje;				

					return View(cie10);
				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}
		// POST: Cie10/Delete/5
		[HttpPost]		
		public async Task<String> DeleteConfirmed(string codigo)
		{
			try
			{
				var i = (ClaimsIdentity)User.Identity;
				var cie10 = await _context.Cie10.SingleOrDefaultAsync(f => f.Codigo == codigo);
				_context.Cie10.Remove(cie10);
				await _context.SaveChangesAsync();
				await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "CIE10", cie10.Codigo, "D");
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
