using System;

namespace HC_Odontologicas.Models
{
	public class WebAPIEvent
	{
		public string id { get; set; }
		//public string text { get; set; }
		public string start_date { get; set; }
		public string end_date { get; set; }


		public string paciente { get; set; }
		public string doctor { get; set; }
		public string observaciones { get; set; }
		//public DateTime FechaInicio { get; set; }
		//public DateTime FechaFin { get; set; }


		public static explicit operator WebAPIEvent(CitaOdontologica ev)
		{


			var n = new WebAPIEvent
			{
				id = ev.Codigo,
				paciente = ev.CodigoPaciente,
				doctor = ev.CodigoPersonal,
				observaciones = ev.Observaciones,
				start_date = ev.FechaInicio.ToString("yyyy-MM-dd HH:mm"),
				end_date = ev.FechaFin.ToString("yyyy-MM-dd HH:mm"),
			};

			return n;
		}

		public static explicit operator CitaOdontologica(WebAPIEvent ev)
		{

			var c = new CitaOdontologica
			{
				Codigo = ev.id,
				CodigoPaciente = ev.paciente,
				CodigoPersonal = ev.doctor,
				Observaciones = ev.observaciones,
				FechaInicio = DateTime.Parse(ev.start_date),
				FechaFin = DateTime.Parse(ev.end_date)

			};

			return c;
		}
	}
}
