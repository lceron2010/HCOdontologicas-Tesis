using HC_Odontologicas.FuncionesGenerales;
using HC_Odontologicas.Models;
using Microsoft.AspNetCore.Identity;
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
	public class UsuariosController : Controller
	{
		private readonly HCOdontologicasContext _context;
		private ValidacionesController validaciones;
		private readonly AuditoriaController _auditoria;
		SelectListItem vacio = new SelectListItem(value: "0", text: "Seleccione...");
		private readonly UserManager<UsuarioLogin> _userManager;

		public UsuariosController(HCOdontologicasContext context, UserManager<UsuarioLogin> userManager)
		{
			_context = context;
			validaciones = new ValidacionesController(_context);
			_auditoria = new AuditoriaController(context);
			_userManager = userManager;
		}

		// GET: Usuario
		public async Task<IActionResult> Index(string search, string Filter, string sortOrder, int? page)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				//Permisos de usuario
				var permisos = i.Claims.Where(c => c.Type == "Usuarios").Select(c => c.Value).SingleOrDefault().Split(";");
				ViewData["Crear"] = Convert.ToBoolean(permisos[1]);
				ViewData["Editar"] = Convert.ToBoolean(permisos[2]);
				ViewData["Eliminar"] = Convert.ToBoolean(permisos[3]);
				ViewData["Exportar"] = Convert.ToBoolean(permisos[4]);

				if (Convert.ToBoolean(permisos[0]))
				{
					ViewData["NombreSortParam"] = String.IsNullOrEmpty(sortOrder) ? "nombre_desc" : "";
					ViewData["PerfilSortParam"] = sortOrder == "per_asc" ? "per_desc" : "per_asc";


					//permite mantener la busqueda introducida en el filtro de busqueda
					if (search != null)
						page = 1;
					else
						search = Filter;

					ViewData["Filter"] = search;
					ViewData["CurrentSort"] = sortOrder;

					var usuario = from c in _context.Usuario.Include(u => u.Perfil).Include(u => u.Personal) select c;
					if (!String.IsNullOrEmpty(search))
						usuario = usuario.Where(s => s.CorreoElectronico.Contains(search) || s.CorreoElectronico.Contains(search));

					switch (sortOrder)
					{
						case "nombre_desc":
							usuario = usuario.OrderByDescending(s => s.CorreoElectronico);
							break;
						case "per_asc":
							usuario = usuario.OrderBy(s => s.Perfil.Nombre);
							break;
						case "per_desc":
							usuario = usuario.OrderByDescending(s => s.Perfil.Nombre);
							break;
						default:
							usuario = usuario.OrderBy(s => s.CorreoElectronico);
							break;

					}
					int pageSize = 10;
					return View(await Paginacion<Usuario>.CreateAsync(usuario, page ?? 1, pageSize));

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

		// GET: Usuario/Create
		public IActionResult Create()
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "Usuarios").Select(c => c.Value).SingleOrDefault().Split(";");

				if (Convert.ToBoolean(permisos[1]))
				{
					List<SelectListItem> Perfil = new SelectList(_context.Perfil.OrderBy(f => f.Nombre), "Codigo", "Nombre").ToList();
					Perfil.Insert(0, vacio);
					ViewData["CodigoPerfil"] = Perfil;

					List<SelectListItem> Personal = new SelectList(_context.Personal.OrderBy(f => f.NombreCompleto), "Codigo", "NombreCompleto").ToList();
					Personal.Insert(0, vacio);
					ViewData["CodigoPersonal"] = Personal;

					return View();
				}
				else
					return Redirect("../Usuarios");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// POST: Usuario/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<IActionResult> Create(Usuario usuario)
		{
			var i = (ClaimsIdentity)User.Identity;
			List<SelectListItem> Perfil = new SelectList(_context.Perfil.OrderBy(f => f.Nombre), "Codigo", "Nombre").ToList();
			List<SelectListItem> Personal = new SelectList(_context.Personal.OrderBy(f => f.NombreCompleto), "Codigo", "NombreCompleto").ToList();
			if (i.IsAuthenticated)
			{
				try
				{
					if (ModelState.IsValid)
					{
						Int64 maxCodigo = 0;
						maxCodigo = Convert.ToInt64(_context.Usuario.Max(f => f.Codigo));
						maxCodigo += 1;
						usuario.Codigo = maxCodigo.ToString("D4");

						usuario.SecurityStamp = Guid.NewGuid().ToString();
						var us = new UsuarioLogin { UserName = usuario.CorreoElectronico };
						usuario.PasswordHash = _userManager.PasswordHasher.HashPassword(us, usuario.Contrasenia);

						if (usuario.CodigoPersonal == "0")
						{
							usuario.CodigoPersonal = null;
						}

						_context.Add(usuario);
						await _context.SaveChangesAsync();
						await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "Usuario", usuario.Codigo, "I");

						Perfil.Insert(0, vacio);
						ViewData["CodigoPerfil"] = Perfil;

						Personal.Insert(0, vacio);
						ViewData["CodigoPersonal"] = Personal;

						ViewBag.Message = "Save";
						return View(usuario);
					}
					return View(usuario);

				}
				catch (Exception e)
				{
					string mensaje = e.Message;
					if (e.InnerException != null)
						mensaje = MensajesError.UniqueKey(e.InnerException.Message);

					Perfil.Insert(0, vacio);
					ViewData["CodigoPerfil"] = Perfil;

					Personal.Insert(0, vacio);
					ViewData["CodigoPersonal"] = Personal;

					ViewBag.Message = mensaje;


					return View(usuario);
				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}

		}
		// GET: Usuario/Edit/5
		public async Task<IActionResult> Edit(String codigo)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "Usuarios").Select(c => c.Value).SingleOrDefault().Split(";");
				codigo = Encriptacion.Decrypt(codigo);
				if (Convert.ToBoolean(permisos[2]))
				{
					if (codigo == null)
						return NotFound();

					var usuario = await _context.Usuario.SingleOrDefaultAsync(f => f.Codigo == codigo);

					if (usuario == null)
						return NotFound();

					List<SelectListItem> Perfil = new SelectList(_context.Perfil.OrderBy(f => f.Nombre), "Codigo", "Nombre", usuario.CodigoPerfil).ToList();
					Perfil.Insert(0, vacio);
					ViewData["CodigoPerfil"] = Perfil;

					List<SelectListItem> Personal = new SelectList(_context.Personal.OrderBy(f => f.NombreCompleto), "Codigo", "NombreCompleto", usuario.CodigoPersonal).ToList();
					Personal.Insert(0, vacio);
					ViewData["CodigoPersonal"] = Personal;

					return View(usuario);
				}
				else
					return Redirect("../Usuarios");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}
		// POST: Usuario/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<IActionResult> Edit(Usuario usuario)
		{
			var i = (ClaimsIdentity)User.Identity;
			List<SelectListItem> Perfil = new SelectList(_context.Perfil.OrderBy(f => f.Nombre), "Codigo", "Nombre").ToList();
			List<SelectListItem> Personal = new SelectList(_context.Personal.OrderBy(f => f.NombreCompleto), "Codigo", "NombreCompleto").ToList();
			if (i.IsAuthenticated)
			{
				try
				{

					if (ModelState.IsValid)
					{
						try
						{
							usuario.Codigo = Encriptacion.Decrypt(usuario.Codigo);

							usuario.SecurityStamp = Guid.NewGuid().ToString();
							var us = new UsuarioLogin { UserName = usuario.CorreoElectronico };
							usuario.PasswordHash = _userManager.PasswordHasher.HashPassword(us, usuario.Contrasenia);


							if (usuario.CodigoPersonal == "0")
							{
								usuario.CodigoPersonal = null;
							}

							_context.Update(usuario);
							await _context.SaveChangesAsync();
							await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "Usuario", usuario.Codigo, "U");
							ViewBag.Message = "Save";

							Perfil.Insert(0, vacio);
							ViewData["CodigoPerfil"] = Perfil;

							Personal.Insert(0, vacio);
							ViewData["CodigoPersonal"] = Personal;

							return View(usuario);
						}
						catch (DbUpdateConcurrencyException)
						{
							throw;
						}
					}

					Perfil.Insert(0, vacio);
					ViewData["CodigoPerfil"] = Perfil;

					Personal.Insert(0, vacio);
					ViewData["CodigoPersonal"] = Personal;

					return View(usuario);
				}
				catch (Exception e)
				{
					string mensaje = e.Message;
					if (e.InnerException != null)
						mensaje = MensajesError.UniqueKey(e.InnerException.Message);

					Perfil.Insert(0, vacio);
					ViewData["CodigoPerfil"] = Perfil;

					Personal.Insert(0, vacio);
					ViewData["CodigoPersonal"] = Personal;

					ViewBag.Message = mensaje;

					return View(usuario);
				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}
		// POST: usuario/Delete/5
		[HttpPost]
		public async Task<String> DeleteConfirmed(string codigo)
		{
			try
			{
				var i = (ClaimsIdentity)User.Identity;
				var usuario = await _context.Usuario.SingleOrDefaultAsync(f => f.Codigo == codigo);
				_context.Usuario.Remove(usuario);
				await _context.SaveChangesAsync();
				await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "Usuario", usuario.Codigo, "D");
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
