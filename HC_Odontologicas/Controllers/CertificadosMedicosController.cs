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
using Rotativa.AspNetCore;

namespace HC_Odontologicas.Controllers
{
    public class CertificadosMedicosController : Controller
    {
        private readonly HCOdontologicasContext _context;
        private ValidacionesController validaciones;
        private readonly AuditoriaController _auditoria;
        SelectListItem vacio = new SelectListItem(value: "0", text: "Seleccione...");

        public CertificadosMedicosController(HCOdontologicasContext context)
        {
            _context = context;
            validaciones = new ValidacionesController(_context);
            _auditoria = new AuditoriaController(context);
        }

        // GET: CertificadosMedicos
        public async Task<IActionResult> Index(string sortOrder, string Filter, int? page, string search)
        {
            var i = (ClaimsIdentity)User.Identity;
            if (i.IsAuthenticated)
            {
                //Permisos de usuario
                var permisos = i.Claims.Where(c => c.Type == "CertificadosMedicos").Select(c => c.Value).SingleOrDefault().Split(";");
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
                    var certificadoMedico = from c in _context.PlantillaCertificadoMedico.OrderBy(p => p.Nombre) select c;
                    //var personal = from c in _context.Personal select c;

                    if (!String.IsNullOrEmpty(search))
                        certificadoMedico = certificadoMedico.Where(s => s.Nombre.Contains(search));

                    switch (sortOrder)
                    {
                        case "nombre_desc":
                            certificadoMedico = certificadoMedico.OrderByDescending(s => s.Nombre);
                            break;
                        default:
                            certificadoMedico = certificadoMedico.OrderBy(s => s.Nombre);
                            break;

                    }
                    int pageSize = 10;
                    return View(await Paginacion<PlantillaCertificadoMedico>.CreateAsync(certificadoMedico, page ?? 1, pageSize)); // page devuelve valor si lo tiene caso contrario devuelve 1
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


        //[HttpGet]
        //public IActionResult DemoViewAsPdf()
        //{
        //    return new ViewAsPdf("Details");
        //}

        //[HttpGet]
        //public IActionResult DemoViewAsPdfTest()
        //{
        //    return new ViewAsPdf("Index");
        //}


        [HttpGet]
        public IActionResult DemoViewAsPdf()
        {
            return new ViewAsPdf("Details");
        }


        //GET: CertificadosMedicos/Details/5
        public async Task<IActionResult> Details(string codigo)
        {

            codigo = "00000016";

            if (codigo == null)
            {
                return NotFound();
            }                     

            var citaOdontologica = await _context.CitaOdontologica.Include(p =>p.Paciente).Include(p=>p.Personal)
                .FirstOrDefaultAsync(m => m.Codigo == codigo);


            List<SelectListItem> Personal = new SelectList(_context.Personal.OrderBy(c => c.NombreCompleto).Where(c => c.Estado == true), "Codigo", "NombreCompleto", citaOdontologica.CodigoPersonal).ToList();
            List<SelectListItem> Paciente = new SelectList(_context.Paciente.OrderBy(p => p.NombreCompleto).Where(p => p.Estado == true), "Codigo", "NombreCompleto", citaOdontologica.CodigoPaciente).ToList();
            

            ViewData["CodigoPersonal"] = Personal;
            ViewData["CodigoPaciente"] = Paciente;
            ViewData["CedulaPaciente"] = citaOdontologica.Paciente.Identificacion;
            
            var diagnostico = await _context.Diagnostico.Where(d => d.CodigoCitaOdontologica == codigo)
                .Include(d =>d.DiagnosticoCie10)
                .ThenInclude(dc => dc.Cie10)
                .SingleOrDefaultAsync();


            List<SelectListItem> Cie10 = new SelectList(_context.Cie10.OrderBy(c => c.Nombre), "Codigo", "Nombre", diagnostico.DiagnosticoCie10[0].Cie10.Nombre).ToList();
            ViewData["Cie10"] = Cie10;
            ViewData["CodigoCie10"] = diagnostico.DiagnosticoCie10[0].Cie10.CodigoInterno;
            ViewData["Pieza"] = diagnostico.Pieza;
            ViewData["NombreDoctor"] = citaOdontologica.Personal.NombreCompleto;
            ViewData["CorreoDoctor"] = citaOdontologica.Personal.CorreoElectronico;

            var plantillaCertificadoMedico = await _context.PlantillaCertificadoMedico.Where(c => c.Nombre == "Reposo-Recomendación").SingleOrDefaultAsync();

            var contenido = plantillaCertificadoMedico.Descripcion;

            if (citaOdontologica == null)
            {
                return NotFound();
            }

            //return View(plantillaCertificadoMedico);

            CertificadoMedicoImprimir cmi = new CertificadoMedicoImprimir();
            
            cmi.Fecha = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
            cmi.NombrePaciente = citaOdontologica.Paciente.NombreCompleto;
            cmi.CedulaPaciente = citaOdontologica.Paciente.Identificacion;
            cmi.CIE10Nombre = diagnostico.DiagnosticoCie10[0].Cie10.Nombre;
            cmi.CIE10Codigo = diagnostico.DiagnosticoCie10[0].Cie10.CodigoInterno;
            cmi.Pieza = diagnostico.Pieza.ToString();
            cmi.Observacion = diagnostico.Observacion;
            cmi.Recomendacion = diagnostico.Recomendacion;
            //var onservacionDiagnostico = diagnostico.Observacion;


            //Certifico que[@Paciente] con N[@Cedula] 
            // presenta[@EnfermedadNombre] CIE10[@EnfermedadCodigo] en el la pieza bucal 
            //# [@NumeroPieza] Por lo cual equiere reposo de  [@tiempoReposo] del dia [@FechaEnNombre], 
            //ademas se le recomendó [@Recomendacion]. Es todo cuanto puedo certificar en honor a la verdad 
            //y el interesado puede hacer uso del presente. 
            //Saludo Cordial, [@NombreMedico] Odontólogo Institucional de la Escuela Politécnica Nacional.
            //[@DoctorCorreo]
            var contenidoRemplazado = contenido.Replace("\r\n", "<br />")
                .Replace("\r\n\r\n\r\n\r\n", "<br /><br /><br /><br />").
                Replace("\r\n\r\n\r\n", "<br /><br /><br />");
            var final= contenidoRemplazado.Replace("[@Fecha]",cmi.Fecha.ToString("dd/MM/yyyy"))
                .Replace("[@Paciente]",cmi.NombrePaciente)
                .Replace("[@Cedula]",cmi.CedulaPaciente)
                .Replace("[@EnfermedadNombre]",cmi.CIE10Nombre)
                .Replace("[@EnfermedadCodigo]",cmi.CIE10Codigo)
                .Replace("[@NumeroPieza]", cmi.Pieza)
                .Replace("[@Observacion]", cmi.Observacion)
                .Replace("[@Recomendacion]", cmi.Recomendacion)
                .Replace("[@NombreMedico]", cmi.NombreMedico)
                .Replace("[@DoctorCorreo]",cmi.CorreoMedico);

            cmi.CuerpoCertificado = final.ToString();

            ViewData["Contenido"] = final;

            return View(cmi);
        }

        // GET: CertificadosMedicos/Create
        public IActionResult Create()
        {
            var i = (ClaimsIdentity)User.Identity;
            if (i.IsAuthenticated)
            {
                var permisos = i.Claims.Where(c => c.Type == "CertificadosMedicos").Select(c => c.Value).SingleOrDefault().Split(";");

                if (Convert.ToBoolean(permisos[1]))
                {

                    return View();
                }
                else
                    return Redirect("../CertificadosMedicos");
            }
            else
            {
                return Redirect("../Identity/Account/Login");
            }
        }

        // POST: CertificadosMedicos/Create
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
                        var c = _context.PlantillaCertificadoMedico.Count();
                        if (c > 0)
                        {
                            maxCodigo = Convert.ToInt64(_context.PlantillaCertificadoMedico.Max(f => f.Codigo));
                        }                       
                        
                        maxCodigo += 1;
                        _context.Add(plantillaCertificadoMedico);
                        await _context.SaveChangesAsync();
                        await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "plantillaCertificadoMedico", maxCodigo.ToString(), "I");
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

        // GET: CertificadosMedicos/Edit/5
        public async Task<IActionResult> Edit(string codigo)
        {
            var i = (ClaimsIdentity)User.Identity;
            if (i.IsAuthenticated)
            {
                var permisos = i.Claims.Where(c => c.Type == "CertificadosMedicos").Select(c => c.Value).SingleOrDefault().Split(";");
                codigo = Encriptacion.Decrypt(codigo);
                var codigoInt = Convert.ToInt32(codigo);
                if (Convert.ToBoolean(permisos[2]))
                {
                    if (codigo == null)
                        return NotFound();

                    var plantillaCertificadoMedico = await _context.PlantillaCertificadoMedico.SingleOrDefaultAsync(f => f.Codigo == codigoInt);

                    if (plantillaCertificadoMedico == null)
                        return NotFound();

                    return View(plantillaCertificadoMedico);
                }
                else
                    return Redirect("../CertificadosMedicos");
            }
            else
            {
                return Redirect("../Identity/Account/Login");
            }

        }

        // POST: CertificadosMedicos/Edit/5
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
                    var codigoModel = ModelState.Root.Children[0].RawValue.ToString();
                    var codigo = Encriptacion.Decrypt(codigoModel);
                    plantillaCertificadoMedico.Codigo = Convert.ToInt32(codigo);
                   
                    ModelState.Remove("{Codigo}");

                    //if (ModelState.IsValid)
                    //{
                        try
                        {
                            //plantillaCertificadoMedico.Codigo = Convert.ToInt32(Encriptacion.Decrypt(plantillaCertificadoMedico.Codigo.ToString()));
                            _context.PlantillaCertificadoMedico.Update(plantillaCertificadoMedico);
                            await _context.SaveChangesAsync();
                            await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "PlantillaCertificadoMedico", plantillaCertificadoMedico.Codigo.ToString(), "U");
                            ViewBag.Message = "Save";
                            return View(plantillaCertificadoMedico);
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            throw;
                        }
                    //}

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

        // POST: CertificadosMedicos/Delete/5
        [HttpPost]
        public async Task<String> DeleteConfirmed(int codigo)
        {
            try
            {
                var i = (ClaimsIdentity)User.Identity;
                var plantillaCertificadoMedico = await _context.PlantillaCertificadoMedico.SingleOrDefaultAsync(f => f.Codigo == codigo);
                _context.PlantillaCertificadoMedico.Remove(plantillaCertificadoMedico);
                await _context.SaveChangesAsync();
                await _auditoria.GuardarLogAuditoria(Funciones.ObtenerFechaActual("SA Pacific Standard Time"), i.Name, "PlantillaCertificadoMedico", plantillaCertificadoMedico.Codigo.ToString(), "D");
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
