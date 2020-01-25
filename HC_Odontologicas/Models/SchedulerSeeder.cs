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

			
		}
	}

}
