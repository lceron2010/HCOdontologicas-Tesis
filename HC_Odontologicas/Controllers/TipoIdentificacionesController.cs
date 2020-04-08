using HC_Odontologicas.FuncionesGenerales;
using HC_Odontologicas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HC_Odontologicas.Controllers
{
	public class TipoIdentificacionesController : Controller
	{
		private readonly HCOdontologicasContext _context;
		private ValidacionesController validaciones;
		private readonly AuditoriaController _auditoria;
		public TipoIdentificacionesController(HCOdontologicasContext context)
		{
			_context = context;
			validaciones = new ValidacionesController(_context);
			_auditoria = new AuditoriaController(context);
		}

		// GET: TipoIdentificaciones
		public async Task<IActionResult> Index(string search, string Filter, string sortOrder, int? page)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				//Permisos de usuario
				var permisos = i.Claims.Where(c => c.Type == "TipoIdentificaciones").Select(c => c.Value).SingleOrDefault().Split(";");
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

					var tipoIdentificacion = from c in _context.TipoIdentificacion select c;
					if (!String.IsNullOrEmpty(search))
						tipoIdentificacion = tipoIdentificacion.Where(s => s.Nombre.Contains(search));

					switch (sortOrder)
					{
						case "nombre_desc":
							tipoIdentificacion = tipoIdentificacion.OrderByDescending(s => s.Nombre);
							break;
						default:
							tipoIdentificacion = tipoIdentificacion.OrderBy(s => s.Nombre);
							break;

					}
					int pageSize = 10;
					return View(await Paginacion<TipoIdentificacion>.CreateAsync(tipoIdentificacion, page ?? 1, pageSize));

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
		// GET: TipoIdentificaciones/Create
		public IActionResult Create()
		{

			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "TipoIdentificaciones").Select(c => c.Value).SingleOrDefault().Split(";");

				if (Convert.ToBoolean(permisos[1]))
				{

					return View();
				}
				else
					return Redirect("../TipoIdentificaciones");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// POST: TipoIdentificaciones/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<IActionResult> Create(TipoIdentificacion tipoIdentificacion)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				try
				{
					if (ModelState.IsValid)
					{
						Int64 maxCodigo = 0;
						maxCodigo = Convert.ToInt64(_context.TipoIdentificacion.Max(f => f.Codigo));
						maxCodigo += 1;
						tipoIdentificacion.Codigo = maxCodigo.ToString("D4");
						_context.Add(tipoIdentificacion);
						await _context.SaveChangesAsync();
						await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "TipoIdentificacion", tipoIdentificacion.Codigo, "I");
						ViewBag.Message = "Save";
						return View(tipoIdentificacion);
					}
					return View(tipoIdentificacion);

				}
				catch (Exception e)
				{
					string mensaje = e.Message;
					if (e.InnerException != null)
						mensaje = MensajesError.UniqueKey(e.InnerException.Message);

					ViewBag.Message = mensaje;
					return View(tipoIdentificacion);
				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}

		}

		// GET: TipoIdentificaciones/Edit/5
		public async Task<IActionResult> Edit(string codigo)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "TipoIdentificaciones").Select(c => c.Value).SingleOrDefault().Split(";");
				codigo = Encriptacion.Decrypt(codigo);
				if (Convert.ToBoolean(permisos[2]))
				{
					if (codigo == null)
						return NotFound();

					var tipoIdentificacion = await _context.TipoIdentificacion.SingleOrDefaultAsync(f => f.Codigo == codigo);

					if (tipoIdentificacion == null)
						return NotFound();

					return View(tipoIdentificacion);
				}
				else
					return Redirect("../TipoIdentificaciones");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}

		}

		// POST: TipoIdentificaciones/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<IActionResult> Edit(TipoIdentificacion tipoIdentificacion)
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
							tipoIdentificacion.Codigo = Encriptacion.Decrypt(tipoIdentificacion.Codigo);
							_context.Update(tipoIdentificacion);
							await _context.SaveChangesAsync();
							await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "TipoIdentificacion", tipoIdentificacion.Codigo, "U");
							ViewBag.Message = "Save";

							return View(tipoIdentificacion);
						}
						catch (DbUpdateConcurrencyException)
						{
							throw;
						}
					}

					return View(tipoIdentificacion);
				}
				catch (Exception e)
				{
					string mensaje = e.Message;
					if (e.InnerException != null)
						mensaje = MensajesError.UniqueKey(e.InnerException.Message);

					ViewBag.Message = mensaje;

					return View(tipoIdentificacion);
				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// POST: TipoIdentificaciones/Delete/5
		[HttpPost]
		public async Task<string> DeleteConfirmed(string codigo)
		{
			try
			{
				var i = (ClaimsIdentity)User.Identity;
				var tipoIdentificacion = await _context.TipoIdentificacion.SingleOrDefaultAsync(f => f.Codigo == codigo);
				_context.TipoIdentificacion.Remove(tipoIdentificacion);
				await _context.SaveChangesAsync();
				await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "tipoIdentificacion", tipoIdentificacion.Codigo, "D");
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
