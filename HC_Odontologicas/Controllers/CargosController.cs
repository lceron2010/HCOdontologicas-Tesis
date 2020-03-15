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
    public class CargosController : Controller
    {
        private readonly HCOdontologicasContext _context;
        private ValidacionesController validaciones;
        private readonly AuditoriaController _auditoria;
        public CargosController(HCOdontologicasContext context)
        {
            _context = context;
            validaciones = new ValidacionesController(_context);
            _auditoria = new AuditoriaController(context);
        }

        // GET: Cargos
        public async Task<IActionResult> Index(string search, string Filter, string sortOrder, int? page)
        {
            var i = (ClaimsIdentity)User.Identity;
            if (i.IsAuthenticated)
            {
                //Permisos de usuario
                var permisos = i.Claims.Where(c => c.Type == "Cargos").Select(c => c.Value).SingleOrDefault().Split(";");
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

                    var cargo = from c in _context.Cargo select c;
                    if (!String.IsNullOrEmpty(search))
                        cargo = cargo.Where(s => s.Nombre.Contains(search));

                    switch (sortOrder)
                    {
                        case "nombre_desc":
                            cargo = cargo.OrderByDescending(s => s.Nombre);
                            break;
                        default:
                            cargo = cargo.OrderBy(s => s.Nombre);
                            break;

                    }
                    //int pageSize = 10;
                    // return View(await Paginacion<Anamnesis>.CreateAsync(cargo, page ?? 1, pageSize));
                    return View(cargo);
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

        // GET: Cargos/Create
        public IActionResult Create()
        {
            var i = (ClaimsIdentity)User.Identity;
            if (i.IsAuthenticated)
            {
                var permisos = i.Claims.Where(c => c.Type == "Cargos").Select(c => c.Value).SingleOrDefault().Split(";");

                if (Convert.ToBoolean(permisos[1]))
                {

                    return View();
                }
                else
                    return Redirect("../Cargos");
            }
            else
            {
                return Redirect("../Identity/Account/Login");
            }
        }

        // POST: Cargos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]      
        public async Task<IActionResult> Create(Cargo cargo)
        {
            var i = (ClaimsIdentity)User.Identity;
            if (i.IsAuthenticated)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        Int64 maxCodigo = 0;
                        maxCodigo = Convert.ToInt64(_context.Cargo.Max(f => f.Codigo));
                        maxCodigo += 1;
                        cargo.Codigo = maxCodigo.ToString("D4");
                        _context.Add(cargo);
                        await _context.SaveChangesAsync();
                        await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "Cargo", cargo.Codigo, "I");
                        ViewBag.Message = "Save";
                        return View(cargo);
                    }
                    return View(cargo);

                }
                catch (Exception e)
                {
                    string mensaje = e.Message;
                    if (e.InnerException != null)
                        mensaje = MensajesError.UniqueKey(e.InnerException.Message);
                    ViewBag.Message = mensaje;
                    return View(cargo);
                }
            }
            else
            {
                return Redirect("../Identity/Account/Login");
            }

        }

        // GET: Cargos/Edit/5
        public async Task<IActionResult> Edit(String codigo)
        {
            var i = (ClaimsIdentity)User.Identity;
            if (i.IsAuthenticated)
            {
                var permisos = i.Claims.Where(c => c.Type == "Cargos").Select(c => c.Value).SingleOrDefault().Split(";");
                codigo = Encriptacion.Decrypt(codigo);
                if (Convert.ToBoolean(permisos[2]))
                {
                    if (codigo == null)
                        return NotFound();

                    var cargo = await _context.Cargo.SingleOrDefaultAsync(f => f.Codigo == codigo);

                    if (cargo == null)
                        return NotFound();

                    return View(cargo);
                }
                else
                    return Redirect("../Cargos");
            }
            else
            {
                return Redirect("../Identity/Account/Login");
            }
        }

        // POST: Cargos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]        
        public async Task<IActionResult> Edit(Cargo cargo)
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
                            cargo.Codigo = Encriptacion.Decrypt(cargo.Codigo);
                            _context.Update(cargo);
                            await _context.SaveChangesAsync();
                            await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "Cargo", cargo.Codigo, "U");
                            ViewBag.Message = "Save";

                            return View(cargo);
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            throw;
                        }
                    }

                    return View(cargo);
                }
                catch (Exception e)
                {
                    string mensaje = e.Message;
                    if (e.InnerException != null)
                        mensaje = MensajesError.UniqueKey(e.InnerException.Message);

                    ViewBag.Message = mensaje;

                    return View(cargo);
                }
            }
            else
            {
                return Redirect("../Identity/Account/Login");
            }
        }

       
        // POST: Cargos/Delete/5
        [HttpPost]        
        public async Task<string> DeleteConfirmed(string codigo)
        {
            try
            {
                var i = (ClaimsIdentity)User.Identity;
                var cargo = await _context.Cargo.SingleOrDefaultAsync(f => f.Codigo == codigo);
                _context.Cargo.Remove(cargo);
                await _context.SaveChangesAsync();
                await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "Cargo", cargo.Codigo, "D");
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
