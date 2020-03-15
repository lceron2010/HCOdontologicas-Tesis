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
    public class PlantillaConsentimientosInformadosController : Controller
    {
        private readonly HCOdontologicasContext _context;
        private ValidacionesController validaciones;
        private readonly AuditoriaController _auditoria;
        public PlantillaConsentimientosInformadosController(HCOdontologicasContext context)
        {
            _context = context;
            validaciones = new ValidacionesController(_context);
            _auditoria = new AuditoriaController(context);
        }

        // GET: PlantillaConsentimientosInformados
        public async Task<IActionResult> Index(string search, string Filter, string sortOrder, int? page)
        {
            var i = (ClaimsIdentity)User.Identity;
            if (i.IsAuthenticated)
            {
                //Permisos de usuario
                var permisos = i.Claims.Where(c => c.Type == "PlantillaConcentimientosInformados").Select(c => c.Value).SingleOrDefault().Split(";");
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

                    var plantillaConsentimientoInformado = from c in _context.PlantillaConsentimientoInformado select c;
                    if (!String.IsNullOrEmpty(search))
                        plantillaConsentimientoInformado = plantillaConsentimientoInformado.Where(s => s.Nombre.Contains(search));

                    switch (sortOrder)
                    {
                        case "nombre_desc":
                            plantillaConsentimientoInformado = plantillaConsentimientoInformado.OrderByDescending(s => s.Nombre);
                            break;
                        default:
                            plantillaConsentimientoInformado = plantillaConsentimientoInformado.OrderBy(s => s.Nombre);
                            break;

                    }
                    //int pageSize = 10;
                    // return View(await Paginacion<Anamnesis>.CreateAsync(plantillaConsentimientoInformado, page ?? 1, pageSize));
                    return View(plantillaConsentimientoInformado);
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

        // GET: PlantillaConsentimientosInformados/Create
        public IActionResult Create()
        {
            var i = (ClaimsIdentity)User.Identity;
            if (i.IsAuthenticated)
            {
                var permisos = i.Claims.Where(c => c.Type == "PlantillaConcentimientosInformados").Select(c => c.Value).SingleOrDefault().Split(";");

                if (Convert.ToBoolean(permisos[1]))
                {

                    return View();
                }
                else
                    return Redirect("../PlantillaConcentimientosInformados");
            }
            else
            {
                return Redirect("../Identity/Account/Login");
            }
        }

        // POST: PlantillaConsentimientosInformados/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]       
        public async Task<IActionResult> Create(PlantillaConsentimientoInformado plantillaConsentimientoInformado)
        {
            var i = (ClaimsIdentity)User.Identity;
            if (i.IsAuthenticated)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        Int64 maxCodigo = 0;
                        maxCodigo = Convert.ToInt64(_context.PlantillaConsentimientoInformado.Max(f => f.Codigo));
                        maxCodigo += 1;
                        plantillaConsentimientoInformado.Codigo = maxCodigo.ToString("D4");
                        _context.Add(plantillaConsentimientoInformado);
                        await _context.SaveChangesAsync();
                        await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "plantillaConsentimientoInformado", plantillaConsentimientoInformado.Codigo, "I");
                        ViewBag.Message = "Save";
                        return View(plantillaConsentimientoInformado);
                    }
                    return View(plantillaConsentimientoInformado);

                }
                catch (Exception e)
                {
                    string mensaje = e.Message;
                    if (e.InnerException != null)
                        mensaje = MensajesError.UniqueKey(e.InnerException.Message);

                    ViewBag.Message = mensaje;

                    return View(plantillaConsentimientoInformado);
                }
            }
            else
            {
                return Redirect("../Identity/Account/Login");
            }

        }

        // GET: PlantillaConsentimientosInformados/Edit/5
        public async Task<IActionResult> Edit(String codigo)
        {
            var i = (ClaimsIdentity)User.Identity;
            if (i.IsAuthenticated)
            {
                var permisos = i.Claims.Where(c => c.Type == "PlantillaConsentimientosInformados").Select(c => c.Value).SingleOrDefault().Split(";");
                codigo = Encriptacion.Decrypt(codigo);
                if (Convert.ToBoolean(permisos[2]))
                {
                    if (codigo == null)
                        return NotFound();

                    var plantillaConsentimientoInformado = await _context.PlantillaConsentimientoInformado.SingleOrDefaultAsync(f => f.Codigo == codigo);

                    if (plantillaConsentimientoInformado == null)
                        return NotFound();

                    return View(plantillaConsentimientoInformado);
                }
                else
                    return Redirect("../PlantillaConcentimientosInformados");
            }
            else
            {
                return Redirect("../Identity/Account/Login");
            }
        }

        // POST: PlantillaConsentimientosInformados/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(PlantillaConsentimientoInformado plantillaConsentimientoInformado)
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
                            plantillaConsentimientoInformado.Codigo = Encriptacion.Decrypt(plantillaConsentimientoInformado.Codigo);
                            _context.Update(plantillaConsentimientoInformado);
                            await _context.SaveChangesAsync();
                            await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "plantillaConsentimientoInformado", plantillaConsentimientoInformado.Codigo, "U");
                            ViewBag.Message = "Save";

                            return View(plantillaConsentimientoInformado);
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            throw;
                        }
                    }

                    return View(plantillaConsentimientoInformado);
                }
                catch (Exception e)
                {
                    string mensaje = e.Message;
                    if (e.InnerException != null)
                        mensaje = MensajesError.UniqueKey(e.InnerException.Message);

                    ViewBag.Message = mensaje;

                    return View(plantillaConsentimientoInformado);
                }
            }
            else
            {
                return Redirect("../Identity/Account/Login");
            }
        }

        // POST: PlantillaConsentimientosInformados/Delete/5
        [HttpPost]        
        public async Task<string> DeleteConfirmed(string codigo)
        {
            try
            {
                var i = (ClaimsIdentity)User.Identity;
                var plantillaConsentimientoInformado = await _context.PlantillaConsentimientoInformado.SingleOrDefaultAsync(f => f.Codigo == codigo);
                _context.PlantillaConsentimientoInformado.Remove(plantillaConsentimientoInformado);
                await _context.SaveChangesAsync();
                await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "PlantillaConsentimientoInformado", plantillaConsentimientoInformado.Codigo, "D");
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
