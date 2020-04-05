using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HC_Odontologicas.Models;
using Microsoft.AspNetCore.Mvc;

namespace HC_Odontologicas.Controllers
{
    public class ValidacionesController : Controller
    {
		private readonly HCOdontologicasContext _context;

		public ValidacionesController(HCOdontologicasContext context)
		{
			_context = context;
		}

		
		[AcceptVerbs("Get", "Post")]
		public IActionResult VerificarCodigoFacultad(string CodigoFacultad)
		{
			return ValidarCodigo4(CodigoFacultad);
		}
		[AcceptVerbs("Get", "Post")]
		public IActionResult VerificarCodigoCarrera(string CodigoCarrera)
		{
			return ValidarCodigo4(CodigoCarrera);
		}
		[AcceptVerbs("Get", "Post")]
		public IActionResult VerificarEstadoCivil(string EstadoCivil)
		{
			return ValidarCodigoCero(EstadoCivil);
		}
		[AcceptVerbs("Get", "Post")]
		public IActionResult VerificarGenero(string Genero)
		{
			return ValidarCodigoCero(Genero);
		}


		[AcceptVerbs("Get", "Post")]
		public IActionResult ValidarTipoPaciente(string TipoPaciente)
		{
			return ValidarCodigoCero(TipoPaciente);
		}
		[AcceptVerbs("Get", "Post")]
		public IActionResult ValidarCodigoTipoIdentificacion(string CodigoTipoIdentificacion)
		{
			return ValidarCodigo4(CodigoTipoIdentificacion);
		}

		[AcceptVerbs("Get", "Post")]
		public IActionResult VerifyCodigoPerfil(string CodigoPerfil)
		{
			return ValidarCodigo4(CodigoPerfil);
		}

		
		[AcceptVerbs("Get", "Post")]
		public string VerifyCampoRequerido(string Campo)
		{
			if (string.IsNullOrEmpty(Campo))
			{
				return "Campo requerido.";
			}
			return string.Empty;
		}

		public string VerifyComboRequerido(string Campo)
		{
			Campo = Campo == "0" ? null : Campo;
			if (string.IsNullOrEmpty(Campo))
			{
				return "Campo requerido.";
			}
			return string.Empty;
		}

		[AcceptVerbs("Get", "Post")]
		public string ValidarLongitudes(string NumDocMinimo, string NumDocMaximo)
		{
			int minimo = string.IsNullOrEmpty(NumDocMinimo) ? 0 : Convert.ToInt32(NumDocMinimo);
			int maximo = string.IsNullOrEmpty(NumDocMaximo) ? 0 : Convert.ToInt32(NumDocMaximo);
			if (minimo > maximo)
				return "La longitud mínima no debe ser mayor a la longitud máxima.";

			return string.Empty;
		}

		public static String VerifyCodigo(string Codigo)
		{
			if (Codigo.Length != 4)
			{
				return "Campo requerido.";
			}
			return "";
		}
				
		//public string ValidarIdentificacion(string CodigoCompania, string TipoIdentificacion, string Identificacion)
		//{
		//	var tipoIdentificacion = _context.Personal.SingleOrDefault(a => a.Cedula == ce);
		//	var compania = _context.Compania.SingleOrDefault(c => c.Codigo == CodigoCompania).Nombre.ToUpper();
		//	int numMinimo = tipoIdentificacion.NumeroMinino;
		//	int numMaximo = tipoIdentificacion.NumeroMaximo;
		//	string tipoId = tipoIdentificacion.TipoId;
		//	string tipoDato = tipoIdentificacion.TipoDato;
		//	string mensaje = string.Empty;

		//	//Longitud
		//	if (numMinimo == numMaximo && numMaximo != 0)
		//	{
		//		if (Identificacion.Length != numMinimo)
		//			mensaje = "La longitud de la identificación debe ser de " + numMinimo + ".";
		//	}
		//	else if (numMinimo != numMaximo)
		//	{
		//		if (Identificacion.Length < numMinimo || Identificacion.Length > numMaximo)
		//			mensaje = "La longitud de la identificación debe estar en un rango de " + numMinimo + " - " + numMaximo + ".";
		//	}

		//	//Tipo de Dato
		//	if (tipoDato == "N")
		//	{
		//		foreach (var ch in Identificacion)
		//		{
		//			if (!Char.IsNumber(ch))
		//				mensaje = "La identificación solo admite caracteres numéricos.";
		//		}
		//	}
		//	else if (tipoDato == "A")
		//	{
		//		if (compania == "CHILE")
		//		{
		//			if (tipoId == "N")
		//			{
		//				if (!Identificacion.Contains("-"))
		//					mensaje = "La identificación debe contener un guión '-' en la penúltima posición.";
		//				else
		//				{
		//					int cont = 0;
		//					int posicionCaracter = Identificacion.Length - 2;
		//					char[] word = Identificacion.ToCharArray();
		//					foreach (char a in word)
		//						if (a.ToString() == "-")
		//							cont++;
		//					if (word[posicionCaracter].ToString() != "-")
		//						mensaje = "El guión debe estar en la penúltima posición.";
		//					if (cont > 0)
		//						mensaje = "Solo debe haber un guión, debe estar en la penúltima posición.";
		//				}
		//			}
		//			else
		//			{
		//				if (Identificacion.Contains("-"))
		//				{
		//					int posicionCaracter = Identificacion.Length - 2;
		//					char[] word = Identificacion.ToCharArray();
		//					if (word[posicionCaracter].ToString() != "-")
		//						mensaje = "El guión debe estar en la penúltima posición.";
		//				}
		//			}
		//		}
		//		else
		//		{
		//			foreach (var ch in Identificacion)
		//			{
		//				if (!Char.IsLetterOrDigit(ch))
		//					mensaje = "La identificación solo admite caracteres alfanuméricos.";
		//			}
		//		}
		//	}

		//	return mensaje;
		//}

		public string ValidarContrasenia(string Contrasenia)
		{
			if (string.IsNullOrEmpty(Contrasenia))
			{
				return "Campo requerido.";
			}
			else
			{
				Regex regex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-.]).{8,}$");
				Match match = regex.Match(Contrasenia);
				if (!match.Success)
					return "La contraseña debe tener al menos 8 caracteres, una letra mayúscula, una letra minúscula, un dígito y un caracter especial.";
				return "";
			}
		}
			
		private IActionResult ValidarCodigo4(String Codigo)
		{
			if (Codigo.Length != 4)
			{
				return Json($"Campo requerido.");
			}

			return Json(true);
		}

		private IActionResult ValidarCodigo8(String Codigo)
		{
			if (Codigo.Length != 8)
			{
				return Json($"Campo requerido.");
			}

			return Json(true);
		}

		private IActionResult ValidarCodigoCero(string Codigo)
		{
			if (Codigo == "0")
			{
				return Json($"Campo requerido.");
			}
			return Json(true);
		}
	}
}