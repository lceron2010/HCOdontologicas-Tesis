using HC_Odontologicas.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.RegularExpressions;

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
		public string VerifyCedula(string Identificacion)
		{
			return ValidarCedula(Identificacion);
		}


		private string ValidarCedula(String identificacion)
		{

			bool estado = false;
			char[] valced = new char[13];
			int provincia;
			if (identificacion.Length >= 10)
			{
				valced = identificacion.Trim().ToCharArray();
				provincia = int.Parse((valced[0].ToString() + valced[1].ToString()));
				if (provincia > 0 && provincia < 25)
				{
					if (int.Parse(valced[2].ToString()) < 6)
					{
						estado = VerificaCedula(valced);
					}					
				}
			}
	
			if (!estado)
			{
				return "Cédula Incorrecta.";
			}
			return string.Empty;

		}

		public static bool VerificaCedula(char[] validarCedula)
		{
			int aux = 0, par = 0, impar = 0, verifi;
			for (int i = 0; i < 9; i += 2)
			{
				aux = 2 * int.Parse(validarCedula[i].ToString());
				if (aux > 9)
					aux -= 9;
				par += aux;
			}
			for (int i = 1; i < 9; i += 2)
			{
				impar += int.Parse(validarCedula[i].ToString());
			}

			aux = par + impar;
			if (aux % 10 != 0)
			{
				verifi = 10 - (aux % 10);
			}
			else
				verifi = 0;
			if (verifi == int.Parse(validarCedula[9].ToString()))
				return true;
			else
				return false;
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