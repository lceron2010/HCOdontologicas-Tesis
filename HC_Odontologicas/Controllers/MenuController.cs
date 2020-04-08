using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HC_Odontologicas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HC_Odontologicas.Controllers
{
    public class MenuController : Controller
    {
		private readonly HCOdontologicasContext _context;

		public MenuController(HCOdontologicasContext context)
		{
			_context = context;
		}

		// GET: Menus
		public async Task<IActionResult> Index()
		{
			return View(await _context.Menu.ToListAsync());
		}

		// GET: Menus
		public List<Menu> _Menu()
		{
			var i = (ClaimsIdentity)User.Identity;
			//var CodigoCompania = i.Claims.Where(c => c.Type == "CodigoCompania").Select(c => c.Value).SingleOrDefault();
			var CodigoPerfil = i.Claims.Where(c => c.Type == "CodigoPerfil").Select(c => c.Value).SingleOrDefault();
			try
			{
				List<Menu> menus = _context.Menu.Include(m => m.SubMenu).Where(m => m.CodigoMenu == null && m.Visible == true).ToList();
				List<PerfilDetalle> perfilDetalles = _context.PerfilDetalle
					.Where(m =>  m.CodigoPerfil == CodigoPerfil && m.Ver == true)
					.ToList();
				List<Menu> _nuevoMenu = new List<Menu>();

				foreach (Menu menu in menus.Where(m => m.CodigoMenu == null && m.Visible == true).OrderBy(m => m.Orden).ToList())
				{
					// if (menu.PerfilDetalle.Count > 0)
					// {
					Menu mn = new Menu();
					mn.Codigo = menu.Codigo;
					mn.Nombre = menu.Nombre;
					mn.Accion = menu.Accion;
					mn.Icono = menu.Icono;
					mn.Orden = menu.Orden;
					mn.Visible = menu.Visible;
					mn.Ver = menu.Ver;
					foreach (Menu m in menu.SubMenu.OrderBy(m => m.Orden))
					{
						var perf = perfilDetalles.Where(p => p.CodigoMenu == m.Codigo).ToList();
						if (perf.Count > 0 && m.Visible == true)
						{
							Menu subMenu = new Menu();
							subMenu.Codigo = m.Codigo;
							subMenu.Nombre = m.Nombre;
							subMenu.Accion = m.Accion;
							subMenu.Icono = m.Icono;
							mn.SubMenu.Add(subMenu);
						}

					}
					if (mn.SubMenu.Count > 0)
						_nuevoMenu.Add(mn);
					//}
				}

				return _nuevoMenu;
			}
			catch
			{
				return null;
			}
		}
	}
}