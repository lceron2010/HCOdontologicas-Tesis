using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HC_Odontologicas.Models
{
	public class SchedulerSeeder
	{
		public static void Seed(HCOdontologicasContext context)
		{
			if (context.CitaOdontologica.Any())
			{
				return;   // DB has been seeded
			}

			var events = new List<CitaOdontologica>()
			{
				new CitaOdontologica
				{
					Codigo = "00000006",
					CodigoPaciente = "00000001",
					CodigoPersonal="00000001",
					FechaCreacion = new DateTime(),
					Observaciones ="ejemplo1",
					Estado="C",
					FechaInicio= new DateTime(2019, 1, 22, 2, 0, 0),
					FechaFin=  new DateTime(2019, 1, 22, 2, 0, 0),
					HoraInicio= new TimeSpan(15, 2, 0, 0),
					HoraFin=  new TimeSpan( 16, 2, 0, 0),
					UsuarioCreacion ="test"

				},
				new CitaOdontologica()
				{
					Codigo = "00000007",
					CodigoPaciente = "00000002",
					CodigoPersonal="00000001",
					FechaCreacion = new DateTime(),
					Observaciones ="ejemplo2",
					Estado="C",
					FechaInicio= new DateTime(2019, 1, 23, 2, 0, 0),
					FechaFin=  new DateTime(2019, 1, 23, 2, 0, 0),
					HoraInicio= new TimeSpan(15, 2, 0, 0),
					HoraFin=  new TimeSpan( 16, 2, 0, 0),
					UsuarioCreacion ="test"
				},
				
			};

			events.ForEach(s => context.CitaOdontologica.Add(s));
			context.SaveChanges();
		}
	}

}
