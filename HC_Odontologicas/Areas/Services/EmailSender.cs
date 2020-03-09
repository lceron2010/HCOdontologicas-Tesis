using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace HC_Odontologicas.Areas.Services
{
	public class EmailSender : IEmailSender
	{
		// Our private configuration variables
		private string host;
		private int port;
		private bool enableSSL;
		private string userName;
		private string password;

		// Get our parameterized configuration
		public EmailSender(string host, int port, bool enableSSL, string userName, string password)
		{
			this.host = host;
			this.port = port;
			this.enableSSL = enableSSL;
			this.userName = userName;
			this.password = password;
		}

		public Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			Execute(email, subject, htmlMessage).Wait();
			return Task.FromResult(0);
		}


		public async Task Execute(string email, string subject, string message)
		{
			try
			{

				MailMessage mail = new MailMessage()
				{
					From = new MailAddress(userName, "Notificaciones - TravelWell")
				};

				string[] emails = email.Split(',');
				foreach (string em in emails)
				{
					mail.To.Add(new MailAddress(em));
				}

				mail.Subject = subject;
				mail.IsBodyHtml = true;

				String html = RecuperarMensajeTest(message);
				AlternateView altView = AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html);
				var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "travelswellEmail.png");
				LinkedResource res = new LinkedResource(path, MediaTypeNames.Image.Gif);
				res.ContentId = "logo";
				altView.LinkedResources.Add(res);
				//inline.ContentId = Guid.NewGuid().ToString();
				mail.AlternateViews.Add(altView);
				mail.Body = html;

				SmtpClient oSmtp = new SmtpClient();
				oSmtp.Host = host;
				oSmtp.EnableSsl = false;
				oSmtp.Port = port;

				string usuario = userName;
				string pwd = password;
				if (usuario != string.Empty & pwd != string.Empty)
					oSmtp.Credentials = new System.Net.NetworkCredential(usuario, pwd);

				await oSmtp.SendMailAsync(mail);

			}
			catch (Exception ex)
			{
				ex.Message.ToString();
			}
		}

		public static string RecuperarMensajeTest(string mensaje)
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
