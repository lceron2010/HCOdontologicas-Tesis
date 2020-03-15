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
    public class PlantillasRecetasMedicasController : Controller
    {
        private readonly HCOdontologicasContext _context;
        private ValidacionesController validaciones;
        private readonly AuditoriaController _auditoria;
        public PlantillasRecetasMedicasController(HCOdontologicasContext context)
        {
            _context = context;
            validaciones = new ValidacionesController(_context);
            _auditoria = new AuditoriaController(context);
        }

        // GET: PlantillasRecetasMedicas
        public async Task<IActionResult> Index(string search, string Filter, string sortOrder, int? page)
        {
            var i = (ClaimsIdentity)User.Identity;
            if (i.IsAuthenticated)
            {
                //Permisos de usuario
                var permisos = i.Claims.Where(c => c.Type == "PlantillasRecetasMedicas").Select(c => c.Value).SingleOrDefault().Split(";");
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

                    var plantillaReceta = from c in _context.PlantillaRecetaMedica select c;
                    if (!String.IsNullOrEmpty(search))
                        plantillaReceta = plantillaReceta.Where(s => s.Nombre.Contains(search));

                    switch (sortOrder)
                    {
                        case "nombre_desc":
                            plantillaReceta = plantillaReceta.OrderByDescending(s => s.Nombre);
                            break;
                        default:
                            plantillaReceta = plantillaReceta.OrderBy(s => s.Nombre);
                            break;

                    }
                    //int pageSize = 10;
                    // return View(await Paginacion<Anamnesis>.CreateAsync(cie10, page ?? 1, pageSize));
                    return View(plantillaReceta);
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

        // GET: PlantillasRecetasMedicas/Create
        public IActionResult Create()
        {

            var i = (ClaimsIdentity)User.Identity;
            if (i.IsAuthenticated)
            {
                var permisos = i.Claims.Where(c => c.Type == "PlantillasRecetasMedicas").Select(c => c.Value).SingleOrDefault().Split(";");

                if (Convert.ToBoolean(permisos[1]))
                {

                    return View();
                }
                else
                    return Redirect("../PlantillasRecetasMedicas");
            }
            else
            {
                return Redirect("../Identity/Account/Login");
            }
        }

        // POST: PlantillasRecetasMedicas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]        
        public async Task<IActionResult> Create(PlantillaRecetaMedica plantillaRecetaMedica)
        {
            var i = (ClaimsIdentity)User.Identity;
            if (i.IsAuthenticated)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        Int64 maxCodigo = 0;
                        maxCodigo = Convert.ToInt64(_context.PlantillaRecetaMedica.Max(f => f.Codigo));
                        maxCodigo += 1;
                        plantillaRecetaMedica.Codigo = maxCodigo.ToString("D4");
                        _context.Add(plantillaRecetaMedica);
                        await _context.SaveChangesAsync();
                        await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "PlantillaRecetaMedica", plantillaRecetaMedica.Codigo, "I");
                        ViewBag.Message = "Save";
                        return View(plantillaRecetaMedica);
                    }
                    return View(plantillaRecetaMedica);

                }
                catch (Exception e)
                {
                    string mensaje = e.Message;
                    if (e.InnerException != null)
                        mensaje = MensajesError.UniqueKey(e.InnerException.Message);

                    ViewBag.Message = mensaje;
                    return View(plantillaRecetaMedica);
                }
            }
            else
            {
                return Redirect("../Identity/Account/Login");
            }

        }

        // GET: PlantillasRecetasMedicas/Edit/5
        public async Task<IActionResult> Edit(string codigo)
        {
            var i = (ClaimsIdentity)User.Identity;
            if (i.IsAuthenticated)
            {
                var permisos = i.Claims.Where(c => c.Type == "PlantillasRecetasMedicas").Select(c => c.Value).SingleOrDefault().Split(";");
                codigo = Encriptacion.Decrypt(codigo);
                if (Convert.ToBoolean(permisos[2]))
                {
                    if (codigo == null)
                        return NotFound();

                    var plantillaRecetaMedica = await _context.PlantillaRecetaMedica.SingleOrDefaultAsync(f => f.Codigo == codigo);

                    if (plantillaRecetaMedica == null)
                        return NotFound();

                    return View(plantillaRecetaMedica);
                }
                else
                    return Redirect("../PlantillasRecetasMedicas");
            }
            else
            {
                return Redirect("../Identity/Account/Login");
            }

        }

        // POST: PlantillasRecetasMedicas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Edit(PlantillaRecetaMedica plantillaRecetaMedica)
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
                            plantillaRecetaMedica.Codigo = Encriptacion.Decrypt(plantillaRecetaMedica.Codigo);
                            _context.Update(plantillaRecetaMedica);
                            await _context.SaveChangesAsync();
                            await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "PlantillaRecetaMedica", plantillaRecetaMedica.Codigo, "U");
                            ViewBag.Message = "Save";

                            return View(plantillaRecetaMedica);
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            throw;
                        }
                    }

                    return View(plantillaRecetaMedica);
                }
                catch (Exception e)
                {
                    string mensaje = e.Message;
                    if (e.InnerException != null)
                        mensaje = MensajesError.UniqueKey(e.InnerException.Message);

                    ViewBag.Message = mensaje;

                    return View(plantillaRecetaMedica);
                }
            }
            else
            {
                return Redirect("../Identity/Account/Login");
            }
        }

       

        // POST: PlantillasRecetasMedicas/Delete/5
        [HttpPost]    
        public async Task<string> DeleteConfirmed(string codigo)
        {
            try
            {
                var i = (ClaimsIdentity)User.Identity;
                var plantillaRecetaMedica = await _context.PlantillaRecetaMedica.SingleOrDefaultAsync(f => f.Codigo == codigo);
                _context.PlantillaRecetaMedica.Remove(plantillaRecetaMedica);
                await _context.SaveChangesAsync();
                await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "PlantillaRecetaMedica", plantillaRecetaMedica.Codigo, "D");
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
