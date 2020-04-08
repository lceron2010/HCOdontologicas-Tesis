using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HC_Odontologicas.FuncionesGenerales
{
	public class Funciones
	{
		public static DateTime ObtenerFechaActual(String zonaHoraria)
		{
			TimeZoneInfo zona = TimeZoneInfo.FindSystemTimeZoneById(zonaHoraria);
			return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zona);
		}
	}
}
