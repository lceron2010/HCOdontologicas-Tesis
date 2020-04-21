using HC_Odontologicas.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace HC_Odontologicas.Controllers
{
	public class AuditoriaController : Controller
	{
		private readonly HCOdontologicasContext _context;
		public AuditoriaController(HCOdontologicasContext context)
		{
			_context = context;
		}

		public async Task<string> GuardarLogAuditoria(DateTime Fecha, string Usuario, string Tabla, string Clave, string Accion)
		{
			try
			{
				//consultar la ip 
				var ip1 = GetLocalIPv4(NetworkInterfaceType.Ethernet).FirstOrDefault();
				var ip2 = GetLocalIPv4(NetworkInterfaceType.Wireless80211).FirstOrDefault();
				String ip = string.Empty;
				if (string.IsNullOrEmpty(ip1))
					ip = ip2;
				else
					ip = ip1;

				//string codigoCompania = _context.Empleado.SingleOrDefault(u => u.NombreUsuario == Usuario).CodigoCompania;

				LogAuditoria logAuditoria = new LogAuditoria();
				//logAuditoria.CodigoCompania = codigoCompania;

				//Fecha
				logAuditoria.Fecha = Fecha;

				//usuario		
				if (string.IsNullOrEmpty(Usuario))
					logAuditoria.Usuario = Environment.UserName;
				else if ((!string.IsNullOrEmpty(Usuario)) && (Usuario.Length > 32))
					logAuditoria.Usuario = Usuario.Substring(0, 32);
				else
					logAuditoria.Usuario = Usuario;

				//tabla
				if ((!string.IsNullOrEmpty(Tabla)) && (Tabla.Length > 32))
					Tabla = Tabla.Substring(0, 32);

				logAuditoria.Tabla = Tabla;
				logAuditoria.Clave = Clave;
				//accion
				logAuditoria.Accion = string.IsNullOrEmpty(Accion) ? "A" : Accion.Substring(0, 1);
				logAuditoria.DireccionIp = ip;
				_context.LogAuditoria.Add(logAuditoria);
				await _context.SaveChangesAsync();
				return "Save";
			}
			catch (Exception e)
			{
				return e.Message.ToString();
			}
		}

		public string[] GetLocalIPv4(NetworkInterfaceType _type)
		{
			List<string> ipAddrList = new List<string>();
			foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
			{
				if (!((item.Description.Contains("VirtualBox")) || (item.Description.Contains("VMware"))))
				{
					if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
					{
						foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
						{
							if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
							{
								ipAddrList.Add(ip.Address.ToString());
							}
						}
					}
				}
			}
			return ipAddrList.ToArray();
		}
	}
}