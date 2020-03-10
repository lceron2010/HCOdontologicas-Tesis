using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HC_Odontologicas.FuncionesGenerales;
using HC_Odontologicas.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HC_Odontologicas.Controllers
{
	[Route("api/[controller]")]
	public class EventsController : ControllerBase
	{
		private readonly HCOdontologicasContext _context;
		private readonly IEmailSender _emailSender;
		public EventsController(HCOdontologicasContext context, IEmailSender emailSender)
		{
			_emailSender = emailSender;
			_context = context;
		}


		// GET: api/<controller>
		[HttpGet]
		public IEnumerable<WebAPIEvent> Get()
		{

			var hola = _context.CitaOdontologica.ToList().Select(e => (WebAPIEvent)e);
			var json = JsonConvert.SerializeObject(hola);
			return hola;
		}

		// GET api/<controller>/5
		[HttpGet("{id}")]
		public WebAPIEvent Get(string codigo)
		{

			return (WebAPIEvent)_context
				.CitaOdontologica
				.Find(codigo);
		}

		// POST api/<controller>
		[HttpPost]
		public ObjectResult Post([FromForm] WebAPIEvent apiEvent)
		{
			var newEvent = (CitaOdontologica)apiEvent;
			var i = (ClaimsIdentity)User.Identity;
			Int64 maxCodigo = 0;
			maxCodigo = Convert.ToInt64(_context.CitaOdontologica.Max(f => f.Codigo));
			maxCodigo += 1;
			newEvent.Codigo = maxCodigo.ToString("D8");
			newEvent.CodigoPaciente = apiEvent.paciente;
			newEvent.CodigoPersonal = apiEvent.doctor;
			newEvent.FechaCreacion = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
			newEvent.Observaciones = apiEvent.observaciones;
			newEvent.FechaInicio = Convert.ToDateTime(apiEvent.start_date);
			newEvent.FechaFin = Convert.ToDateTime(apiEvent.end_date);
			newEvent.HoraInicio = new TimeSpan(newEvent.FechaInicio.Hour, newEvent.FechaInicio.Minute, newEvent.FechaInicio.Second);
			newEvent.HoraFin = new TimeSpan(newEvent.FechaFin.Hour, newEvent.FechaFin.Minute, newEvent.FechaFin.Second);
			newEvent.Estado = "C";
			newEvent.UsuarioCreacion = i.Name;

			_context.CitaOdontologica.Add(newEvent);
			_context.SaveChanges();

			//enviar el mail

			PlantillaCorreoElectronico correo = new PlantillaCorreoElectronico();
			correo = _context.PlantillaCorreoElectronico.SingleOrDefault(p => p.Nombre.Contains("Cita"));
			var paciente = _context.Paciente.Where(p => p.Codigo == apiEvent.paciente).FirstOrDefault();
			var doctor = _context.Personal.Where(d => d.Codigo == apiEvent.doctor).FirstOrDefault();			
			var soloFecha = Convert.ToDateTime(newEvent.FechaInicio.ToString("dd/MM/yyyy"));
			var fechaLarga = soloFecha.ToLongDateString();
			var hora = newEvent.FechaInicio.ToString("HH:mm"); // newEvent.FechaInicio.TimeOfDay.ToString();//newEvent.FechaInicio.Hour.ToString() + ":" + newEvent.FechaInicio.Minute.ToString();//newEvent.FechaInicio.ToString("hh:mm");

			//envio del email

			var correoMensaje = FuncionesEmail.EnviarEmail(_emailSender,paciente.MailEpn, correo.Asunto, 
				FuncionesEmail.AsuntoCitaOdontologica(correo.Cuerpo, 
				paciente.NombreCompleto, fechaLarga, hora, doctor.NombreCompleto));


			return Ok(new
			{
				tid = newEvent.Codigo,
				action = "inserted"
			});
		}

		// PUT api/<controller>/5
		[HttpPut("{id}")]
		public ObjectResult Put(string id, [FromForm] WebAPIEvent apiEvent)
		{
			var i = (ClaimsIdentity)User.Identity;
			var updatedEvent = (CitaOdontologica)apiEvent;
			var dbEvent = _context.CitaOdontologica.Find(id);

			//dbEvent.CodigoPaciente = updatedEvent.CodigoPaciente;
			dbEvent.CodigoPaciente = updatedEvent.CodigoPaciente;
			dbEvent.CodigoPersonal = updatedEvent.CodigoPersonal;
			dbEvent.FechaCreacion = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
			dbEvent.Observaciones = updatedEvent.Observaciones;
			dbEvent.FechaInicio = Convert.ToDateTime(updatedEvent.FechaInicio);
			dbEvent.FechaFin = Convert.ToDateTime(updatedEvent.FechaFin);
			dbEvent.HoraInicio = new TimeSpan(updatedEvent.FechaInicio.Hour, updatedEvent.FechaInicio.Minute, updatedEvent.FechaInicio.Second);
			dbEvent.HoraFin = new TimeSpan(updatedEvent.FechaFin.Hour, updatedEvent.FechaFin.Minute, updatedEvent.FechaFin.Second);
			dbEvent.Estado = "M";
			dbEvent.UsuarioCreacion = i.Name;


			_context.SaveChanges();

			return Ok(new
			{
				action = "updated"
			});
		}

		// DELETE api/<controller>/5
		[HttpDelete("{id}")]
		public ObjectResult DeleteEvent(string id)
		{
			var e = _context.CitaOdontologica.Find(id);
			if (e != null)
			{
				_context.CitaOdontologica.Remove(e);
				_context.SaveChanges();
			}

			return Ok(new
			{
				action = "deleted"
			});
		}
	}
}
