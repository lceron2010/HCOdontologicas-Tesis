using System;

namespace HC_Odontologicas.FuncionesGenerales
{
	public class Funciones
	{
		public static DateTime ObtenerFechaActual(String zonaHoraria)
		{
			TimeZoneInfo zona = TimeZoneInfo.FindSystemTimeZoneById(zonaHoraria);
			return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zona);
		}

		public static DateTime ObtenerFecha(DateTime fecha, String zonaHoraria)
		{
			TimeZoneInfo zona = TimeZoneInfo.FindSystemTimeZoneById(zonaHoraria);
			return TimeZoneInfo.ConvertTimeFromUtc(fecha, zona);
		}

	}
}
