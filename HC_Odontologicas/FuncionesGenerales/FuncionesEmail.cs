using HC_Odontologicas.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HC_Odontologicas.FuncionesGenerales
{
	public class FuncionesEmail
	{
		public static async Task<string> EnviarEmail(IEmailSender emailSender, String emailDestino, String asunto, String mensaje)
		{
			try
			{
				//enviar mail
				await emailSender.SendEmailAsync(emailDestino, asunto, mensaje);

				//guardar email
				//LogEmail logEmail = new LogEmail();
				//logEmail.CodigoCompania = CodigoCompania;
				//logEmail.EmailOrigen = "lfvasquez@cavccsa.com";
				//logEmail.EmailDestino = emailDestino;
				//logEmail.Fecha = Funciones.ObtenerFechaActual("SA Pacific Standard Time");
				//logEmail.Asunto = asunto;
				//logEmail.Mensaje = mensaje;
				//context.LogEmail.Add(logEmail);
				//await context.SaveChangesAsync();

				return "Save";
			}
			catch (Exception e)
			{
				return e.Message.ToString();
			}
		}

		public static String AsuntoCitaOdontologica(String mensaje, String paciente, String fecha, string hora, string odontologo)
		{
			mensaje = mensaje.Replace("\r\n\r\n", "<br/><br/>").Replace("[#strong]", "<strong>").Replace("[#/strong]", "</strong>");
			mensaje = mensaje.Replace("[@Paciente]", paciente).Replace("[@Fecha]", fecha).Replace("[@HoraInicio]", hora);
			return mensaje;		
		}


		
		public static String CambiarContrasenia(String mensaje, String nombreCompleto, String usuario, String contrasenia, String callbackUrl)
		{
			mensaje = mensaje.Replace("\r\n\r\n", "<br/><br/>").Replace("[#strong]", "<strong>").Replace("[#/strong]", "</strong>");
			mensaje = mensaje.Replace("[@NombreCompleto]", nombreCompleto).Replace("[@NombreUsuario]", usuario).Replace("[@Contrasenia]", contrasenia).Replace("[@Url]", "<a href=" + callbackUrl + ">Aquí</a>");
			return mensaje;
		}
		public static String RecuperarContrasenia(String mensaje, String nombreCompleto, String usuario, String contrasenia, String callbackUrl)
		{
			mensaje = mensaje.Replace("\r\n\r\n", "<br/><br/>").Replace("[#strong]", "<strong>").Replace("[#/strong]", "</strong>");
			mensaje = mensaje.Replace("[@NombreCompleto]", nombreCompleto).Replace("[@NombreUsuario]", usuario).Replace("[@Contrasenia]", contrasenia).Replace("[@Url]", "<a href=" + callbackUrl + ">Aquí</a>");
			return mensaje;
		}
		public static String AsuntoReembolsoAprobado(String asunto, String solicitante)
		{
			asunto = asunto.Replace("[@Solictante]", solicitante);
			return asunto;
		}
	

		public static string RecuperarMensaje(string mensaje)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("<html>");
			str.AppendLine("<head>");
			str.AppendLine("<meta name='format-detection' content='telephone=no'>");
			str.AppendLine("<title>Notificaciones TravelWell</title>");
			str.AppendLine("</head>");
			str.AppendLine("<body>");
			str.AppendLine("<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
			str.AppendLine("<tr>");
			str.AppendLine("<td></td>");
			str.AppendLine("<td width='80%' style='padding:10px; border:1px solid LightGrey; font-family: Tahoma, Arial; font-size: 0.8em; text-align:left;' >");
			str.AppendLine("<div style='text-align: center;'>");
			str.AppendLine("<img src='cid:logo'>");
			str.AppendLine("</div>");
			str.AppendLine("<hr/>");
			str.AppendLine(mensaje);
			str.AppendLine("</td>");
			str.AppendLine("<td></td>");
			str.AppendLine("</tr>");
			str.AppendLine("</table>");
			str.AppendLine("</body>");
			str.AppendLine("</html>");
			return (str.ToString());
		}
	}

}

