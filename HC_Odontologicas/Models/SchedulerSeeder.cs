using System.Linq;

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
