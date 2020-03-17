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
    public class EnfermedadesController : Controller
    {
        private readonly HCOdontologicasContext _context;
        private ValidacionesController validaciones;
        private readonly AuditoriaController _auditoria;
        public EnfermedadesController(HCOdontologicasContext context)
        {
            _context = context;
            validaciones = new ValidacionesController(_context);
            _auditoria = new AuditoriaController(context);
        }

        // GET: Enfermedads
        public async Task<IActionResult> Index(string search, string Filter, string sortOrder, int? page)
        {
            var i = (ClaimsIdentity)User.Identity;
            if (i.IsAuthenticated)
            {
                //Permisos de usuario
                var permisos = i.Claims.Where(c => c.Type == "Enfermedades").Select(c => c.Value).SingleOrDefault().Split(";");
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

                    var enfermedad = from c in _context.Enfermedad select c;
                    if (!String.IsNullOrEmpty(search))
                        enfermedad = enfermedad.Where(s => s.Nombre.Contains(search));

                    switch (sortOrder)
                    {
                        case "nombre_desc":
                            enfermedad = enfermedad.OrderByDescending(s => s.Nombre);
                            break;
                        default:
                            enfermedad = enfermedad.OrderBy(s => s.Nombre);
                            break;

                    }
                    int pageSize = 10;
                    return View(await Paginacion<Enfermedad>.CreateAsync(enfermedad, page ?? 1, pageSize));
                    
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

        // GET: Enfermedads/Create
        public IActionResult Create()
        {
            var i = (ClaimsIdentity)User.Identity;
            if (i.IsAuthenticated)
            {
                var permisos = i.Claims.Where(c => c.Type == "Enfermedades").Select(c => c.Value).SingleOrDefault().Split(";");

                if (Convert.ToBoolean(permisos[1]))
                {

                    return View();
                }
                else
                    return Redirect("../Enfermedades");
            }
            else
            {
                return Redirect("../Identity/Account/Login");
            }
        }

        // POST: Enfermedads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]        
        public async Task<IActionResult> Create(Enfermedad enfermedad)
        {
            var i = (ClaimsIdentity)User.Identity;
            if (i.IsAuthenticated)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        Int64 maxCodigo = 0;
                        maxCodigo = Convert.ToInt64(_context.Enfermedad.Max(f => f.Codigo));
                        maxCodigo += 1;
                        enfermedad.Codigo = maxCodigo.ToString("D8");
                        _context.Add(enfermedad);
                        await _context.SaveChangesAsync();
                        await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "Enfermedad", enfermedad.Codigo, "I");
                        ViewBag.Message = "Save";
                        return View(enfermedad);
                    }
                    return View(enfermedad);

                }
                catch (Exception e)
                {
                    string mensaje = e.Message;
                    if (e.InnerException != null)
                        mensaje = MensajesError.UniqueKey(e.InnerException.Message);
                    ViewBag.Message = mensaje;

                    return View(enfermedad);
                }
            }
            else
            {
                return Redirect("../Identity/Account/Login");
            }

        }

        // GET: Enfermedads/Edit/5
        public async Task<IActionResult> Edit(String codigo)
        {
            var i = (ClaimsIdentity)User.Identity;
            if (i.IsAuthenticated)
            {
                var permisos = i.Claims.Where(c => c.Type == "Enfermedades").Select(c => c.Value).SingleOrDefault().Split(";");
                codigo = Encriptacion.Decrypt(codigo);
                if (Convert.ToBoolean(permisos[2]))
                {
                    if (codigo == null)
                        return NotFound();

                    var enfermedad = await _context.Enfermedad.SingleOrDefaultAsync(f => f.Codigo == codigo);

                    if (enfermedad == null)
                        return NotFound();

                    return View(enfermedad);
                }
                else
                    return Redirect("../Enfermedades");
            }
            else
            {
                return Redirect("../Identity/Account/Login");
            }
        }

        // POST: Enfermedads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]        
        public async Task<IActionResult> Edit(Enfermedad enfermedad)
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
                            enfermedad.Codigo = Encriptacion.Decrypt(enfermedad.Codigo);
                            _context.Update(enfermedad);
                            await _context.SaveChangesAsync();
                            await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "Enfermedad", enfermedad.Codigo, "U");
                            ViewBag.Message = "Save";

                            return View(enfermedad);
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            throw;
                        }
                    }

                    return View(enfermedad);
                }
                catch (Exception e)
                {
                    string mensaje = e.Message;
                    if (e.InnerException != null)
                        mensaje = MensajesError.UniqueKey(e.InnerException.Message);

                    ViewBag.Message = mensaje;

                    return View(enfermedad);
                }
            }
            else
            {
                return Redirect("../Identity/Account/Login");
            }

        }

     
        // POST: Enfermedads/Delete/5
        [HttpPost]       
        public async Task<string> DeleteConfirmed(string codigo)
        {
            try
            {
                var i = (ClaimsIdentity)User.Identity;
                var enfermedad = await _context.Enfermedad.SingleOrDefaultAsync(f => f.Codigo == codigo);
                _context.Enfermedad.Remove(enfermedad);
                await _context.SaveChangesAsync();
                await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "Enfermedad", enfermedad.Codigo, "D");
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
