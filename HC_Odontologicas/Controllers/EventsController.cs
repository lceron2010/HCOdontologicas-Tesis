using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HC_Odontologicas.FuncionesGenerales;
using HC_Odontologicas.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HC_Odontologicas.Controllers
{
	[Route("api/[controller]")]
	public class EventsController : ControllerBase
	{
		private readonly HCOdontologicasContext _context;
		public EventsController(HCOdontologicasContext context)
		{
			_context = context;
		}


		// GET: api/<controller>
		[HttpGet]
		public IEnumerable<WebAPIEvent> Get()
		{
			
			return _context.CitaOdontologica
				.ToList()
				.Select(e => (WebAPIEvent)e);
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
			newEvent.CodigoPaciente = apiEvent.Paciente;
			newEvent.CodigoPersonal = apiEvent.Doctor;
			newEvent.FechaCreacion = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
			newEvent.Observaciones = apiEvent.Observaciones;
			newEvent.FechaInicio = Convert.ToDateTime(apiEvent.start_date);
			newEvent.FechaFin = Convert.ToDateTime(apiEvent.end_date);
			newEvent.HoraInicio = new TimeSpan(newEvent.FechaInicio.Hour, newEvent.FechaInicio.Minute, newEvent.FechaInicio.Second);
			newEvent.HoraFin = new TimeSpan(newEvent.FechaFin.Hour, newEvent.FechaFin.Minute, newEvent.FechaFin.Second);
			newEvent.Estado = "C";
			newEvent.UsuarioCreacion = i.Name;

			_context.CitaOdontologica.Add(newEvent);
			_context.SaveChanges();

			return Ok(new
			{
				tid = newEvent.Codigo,
				action = "inserted"
			});
		}

		// PUT api/<controller>/5
		[HttpPut("{id}")]
		public ObjectResult Put(string codigo, [FromForm] WebAPIEvent apiEvent)
		{
			var updatedEvent = (CitaOdontologica)apiEvent;
			var dbEvent = _context.CitaOdontologica.Find(codigo);
			dbEvent.CodigoPaciente = updatedEvent.CodigoPaciente;
			dbEvent.CodigoPersonal = updatedEvent.CodigoPersonal;
			dbEvent.Observaciones = updatedEvent.Observaciones;
			dbEvent.FechaInicio = updatedEvent.FechaInicio;
			dbEvent.FechaFin = updatedEvent.FechaCreacion;
			dbEvent.HoraInicio = updatedEvent.HoraInicio;
			dbEvent.HoraFin = updatedEvent.HoraFin;
			dbEvent.UsuarioCreacion = updatedEvent.UsuarioCreacion;
			
			_context.SaveChanges();

			return Ok(new
			{
				action = "updated"
			});
		}

		// DELETE api/<controller>/5
		[HttpDelete("{id}")]
		public ObjectResult DeleteEvent(string codigo)
		{
			var e = _context.CitaOdontologica.Find(codigo);
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
