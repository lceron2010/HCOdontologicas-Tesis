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
    public class PerfilesController : Controller
    {
		private readonly HCOdontologicasContext _context;
		private ValidacionesController validaciones;
		private readonly AuditoriaController _auditoria;		

		public PerfilesController(HCOdontologicasContext context)
        {
			_context = context;
			validaciones = new ValidacionesController(_context);
			_auditoria = new AuditoriaController(context);
		}

        // GET: Perfiles
        public async Task<IActionResult> Index(string sortOrder, string Filter, int? page, string search)
        {

			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				//Permisos de usuario
				var permisos = i.Claims.Where(c => c.Type == "Perfil").Select(c => c.Value).SingleOrDefault().Split(";");
				ViewData["Crear"] = Convert.ToBoolean(permisos[1]);
				ViewData["Editar"] = Convert.ToBoolean(permisos[2]);
				ViewData["Eliminar"] = Convert.ToBoolean(permisos[3]);
				ViewData["Exportar"] = Convert.ToBoolean(permisos[4]);

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
					var perfil = from p in _context.Perfil select p;

					if (!string.IsNullOrEmpty(search))
						perfil = perfil.Where(s => s.Nombre.Contains(search));

					switch (sortOrder)
					{
						case "nombre_desc":
							perfil = perfil.OrderByDescending(s => s.Nombre);
							break;
						default:
							perfil = perfil.OrderBy(s => s.Nombre);
							break;
					}
					int pageSize = 10;
					return View(await Paginacion<Perfil>.CreateAsync(perfil, page ?? 1, pageSize)); // page devuelve valor si lo tiene caso contrario devuelve 1   
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

        // GET: Perfiles/Create
        public IActionResult Create()
        {
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "Perfil").Select(c => c.Value).SingleOrDefault().Split(";");
				
				if (Convert.ToBoolean(permisos[1]))
				{
					List<Menu> menus = _context.Menu.Include(m => m.SubMenu).Where(m => m.CodigoMenu == null).ToList();
					Perfil perfil = new Perfil
					{
						PerfilDetalle = new List<PerfilDetalle>()
					};
					foreach (Menu menu in menus.Where(m => m.CodigoMenu == null))
					{
						PerfilDetalle detalle = new PerfilDetalle
						{
							//CodigoCompania = CodigoCompania,
							CodigoMenu = menu.Codigo,
							Menu = new Menu()
						};
						detalle.Menu.Codigo = menu.Codigo;
						detalle.Menu.CodigoMenu = menu.CodigoMenu;
						detalle.Menu.Nombre = menu.Nombre;
						detalle.Menu.Visible = menu.Visible;
						detalle.Menu.Ver = menu.Ver;
						detalle.Menu.Crear = menu.Crear;
						detalle.Menu.Editar = menu.Editar;
						detalle.Menu.Eliminar = menu.Eliminar;
						detalle.Menu.Exportar = menu.Exportar;
						detalle.Menu.Importar = menu.Importar;
						perfil.PerfilDetalle.Add(detalle);
						foreach (Menu subMenu in menu.SubMenu.OrderBy(s => s.Orden))
						{
							PerfilDetalle detalle1 = new PerfilDetalle
							{
								//CodigoCompania = CodigoCompania,
								CodigoMenu = subMenu.Codigo,
								Menu = new Menu()
							};
							detalle1.Menu.Codigo = subMenu.Codigo;
							detalle1.Menu.CodigoMenu = subMenu.CodigoMenu;
							detalle1.Menu.Nombre = subMenu.Nombre;
							detalle1.Menu.Visible = subMenu.Visible;
							detalle1.Menu.Ver = subMenu.Ver;
							detalle1.Menu.Crear = subMenu.Crear;
							detalle1.Menu.Editar = subMenu.Editar;
							detalle1.Menu.Eliminar = subMenu.Eliminar;
							detalle1.Menu.Exportar = subMenu.Exportar;
							detalle1.Menu.Importar = subMenu.Importar;
							perfil.PerfilDetalle.Add(detalle1);
						}
					}
					return View(perfil);
				}
				else
					return Redirect("../Perfiles");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

        // POST: Perfiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]        
        public async Task<IActionResult> Create(Perfil perfil)
        {
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{				
				try
				{
					if (ModelState.IsValid)
					{
						Int64 maxCodigo = 0;
						maxCodigo = Convert.ToInt64(_context.Perfil.Max(f => f.Codigo));
						maxCodigo += 1;
						perfil.Codigo = maxCodigo.ToString("D4");
						//perfil.CodigoCompania = CodigoCompania;
						perfil.FechaCreacion = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
						perfil.UsuarioCreacion = i.Name;
						foreach (PerfilDetalle perfilDetalle in perfil.PerfilDetalle)
						{
							//perfilDetalle.CodigoCompania = CodigoCompania;
							perfilDetalle.CodigoPerfil = perfil.Codigo;
							perfilDetalle.Menu = null;
						}
						_context.Add(perfil);
						await _context.SaveChangesAsync();
						await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), perfil.UsuarioCreacion, "Perfil", perfil.Codigo, "I");
						foreach (PerfilDetalle perfilDetalle in perfil.PerfilDetalle)
						{
							perfilDetalle.Menu = _context.Menu.Single(m => m.Codigo == perfilDetalle.CodigoMenu);
						}
						ViewBag.Message = "Save";
						return View(perfil);
					}
					return View(perfil);
				}
				catch (Exception e)
				{
					string mensaje = e.Message;
					if (e.InnerException != null)
						mensaje = MensajesError.UniqueKey(e.InnerException.Message);

					ViewBag.Message = mensaje;
					foreach (PerfilDetalle perfilDetalle in perfil.PerfilDetalle)
					{
						perfilDetalle.Menu = _context.Menu.Single(m => m.Codigo == perfilDetalle.CodigoMenu);
					}
					return View(perfil);
				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

        // GET: Perfiles/Edit/5
        public async Task<IActionResult> Edit(string codigo)
        {

			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "Perfil").Select(c => c.Value).SingleOrDefault().Split(";");
				var CodigoCompania = i.Claims.Where(c => c.Type == "CodigoCompania").Select(c => c.Value).SingleOrDefault();
				codigo = Encriptacion.Decrypt(codigo);
				if (Convert.ToBoolean(permisos[2]))
				{
					if (CodigoCompania == null && codigo == null)
					{
						return NotFound();
					}

					var perfil = await _context.Perfil.Include(f => f.PerfilDetalle).SingleOrDefaultAsync(f => f.Codigo == codigo);
					if (perfil == null)
					{
						return NotFound();
					}

					List<Menu> menus = _context.Menu.Include(m => m.SubMenu).Where(m => m.CodigoMenu == null).ToList();

					Perfil p = new Perfil
					{
						
						Codigo = perfil.Codigo,
						Nombre = perfil.Nombre,						
						FechaCreacion = perfil.FechaCreacion,
						UsuarioCreacion = perfil.UsuarioCreacion,
						PerfilDetalle = new List<PerfilDetalle>()
					};
					foreach (Menu menu in menus.Where(m => m.CodigoMenu == null))
					{
						var a = perfil.PerfilDetalle.Single(f => f.CodigoMenu == menu.Codigo);
						PerfilDetalle detalle = new PerfilDetalle
						{
							
							CodigoPerfil = a.CodigoPerfil,
							CodigoMenu = a.CodigoMenu,
							Menu = new Menu()
						};
						detalle.Menu.Codigo = a.Menu.Codigo;
						detalle.Menu.CodigoMenu = menu.CodigoMenu;
						detalle.Menu.Nombre = menu.Nombre;
						detalle.Menu.Visible = menu.Visible;
						detalle.Menu.Ver = menu.Ver;
						detalle.Menu.Crear = menu.Crear;
						detalle.Menu.Editar = menu.Editar;
						detalle.Menu.Eliminar = menu.Eliminar;
						detalle.Menu.Exportar = menu.Exportar;
						detalle.Menu.Importar = menu.Importar;
						p.PerfilDetalle.Add(detalle);
						foreach (Menu subMenu in menu.SubMenu.OrderBy(s => s.Orden))
						{
							var b = perfil.PerfilDetalle.Single(f => f.CodigoMenu == subMenu.Codigo);
							PerfilDetalle detalle1 = new PerfilDetalle
							{
								
								CodigoPerfil = b.CodigoPerfil,
								CodigoMenu = b.CodigoMenu,
								Ver = b.Ver,
								Crear = b.Crear,
								Editar = b.Editar,
								Eliminar = b.Eliminar,
								Exportar = b.Exportar,
								Importar = b.Importar,
								Menu = new Menu
								{
									Codigo = subMenu.Codigo,
									CodigoMenu = subMenu.CodigoMenu,
									Nombre = subMenu.Nombre,
									Visible = subMenu.Visible,
									Ver = subMenu.Ver,
									Crear = subMenu.Crear,
									Editar = subMenu.Editar,
									Eliminar = subMenu.Eliminar,
									Exportar = subMenu.Exportar,
									Importar = subMenu.Importar
								}
							};
							p.PerfilDetalle.Add(detalle1);
						}
					}

					return View(p);
				}
				else
					return Redirect("../Perfiles");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

        // POST: Perfiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(Perfil perfil)
        {
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "Perfil").Select(c => c.Value).SingleOrDefault().Split(",");
				try
				{
					if (perfil.Codigo == null)
					{
						return NotFound();
					}

					if (ModelState.IsValid)
					{
						try
						{
							perfil.Codigo = Encriptacion.Decrypt(perfil.Codigo);							
							var PerfilAntiguo = _context.Perfil.Include(p => p.PerfilDetalle).SingleOrDefault(p => p.Codigo == perfil.Codigo);
							PerfilAntiguo.Nombre = perfil.Nombre;
							PerfilAntiguo.FechaModificacion = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
							PerfilAntiguo.UsuarioModificacion = i.Name;
							foreach (PerfilDetalle perfilDetalle in perfil.PerfilDetalle)
							{
								perfilDetalle.CodigoPerfil = perfil.Codigo;
								perfilDetalle.Menu = null;
							}
							PerfilAntiguo.PerfilDetalle = perfil.PerfilDetalle;
							_context.Update(PerfilAntiguo);
							await _context.SaveChangesAsync();
							await _auditoria.GuardarLogAuditoria(Convert.ToDateTime(PerfilAntiguo.FechaModificacion), PerfilAntiguo.UsuarioModificacion, "Perfil", perfil.Codigo, "U");
							ViewBag.Message = "Save";
							List<Menu> men = _context.Menu.ToList();
							return View(perfil);
						}
						catch (DbUpdateConcurrencyException)
						{
							throw;
						}
					}
					List<Menu> menus = _context.Menu.ToList();
					return View(perfil);
				}
				catch (Exception e)
				{
					string mensaje = e.Message;
					if (e.InnerException != null)
						mensaje = MensajesError.UniqueKey(e.InnerException.Message);

					ViewBag.Message = mensaje;
					List<Menu> menus = _context.Menu.ToList();
					return View(perfil);
				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

        // POST: Perfiles/Delete/5
        [HttpPost]
        
        public async Task<String> DeleteConfirmed(string codigo)
        {
			try
			{
				var i = (ClaimsIdentity)User.Identity;
				var perfil = await _context.Perfil.Include(pd => pd.PerfilDetalle).SingleOrDefaultAsync(f => f.Codigo == codigo);
				_context.Remove(perfil);
				await _context.SaveChangesAsync();
				await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "Perfil", perfil.Codigo, "D");
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
