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
    public class PlantillasCertificadosMedicosController : Controller
    {
        private readonly HCOdontologicasContext _context;
        private ValidacionesController validaciones;
        private readonly AuditoriaController _auditoria;
        public PlantillasCertificadosMedicosController(HCOdontologicasContext context)
        {
            _context = context;
            validaciones = new ValidacionesController(_context);
            _auditoria = new AuditoriaController(context);
        }

        // GET: PlantillasCertificadosMedicos
        public async Task<IActionResult> Index(string search, string Filter, string sortOrder, int? page)
        {
            var i = (ClaimsIdentity)User.Identity;
            if (i.IsAuthenticated)
            {
                //Permisos de usuario
                var permisos = i.Claims.Where(c => c.Type == "PlantillasCertificadosMedicos").Select(c => c.Value).SingleOrDefault().Split(";");
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

                    var plantillaCertificado = from c in _context.PlantillaCertificadoMedico select c;
                    if (!String.IsNullOrEmpty(search))
                        plantillaCertificado = plantillaCertificado.Where(s => s.Nombre.Contains(search));

                    switch (sortOrder)
                    {
                        case "nombre_desc":
                            plantillaCertificado = plantillaCertificado.OrderByDescending(s => s.Nombre);
                            break;
                        default:
                            plantillaCertificado = plantillaCertificado.OrderBy(s => s.Nombre);
                            break;

                    }
                    int pageSize = 10;
                    return View(await Paginacion<PlantillaCertificadoMedico>.CreateAsync(plantillaCertificado, page ?? 1, pageSize));
                    
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

        

        // GET: PlantillasCertificadosMedicos/Create
        public IActionResult Create()
        {

            var i = (ClaimsIdentity)User.Identity;
            if (i.IsAuthenticated)
            {
                var permisos = i.Claims.Where(c => c.Type == "PlantillasCertificadosMedicos").Select(c => c.Value).SingleOrDefault().Split(";");

                if (Convert.ToBoolean(permisos[1]))
                {

                    return View();
                }
                else
                    return Redirect("../PlantillasCertificadosMedicos");
            }
            else
            {
                return Redirect("../Identity/Account/Login");
            }
        }

        // POST: PlantillasCertificadosMedicos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]       
        public async Task<IActionResult> Create(PlantillaCertificadoMedico plantillaCertificadoMedico)
        {
            var i = (ClaimsIdentity)User.Identity;
            if (i.IsAuthenticated)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        Int64 maxCodigo = 0;
                        maxCodigo = Convert.ToInt64(_context.PlantillaCertificadoMedico.Max(f => f.Codigo));
                        maxCodigo += 1;
                        plantillaCertificadoMedico.Codigo = maxCodigo.ToString("D4");
                        _context.Add(plantillaCertificadoMedico);
                        await _context.SaveChangesAsync();
                        string clave = plantillaCertificadoMedico.Nombre;
                        if (clave.Length > 54)
                        {
                            clave = clave.Substring(0, 53);
                        }
                        await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "PlantillaCertificadoMedico", clave, "I");
                        ViewBag.Message = "Save";
                        return View(plantillaCertificadoMedico);
                    }
                    return View(plantillaCertificadoMedico);

                }
                catch (Exception e)
                {
                    string mensaje = e.Message;
                    if (e.InnerException != null)
                        mensaje = MensajesError.UniqueKey(e.InnerException.Message);

                    ViewBag.Message = mensaje;

                    return View(plantillaCertificadoMedico);
                }
            }
            else
            {
                return Redirect("../Identity/Account/Login");
            }

        }

        // GET: PlantillasCertificadosMedicos/Edit/5
        public async Task<IActionResult> Edit(string codigo)
        {
            var i = (ClaimsIdentity)User.Identity;
            if (i.IsAuthenticated)
            {
                var permisos = i.Claims.Where(c => c.Type == "PlantillasCertificadosMedicos").Select(c => c.Value).SingleOrDefault().Split(";");
                codigo = Encriptacion.Decrypt(codigo);
                if (Convert.ToBoolean(permisos[2]))
                {
                    if (codigo == null)
                        return NotFound();

                    var plantillaCertificadoMedico = await _context.PlantillaCertificadoMedico.SingleOrDefaultAsync(f => f.Codigo == codigo);

                    if (plantillaCertificadoMedico == null)
                        return NotFound();

                    return View(plantillaCertificadoMedico);
                }
                else
                    return Redirect("../PlantillasCertificadosMedicos");
            }
            else
            {
                return Redirect("../Identity/Account/Login");
            }
        }

        // POST: PlantillasCertificadosMedicos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]        
        public async Task<IActionResult> Edit(PlantillaCertificadoMedico plantillaCertificadoMedico)
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
                            plantillaCertificadoMedico.Codigo =Encriptacion.Decrypt(plantillaCertificadoMedico.Codigo);
                            _context.Update(plantillaCertificadoMedico);
                            await _context.SaveChangesAsync();
                            await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "PlantillaCertificadoMedico", plantillaCertificadoMedico.Codigo, "U");
                            ViewBag.Message = "Save";

                            return View(plantillaCertificadoMedico);
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            throw;
                        }
                    }

                    return View(plantillaCertificadoMedico);
                }
                catch (Exception e)
                {
                    string mensaje = e.Message;
                    if (e.InnerException != null)
                        mensaje = MensajesError.UniqueKey(e.InnerException.Message);

                    ViewBag.Message = mensaje;

                    return View(plantillaCertificadoMedico);
                }
            }
            else
            {
                return Redirect("../Identity/Account/Login");
            }
        }


        // POST: PlantillasCertificadosMedicos/Delete/5
        [HttpPost]        
        public async Task<string> DeleteConfirmed(string codigo)
        {
            try
            {
                var i = (ClaimsIdentity)User.Identity;
                var plantillaCertificadoMedico = await _context.PlantillaCertificadoMedico.SingleOrDefaultAsync(f => f.Codigo == codigo);
                _context.PlantillaCertificadoMedico.Remove(plantillaCertificadoMedico);
                await _context.SaveChangesAsync();
                await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "PlantillaCertificadoMedico", plantillaCertificadoMedico.Codigo, "D");
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
