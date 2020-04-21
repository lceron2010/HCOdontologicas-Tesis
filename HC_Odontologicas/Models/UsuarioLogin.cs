using Microsoft.AspNetCore.Identity;

namespace HC_Odontologicas.Models
{
	public class UsuarioLogin : IdentityUser
	{
		public UsuarioLogin() : base() { }

		public string CodigoPerfil { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
	}

}
