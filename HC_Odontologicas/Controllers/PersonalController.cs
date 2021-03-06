﻿using HC_Odontologicas.FuncionesGenerales;
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
	public class PersonalController : Controller
	{
		private readonly HCOdontologicasContext _context;
		private ValidacionesController validaciones;
		private readonly AuditoriaController _auditoria;
		SelectListItem vacio = new SelectListItem(value: "", text: "Seleccione...");
		public PersonalController(HCOdontologicasContext context)
		{
			_context = context;
			validaciones = new ValidacionesController(_context);
			_auditoria = new AuditoriaController(context);
		}

		// GET: Personals
		public async Task<IActionResult> Index(string sortOrder, string Filter, int? page, string search)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				//Permisos de usuario
				var permisos = i.Claims.Where(c => c.Type == "Personal").Select(c => c.Value).SingleOrDefault().Split(";");
				ViewData["Crear"] = Convert.ToBoolean(permisos[1]);
				ViewData["Editar"] = Convert.ToBoolean(permisos[2]);
				ViewData["Eliminar"] = Convert.ToBoolean(permisos[3]);
				ViewData["Exportar"] = Convert.ToBoolean(permisos[4]);

				if (Convert.ToBoolean(permisos[0]))
				{
					ViewData["NombreSortParam"] = String.IsNullOrEmpty(sortOrder) ? "nombre_desc" : "";
					ViewData["CargoSortParam"] = sortOrder == "carg_asc" ? "carg_desc" : "carg_asc";
					//permite mantener la busqueda introducida en el filtro de busqueda
					if (search != null)
						page = 1;
					else
						search = Filter;

					ViewData["Filter"] = search;
					ViewData["CurrentSort"] = sortOrder;
					var personal = from c in _context.Personal.Include(a => a.Cargo).OrderBy(p => p.NombreCompleto) select c;
					//var personal = from c in _context.Personal select c;

					if (!String.IsNullOrEmpty(search))
						personal = personal.Include(p => p.Cargo).Where(s => s.NombreCompleto.Contains(search)
						|| s.Nombres.Contains(search) || s.Apellidos.Contains(search) || s.Identificacion.Contains(search));

					switch (sortOrder)
					{
						case "nombre_desc":
							personal = personal.OrderByDescending(s => s.NombreCompleto);
							break;
						case "carg_asc":
							personal = personal.OrderBy(s => s.Cargo.Nombre);
							break;
						case "carg_desc":
							personal = personal.OrderByDescending(s => s.Cargo.Nombre);
							break;
						default:
							personal = personal.OrderBy(s => s.NombreCompleto);
							break;

					}
					int pageSize = 10;
					return View(await Paginacion<Personal>.CreateAsync(personal, page ?? 1, pageSize)); // page devuelve valor si lo tiene caso contrario devuelve 1
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

		// GET: Personals/Create
		public IActionResult Create()
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "Personal").Select(c => c.Value).SingleOrDefault().Split(";");

				if (Convert.ToBoolean(permisos[1]))
				{

					List<SelectListItem> Cargo = new SelectList(_context.Cargo.OrderBy(c => c.Nombre), "Codigo", "Nombre").ToList();
					Cargo.Insert(0, vacio);
					ViewData["CodigoCargo"] = Cargo;

					List<SelectListItem> TipoIdentificacion = new SelectList(_context.TipoIdentificacion.OrderBy(ti => ti.Nombre), "Codigo", "Nombre").ToList();
					TipoIdentificacion.Insert(0, vacio);
					ViewData["CodigoTipoIdentificacion"] = TipoIdentificacion;

					return View();
				}
				else
					return Redirect("../Personal");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// POST: Personals/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<IActionResult> Create(Personal personal)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				try
				{					

					if (personal.CodigoTipoIdentificacion == "0001")
					{
						var mensajeR = validaciones.VerifyCedula(personal.Identificacion);
						if (!string.IsNullOrEmpty(mensajeR))
							ModelState.AddModelError("Identificacion", mensajeR);
					}

					if (ModelState.IsValid)
					{
						Int64 maxCodigo = 0;
						maxCodigo = Convert.ToInt64(_context.Personal.Max(f => f.Codigo));
						maxCodigo += 1;
						personal.Codigo = maxCodigo.ToString("D8");
						_context.Add(personal);
						await _context.SaveChangesAsync();
						await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "Personal", personal.Codigo, "I");
						ViewBag.Message = "Save";
						return View(personal);

					}
					List<SelectListItem> Cargo = new SelectList(_context.Cargo.OrderBy(c => c.Nombre), "Codigo", "Nombre").ToList();
					Cargo.Insert(0, vacio);
					ViewData["CodigoCargo"] = Cargo;


					List<SelectListItem> TipoIdentificacion = new SelectList(_context.TipoIdentificacion.OrderBy(ti => ti.Nombre), "Codigo", "Nombre").ToList();
					TipoIdentificacion.Insert(0, vacio);
					ViewData["CodigoTipoIdentificacion"] = TipoIdentificacion;
					return View(personal);
				}
				catch (Exception e)
				{
					string mensaje = e.Message;
					if (e.InnerException != null)
						mensaje = MensajesError.UniqueKey(e.InnerException.Message);

					ViewBag.Message = mensaje;
					List<SelectListItem> Cargo = new SelectList(_context.Cargo.OrderBy(c => c.Nombre), "Codigo", "Nombre").ToList();
					Cargo.Insert(0, vacio);
					ViewData["CodigoCargo"] = Cargo;


					List<SelectListItem> TipoIdentificacion = new SelectList(_context.TipoIdentificacion.OrderBy(ti => ti.Nombre), "Codigo", "Nombre").ToList();
					TipoIdentificacion.Insert(0, vacio);
					ViewData["CodigoTipoIdentificacion"] = TipoIdentificacion;
					return View(personal);
				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// GET: Personals/Edit/5
		public async Task<IActionResult> Edit(string codigo)
		{
			var i = (ClaimsIdentity)User.Identity;
			if (i.IsAuthenticated)
			{
				var permisos = i.Claims.Where(c => c.Type == "Personal").Select(c => c.Value).SingleOrDefault().Split(";");
				codigo = Encriptacion.Decrypt(codigo);
				if (Convert.ToBoolean(permisos[2]))
				{
					if (codigo == null)
						return NotFound();

					var personal = await _context.Personal.SingleOrDefaultAsync(f => f.Codigo == codigo);

					if (personal == null)
						return NotFound();


					List<SelectListItem> Cargo = new SelectList(_context.Cargo.OrderBy(c => c.Nombre), "Codigo", "Nombre").ToList();
					Cargo.Insert(0, vacio);
					ViewData["CodigoCargo"] = Cargo;


					List<SelectListItem> TipoIdentificacion = new SelectList(_context.TipoIdentificacion.OrderBy(ti => ti.Nombre), "Codigo", "Nombre").ToList();
					TipoIdentificacion.Insert(0, vacio);
					ViewData["CodigoTipoIdentificacion"] = TipoIdentificacion;

					return View(personal);
				}
				else
					return Redirect("../Personal");
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}

		// POST: Personals/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<IActionResult> Edit(Personal personal)
		{
			var i = (ClaimsIdentity)User.Identity;
			List<SelectListItem> Cargo = new SelectList(_context.Cargo.OrderBy(c => c.Nombre), "Codigo", "Nombre").ToList();
			List<SelectListItem> TipoIdentificacion = new SelectList(_context.TipoIdentificacion.OrderBy(f => f.Nombre), "Codigo", "Nombre").ToList();

			if (i.IsAuthenticated)
			{
				try
				{
					if (ModelState.IsValid)
					{
						try
						{
							personal.Codigo = Encriptacion.Decrypt(personal.Codigo);
							_context.Update(personal);
							await _context.SaveChangesAsync();
							await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "Personal", personal.Codigo, "U");
							ViewBag.Message = "Save";


							Cargo.Insert(0, vacio);
							ViewData["CodigoCargo"] = Cargo;


							TipoIdentificacion.Insert(0, vacio);
							ViewData["CodigoTipoIdentificacion"] = TipoIdentificacion;
							return View(personal);
						}
						catch (DbUpdateConcurrencyException)
						{
							throw;
						}
					}

					Cargo.Insert(0, vacio);
					ViewData["CodigoCargo"] = Cargo;


					TipoIdentificacion.Insert(0, vacio);
					ViewData["CodigoTipoIdentificacion"] = TipoIdentificacion;

					return View(personal);
				}
				catch (Exception e)
				{
					string mensaje = e.Message;
					if (e.InnerException != null)
						mensaje = MensajesError.UniqueKey(e.InnerException.Message);

					ViewBag.Message = mensaje;

					Cargo.Insert(0, vacio);
					ViewData["CodigoCargo"] = Cargo;


					TipoIdentificacion.Insert(0, vacio);
					ViewData["CodigoTipoIdentificacion"] = TipoIdentificacion;

					return View(personal);
				}
			}
			else
			{
				return Redirect("../Identity/Account/Login");
			}
		}


		// POST: Personals/Delete/5
		[HttpPost]

		public async Task<String> DeleteConfirmed(string codigo)
		{
			try
			{
				var i = (ClaimsIdentity)User.Identity;
				var personal = await _context.Personal.SingleOrDefaultAsync(f => f.Codigo == codigo);
				_context.Personal.Remove(personal);
				await _context.SaveChangesAsync();
				await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "Personal", personal.Codigo, "D");
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


		[HttpGet]
		public async Task<List<SelectListItem>> CargarDoctores()
		{
			var i = (ClaimsIdentity)User.Identity;
			var perfil = i.Claims.Where(c => c.Type == "NombrePerfil").Select(c => c.Value).SingleOrDefault();
			var codigoPersonal = "";
			List<SelectListItem> list = new List<SelectListItem>();
			var Doctores = await _context.Personal.OrderBy(f => f.NombreCompleto).Where(p => p.Estado == true && p.Cargo.Nombre == "Doctor").ToListAsync();

			if (perfil.Contains("Doctor"))
			{
				codigoPersonal = i.Claims.Where(c => c.Type == "CodigoPersonal").Select(c => c.Value).SingleOrDefault();
				var personalN = _context.Personal.Where(p => p.Estado == true && p.Codigo == codigoPersonal).SingleOrDefault().NombreCompleto;
				list.Insert(0, new SelectListItem(personalN, codigoPersonal, true));
				foreach (Personal item in Doctores.ToList())
				{
					if (item.Codigo != codigoPersonal)
					{
						list.Add(new SelectListItem(item.NombreCompleto, item.Codigo));
					}
				}
			}
			else
			{
				//list.Insert(0, new SelectListItem("Seleccione...", "0"));
				foreach (Personal item in Doctores.ToList())
					list.Add(new SelectListItem(item.NombreCompleto, item.Codigo));
			}


			return list;
		}


	}
}
