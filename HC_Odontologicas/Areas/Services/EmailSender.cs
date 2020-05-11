using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
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
					From = new MailAddress(userName, "Notificaciones - Servicio de Odontología EPN")
				};

				string[] emails = email.Split(',');
				foreach (string em in emails)
				{
					mail.To.Add(new MailAddress(em));
				}

				mail.Subject = subject;
				mail.IsBodyHtml = true;

				String html = RecuperarMensaje(message);
				AlternateView altView = AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html);
				var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagen", "epnEmail.png");
				LinkedResource res = new LinkedResource(path, MediaTypeNames.Image.Gif);
				res.ContentId = "logo";
				altView.LinkedResources.Add(res);
				//inline.ContentId = Guid.NewGuid().ToString();
				mail.AlternateViews.Add(altView);
				mail.Body = html;

				SmtpClient oSmtp = new SmtpClient();
				oSmtp.Host = host;
				oSmtp.EnableSsl = true;
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


		//public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
		//{
		//	Options = optionsAccessor.Value;
		//}

		//public AuthMessageSenderOptions Options { get; } //set only via Secret Manager

		//public Task SendEmailAsync(string email, string subject, string message)
		//{

		//	if (subject == "CITA ODONTOLÓGICA")
		//	{
		//		return Execute(subject, message, email);
		//	}
		//	else if (subject == "Recuperar Contrasenia")
		//	{
		//		return Execute(subject, message, email);
		//	}
		//	else
		//	{
		//		return ExecuteCampania(subject, message, email);
		//	}

		//	//return Execute(subject, message, email);
		//}
		//public Task Execute(string subject, string message, string email)
		//{			
		//	var apiKey2 = "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";
		//	var client = new SendGridClient(apiKey2);
		//	Options.SendGridKey = "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";
			
		//	var from = new EmailAddress("serviciodeodontologiaepn@gmail.com", "Notificaciones - Servicio de Odontología EPN");
		//	List<EmailAddress> tos = new List<EmailAddress>();
		//	string[] emails = email.Split(',');
		//	foreach (string em in emails)
		//	{
		//		tos.Add(new EmailAddress(em));
		//	}
		//	var msg = MailHelper.CreateSingleEmailToMultipleRecipients(from, tos, subject, message, RecuperarMensaje(message), false);
		//	msg.HtmlContent = message;
		//	msg.SetClickTracking(false, false);
		//	return client.SendEmailAsync(msg);
		//}

		public async Task ExecuteCampania(string subject, string message, string email)
		{
			try
			{

				MailMessage mailMsg = new MailMessage();
				// From
				mailMsg.From = new MailAddress("serviciodeodontologiaepn@gmail.com", "Notificaciones - Servicio de Odontología EPN");

				string[] emails = email.Split(',');
				foreach (string em in emails)
				{
					if (!string.IsNullOrEmpty(em))
					{
						mailMsg.To.Add(new MailAddress(em));
					}
				}

				mailMsg.Subject = subject;
				string text = "";
				string html = message;
				AlternateView altView = AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html);
				var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "imagen", "campania1.jpg");
				LinkedResource res = new LinkedResource(path, MediaTypeNames.Image.Gif);
				res.ContentId = "logo";
				altView.LinkedResources.Add(res);
				mailMsg.AlternateViews.Add(altView);
				// Init SmtpClient and send
				SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
				System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("azure_3a3f7f8298aa13076779d4c1dfe4c697@azure.com", "Laura27**");
				smtpClient.Credentials = credentials;
				smtpClient.Send(mailMsg);

				//return new Task("Save"); //"Save";

			}
			catch (Exception ex)
			{
				ex.Message.ToString();
				//return ex.Message
			}
		}

		public static string RecuperarMensaje(string mensaje)
		{
			StringBuilder str = new StringBuilder();
			str.AppendLine("<html>");
			str.AppendLine("<head>");
			str.AppendLine("<meta name='format-detection' content='telephone=no'>");
			str.AppendLine("<title>Notificaciones Servicio Odontología</title>");
			str.AppendLine("</head>");
			str.AppendLine("<body>");
			str.AppendLine("<table width='100%' border='0' cellspacing='0' cellpadding='0'>");
			str.AppendLine("<tr>");
			str.AppendLine("<td></td>");
			str.AppendLine("<td width='80%' style='padding:10px; border:1px solid LightGrey; font-family: Tahoma, Arial; font-size: 0.8em; text-align:left;' >");
			str.AppendLine("<div style='text-align: center;'>");
			str.AppendLine("<img src='cid:logo'>");
			str.AppendLine("<p>Escuela Politécnica Nacional</p>");
			str.AppendLine("<p>Servicio de Odontología</p>");
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
