using System.ComponentModel.DataAnnotations;

namespace HC_Odontologicas.Models
{
	public partial class Usuario
	{
		public string Codigo { get; set; }
		[Required(ErrorMessage = "Campo requerido.")]
		public string CodigoPerfil { get; set; }
		[Required(ErrorMessage = "Campo requerido.")]
		public string Contrasenia { get; set; }
		[Required(ErrorMessage = "Campo requerido.")]
		public string CorreoElectronico { get; set; }
		public string CodigoPersonal { get; set; }
		public string PasswordHash { get; set; }
		public string SecurityStamp { get; set; }

		public virtual Perfil Perfil { get; set; }
		public virtual Personal Personal { get; set; }
	}
}
