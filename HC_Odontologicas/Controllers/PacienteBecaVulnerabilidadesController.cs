using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HC_Odontologicas.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using HC_Odontologicas.FuncionesGenerales;
using System.Security.Claims;

namespace HC_Odontologicas.Controllers
{
    public class PacienteBecaVulnerabilidadesController : Controller
    {
        private readonly HCOdontologicasContext _context;
        private readonly IEmailSender _emailSender;
        
        public PacienteBecaVulnerabilidadesController(HCOdontologicasContext context, IEmailSender emailSender)
        {
            _context = context;
            _emailSender = emailSender;
        }

        // GET: PacienteBecaVulnerabilidades
        public async Task<IActionResult> Index()
        {
            var i = (ClaimsIdentity)User.Identity;
            if (i.IsAuthenticated)
            {
                //Permisos de usuario
                var permisos = i.Claims.Where(c => c.Type == "PacienteBecaVulnerabilidades").Select(c => c.Value).SingleOrDefault().Split(";");
                ViewData["Crear"] = Convert.ToBoolean(permisos[1]);
                ViewData["Editar"] = Convert.ToBoolean(permisos[2]);
                ViewData["Eliminar"] = Convert.ToBoolean(permisos[3]);
                ViewData["Exportar"] = Convert.ToBoolean(permisos[4]);

                if (Convert.ToBoolean(permisos[0]))
                {
                    return View(await _context.PacienteBecaVulnerabilidad.ToListAsync());
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

        // GET: PacienteBecaVulnerabilidads/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pacienteBecaVulnerabilidad = await _context.PacienteBecaVulnerabilidad
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (pacienteBecaVulnerabilidad == null)
            {
                return NotFound();
            }

            return View(pacienteBecaVulnerabilidad);
        }

        // GET: PacienteBecaVulnerabilidads/Create
        public IActionResult Create()
        {
           

            return View();
        }

        // POST: PacienteBecaVulnerabilidads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]        
        public async Task<IActionResult> Create(PacienteBecaVulnerabilidad pacienteBecaVulnerabilidad)
        {

            if (ModelState.IsValid)
            {

                PlantillaCorreoElectronico correo = new PlantillaCorreoElectronico();
                correo = _context.PlantillaCorreoElectronico.SingleOrDefault(p => p.Asunto.Contains("Campaña Informativa"));

                List<PacienteBecaVulnerabilidad> pacienteLista = new List<PacienteBecaVulnerabilidad>();
                pacienteLista = _context.PacienteBecaVulnerabilidad.Where(p => p.Grupo == pacienteBecaVulnerabilidad.Grupo).ToList();

                String correosLista = string.Empty;

                foreach (var Paciente in pacienteLista)
                {
                    correosLista = correosLista + Paciente.EmailPersonal + "," ;   
                }

                string cuerpo = "";
                cuerpo = FuncionesEmail.RecuperarMensajeCampania1(correo.Cuerpo);


                //string message = await FuncionesEmail.EnviarEmail(_emailSender, correosLista, correo.Asunto, cuerpo);
                string message = await FuncionesEmail.EnviarEmail(_emailSender, "ceron_laura2@hotmail.com", correo.Asunto, cuerpo);

                ModelState.AddModelError(string.Empty, message);


                //actualizar los datos
                

               // return RedirectToAction(nameof(Index));
            }
            return View(pacienteBecaVulnerabilidad);
        }

        // GET: PacienteBecaVulnerabilidads/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pacienteBecaVulnerabilidad = await _context.PacienteBecaVulnerabilidad.FindAsync(id);
            if (pacienteBecaVulnerabilidad == null)
            {
                return NotFound();
            }
            return View(pacienteBecaVulnerabilidad);
        }

        // POST: PacienteBecaVulnerabilidads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Codigo,NombreCompleto,Cedula,TipoBeca,Carrera,Periodo,EmailPersonal,EmailEPN")] PacienteBecaVulnerabilidad pacienteBecaVulnerabilidad)
        {
            if (id != pacienteBecaVulnerabilidad.Codigo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pacienteBecaVulnerabilidad);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteBecaVulnerabilidadExists(pacienteBecaVulnerabilidad.Codigo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pacienteBecaVulnerabilidad);
        }

        // GET: PacienteBecaVulnerabilidads/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pacienteBecaVulnerabilidad = await _context.PacienteBecaVulnerabilidad
                .FirstOrDefaultAsync(m => m.Codigo == id);
            if (pacienteBecaVulnerabilidad == null)
            {
                return NotFound();
            }

            return View(pacienteBecaVulnerabilidad);
        }

        // POST: PacienteBecaVulnerabilidads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var pacienteBecaVulnerabilidad = await _context.PacienteBecaVulnerabilidad.FindAsync(id);
            _context.PacienteBecaVulnerabilidad.Remove(pacienteBecaVulnerabilidad);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PacienteBecaVulnerabilidadExists(string id)
        {
            return _context.PacienteBecaVulnerabilidad.Any(e => e.Codigo == id);
        }
    }
}
