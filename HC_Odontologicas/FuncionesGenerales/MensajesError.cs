using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HC_Odontologicas.FuncionesGenerales
{
	public class MensajesError
	{
		public static string UniqueKey(string mensajeError)
		{
			if (mensajeError.Contains("UNIQUE KEY"))
			{
				if (mensajeError.Contains("Nombre"))
					mensajeError = "El nombre ya ha sido registrado.";
				if (mensajeError.Contains("Usuario"))
					mensajeError = "El nombre de usuario ya ha sido registrado.";
				if (mensajeError.Contains("Abreviado"))
					mensajeError = "El nombre abreviado ya ha sido registrado.";
				if (mensajeError.Contains("CorreoElectronico"))
					mensajeError = "El correo ya ha sido registrado.";
				if (mensajeError.Contains("Numerounico"))
					mensajeError = "El número unico ya ha sido registrado.";
				if (mensajeError.Contains("Asunto"))
					mensajeError = "El asunto ya ha sido registrado.";
			}
						return mensajeError;
		}

		public static string ForeignKey(string mensajeError)
		{
			if (mensajeError.Contains("FK"))
				mensajeError = "No puede eliminar este registro. Verifique que no este siendo usado.";

			return mensajeError;
		}
	}
}
