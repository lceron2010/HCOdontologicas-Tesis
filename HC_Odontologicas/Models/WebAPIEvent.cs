using HC_Odontologicas.FuncionesGenerales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HC_Odontologicas.Models
{
	public class WebAPIEvent
	{
		//public int id { get; set; }
		//public string text { get; set; }
		public string start_date { get; set; }
		public string end_date { get; set; }

		
		public string Paciente { get; set; }
		public string Doctor { get; set; }
		public string Observaciones { get; set; }
		public DateTime FechaInicio { get; set; }
		public DateTime FechaFin { get; set; }
			

		public static explicit operator WebAPIEvent(CitaOdontologica ev)
		{
			return new WebAPIEvent
			{
				//CodigoPaciente = ev.CodigoPaciente,
				//CodigoPersonal = ev.Codigo,
				//Observaciones = ev.Observaciones,
				Paciente = ev.CodigoPaciente,
				Doctor = ev.CodigoPersonal,
				Observaciones = ev.Observaciones,
				start_date = ev.FechaInicio.ToString("yyyy-MM-dd HH:mm"),
				end_date = ev.FechaFin.ToString("yyyy-MM-dd HH:mm"),
			};
		}

		public static explicit operator CitaOdontologica(WebAPIEvent ev)
		{
			
			DateTime fechaInicio = DateTime.Parse(ev.start_date);
			DateTime fechaFin = DateTime.Parse(ev.end_date);
			return new CitaOdontologica
			{
				
				CodigoPaciente = ev.Paciente,
				CodigoPersonal = ev.Doctor,				
				Observaciones = ev.Observaciones,				
				FechaInicio = fechaInicio,
				FechaFin = fechaFin
				
			};
		}
	}
}
